using System.Windows;
using System.Windows.Input;

namespace EasyGrid.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState != WindowState.Maximized && e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private bool _restoreIfMove = false;
        private void Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                WindowState = WindowState switch
                {
                    WindowState.Normal => WindowState.Maximized,
                    WindowState.Maximized => WindowState.Normal,
                    _ => WindowState
                };
            } 
            else if (WindowState == WindowState.Maximized)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    _restoreIfMove = true;
                    DragMove();
                }
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _restoreIfMove = false;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (_restoreIfMove)
            {
                _restoreIfMove = false;
                var point = PointToScreen(e.MouseDevice.GetPosition(this));
                Left = point.X - (RestoreBounds.Width * 0.5);
                Top = point.Y;
                WindowState = WindowState.Normal;
                DragMove();
            }
        }
    }
}
