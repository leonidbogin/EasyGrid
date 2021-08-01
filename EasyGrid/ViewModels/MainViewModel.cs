using DevExpress.Mvvm;
using EasyGrid.Commands;
using EasyGrid.Core.Models;
using EasyGrid.Core.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace EasyGrid.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly SASLastSelectionProvider lastSelectionProvider;
        private readonly CreateGridProvider createGridProvider;
        private readonly ConvertGridToGpxProvider convertGridToGpxProvider;
        private readonly SaveGpxProvider saveGpxProvider;

        public MainViewModel()
        {
            squareSizes = new[] { 500, 250, 200, 100 };
            squareSize = squareSizes.First();

            lastSelectionProvider = new SASLastSelectionProvider();
            SetSelection(lastSelectionProvider.GetLastSelection());

            createGridProvider = new CreateGridProvider();
            convertGridToGpxProvider = new ConvertGridToGpxProvider();
            saveGpxProvider = new SaveGpxProvider();
        }

        public ICommand CreateGridCommand
        {
            get
            {
                return new CreateGridCommand((obj) =>
                {
                    var grid = createGridProvider.CreateGrid(leftTopLat, leftTopLon, rightBottomLat, rightBottomLon, squareSize);
                    var gpx = convertGridToGpxProvider.ConvertToGpx(grid);
                    saveGpxProvider.Save(gpx, "grid.gpx");
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
