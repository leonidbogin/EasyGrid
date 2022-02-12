using System;
using System.Linq;
using System.Windows;

namespace EasyGrid.Core
{
    internal class MappingViewFactory : IViewFactory
    {
        public FrameworkElement ResolveView(object viewModel)
        {
            var vmName = viewModel.GetType().Name;
            var viewName = vmName.Contains("Page") ? vmName.Replace("PageViewModel", "View") : vmName.Replace("ViewModel", "View");
            var viewType = typeof(App).Assembly.DefinedTypes.FirstOrDefault(x => x.Name == viewName);
            if (viewType == null) return null;

            var view = Activator.CreateInstance(viewType);
            return (FrameworkElement)view;
        }
    }
}
