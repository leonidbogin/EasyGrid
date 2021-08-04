using System;
using DevExpress.Mvvm;

namespace EasyGrid.Models
{
    public class ResultModel : ViewModelBase
    {
        private string filePath;
        public string FilePath
        {
            get => filePath;
            set 
            {
                filePath = value;
                RaisePropertyChanged(() => FilePath);
            }
        }

        private double squareSize;
        public double SquareSize
        {
            get => squareSize;
            set
            {
                squareSize = value;
                RaisePropertyChanged(() => SquareSize);
            }
        }

        private double gridWidth;
        public double GridWidth
        {
            get => gridWidth;
            set
            {
                gridWidth = value;
                RaisePropertyChanged(() => GridWidth);
            }
        }

        private double gridHeight;
        public double GridHeight
        {
            get => gridHeight;
            set
            {
                gridHeight = value;
                RaisePropertyChanged(() => GridHeight);
            }
        }

        private int numberSquaresWidth;
        public int NumberSquaresWidth
        {
            get => numberSquaresWidth;
            set
            {
                numberSquaresWidth = value;
                RaisePropertyChanged(() => NumberSquaresWidth);
            }
        }

        private int numberSquaresHeight;
        public int NumberSquaresHeight
        {
            get => numberSquaresHeight;
            set
            {
                numberSquaresHeight = value;
                RaisePropertyChanged(() => NumberSquaresHeight);
            }
        }

        private string fileType;
        public string FileType
        {
            get => fileType;
            set
            {
                fileType = value;
                RaisePropertyChanged(() => FileType);
            }
        }

        private double fileSize;
        public double FileSize
        {
            get => fileSize;
            set
            {
                fileSize = Math.Round(value, 2);
                RaisePropertyChanged(() => FileSize);
            }
        }
    }
}
