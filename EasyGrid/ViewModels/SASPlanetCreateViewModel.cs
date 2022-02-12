using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EasyGrid.ViewModels
{
    internal class SASPlanetCreateViewModel : ObservableObject
    {
        public IRelayCommand LoadedCommand { get; }

        public SASPlanetCreateViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded);
        }

        private void Loaded()
        {
            
        }
    }
}
