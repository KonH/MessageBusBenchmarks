using System;
using Custom.Manager;
using Custom.Shared;

namespace Custom.Bus {
	public sealed class EventBus<T> : BaseEventBus {
		readonly Handler<T> _handler = new Handler<T>();

		protected override HandlerBase Handler => _handler;

		public void Subscribe(object watcher, Action<T> action) {
			TryRegister();
			_handler.Subscribe(watcher, action);
		}

		public void Unsubscribe(Action<T> action) =>
			_handler.Unsubscribe(action);

		public void Fire(T arg) {
			_handler.Fire(arg);
			TryUnregister();
		}

		public override void FixWatchers() {
			_handler.FixWatchers();
			TryUnregister();
		}

		public override void CleanUp() {
			_handler.CleanUp();
			TryUnregister();
		}

		public override string ToString() => typeof(T).Name;
	}
}