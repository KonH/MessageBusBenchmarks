using Custom.Shared;

namespace Custom.Bus {
	public abstract class BaseEventBus {
		protected abstract HandlerBase Handler { get; }

		public abstract void FixWatchers();
		public abstract void CleanUp();

		protected void TryRegister() =>
			EventBusManager.Instance.TryRegister(this);

		protected void TryUnregister() {
			if ( Handler.Watchers.Count > 0 ) {
				return;
			}
			if ( EventBusManager.Exists ) {
				EventBusManager.Instance.TryUnregister(this);
			}
		}
	}
}