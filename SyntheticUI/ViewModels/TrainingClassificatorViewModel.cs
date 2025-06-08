using ReactiveUI;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Controls;
using System.IO;

namespace SyntheticUI.ViewModels;

public class TrainingClassificatorViewModel : ReactiveObject, IRoutableViewModel
{
	IServiceProvider _serviceProvider;

	#region View Model Settings
	public IScreen HostScreen { get; }

	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	public CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    #endregion

    #region Private Fields
    private int _epochs = 10;
    private int _batchSize = 32;
    private string _learningRate = "0.001";
    private string _imageSize = "224";
    private bool _useAugmentation = true;
    private bool _saveBestWeights = true;
    private bool _useEarlyStopping = true;
    private int _earlyStoppingPatience = 10;
    private double _trainingProgress;
    private string _trainingStatus = "Готов к обучению";
    private string _trainingLog = string.Empty;
    private bool _isTrainingIndeterminate;
    private bool _isTraining;
    private string _selectedModel;
    private bool _canStartTraining;
    private bool _canStopTraining;
    private string _weightsOutputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ModelWeights");
    public Window? Target => App.Current?.CurrentWindow;
    #endregion

    #region Public Properties
    public List<string> AvailableModels { get; } = new List<string> { "EfficientNet", "MixNet", "ResNet" };

    public string SelectedModel
    {
        get => _selectedModel;
        set => this.RaiseAndSetIfChanged(ref _selectedModel, value);
    }

    public int Epochs
    {
        get => _epochs;
        set => this.RaiseAndSetIfChanged(ref _epochs, value);
    }

    public int BatchSize
    {
        get => _batchSize;
        set => this.RaiseAndSetIfChanged(ref _batchSize, value);
    }

    public string LearningRate
    {
        get => _learningRate;
        set => this.RaiseAndSetIfChanged(ref _learningRate, value);
    }

    public string ImageSize
    {
        get => _imageSize;
        set => this.RaiseAndSetIfChanged(ref _imageSize, value);
    }

    public bool UseAugmentation
    {
        get => _useAugmentation;
        set => this.RaiseAndSetIfChanged(ref _useAugmentation, value);
    }

    public bool SaveBestWeights
    {
        get => _saveBestWeights;
        set => this.RaiseAndSetIfChanged(ref _saveBestWeights, value);
    }

    public bool UseEarlyStopping
    {
        get => _useEarlyStopping;
        set => this.RaiseAndSetIfChanged(ref _useEarlyStopping, value);
    }

    public int EarlyStoppingPatience
    {
        get => _earlyStoppingPatience;
        set => this.RaiseAndSetIfChanged(ref _earlyStoppingPatience, value);
    }

    public double TrainingProgress
    {
        get => _trainingProgress;
        set => this.RaiseAndSetIfChanged(ref _trainingProgress, value);
    }

    public string TrainingStatus
    {
        get => _trainingStatus;
        set => this.RaiseAndSetIfChanged(ref _trainingStatus, value);
    }

    public string TrainingLog
    {
        get => _trainingLog;
        set => this.RaiseAndSetIfChanged(ref _trainingLog, value);
    }

    public bool IsTrainingIndeterminate
    {
        get => _isTrainingIndeterminate;
        set => this.RaiseAndSetIfChanged(ref _isTrainingIndeterminate, value);
    }

    public bool CanStartTraining
    {
        get => _canStartTraining;
        set => this.RaiseAndSetIfChanged(ref _canStartTraining, value);
    }
    public bool CanStopTraining
    {
        get => _canStopTraining;
        set => this.RaiseAndSetIfChanged(ref _canStopTraining, value);
    }

    public string WeightsOutputPath
    {
        get => _weightsOutputPath;
        set => this.RaiseAndSetIfChanged(ref _weightsOutputPath, value);
    }
    #endregion

    #region Commands
    public ReactiveCommand<Unit, Unit> StartTrainingCommand { get; }

    public ReactiveCommand<Unit, Unit> StopTrainingCommand { get; }

    public ReactiveCommand<Unit, Unit> SelectOutputFolderCommand { get; }
    #endregion

    public TrainingClassificatorViewModel(IScreen screen, IServiceProvider serviceProvider)
	{
		HostScreen = screen;

		_serviceProvider = serviceProvider;

        StartTrainingCommand = ReactiveCommand.CreateFromTask(StartTrainingAsync);
        StopTrainingCommand = ReactiveCommand.Create(StopTraining);
        SelectOutputFolderCommand = ReactiveCommand.CreateFromTask(SelectOutputFolderAsync);

        CanStartTraining = true;
        CanStopTraining = false;
        SelectedModel = AvailableModels.First();
    }

    #region Methods
    private async Task StartTrainingAsync()
    {
        _isTraining = true;
        CanStartTraining = false;
        CanStopTraining = true;
        this.RaisePropertyChanged(nameof(CanStartTraining));
        this.RaisePropertyChanged(nameof(CanStopTraining));
        _cancellationTokenSource = new CancellationTokenSource();

        TrainingStatus = "Обучение классификатора начато...";
        TrainingLog += $"[{DateTime.Now}] Начато обучение модели {SelectedModel}\n";
        IsTrainingIndeterminate = true;

        await Task.Delay(10000);

        try
        {
            for (int epoch = 1; epoch <= Epochs; epoch++)
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                    break;

                await Task.Delay(500);
                TrainingProgress = (double)epoch / Epochs * 100;
                IsTrainingIndeterminate = false;

                TrainingLog += $"[{DateTime.Now}] Эпоха {epoch}/{Epochs} завершена\n";
                TrainingStatus = $"Обработана эпоха {epoch}/{Epochs}";
            }

            if (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                TrainingStatus = "Обучение классификатора завершено успешно!";
                TrainingLog += $"[{DateTime.Now}] Обучение завершено успешно!\n";
            }
        }
        catch (Exception ex)
        {
            TrainingStatus = "Ошибка при обучении классификатора";
            TrainingLog += $"[{DateTime.Now}] Ошибка: {ex.Message}\n";
        }
        finally
        {
            _isTraining = false;
            this.RaisePropertyChanged(nameof(CanStartTraining));
            this.RaisePropertyChanged(nameof(CanStopTraining));
            IsTrainingIndeterminate = false;
            CanStartTraining = true;
            CanStopTraining = false;
        }
    }

    private void StopTraining()
    {
        _cancellationTokenSource.Cancel();
        CanStartTraining = true;
        CanStopTraining = false;
        TrainingStatus = "Обучение классификатора остановлено";
        TrainingLog += $"[{DateTime.Now}] Обучение остановлено пользователем\n";
    }

    private async Task SelectOutputFolderAsync()
    {
        var folder = await Target.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Выберите папку для сохранения весов",
            AllowMultiple = false
        });

        if (folder.Count > 0 && folder[0].TryGetLocalPath() is string path)
        {
            WeightsOutputPath = path;
        }
    }
    #endregion
}
