using System;
using MessagePipe;

public class MessagePipePerformanceTestCase {
	sealed class Handler {
		public int Counter;

		IDisposable _disposable;

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

	public const int SubjectCount = 500;

	Handler[] _handlers;

	public void Init() {
		var builder = new BuiltinContainerBuilder();
		builder.AddMessagePipe();
		builder.AddMessageBroker<EventArgument>();
		var provider = builder.BuildServiceProvider();
		GlobalMessagePipe.SetProvider(provider);
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
		var publisher = GlobalMessagePipe.GetPublisher<EventArgument>();
		var arg = new EventArgument(0, 1, 2);
		for ( var i = 0; i < count; i++ ) {
			publisher.Publish(arg);
		}
	}

	void Unsubscribe() {
		for ( var i = 0; i < SubjectCount; i++ ) {
			_handlers[i].Unsubscribe();
		}
	}
}