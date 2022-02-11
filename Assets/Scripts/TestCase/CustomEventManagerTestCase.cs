using Custom.Manager;
using UnityEngine;

public class CustomEventManagerTestCase : TestCaseBase {
	sealed class Handler {
		public int Counter;

		public void Baseline() {
			Counter = 0;
		}

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

	Handler[] _handlers;

	private void Awake() {
		_handlers = new Handler[SubjectCount];
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i] = new Handler();
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
			EventManager.Fire(arg);
		}
	}

	protected override void Unsubscribe() {
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i].Unsubscribe();
		}
	}
}