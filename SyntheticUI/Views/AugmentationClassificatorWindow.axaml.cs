using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SyntheticUI.ViewModels;

namespace SyntheticUI.Views;

public partial class AugmentationClassificatorWindow : ReactiveUserControl<AugmentationClassificatorViewModel>
{
    public AugmentationClassificatorWindow()
    {
		this.WhenActivated(disposables => { });
		AvaloniaXamlLoader.Load(this);
	}
}