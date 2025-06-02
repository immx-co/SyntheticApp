using ReactiveUI;

namespace SyntheticUI;

public class IScreenRealization : ReactiveObject, IScreen
{
	public RoutingState Router { get; } = new RoutingState();
}
