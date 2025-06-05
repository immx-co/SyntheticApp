using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SyntheticUI.ViewModels;

namespace SyntheticUI.Views;

public partial class AugmentationDetectorWindow : ReactiveUserControl<AugmentationDetectorViewModel>
{
    public AugmentationDetectorWindow()
    {
		this.WhenActivated(disposables => { });
		AvaloniaXamlLoader.Load(this);
	}
}