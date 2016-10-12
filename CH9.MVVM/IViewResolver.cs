using System;
using System.Windows;

namespace CH9.MVVM
{
    public interface IViewResolver
    {
        UIElement Resolve(object context);
        Type ResolveType(object context);
        void Bind(FrameworkElement view, object viewModel);
        UIElement ResolveAndBind(object context);
    }
}