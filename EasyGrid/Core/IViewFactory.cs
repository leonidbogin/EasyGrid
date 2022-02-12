using System.Windows;

namespace EasyGrid.Core
{
    internal interface IViewFactory
    {
        FrameworkElement ResolveView(object viewModel);
    }
}
