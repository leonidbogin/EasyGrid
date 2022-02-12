using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;

namespace EasyGrid.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        private ObservableObject _selectedViewModel;
        public ObservableObject SelectedViewModel
        {
            get => _selectedViewModel;
            set => SetProperty(ref _selectedViewModel, value);
        }

        public ICommand ShowCreateGridViewModel { get; }
        public ICommand ShowSettingsViewModel { get; }

        public ICommand CloseWindowCommand { get; }
        public ICommand MinimazeWindowCommand { get; }

        public MainViewModel()
        {
            ShowCreateGridViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<CreateViewModel>());
            ShowSettingsViewModel = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<SettingsViewModel>());

            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
            MinimazeWindowCommand = new RelayCommand<Window>(MinimazeWindow);

            SelectedViewModel = Ioc.Default.GetRequiredService<CreateViewModel>();
        }

        private static void MinimazeWindow(Window window)
        {
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private static void CloseWindow(Window window)
        {
            window?.Close();
        }
    }
}
