using Custom.Bus;
using UnityEngine;

public class CustomEventBusTestCase : TestCaseBase {
	sealed class Handler {
		public int Counter;

		readonly EventBus<EventArgument> _bus;

		public Handler(EventBus<EventArgument> bus) {
			_bus = bus;
		}

		public void Baseline() {
			Counter = 0;
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

	EventBus<EventArgument> _bus;
	Handler[] _handlers;

	private void Awake() {
		_bus = new EventBus<EventArgument>();
		_handlers = new Handler[SubjectCount];
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i] = new Handler(_bus);
		}
	}

	protected override void Baseline() {
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i].Baseline();
		}
	}

	protected override void Assert(int count) {
		for ( var i = 0; i < SubjectCount; i++ ) {
			if ( _handlers[i].Counter != count ) {
				Debug.LogErrorFormat(
					"Invalid state: {0} found, {1} expected", _handlers[i].Counter, count);
			}
		}
	}

	protected override void Subscribe() {
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i].Subscribe();
		}
	}

	protected override void Fire(int count) {
		var arg = new EventArgument(0, 1, 2);
		for ( var i = 0; i < count; i++ ) {
			_bus.Fire(arg);
		}
	}

	protected override void Unsubscribe() {
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i].Unsubscribe();
		}
	}
}