using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading;

namespace SyntheticUI.ViewModels;

public class AugmentationDetectorViewModel : ReactiveObject, IRoutableViewModel
{
	IServiceProvider _serviceProvider;

	#region View Model Settings
	public IScreen HostScreen { get; }

	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	public CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
	#endregion

	#region Propetries
	private Bitmap? _currentImage;
	public Bitmap? CurrentImage
	{
		get => _currentImage;
		set => this.RaiseAndSetIfChanged(ref _currentImage, value);
	}
    #endregion

    #region Public Commands
    public ReactiveCommand<Unit, Unit> LoadDatasetCommand { get; }

	public ReactiveCommand<Unit, Unit> AugmentDetectorCommand { get; }
    #endregion

    public AugmentationDetectorViewModel(IScreen screen, IServiceProvider serviceProvider)
	{
		HostScreen = screen;

		_serviceProvider = serviceProvider;

		LoadDatasetCommand = ReactiveCommand.Create(LoadDataset);
		AugmentDetectorCommand = ReactiveCommand.Create(AugmentDetector);
    }

    #region Private Methods
	private async void LoadDataset()
	{
		;
	}

	private async void AugmentDetector()
	{
		;
	}
    #endregion
}
