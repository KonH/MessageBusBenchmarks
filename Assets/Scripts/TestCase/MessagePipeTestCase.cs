using System;
using MessagePipe;
using UnityEngine;

public class MessagePipeTestCase : TestCaseBase {
	sealed class Handler {
		public int Counter;

		IDisposable _disposable;

		public void Baseline() {
			Counter = 0;
		}

		public void Subscribe() {
			var subscriber = GlobalMessagePipe.GetSubscriber<EventArgument>();
			_disposable = subscriber
				.Subscribe(OnEvent);
		}

		public void Unsubscribe() {
			_disposable.Dispose();
		}

		void OnEvent(EventArgument arg) {
			Counter++;
		}
	}

	Handler[] _handlers;

	private void Awake() {
		var builder = new BuiltinContainerBuilder();
		builder.AddMessagePipe();
		builder.AddMessageBroker<EventArgument>();
		var provider = builder.BuildServiceProvider();
		GlobalMessagePipe.SetProvider(provider);
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
		var publisher = GlobalMessagePipe.GetPublisher<EventArgument>();
		var arg = new EventArgument(0, 1, 2);
		for ( var i = 0; i < count; i++ ) {
			publisher.Publish(arg);
		}
	}

	protected override void Unsubscribe() {
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i].Unsubscribe();
		}
	}
}