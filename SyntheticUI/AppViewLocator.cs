using ReactiveUI;
using System;

namespace SyntheticUI
{
    public class AppViewLocator : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null) => viewModel switch
        {
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
    }
}
