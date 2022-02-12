using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EasyGrid.ViewModels
{
    internal class SettingsViewModel : ObservableObject
    {
        public IRelayCommand LoadedCommand { get; }

        public SettingsViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded);
        }

        private void Loaded()
        {

        }
    }
}
