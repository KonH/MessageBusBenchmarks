// ReSharper disable InconsistentNaming
namespace Custom.Manager {
	public abstract class SingleInstance<IT, T> where T : IT, new() {
		public static T Instance => _instance;

		protected static T _instance = new T();

		public static void RecreateInstance() =>
			_instance = new T();
	}

	public abstract class SingleInstance<T> : SingleInstance<T, T> where T : new() {}
}