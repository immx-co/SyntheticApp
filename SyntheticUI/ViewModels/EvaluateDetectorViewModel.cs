using ReactiveUI;
using System;
using System.Reactive;
using System.Threading;
using Avalonia.Collections;

namespace SyntheticUI.ViewModels;

public class EvaluateDetectorViewModel : ReactiveObject, IRoutableViewModel
{
	IServiceProvider _serviceProvider;

	#region View Model Settings
	public IScreen HostScreen { get; }

	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	public CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    #endregion

    #region Commands
    public ReactiveCommand<Unit, Unit> LoadDatasetCommand { get; }
    public ReactiveCommand<Unit, Unit> EvaluateCommand { get; }
    #endregion

    #region Private Fields
    private AvaloniaList<MetricItem> _metrics;
	#endregion

	#region Properties
	public AvaloniaList<MetricItem> Metrics
    {
        get => _metrics;
        set => this.RaiseAndSetIfChanged(ref _metrics, value);
    }
    #endregion

    #region .ctor
    public EvaluateDetectorViewModel(IScreen screen, IServiceProvider serviceProvider)
	{
		HostScreen = screen;

		_serviceProvider = serviceProvider;

        _metrics = new AvaloniaList<MetricItem>();

		LoadDatasetCommand = ReactiveCommand.Create(LoadDataset);
        EvaluateCommand = ReactiveCommand.Create(Evaluate);
    }
    #endregion

    #region Private Methods
    private void LoadDataset()
    {
        ;
    }

    private void Evaluate()
    {
        ;
    }
	#endregion

	#region Public Classes
	public class MetricItem
    {
        public string ClassName { get; set; }
        public float Precision { get; set; }
        public float Recall { get; set; }
		public float F1Score { get; set; }
        public float mAP50 { get; set; }
        public float mAP50_95 { get; set; }
    }
    #endregion
}
