using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SyntheticUI.ViewModels;
using ReactiveUI;

namespace SyntheticUI;

public partial class TrainingClassificatorWindow : ReactiveUserControl<TrainingClassificatorViewModel>
{
    public TrainingClassificatorWindow()
    {
		this.WhenActivated(disposables => { });
		AvaloniaXamlLoader.Load(this);
	}
}