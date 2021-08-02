using DevExpress.Mvvm;
using EasyGrid.Commands;
using EasyGrid.Core.Models;
using EasyGrid.Core.Providers;
using EasyGrid.Providers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
        private readonly OpenFileDialog openFileDialog;

        public MainViewModel()
        {
            settingsProvider = new SettingsProvider();
            SquareSizes = new[] { 500, 250, 200, 100 };
            LoadSettings();

            createGridProvider = new CreateGridProvider();
            convertGridToGpxProvider = new ConvertGridToGpxProvider(CreatorName);
            saveGpxProvider = new SaveGpxProvider();
            lastSelectionProvider = new SASLastSelectionProvider();

            saveFileDialog = new SaveFileDialog
            {
                Filter = "GPX file (*.gpx)|*.gpx",
                RestoreDirectory = true
            };

            openFileDialog = new OpenFileDialog()
            {
                Filter = "SAS Planet selection file (*.hlg)|*.hlg",
                InitialDirectory = Path.GetDirectoryName(SASPlanetPath) ?? string.Empty,
                FileName = Path.GetFileName(SASPlanetPath) ?? string.Empty,
                RestoreDirectory = true
            };

            SelectSASPlanetCommand = new DelegateCommand(SelectSASPlanet, () => true);
            CopyCoordinatesCommand = new DelegateCommand(CopyFromSASPlanet, CanCopyFromSASPlanet);
            CreateGridCommand = new DelegateCommand(CreateGrid, CanCreateGrid);

            if (CopyCoordinatesCommand.CanExecute(null))
            {
                CopyCoordinatesCommand.Execute(null);
            }
        }

        public ICommand SelectSASPlanetCommand { get; }
        public ICommand CopyCoordinatesCommand { get; }
        public ICommand CreateGridCommand { get; }

        private bool IsValid()
        {
            var leftTopLatValid = LeftTopLat is <= 180 and >= -180;
            var leftTopLonValid = LeftTopLon is <= 180 and >= -180;
            var rightBottomLatValid = RightBottomLat is <= 180 and >= -180;
            var rightBottomLonValid = RightBottomLon is <= 180 and >= -180;
            return leftTopLatValid && leftTopLonValid && rightBottomLatValid && rightBottomLonValid;
        }

        private bool CanCopyFromSASPlanet()
        {
            return File.Exists(SASPlanetPath);
        }

        private bool CanCreateGrid()
        {
            if (!IsValid())
            {
                return false;
            }

            if (LeftTopLat == 0 && LeftTopLon == 0 && RightBottomLat == 0 && RightBottomLon == 0)
            {
                return false;
            }

            return SquareSize > 0;
        }

        private void SelectSASPlanet()
        {
            if (openFileDialog.ShowDialog() == true)
            {
                SASPlanetPath = openFileDialog.FileName;

                SaveSettings();
            }
        }

        private void CopyFromSASPlanet()
        {
            if (!File.Exists(SASPlanetPath))
            {
                return;
            }

            var points = lastSelectionProvider.GetLastSelection(SASPlanetPath);
            LeftTopLat = points[0].Lat;
            LeftTopLon = points[0].Lon;
            RightBottomLat = points[1].Lat;
            RightBottomLon = points[1].Lon;
        }

        private void CreateGrid()
        {
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(LastSavePath) ?? string.Empty;
            saveFileDialog.FileName = $"Grid_{squareSize}_{DateTime.Now:yyyy-MM-dd}";

            if (saveFileDialog.ShowDialog() == true)
            {
                var grid = createGridProvider.CreateGrid(LeftTopLat, LeftTopLon, RightBottomLat, RightBottomLon, SquareSize);
                var gpx = convertGridToGpxProvider.ConvertToGpx(grid);
                saveGpxProvider.Save(gpx, saveFileDialog.FileName);

                SaveSettings();
            }
        }

        private void LoadSettings()
        {
            SASPlanetPath = settingsProvider.SASPlanetParameters.LastSelectionPath;
            LastSavePath = settingsProvider.LastCreateParameters.LastSavePath;
            SquareSize = settingsProvider.LastCreateParameters.SquareSize == 0 ? 500 : settingsProvider.LastCreateParameters.SquareSize;
        }

        private void SaveSettings()
        {
            settingsProvider.SASPlanetParameters.LastSelectionPath = SASPlanetPath;
            settingsProvider.LastCreateParameters.LastSavePath = LastSavePath;
            settingsProvider.LastCreateParameters.SquareSize = SquareSize;
            settingsProvider.Save();
        }
        
        public string CreatorName => $"{Assembly.GetExecutingAssembly().GetName().Name} v{Assembly.GetExecutingAssembly().GetName().Version?.Major}.{Assembly.GetExecutingAssembly().GetName().Version?.Minor}";

        private string sasPlanetPath;
        public string SASPlanetPath
        {
            get => sasPlanetPath;
            set
            {
                sasPlanetPath = value;
                RaisePropertyChanged(() => SASPlanetPath);
            }
        }

        private string lastSavePath;
        public string LastSavePath
        {
            get => lastSavePath;
            set
            {
                lastSavePath = value;
                RaisePropertyChanged(() => LastSavePath);
            }
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
