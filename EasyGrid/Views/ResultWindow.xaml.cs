using System.ComponentModel;
using System.Windows;
using EasyGrid.Models;
using EasyGrid.ViewModels;

namespace EasyGrid.Views
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow(Window owner)
        {
            InitializeComponent();
            this.Owner = owner;
        }

        public void SetData(ResultModel result)
        {
            DataContext = new ResultViewModel(result);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
