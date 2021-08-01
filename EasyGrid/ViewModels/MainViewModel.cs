using DevExpress.Mvvm;
using EasyGrid.Commands;
using EasyGrid.Core.Models;
using EasyGrid.Core.Providers;
using EasyGrid.Providers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Input;

namespace EasyGrid.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly SettingsProvider settingsProvider;
        private readonly SASLastSelectionProvider lastSelectionProvider;
        private readonly CreateGridProvider createGridProvider;
        private readonly ConvertGridToGpxProvider convertGridToGpxProvider;
        private readonly SaveGpxProvider saveGpxProvider;
        private readonly SaveFileDialog saveFileDialog;

        public MainViewModel()
        {
            settingsProvider = new SettingsProvider();

            squareSizes = new[] { 500, 250, 200, 100 };
            squareSize = settingsProvider.LastSquareSize;

            lastSelectionProvider = new SASLastSelectionProvider();
            SetSelection(lastSelectionProvider.GetLastSelection());

            createGridProvider = new CreateGridProvider();

            var creatorName = $"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version?.Major}.{Assembly.GetExecutingAssembly().GetName().Version?.Minor}";
            convertGridToGpxProvider = new ConvertGridToGpxProvider(creatorName);
            
            saveGpxProvider = new SaveGpxProvider();

            saveFileDialog = new SaveFileDialog
            {
                Filter = "GPX file (*.gpx)|*.gpx",
                RestoreDirectory = true
            };

        }

        public ICommand CreateGridCommand
        {
            get
            {
                return new CreateGridCommand((obj) =>
                {
                    saveFileDialog.InitialDirectory = GetLastSaveDirectory();
                    saveFileDialog.FileName = $"Grid_{squareSize}_{DateTime.Now:yyyy-MM-dd}";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var grid = createGridProvider.CreateGrid(leftTopLat, leftTopLon, rightBottomLat, rightBottomLon, squareSize);
                        var gpx = convertGridToGpxProvider.ConvertToGpx(grid);
                        saveGpxProvider.Save(gpx, saveFileDialog.FileName);
                        UpdateSettings();
                    }
                });
            }
        }

        public ICommand CopyCoordinatesCommand
        {
            get
            {
                return new CopyCoordinatesCommand((obj) =>
                {
                    SetSelection(lastSelectionProvider.GetLastSelection());
                });
            }
        }

        private void SetSelection(GeoPoint[] points)
        {
            LeftTopLat = points[0].Lat;
            LeftTopLon = points[0].Lon;
            RightBottomLat = points[1].Lat;
            RightBottomLon = points[1].Lon;
        }

        private void UpdateSettings()
        {
            settingsProvider.LastFilePath = saveFileDialog.FileName;
            settingsProvider.LastSquareSize = SquareSize;
            settingsProvider.Save();
        }

        private string GetLastSaveDirectory()
        {
            var lastDirectoryPath = Path.GetFullPath(Path.GetDirectoryName(settingsProvider.LastFilePath) ?? string.Empty);
            if (Directory.Exists(lastDirectoryPath))
            {
                return lastDirectoryPath;
            }
            return Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        private double leftTopLat;
        public double LeftTopLat
        {
            get => leftTopLat;
            set
            {
                leftTopLat = value;
                RaisePropertyChanged(() => LeftTopLat);
            }
        }

        private double leftTopLon;
        public double LeftTopLon
        {
            get => leftTopLon;
            set
            {
                leftTopLon = value;
                RaisePropertyChanged(() => LeftTopLon);
            }
        }

        private double rightBottomLat;
        public double RightBottomLat
        {
            get => rightBottomLat;
            set
            {
                rightBottomLat = value;
                RaisePropertyChanged(() => RightBottomLat);
            }
        }

        private double rightBottomLon;
        public double RightBottomLon
        {
            get => rightBottomLon;
            set
            {
                rightBottomLon = value;
                RaisePropertyChanged(() => RightBottomLon);
            }
        }

        private int squareSize;
        public int SquareSize
        {
            get => squareSize;
            set
            {
                squareSize = value;
                RaisePropertyChanged(() => SquareSize);
            }
        }

        private IEnumerable<int> squareSizes;
        public IEnumerable<int> SquareSizes
        {
            get => squareSizes;
            set
            {
                squareSizes = value;
                RaisePropertyChanged(() => SquareSizes);
            }
        }
    }
}
