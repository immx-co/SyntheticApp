using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using SyntheticUI.ViewModels;
using SyntheticUI.Views;
using System.Linq;

namespace SyntheticUI
{
    public partial class App : Application
    {
        public new static App? Current => Application.Current as App;

        public Window? CurrentWindow
        {
            get
            {
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    return desktop.MainWindow;
                }
                else return null;
            }
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();

                IServiceCollection servicesCollection = new ServiceCollection();

                servicesCollection.AddSingleton<IScreen, IScreenRealization>();

                servicesCollection.AddSingleton<NavigationViewModel>();
                servicesCollection.AddSingleton<AugmentationClassificatorViewModel>();
                servicesCollection.AddSingleton<AugmentationDetectorViewModel>();
                servicesCollection.AddSingleton<TrainingClassificatorViewModel>();
                servicesCollection.AddSingleton<TrainingDetectorViewModel>();
                servicesCollection.AddSingleton<EvaluateClassificatorViewModel>();
                servicesCollection.AddSingleton<EvaluateDetectorViewModel>();
                servicesCollection.AddSingleton<TestingDetectorViewModel>();
                servicesCollection.AddSingleton<TestingClassificatorViewModel>();

                ServiceProvider servicesProvider = servicesCollection.BuildServiceProvider();

                desktop.MainWindow = new NavigationWindow
                {
                    DataContext = servicesProvider.GetRequiredService<NavigationViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}