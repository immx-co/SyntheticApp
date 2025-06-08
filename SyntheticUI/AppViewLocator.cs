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
            TrainingClassificatorViewModel context => new TrainingClassificatorWindow { ViewModel = context },
            TrainingDetectorViewModel context => new TrainingDetectorWindow { ViewModel = context },
            EvaluateClassificatorViewModel context => new EvaluateClassifierWindow { ViewModel = context },
            EvaluateDetectorViewModel context => new EvaluateDetectorWindow { ViewModel = context },
            TestingClassificatorViewModel context => new TestingClassifierWindow { ViewModel = context },
            TestingDetectorViewModel context => new TestingDetectorWindow { ViewModel = context},
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}
