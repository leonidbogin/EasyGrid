using DevExpress.Mvvm;
using System.Windows;

namespace EasyGrid.Models
{
    public class ProgressBarModel : ViewModelBase
    {
        private double progress;
        public double Progress
        {
            get => progress;
            set
            {
                progress = value;

                var indeterminate = progress == 0;
                if (IsIndeterminate != indeterminate)
                {
                    IsIndeterminate = indeterminate;
                }

                RaisePropertyChanged(() => Progress);
            }
        }

        private Visibility visibility = Visibility.Hidden;
        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                RaisePropertyChanged(() => Visibility);
            }
        }

        private bool isIndeterminate;
        public bool IsIndeterminate
        {
            get => isIndeterminate;
            set
            {
                isIndeterminate = value;
                RaisePropertyChanged(() => IsIndeterminate);
            }
        }

        private string status;
        public string Status
        {
            get => status;
            set
            {
                status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        public ProgressBarModel Show()
        {
            Visibility = Visibility.Visible;
            return this;
        }

        public ProgressBarModel Hide()
        {
            Visibility = Visibility.Hidden;
            return this;
        }

        public ProgressBarModel Reset()
        {
            Progress = 0;
            IsIndeterminate = false;
            Status = string.Empty;
            return this;
        }
    }
}
