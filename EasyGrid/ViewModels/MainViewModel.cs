using DevExpress.Mvvm;
using EasyGrid.Commands;
using EasyGrid.Core.Models.Gpx;
using EasyGrid.Core.Providers;
using System.Collections.Generic;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using EasyGrid.Core.Models;

namespace EasyGrid.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly SASLastSelectionProvider lastSelectionProvider;

        public MainViewModel()
        {
            var sizes = new int[] { 500, 250, 200, 100 };
            squareSizes = sizes;
            squareSize = sizes[0];

            lastSelectionProvider = new SASLastSelectionProvider();
            SetSelection(lastSelectionProvider.GetLastSelection());
        }

        public ICommand CreateGridCommand
        {
            get
            {
                return new CreateGridCommand((obj) =>
                {
                    var createGridProvider = new CreateGridProvider(leftTopLat, leftTopLon, rightBottomLat, rightBottomLon, squareSize);
                    var convertToGpxProvider = new ConvertToGpxProvider();

                    var grid = createGridProvider.CreateGrid();
                    var gpx = convertToGpxProvider.ConvertToGpx(grid);

                    var xmlWriterSettings = new XmlWriterSettings { Indent = true };
                    using (var writer = XmlWriter.Create("filepath.gpx", xmlWriterSettings))
                    {
                        var serializer = new XmlSerializer(typeof(Gpx));
                        serializer.Serialize(writer, gpx);
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
