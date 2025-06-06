using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SyntheticUI.ViewModels;
using ReactiveUI;

namespace SyntheticUI;

public partial class TestingDetectorWindow : ReactiveUserControl<TestingDetectorViewModel>
{
    public TestingDetectorWindow()
    {
		this.WhenActivated(disposables => { });
		AvaloniaXamlLoader.Load(this);
	}
}