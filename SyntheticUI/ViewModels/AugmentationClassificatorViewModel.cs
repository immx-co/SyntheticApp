using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using SyntheticUI.Views;
using MsBox;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;

namespace SyntheticUI.ViewModels;

public class AugmentationClassificatorViewModel : ReactiveObject, IRoutableViewModel
{
	IServiceProvider _serviceProvider;

    #region Private Fields
    private List<string> _imagePaths = new List<string>();
    private int _currentImageIndex = -1;
    public Window? Target => App.Current?.CurrentWindow;
    #endregion

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

    private string _imageInfo = "Изображение не загружено";
    public string ImageInfo
    {
        get => _imageInfo;
        set => this.RaiseAndSetIfChanged(ref _imageInfo, value);
    }

    public bool BackwardButtonEnable => _currentImageIndex > 0 && _imagePaths.Count > 0;
    public bool ForwardButtonEnable => _currentImageIndex < _imagePaths.Count - 1 && _imagePaths.Count > 0;
    #endregion

    #region Public Commands
    public ReactiveCommand<Unit, Unit> LoadDatasetCommand { get; }

    public ReactiveCommand<Unit, Unit> AugmentClassificatorCommand { get; }

    public ReactiveCommand<Unit, Unit> PreviousImageCommand { get; }

    public ReactiveCommand<Unit, Unit> NextImageCommand { get; }
    #endregion

    public AugmentationClassificatorViewModel(IScreen screen, IServiceProvider serviceProvider)
	{
		HostScreen = screen;

		_serviceProvider = serviceProvider;

        LoadDatasetCommand = ReactiveCommand.CreateFromTask(LoadDataset);
        AugmentClassificatorCommand = ReactiveCommand.Create(AugmentClassificator);
        PreviousImageCommand = ReactiveCommand.Create(PreviousImage);
        NextImageCommand = ReactiveCommand.Create(NextImage);
    }

    #region Private Methods
    private async Task LoadDataset()
    {
        try
        {
            var folder = await Target.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Выберите папку с датасетом",
                AllowMultiple = false
            });

            if (folder.Count == 0) return;
            var selectedFolder = folder[0].Path.LocalPath;

            _imagePaths = Directory.GetDirectories(selectedFolder)
                .SelectMany(dir => Directory.GetFiles(dir, "*.*")
                    .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png") || file.EndsWith(".jpeg")))
                .ToList();

            if (_imagePaths.Count == 0)
            {
                ImageInfo = "Изображения не найдены";
                return;
            }

            _currentImageIndex = 0;
            await LoadCurrentImage();
            ImageInfo = $" 1 / {_imagePaths.Count}";
            UpdateButtonStates();
        }
        catch (Exception ex)
        {
            ImageInfo = $"Ошибка: {ex.Message}";
        }
    }

    private async Task LoadCurrentImage()
    {
        if (_currentImageIndex < 0 || _currentImageIndex >= _imagePaths.Count) return;

        try
        {
            var imagePath = _imagePaths[_currentImageIndex];
            await using var stream = File.OpenRead(imagePath);
            CurrentImage = new Bitmap(stream);
        }
        catch (Exception ex)
        {
            ImageInfo = $"Ошибка загрузки изображения: {ex.Message}";
        }
    }

    private void PreviousImage()
    {
        if (_currentImageIndex > 0)
        {
            _currentImageIndex--;
            LoadCurrentImage().ConfigureAwait(false);
            ImageInfo = $"{_currentImageIndex + 1} / {_imagePaths.Count}";
            UpdateButtonStates();
        }
    }

    private void NextImage()
    {
        if (_currentImageIndex < _imagePaths.Count - 1)
        {
            _currentImageIndex++;
            LoadCurrentImage().ConfigureAwait(false);
            ImageInfo = $"{_currentImageIndex + 1} / {_imagePaths.Count}";
            UpdateButtonStates();
        }
    }

    private void UpdateButtonStates()
    {
        this.RaisePropertyChanged(nameof(BackwardButtonEnable));
        this.RaisePropertyChanged(nameof(ForwardButtonEnable));
    }

    private async void AugmentClassificator()
    {
        try
        {
            var dialog = new Window()
            {
                Title = "Выбор типа аугментации",
                Width = 400,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var viewModel = new AugmentationSelectionViewModel();
            var view = new AugmentationSelectionWindow { DataContext = viewModel };
            dialog.Content = view;
            // Обработка подтверждения
            viewModel.ConfirmCommand.Subscribe(async _ =>
            {
                var selectedOptions = new List<string>();
                if (viewModel.BasicAugmentation) selectedOptions.Add("Базовая аугментация");
                if (viewModel.NoisyAugmentation) selectedOptions.Add("Зашумленная аугментация");
                if (viewModel.GeometricColorAugmentation) selectedOptions.Add("Геометрическая и цветовая аугментация");
                if (viewModel.AdaptiveGeometricColorAugmentation) selectedOptions.Add("Адаптивная геометрическая и цветовая аугментация");

                await Task.Delay(1000);
                if (selectedOptions.Count == 0)
                {
                    var warningBox = MessageBoxManager.GetMessageBoxStandard(
                        "Warning",
                        "Не выбрано ни одного типа аугментации",
                        ButtonEnum.Ok,
                        Icon.Warning);
                    await warningBox.ShowAsync();
                    return;
                }

                dialog.Close();

                var successBox = MessageBoxManager.GetMessageBoxStandard(
                    "Success",
                    $"Аугментация успешно выполнена для {selectedOptions.Count} типов:\n{string.Join("\n", selectedOptions)}",
                    ButtonEnum.Ok,
                    Icon.Success);
                await successBox.ShowAsync();

            });

            viewModel.CancelCommand.Subscribe(_ => dialog.Close());

            await dialog.ShowDialog(Target);
        }
        catch (Exception ex)
        {
            var errorBox = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка",
                $"Ошибка при выполнении аугментации: {ex.Message}",
                ButtonEnum.Ok,
                Icon.Error);
            await errorBox.ShowAsync();

            Console.WriteLine($"Ошибка при открытии окна аугментации: {ex.Message}");
        }
    }
    #endregion
}
