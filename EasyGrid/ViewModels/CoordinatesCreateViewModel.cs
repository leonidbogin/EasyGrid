using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace EasyGrid.ViewModels
{
    internal class CoordinatesCreateViewModel : ObservableObject
    {
        private double _point1Lat;
        public double Point1Lat
        {
            get => _point1Lat;
            set => SetProperty(ref _point1Lat, value);
        }

        private double _point1Lon;
        public double Point1Lon
        {
            get => _point1Lon;
            set => SetProperty(ref _point1Lon, value);
        }

        private double _point9Lat;
        public double Point9Lat
        {
            get => _point9Lat;
            set => SetProperty(ref _point9Lat, value);
        }

        private double _point9Lon;
        public double Point9Lon
        {
            get => _point9Lon;
            set => SetProperty(ref _point9Lon, value);
        }

        private int _squareSize;
        public int SquareSize
        {
            get => _squareSize;
            set => SetProperty(ref _squareSize, value);
        }

        public ICommand CreateCommand { get; }

        public CoordinatesCreateViewModel()
        {
            CreateCommand = new RelayCommand(Create);
        }

        private static void Create()
        {
            throw new System.NotImplementedException();
        }
    }
}
