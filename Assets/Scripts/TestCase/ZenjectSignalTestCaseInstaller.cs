using Zenject;

public class ZenjectSignalTestCaseInstaller : MonoInstaller {
	public override void InstallBindings() {
		SignalBusInstaller.Install(Container);
		Container.DeclareSignal<EventArgument>();
	}
}