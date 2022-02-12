using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EasyGrid.ViewModels
{
    internal class CoordinatesCreateViewModel : ObservableObject
    {
        public IRelayCommand LoadedCommand { get; }

        public CoordinatesCreateViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded);
        }

        private void Loaded()
        {
            
        }
    }
}
