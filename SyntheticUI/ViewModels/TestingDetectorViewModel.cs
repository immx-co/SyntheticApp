using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyntheticUI.ViewModels;

public class TestingDetectorViewModel : ReactiveObject, IRoutableViewModel
{
    IServiceProvider _serviceProvider;

    public Window? Target => App.Current?.CurrentWindow;

    #region Commands
    public ReactiveCommand<Unit, Unit> SelectModelCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadImageCommand { get; }
    public ReactiveCommand<Unit, Unit> SendToWork { get; }
    public ReactiveCommand<Unit, Unit> PreviousImageCommand { get; }
    public ReactiveCommand<Unit, Unit> NextImageCommand { get; }
    #endregion

    #region View Model Settings
    public IScreen HostScreen { get; }

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    #endregion

    #region Private Fields
    private List<string> _imagePaths = new List<string>();
    private int _currentImageIndex = -1;
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

    private string _imageCounter;
    public string ImageCounter
    {
        get => _imageCounter;
        set => this.RaiseAndSetIfChanged(ref _imageCounter, value);
    }

    public bool CanNavigatePrevious => _currentImageIndex > 0;
    public bool CanNavigateNext => _currentImageIndex < _imagePaths.Count - 1;
    #endregion

    public TestingDetectorViewModel(IScreen screen, IServiceProvider serviceProvider)
    {
        HostScreen = screen;
        _serviceProvider = serviceProvider;

        SelectModelCommand = ReactiveCommand.CreateFromTask(SelectModelAsync);
        LoadImageCommand = ReactiveCommand.CreateFromTask(LoadImageFolderAsync);
        SendToWork = ReactiveCommand.CreateFromTask(SendToWorkAsync);
        PreviousImageCommand = ReactiveCommand.Create(PreviousImage, this.WhenAnyValue(x => x.CanNavigatePrevious));
        NextImageCommand = ReactiveCommand.Create(NextImage, this.WhenAnyValue(x => x.CanNavigateNext));
    }

    #region Private Methods
    private async Task SelectModelAsync()
    {
        var result = await Target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
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
            ModelPath = Path.GetFileNameWithoutExtension(path);
        }
    }

    private async Task LoadImageFolderAsync()
    {
        var folder = await Target.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Выберите папку с изображениями",
            AllowMultiple = false
        });

        if (folder.Count == 0) return;
        var selectedFolder = folder[0].Path.LocalPath;

        _imagePaths = Directory.GetFiles(selectedFolder)
            .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg"))
            .ToList();

        if (_imagePaths.Count == 0)
        {
            ImageCounter = "Изображения не найдены";
            return;
        }

        _currentImageIndex = 0;
        await LoadCurrentImage();
        UpdateNavigationStatus();
    }

    private async Task LoadCurrentImage()
    {
        if (_currentImageIndex < 0 || _currentImageIndex >= _imagePaths.Count) return;

        try
        {
            ImagePath = _imagePaths[_currentImageIndex];
            LoadedImage = new Bitmap(ImagePath);
            ImageCounter = $"{_currentImageIndex + 1} / {_imagePaths.Count}";
        }
        catch (Exception ex)
        {
            ImageCounter = $"Ошибка загрузки изображения: {ex.Message}";
        }
    }

    private void PreviousImage()
    {
        if (_currentImageIndex > 0)
        {
            _currentImageIndex--;
            LoadCurrentImage().ConfigureAwait(false);
            UpdateNavigationStatus();
        }
    }

    private void NextImage()
    {
        if (_currentImageIndex < _imagePaths.Count - 1)
        {
            _currentImageIndex++;
            LoadCurrentImage().ConfigureAwait(false);
            UpdateNavigationStatus();
        }
    }

    private void UpdateNavigationStatus()
    {
        this.RaisePropertyChanged(nameof(CanNavigatePrevious));
        this.RaisePropertyChanged(nameof(CanNavigateNext));
    }

    private async Task SendToWorkAsync()
    {
        if (string.IsNullOrEmpty(ModelPath))
        {
            return;
        }

        // Очищаем текущие данные
        _imagePaths.Clear();
        _currentImageIndex = -1;
        LoadedImage = null;
        ImagePath = string.Empty;
        ImageCounter = string.Empty;

        try
        {
            string imagesFolderPath;

            if (ModelPath == "yolo_base")
            {
                imagesFolderPath = @"E:\DIPLOM\detection\helpers\real";
            }
            else if (ModelPath == "yolo_synth_augment")
            {
                imagesFolderPath = @"E:\DIPLOM\detection\helpers\synthetic";
            }
            else
            {
                return;
            }

            _imagePaths = Directory.GetFiles(imagesFolderPath)
                .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                              file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                              file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (_imagePaths.Count == 0)
            {
                ImageCounter = "Изображения не найдены";
                return;
            }

            _currentImageIndex = 0;
            await LoadCurrentImage();
            UpdateNavigationStatus();
        }
        catch (Exception ex)
        {
            ImageCounter = $"Ошибка: {ex.Message}";
        }
    }
    #endregion
}
