using DevExpress.Mvvm;
using EasyGrid.Core.Models;
using System.Collections.Generic;

namespace EasyGrid.Models
{
    public class GridParametersModel : ViewModelBase
    {
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

        public IEnumerable<int> SquareSizes => new[] { 500, 250, 200, 100 };

        public void SetSelection(LastSelection lastSelection)
        {
            LeftTopLat = lastSelection.FirstPoint.Lat;
            LeftTopLon = lastSelection.FirstPoint.Lon;
            RightBottomLat = lastSelection.SecondPoint.Lat;
            RightBottomLon = lastSelection.SecondPoint.Lon;
        }

        public bool IsValid()
        {
            var leftTopLatValid = LeftTopLat is <= 180 and >= -180;
            var leftTopLonValid = LeftTopLon is <= 180 and >= -180;
            var rightBottomLatValid = RightBottomLat is <= 180 and >= -180;
            var rightBottomLonValid = RightBottomLon is <= 180 and >= -180;

            var allZeroValid = !(LeftTopLat == 0 && LeftTopLon == 0 && RightBottomLat == 0 && RightBottomLon == 0);
            var squareSizeAboveZeroValid = SquareSize > 0;
            var pointOrderValid = leftTopLat >= RightBottomLat && leftTopLon <= rightBottomLon;

            return leftTopLatValid && leftTopLonValid && rightBottomLatValid && rightBottomLonValid && allZeroValid && squareSizeAboveZeroValid && pointOrderValid;
        }

        public GridParametersModel Reset()
        {
            LeftTopLat = 0;
            LeftTopLon = 0;
            RightBottomLat = 0;
            rightBottomLon = 0;
            SquareSize = 500;
            return this;
        }
    }
}
