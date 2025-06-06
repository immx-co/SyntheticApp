using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SyntheticUI.ViewModels;

namespace SyntheticUI;

public partial class TestingClassifierWindow : ReactiveUserControl<TestingClassificatorViewModel>
{
    public TestingClassifierWindow()
    {
		this.WhenActivated(disposables => { });
		AvaloniaXamlLoader.Load(this);
	}
}