using DevExpress.Mvvm;
using EasyGrid.Core.Providers;
using EasyGrid.Providers;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EasyGrid.Models;
using EasyGrid.Views;

namespace EasyGrid.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public static string WindowTitle => $"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version?.Major}.{Assembly.GetExecutingAssembly().GetName().Version?.Minor}";
        public GridParametersModel GridParameters { get; set; }
        public ProgressBarModel ProgressBar { get; set; }

        public ICommand SelectSASCommand { get; }
        public ICommand CopySASCommand { get; }
        public ICommand CreateGridCommand { get; }
        public ICommand CancelCreateGridCommand { get; }

        private readonly Window windowInstance;
        private readonly CreateGridProvider createGridProvider;
        private readonly ConvertGridToGpxProvider convertGridToGpxProvider;
        private readonly SaveGpxProvider saveGpxProvider;
        private readonly SelectionSASProvider selectionProvider;
        private readonly ResultProvider resultProvider;
        private readonly SaveFileDialog saveFileDialog;
        private readonly OpenFileDialog openFileDialog;
        private ResultWindow resultWindow;

        private CancellationTokenSource cancellationTokenSource;
        private SettingsProvider settingsProvider;
        private bool isWorking;
        private string sasLastSelectionPath;
        private string lastSavePath;

        public MainViewModel(Window windowInstance)
        {
            this.windowInstance = windowInstance;

            GridParameters = new GridParametersModel();
            ProgressBar = new ProgressBarModel();

            LoadSettings();

            selectionProvider = new SelectionSASProvider();
            createGridProvider = new CreateGridProvider();
            convertGridToGpxProvider = new ConvertGridToGpxProvider(WindowTitle);
            saveGpxProvider = new SaveGpxProvider(false);
            resultProvider = new ResultProvider();

            saveFileDialog = new SaveFileDialog
            {
                Filter = "GPX file (*.gpx)|*.gpx",
                RestoreDirectory = true
            };

            openFileDialog = new OpenFileDialog()
            {
                Filter = "SAS Planet selection file (*.hlg)|*.hlg",
                InitialDirectory = Path.GetDirectoryName(sasLastSelectionPath) ?? string.Empty,
                FileName = Path.GetFileName(sasLastSelectionPath) ?? string.Empty,
                RestoreDirectory = true
            };

            SelectSASCommand = new DelegateCommand(SelectSAS);
            CopySASCommand = new DelegateCommand(CopyFromSAS, () => File.Exists(sasLastSelectionPath));
            CreateGridCommand = new DelegateCommand(CreateGrid, () => !isWorking && GridParameters.IsValid());
            CancelCreateGridCommand = new DelegateCommand(CancelCreateGrid, () => isWorking);

            if (CopySASCommand.CanExecute(null))
            {
                CopySASCommand.Execute(null);
            }

            createGridProvider.ProgressChanged += (progress) => ProgressBar.Progress = progress * 0.6;
            createGridProvider.StatusChanged += (status) => ProgressBar.Status = status;

            convertGridToGpxProvider.ProgressChanged += (progress) => ProgressBar.Progress = 60 + progress * 0.4;
            convertGridToGpxProvider.StatusChanged += (status) => ProgressBar.Status = status;

            saveGpxProvider.ProgressChanged += (progress) => ProgressBar.Progress = progress;
            saveGpxProvider.StatusChanged += (status) => ProgressBar.Status = status;
        }

        private void SelectSAS()
        {
            if (openFileDialog.ShowDialog() == true)
            {
                sasLastSelectionPath = openFileDialog.FileName;
                SaveSettings();
            }
        }

        private void CopyFromSAS()
        {
            var lastSelection = selectionProvider.GetLastSelection(sasLastSelectionPath);
            GridParameters.SetSelection(lastSelection);
        }

        private void CreateGrid()
        {
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(lastSavePath) ?? string.Empty;
            saveFileDialog.FileName = $"Grid_{GridParameters.SquareSize}_{DateTime.Now:yyyy-MM-dd}";

            if (saveFileDialog.ShowDialog() == true)
            {
                cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;

                Task.Run(() =>
                {
                    isWorking = true;
                    ProgressBar.Show();

                    var grid = createGridProvider.CreateGrid(GridParameters.LeftTopLat, GridParameters.LeftTopLon, 
                        GridParameters.RightBottomLat, GridParameters.RightBottomLon, GridParameters.SquareSize, cancellationToken);

                    if (cancellationToken.IsCancellationRequested) return;

                    var gpx = convertGridToGpxProvider.ConvertToGpx(grid, cancellationToken);
                    if (cancellationToken.IsCancellationRequested) return;

                    saveGpxProvider.SaveAsync(gpx, saveFileDialog.FileName, cancellationToken).Wait(cancellationToken);
                    if (cancellationToken.IsCancellationRequested) return;

                    ProgressBar.Status = "Ready";
                    var result = resultProvider.GetResult(grid.GetLength(0), grid.GetLength(1), saveFileDialog.FileName, GridParameters, cancellationToken);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        resultWindow ??= new ResultWindow(windowInstance);
                        resultWindow.SetData(result);
                        resultWindow.Show();
                    });

                }, cancellationToken).ContinueWith(task =>
                {
                    isWorking = false;
                    Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
                    ProgressBar.Hide().Reset();

                }, cancellationToken);

                SaveSettings();
            }
        }

        private void CancelCreateGrid()
        {
            cancellationTokenSource?.Cancel();
        }

        private void LoadSettings()
        {
            settingsProvider = new SettingsProvider();
            sasLastSelectionPath = settingsProvider.SASPlanetParameters.LastSelectionPath;
            lastSavePath = settingsProvider.LastCreateParameters.LastSavePath;
            GridParameters.SquareSize = settingsProvider.LastCreateParameters.SquareSize == 0 ? 500 : settingsProvider.LastCreateParameters.SquareSize;
        }

        private void SaveSettings()
        {
            settingsProvider.SASPlanetParameters.LastSelectionPath = sasLastSelectionPath;
            settingsProvider.LastCreateParameters.LastSavePath = lastSavePath;
            settingsProvider.LastCreateParameters.SquareSize = GridParameters.SquareSize;
            settingsProvider.Save();
        }
    }
}
