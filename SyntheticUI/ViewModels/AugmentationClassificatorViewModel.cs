using ReactiveUI;
using System;
using System.Threading;

namespace SyntheticUI.ViewModels;

public class AugmentationClassificatorViewModel : ReactiveObject, IRoutableViewModel
{
	IServiceProvider _serviceProvider;

	#region View Model Settings
	public IScreen HostScreen { get; }

	public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

	public CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
	#endregion

	public AugmentationClassificatorViewModel(IScreen screen, IServiceProvider serviceProvider)
	{
		HostScreen = screen;

		_serviceProvider = serviceProvider;
	}
}
