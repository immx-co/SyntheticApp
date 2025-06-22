using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace SyntheticUI.ViewModels;

public class TestingClassificatorViewModel : ReactiveObject, IRoutableViewModel
	{
		IServiceProvider _serviceProvider;

    public Window? Target => App.Current?.CurrentWindow;

    #region View Model Settings
    public IScreen HostScreen { get; }

		public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

		public CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    #endregion

    #region Commands
    public ReactiveCommand<Unit, Unit> SelectModelCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadImageCommand { get; }
    public ReactiveCommand<Unit, Unit> SendToWork { get; }
    #endregion

    #region Properties
    private string _modelPath;
    public string ModelPath
    {
        get => _modelPath;
        set => this.RaiseAndSetIfChanged(ref _modelPath, value);
    }

    private string _imagePath;
    public string ImagePath
    {
        get => _imagePath;
        set => this.RaiseAndSetIfChanged(ref _imagePath, value);
    }

    private Bitmap _loadedImage;
    public Bitmap LoadedImage
    {
        get => _loadedImage;
        set => this.RaiseAndSetIfChanged(ref _loadedImage, value);
    }

    #endregion
    public TestingClassificatorViewModel(IScreen screen, IServiceProvider serviceProvider)
		{
			HostScreen = screen;

			_serviceProvider = serviceProvider;

        SelectModelCommand = ReactiveCommand.CreateFromTask(SelectModelAsync);
        LoadImageCommand = ReactiveCommand.CreateFromTask(LoadImageAsync);
        SendToWork = ReactiveCommand.CreateFromTask(SendToWorkAsync);
    }

    #region Private Methods
    private async Task SelectModelAsync()
    {
        var result = await Target.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
        {
            Title = "Выберите модель",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Model Files")
                {
                    Patterns = new[] { "*.onnx", "*.pt", "*.h5" }
                }
            }
        });

        if (result.Count > 0 && result[0].TryGetLocalPath() is string path)
        {
            ModelPath = path;
        }
    }

    private async Task LoadImageAsync()
    {
        var result = await Target.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
        {
            Title = "Выберите изображение",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Image Files")
                {
                    Patterns = new[] { "*.png", "*.jpg", "*.jpeg" }
                }
            }
        });

        if (result.Count > 0 && result[0].TryGetLocalPath() is string path)
        {
            ImagePath = path;
            try
            {
                LoadedImage = new Bitmap(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
            }
        }
    }
    
    private async Task SendToWorkAsync()
    {
        ;
    }
    #endregion
}
