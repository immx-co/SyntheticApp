using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;

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

    #region Properties
    public ObservableCollection<MetricItem> Metrics { get; }
    #endregion

    #region .ctor
    public EvaluateDetectorViewModel(IScreen screen, IServiceProvider serviceProvider)
	{
		HostScreen = screen;

		_serviceProvider = serviceProvider;

        LoadDatasetCommand = ReactiveCommand.Create(LoadDataset);
        EvaluateCommand = ReactiveCommand.Create(Evaluate);

        Metrics = new ObservableCollection<MetricItem>
        {
            new MetricItem
            {
                ClassName = "Vehicle",
                Precision = 0.85,
                Recall = 0.78,
                F1Score = 0.81,
                mAP50 = 0.79,
                mAP50_95 = 0.72
            },
            new MetricItem
            {
                ClassName = "Pedestrian",
                Precision = 0.76,
                Recall = 0.82,
                F1Score = 0.79,
                mAP50 = 0.81,
                mAP50_95 = 0.75
            }
        };
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
        public double Precision { get; set; } = 0;
        public double Recall { get; set; } = 0;
        public double F1Score { get; set; } = 0;
        public double mAP50 { get; set; } = 0;
        public double mAP50_95 { get; set; } = 0;
    }
    #endregion
}
