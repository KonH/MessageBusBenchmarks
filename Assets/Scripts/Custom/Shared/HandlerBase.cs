using System.Collections.Generic;

namespace Custom.Shared {
	public abstract class HandlerBase {
		protected static bool LogsEnabled => false;
		protected static bool AllFireLogs => LogsEnabled;

		public List<object> Watchers { get; } = new List<object>(100);

		public virtual void CleanUp() {}

		// ReSharper disable once UnusedMethodReturnValue.Global
		public virtual bool FixWatchers() => false;
	}
}