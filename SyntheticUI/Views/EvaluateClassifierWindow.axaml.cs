using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SyntheticUI.ViewModels;

namespace SyntheticUI;

public partial class EvaluateClassifierWindow : ReactiveUserControl<EvaluateClassificatorViewModel>
{
    public EvaluateClassifierWindow()
    {
		this.WhenActivated(disposables => { });
		AvaloniaXamlLoader.Load(this);
	}
}