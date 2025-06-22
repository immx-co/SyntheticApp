using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
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

    #region Private Fields
    public Window? Target => App.Current?.CurrentWindow;

    private Bitmap? _currentImage;

    private string _detectorDatasetPath;
    #endregion

    #region Propetries
    public Bitmap? CurrentImage
	{
		get => _currentImage;
		set => this.RaiseAndSetIfChanged(ref _currentImage, value);
	}

    public string DetectorDatasetPath
    {
        get => _detectorDatasetPath;
        set => this.RaiseAndSetIfChanged(ref _detectorDatasetPath, value);
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
        var folder = await Target.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Выберите папку с датасетом детектора",
            AllowMultiple = false,
            SuggestedStartLocation = await Target.StorageProvider.TryGetFolderFromPathAsync("C:\\Users\\immx\\official\\PicsForDiplom")
        });


        if (folder.Count > 0 && folder[0].TryGetLocalPath() is string path)
        {
            DetectorDatasetPath = path;
        }
    }

	private async void AugmentDetector()
	{
		;
	}
    #endregion
}
