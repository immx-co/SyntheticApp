using ReactiveUI;
using System;
using SyntheticUI.ViewModels;
using SyntheticUI.Views;

namespace SyntheticUI
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
        {
            AugmentationClassificatorViewModel context  => new AugmentationClassificatorWindow { ViewModel = context },
            AugmentationDetectorViewModel context => new AugmentationDetectorWindow { ViewModel = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}
