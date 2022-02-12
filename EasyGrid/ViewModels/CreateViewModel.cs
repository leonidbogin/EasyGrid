using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EasyGrid.ViewModels
{
    internal class CreateViewModel : ObservableObject
    {
        public IRelayCommand LoadedCommand { get; }

        public CreateViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded);
        }

        private void Loaded()
        {
            
        }
    }
}
