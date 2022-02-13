using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace EasyGrid.ViewModels
{
    internal class CoordinatesCreateViewModel : ObservableObject
    {
        private int _point1Lat;
        public int Point1Lat
        {
            get => _point1Lat;
            set => SetProperty(ref _point1Lat, value);
        }

        private int _point1Lon;
        public int Point1Lon
        {
            get => _point1Lon;
            set => SetProperty(ref _point1Lon, value);
        }

        private int _point9Lat;
        public int Point9Lat
        {
            get => _point9Lat;
            set => SetProperty(ref _point9Lat, value);
        }

        private int _point9Lon;
        public int Point9Lon
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
