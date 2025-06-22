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

    private void DataGrid_AutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if (e.PropertyName == "Name")
            e.Cancel = true;
    }
}