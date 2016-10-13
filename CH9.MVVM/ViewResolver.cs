using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.Integration;
using Caliburn.Micro;
using CaliburnIoC = Caliburn.Micro.IoC;

namespace CH9.MVVM
{
    public class ViewResolver : IViewResolver
    {
        internal static bool UiAutomationIdSupport;

        public static void Initialise()
        {
            ViewLocator.ContextSeparator = string.Empty;

            ViewLocator.NameTransformer.AddRule
                (
                    @"(?<nsbefore>([A-Za-z_]\w*\.)*)?(?<nsvm>ViewModels\.)(?<nsafter>[A-Za-z_]\w*\.)*(?<basename>[A-Za-z_]\w*)(?<suffix>ViewModel$)",
                    @"${nsbefore}Views.PropertyEditors.${nsafter}${basename}View",
                    @"(([A-Za-z_]\w*\.)*)?ViewModels\.([A-Za-z_]\w*\.)*[A-Za-z_]\w*ViewModel$"
                );

            ViewLocator.NameTransformer.AddRule
                (
                    @"(?<nsbefore>([A-Za-z_]\w*\.)*)?(?<nsvm>ViewModels\.)(?<nsafter>[A-Za-z_]\w*\.)*(?<basename>[A-Za-z_]\w*)(?<suffix>ViewModel$)",
                    @"${nsbefore}Views.PropertyEditors.${basename}View",
                    @"(([A-Za-z_]\w*\.)*)?ViewModels\.([A-Za-z_]\w*\.)*[A-Za-z_]\w*ViewModel$"
                );

            UiAutomationIdSupport = true;
        }

        public ViewResolver GetViewResolver => new ViewResolver();

        internal static UIElement LocateForModel(object model, DependencyObject displayLocation, object context)
        {
            return LocateForModelType(model.GetType(), model, context);
        }

        internal static UIElement LocateForModelType(Type modelType, object model, object context)
        {
            string viewTypeName = modelType.FullName;
            if (Execute.InDesignMode)
            {
                viewTypeName = ViewLocator.ModifyModelTypeAtDesignTime(viewTypeName);
            }

            var viewType = ViewLocator.LocateTypeForModelType(modelType, null, context);

            UIElement view;

            if (viewType != null && typeof(System.Windows.Forms.Control).IsAssignableFrom(viewType))
            {
                var localCtrl = Enumerable.FirstOrDefault(CaliburnIoC.GetAllInstances(viewType)) as System.Windows.Forms.Control;
                if (localCtrl == null)
                {
                    localCtrl = (System.Windows.Forms.Control)Activator.CreateInstance(viewType);
                }

                var dockPanel = new DockPanel { LastChildFill = true };

                var host = new WindowsFormsHost
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };


                DockPanel.SetDock(host, Dock.Top);
                host.Child = localCtrl;
                ViewLocator.InitializeComponent(localCtrl);
                dockPanel.Children.Add(host);
                view = dockPanel;
            }
            else
            {

                if (viewType == null)
                {
                    return new TextBlock { Text = $"{viewTypeName} not found."};
                }

                view = ViewLocator.GetOrCreateViewType(viewType);
            }


            return view;

        }
        public UIElement Resolve(object context)
        {
            return LocateForModelType(context.GetType(), context, null);
        }


        public Type ResolveType(object context)
        {
            if (context == null)
                throw new NoNullAllowedException();

            var t = ViewLocator.LocateTypeForModelType(context.GetType(), null, null);

            return t;
        }

        public void Bind(FrameworkElement view, object viewModel)
        {
            view.DataContext = viewModel;
            BindingApplied(viewModel, view);
        }

        public UIElement ResolveAndBind(object context)
        {
            var view = Resolve(context);
            if (view != null && view is FrameworkElement)
            {
                Bind((FrameworkElement)view, context);
            }
            return view;
        }


        public static void Bind(object model, UIElement view, object context)
        {
            ViewModelBinder.Bind(model, view, context);
            BindingApplied(model, view, context);
        }

        private static void BindingApplied(object model, UIElement view, object context = null)
        {
            var value = context ?? model;

            if (string.IsNullOrEmpty(AutomationProperties.GetAutomationId(view)))
            {
                BindingOperations.SetBinding(view, AutomationProperties.AutomationIdProperty, new Binding("Title") { Source = value, Mode = BindingMode.OneWay });
            }
        }
    }
}
