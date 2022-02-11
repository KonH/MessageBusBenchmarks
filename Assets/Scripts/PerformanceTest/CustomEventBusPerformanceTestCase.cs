using Custom.Bus;

public class CustomEventBusPerformanceTestCase {
	sealed class Handler {
		public int Counter;

		readonly EventBus<EventArgument> _bus;

		public Handler(EventBus<EventArgument> bus) {
			_bus = bus;
		}

		public void Subscribe() {
			_bus.Subscribe(this, OnEvent);
		}

		public void Unsubscribe() {
			_bus.Unsubscribe(OnEvent);
		}

		void OnEvent(EventArgument arg) {
			Counter++;
		}
	}

	public const int SubjectCount = 500;

	EventBus<EventArgument> _bus;
	Handler[] _handlers;

	public void Init() {
		_bus = new EventBus<EventArgument>();
		_handlers = new Handler[SubjectCount];
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i] = new Handler(_bus);
		}
		Subscribe();
	}

	public void Reset() {
		Unsubscribe();
	}

	void Subscribe() {
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i].Subscribe();
		}
	}

	public void Fire(int count) {
		var arg = new EventArgument(0, 1, 2);
		for ( var i = 0; i < count; i++ ) {
			_bus.Fire(arg);
		}
	}

	void Unsubscribe() {
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i].Unsubscribe();
		}
	}
}