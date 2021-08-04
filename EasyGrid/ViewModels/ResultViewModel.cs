using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using EasyGrid.Models;

namespace EasyGrid.ViewModels
{
    public class ResultViewModel : ViewModelBase
    {
        public static string WindowTitle => $"Grid ready";
        public ResultModel Result { get; set; }

        public ICommand OpenAndExitCommand { get; }
        public ICommand ExitCommand { get; }

        public ResultViewModel(ResultModel result = null)
        {
            Result = result ?? new ResultModel();
            OpenAndExitCommand = new DelegateCommand(OpenAndExit);
            ExitCommand = new DelegateCommand(Exit);
        }


        private void OpenAndExit()
        {
            if (File.Exists(Result.FilePath))
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = Path.GetFullPath(Path.GetDirectoryName(Result.FilePath) ?? string.Empty),
                    UseShellExecute = true,
                    Verb = "open"
                });
            }

            Exit();
        }

        private static void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
