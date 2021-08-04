using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.Mvvm;
using EasyGrid.Core.Providers;
using EasyGrid.Models;
using EasyGrid.Providers;
using EasyGrid.Views;
using Application = System.Windows.Application;

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
        private readonly FolderBrowserDialog openFileDialog;
        private ResultWindow resultWindow;

        private CancellationTokenSource cancellationTokenSource;
        private SettingsProvider settingsProvider;
        private bool isWorking;
        private string sasDirectoryPath;
        private string lastSavePath;

        public MainViewModel(Window windowInstance)
        {
            this.windowInstance = windowInstance;

            GridParameters = new GridParametersModel();
            ProgressBar = new ProgressBarModel();

            LoadSettings();

            selectionProvider = new SelectionSASProvider("LastSelection.hlg");
            createGridProvider = new CreateGridProvider();
            convertGridToGpxProvider = new ConvertGridToGpxProvider(WindowTitle);
            saveGpxProvider = new SaveGpxProvider(false);
            resultProvider = new ResultProvider();

            saveFileDialog = new SaveFileDialog()
            {
                Filter = @"GPX file (*.gpx)|*.gpx",
                RestoreDirectory = true
            };

            openFileDialog = new FolderBrowserDialog()
            {
                RootFolder = Environment.SpecialFolder.Desktop
            };

            SelectSASCommand = new DelegateCommand(SelectSAS);
            CopySASCommand = new DelegateCommand(CopyFromSAS, () => selectionProvider.FileExists(sasDirectoryPath));
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
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                sasDirectoryPath = openFileDialog.SelectedPath;
                SaveSettings();
            }
        }

        private void CopyFromSAS()
        {
            var lastSelection = selectionProvider.GetLastSelection(sasDirectoryPath);
            GridParameters.SetSelection(lastSelection);
        }

        private void CreateGrid()
        {
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(lastSavePath) ?? string.Empty;
            saveFileDialog.FileName = $"Grid_{GridParameters.SquareSize}_{DateTime.Now:yyyy-MM-dd}";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;

                Task.Run(() =>
                {
                    isWorking = true;
                    ProgressBar.Show();

                    try
                    {
                        var grid = createGridProvider.CreateGrid(GridParameters.LeftTopLat, GridParameters.LeftTopLon,
                            GridParameters.RightBottomLat, GridParameters.RightBottomLon, GridParameters.SquareSize,
                            cancellationToken);
                        var gpx = convertGridToGpxProvider.ConvertToGpx(grid, cancellationToken);
                        saveGpxProvider.SaveAsync(gpx, saveFileDialog.FileName, cancellationToken)
                            .Wait(cancellationToken);

                        if (!cancellationToken.IsCancellationRequested)
                        {
                            ProgressBar.Status = "Ready";
                            var result = resultProvider.GetResult(grid.GetLength(0), grid.GetLength(1),
                                saveFileDialog.FileName, GridParameters, cancellationToken);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                resultWindow ??= new ResultWindow(windowInstance);
                                resultWindow.SetData(result);
                                resultWindow.Show();
                            });
                        }
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    finally
                    {
                        isWorking = false;
                        Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
                        ProgressBar.Hide().Reset();
                    }

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
            sasDirectoryPath = settingsProvider.SASPlanetParameters.SASPath;
            lastSavePath = settingsProvider.LastCreateParameters.LastSavePath;
            GridParameters.SquareSize = settingsProvider.LastCreateParameters.SquareSize == 0 ? 500 : settingsProvider.LastCreateParameters.SquareSize;
        }

        private void SaveSettings()
        {
            settingsProvider.SASPlanetParameters.SASPath = sasDirectoryPath;
            settingsProvider.LastCreateParameters.LastSavePath = lastSavePath;
            settingsProvider.LastCreateParameters.SquareSize = GridParameters.SquareSize;
            settingsProvider.Save();
        }
    }
}
