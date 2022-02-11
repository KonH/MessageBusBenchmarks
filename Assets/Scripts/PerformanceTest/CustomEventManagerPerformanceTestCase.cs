using Custom.Manager;

public class CustomEventManagerPerformanceTestCase {
	sealed class Handler {
		public int Counter;

		public void Subscribe() {
			EventManager.Subscribe<EventArgument>(this, OnEvent);
		}

		public void Unsubscribe() {
			EventManager.Unsubscribe<EventArgument>(OnEvent);
		}

		void OnEvent(EventArgument arg) {
			Counter++;
		}
	}

	const int SubjectCount = 500;

	Handler[] _handlers;

	public void Init() {
		_handlers = new Handler[SubjectCount];
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i] = new Handler();
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
			EventManager.Fire(arg);
		}
	}

	void Unsubscribe() {
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i].Unsubscribe();
		}
	}
}