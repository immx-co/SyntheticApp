using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace SyntheticUI.ViewModels;

public class NavigationViewModel : ReactiveObject, IDisposable
{
    #region View Model Settings
    public RoutingState Router { get; }

    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    #endregion

    #region Private Fields
    private readonly IServiceProvider _serviceProvider;

    private bool _isAugmentExpanded;

    private bool _isTrainExpanded;

    private bool _isEvaluateExpanded;

    private bool _isTestExpanded;
    #endregion

    #region Active View Properties
    private bool _isAugmentWindowActive;
    public bool IsAugmentWindowActive
    {
        get => _isAugmentWindowActive;
        set => this.RaiseAndSetIfChanged(ref _isAugmentWindowActive, value);
    }

    private bool _isLearnWindowActive;
    public bool IsLearnWindowActive
    {
        get => _isLearnWindowActive;
        set => this.RaiseAndSetIfChanged(ref _isLearnWindowActive, value);
    }

    private bool _isEvaluateWindowActive;
    public bool IsEvaluateWindowActive
    {
        get => _isEvaluateWindowActive;
        set => this.RaiseAndSetIfChanged(ref _isEvaluateWindowActive, value);
    }

    private bool _isTestWindowActive;
    public bool IsTestWindowActive
    {
        get => _isTestWindowActive;
        set => this.RaiseAndSetIfChanged(ref _isTestWindowActive, value);
    }
    #endregion

    #region State Of Expand Menu Properties
    public bool IsAugmentExpanded
    {
        get => _isAugmentExpanded;
        set => this.RaiseAndSetIfChanged(ref _isAugmentExpanded, value);
    }

    public bool IsTrainExpanded
    {
        get => _isTrainExpanded;
        set => this.RaiseAndSetIfChanged(ref _isTrainExpanded, value);
    }

    public bool IsEvaluateExpanded
    {
        get => _isEvaluateExpanded;
        set => this.RaiseAndSetIfChanged(ref _isEvaluateExpanded, value);
    }

    public bool IsTestExpanded
    {
        get => _isTestExpanded;
        set => this.RaiseAndSetIfChanged(ref _isTestExpanded, value);
    }
    #endregion

    #region Navigation Properties
    private int _navPanelWidth = 200;

    private string _navToggleButtonContent = "M15.41 16.59L10.83 12l4.58-4.59L14 6l-6 6 6 6 1.41-1.41z";

    public int NavPanelWidth
    {
        get => _navPanelWidth;
        set => this.RaiseAndSetIfChanged(ref _navPanelWidth, value);
    }

    public string NavToggleButtonContent
    {
        get => _navToggleButtonContent;
        set => this.RaiseAndSetIfChanged(ref _navToggleButtonContent, value);
    }
    #endregion

    #region Public Commands
    public ReactiveCommand<Unit, Unit> ToggleNavigationCommand { get; }

    public ReactiveCommand<Unit, Unit> ToggleAugmentCommand { get; }

    public ReactiveCommand<Unit, Unit> ToggleTrainCommand { get; }

    public ReactiveCommand<Unit, Unit> ToggleEvaluateCommand { get; }

    public ReactiveCommand<Unit, Unit> ToggleTestCommand { get; }

    public ReactiveCommand<Unit, Unit> GoAugmentationDetectorWindow { get; }

    public ReactiveCommand<Unit, Unit> GoAugmentationClassificatorWindow { get; }

    public ReactiveCommand<Unit, Unit> GoTrainingDetectorWindow { get; }

    public ReactiveCommand<Unit, Unit> GoTrainingClassificatorWindow { get; }

    public ReactiveCommand<Unit, Unit> GoEvaluateClassificatorWindow { get; }

    public ReactiveCommand<Unit, Unit> GoEvaluateDetectorWindow { get; }

    public ReactiveCommand<Unit, Unit> GoTestingClassificatorWindow { get; }

    public ReactiveCommand<Unit, Unit> GoTestingDetectorWindow { get; }
	#endregion

	public NavigationViewModel(IScreen screenRealization, IServiceProvider serviceProvider)
    {
        Router = screenRealization.Router;
        _serviceProvider = serviceProvider;

        ToggleNavigationCommand = ReactiveCommand.Create(ToggleNavigation);
        ToggleAugmentCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            IsAugmentExpanded = !IsAugmentExpanded;
            await Task.CompletedTask;
        });

        ToggleTrainCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            IsTrainExpanded = !IsTrainExpanded;
            await Task.CompletedTask;
        });

        ToggleEvaluateCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            IsEvaluateExpanded = !IsEvaluateExpanded;
            await Task.CompletedTask;
        });

        ToggleTestCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            IsTestExpanded = !IsTestExpanded;
            await Task.CompletedTask;
        });

        GoAugmentationDetectorWindow = ReactiveCommand.Create(NavigateToAugmentationDetectorWindow);
        GoAugmentationClassificatorWindow = ReactiveCommand.Create(NavigateToAugmentationClassificatorWindow);
		GoTrainingDetectorWindow = ReactiveCommand.Create(NavigateToTrainingDetectorWindow);
        GoTrainingClassificatorWindow = ReactiveCommand.Create(NavigateToTrainingClassificatorWindow);
        GoEvaluateClassificatorWindow = ReactiveCommand.Create(NavigateToEvaluateClassificatorWindow);
        GoEvaluateDetectorWindow = ReactiveCommand.Create(NavigateToEvaluateDetectorWindow);
        GoTestingClassificatorWindow = ReactiveCommand.Create(NavigateToTestingClassificatorWindow);
        GoTestingDetectorWindow = ReactiveCommand.Create(NavigateToTestingDetectorWindow);
	}

    #region Private Methods
    private void ToggleNavigation()
    {
        if (NavPanelWidth > 0)
        {
            NavPanelWidth = 0;
            NavToggleButtonContent = "M8.59 16.59L13.17 12 8.59 7.41 10 6l6 6-6 6-1.41-1.41z";
        }
        else
        {
            NavPanelWidth = 200;
            NavToggleButtonContent = "M15.41 16.59L10.83 12l4.58-4.59L14 6l-6 6 6 6 1.41-1.41z";
        }
    }

    private void NavigateToAugmentationDetectorWindow()
    {
        CheckDisposedCancelletionToken();
        Router.Navigate.Execute(_serviceProvider.GetRequiredService<AugmentationDetectorViewModel>());
    }

	private void NavigateToAugmentationClassificatorWindow()
	{
		CheckDisposedCancelletionToken();
		Router.Navigate.Execute(_serviceProvider.GetRequiredService<AugmentationClassificatorViewModel>());
	}

    private void NavigateToTrainingClassificatorWindow()
    {
        CheckDisposedCancelletionToken();
        Router.Navigate.Execute(_serviceProvider.GetRequiredService<TrainingClassificatorViewModel>());
    }

	private void NavigateToTrainingDetectorWindow()
	{
		CheckDisposedCancelletionToken();
		Router.Navigate.Execute(_serviceProvider.GetRequiredService<TrainingDetectorViewModel>());
	}

    private void NavigateToEvaluateClassificatorWindow()
    {
        CheckDisposedCancelletionToken();
        Router.Navigate.Execute(_serviceProvider.GetRequiredService<EvaluateClassificatorViewModel>());
    }

	private void NavigateToEvaluateDetectorWindow()
	{
		CheckDisposedCancelletionToken();
		Router.Navigate.Execute(_serviceProvider.GetRequiredService<EvaluateDetectorViewModel>());
	}

    private void NavigateToTestingClassificatorWindow()
    {
        CheckDisposedCancelletionToken();
        Router.Navigate.Execute(_serviceProvider.GetRequiredService<TestingClassificatorViewModel>());
    }

	private void NavigateToTestingDetectorWindow()
	{
		CheckDisposedCancelletionToken();
		Router.Navigate.Execute(_serviceProvider.GetRequiredService<TestingDetectorViewModel>());
	}


	private void CheckDisposedCancelletionToken()
    {
        if (Router.NavigationStack.Count > 0)
        {
            var currentViewModel = Router.NavigationStack.Last();
            if (currentViewModel is IDisposable disposableViewModel)
            {
                disposableViewModel.Dispose();
            }
        }
    }
    #endregion

    #region Public Methods
    public void Dispose()
    {
        _disposables?.Dispose();
    }
    #endregion
}
