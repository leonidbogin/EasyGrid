using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
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
            set
            {
                SetProperty(ref _selectedViewModel, value);
                RefreshMenuCommands();
            }
        }

        public ICommand ShowCoordinatesCreateCommand { get; }
        public ICommand ShowSASPlanetCreateCommand { get; }

        public ICommand CloseWindowCommand { get; }
        public ICommand MinimazeWindowCommand { get; }
        public ICommand MaximazeWindowCommand { get; }

        private readonly List<RelayCommand> _menuCommands = new List<RelayCommand>();

        public MainViewModel()
        {
            ShowCoordinatesCreateCommand = CreateMenuCommand<CoordinatesCreateViewModel>();
            ShowSASPlanetCreateCommand = CreateMenuCommand<SASPlanetCreateViewModel>();
            CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
            MinimazeWindowCommand = new RelayCommand<Window>(MinimazeWindow);
            MaximazeWindowCommand = new RelayCommand<Window>(MaximazeWindow);

            SelectedViewModel = Ioc.Default.GetRequiredService<CoordinatesCreateViewModel>();
        }

        private ICommand CreateMenuCommand<T>() where T : ObservableObject
        {
            var command = new RelayCommand(() => SelectedViewModel = Ioc.Default.GetRequiredService<T>(), () => !(SelectedViewModel is T));
            _menuCommands.Add(command);
            return command;
        }

        private void RefreshMenuCommands()
        {
            foreach (var command in _menuCommands)
            {
                command.NotifyCanExecuteChanged();
            }
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

        private static void MaximazeWindow(Window window)
        {
            if (window != null)
            {
                window.WindowState = window.WindowState switch
                {
                    WindowState.Normal => WindowState.Maximized,
                    WindowState.Maximized => WindowState.Normal,
                    _ => window.WindowState
                };
            }
        }
    }
}
