using ReactiveUI;
using System.Reactive;

namespace SyntheticUI.ViewModels;

public class AugmentationSelectionViewModel : ReactiveObject
{
    private bool _basicAugmentation;
    public bool BasicAugmentation
    {
        get => _basicAugmentation;
        set => this.RaiseAndSetIfChanged(ref _basicAugmentation, value);
    }

    private bool _noisyAugmentation;
    public bool NoisyAugmentation
    {
        get => _noisyAugmentation;
        set => this.RaiseAndSetIfChanged(ref _noisyAugmentation, value);
    }

    private bool _geometricColorAugmentation;
    public bool GeometricColorAugmentation
    {
        get => _geometricColorAugmentation;
        set => this.RaiseAndSetIfChanged(ref _geometricColorAugmentation, value);
    }

    private bool _adaptiveGeometricColorAugmentation;
    public bool AdaptiveGeometricColorAugmentation
    {
        get => _adaptiveGeometricColorAugmentation;
        set => this.RaiseAndSetIfChanged(ref _adaptiveGeometricColorAugmentation, value);
    }

    public ReactiveCommand<Unit, Unit> ConfirmCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public AugmentationSelectionViewModel()
    {
        ConfirmCommand = ReactiveCommand.Create(() => { });
        CancelCommand = ReactiveCommand.Create(() => { });
    }
}
