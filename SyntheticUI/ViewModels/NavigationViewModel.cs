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

    private void NavigateToAugmentWindow()
    {
        CheckDisposedCancelletionToken();
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
