using System;
using Custom.Shared;

namespace Custom.Bus {
	public sealed class EventBus : BaseEventBus {
		readonly UntypedHandler _handler = new UntypedHandler();

		protected override HandlerBase Handler => _handler;

		public void Subscribe(object watcher, Action action) {
			TryRegister();
			_handler.Subscribe(watcher, action);
		}

		public void Unsubscribe(Action action) =>
			_handler.Unsubscribe(action);

		public void Fire() {
			_handler.Fire();
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

		public override string ToString() => "Untyped";
	}
}