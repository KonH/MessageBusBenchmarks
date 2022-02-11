using UniRx;
using UnityEngine;

public class UniRxMessageBrokerTestCase : TestCaseBase {
	sealed class Handler {
		public int Counter;

		readonly MessageBroker _broker;

		CompositeDisposable _disposable;

		public Handler(MessageBroker broker) {
			_broker = broker;
		}

		public void Baseline() {
			Counter = 0;
		}

		public void Subscribe() {
			_disposable = new CompositeDisposable();
			_broker
				.Receive<EventArgument>()
				.Subscribe(OnEvent)
				.AddTo(_disposable);
		}

		public void Unsubscribe() {
			_disposable.Dispose();
		}

		void OnEvent(EventArgument arg) {
			Counter++;
		}
	}

	MessageBroker _broker;
	Handler[] _handlers;

	private void Awake() {
		_broker = new MessageBroker();
		_handlers = new Handler[SubjectCount];
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i] = new Handler(_broker);
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
			_broker.Publish(arg);
		}
	}

	protected override void Unsubscribe() {
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i].Unsubscribe();
		}
	}
}