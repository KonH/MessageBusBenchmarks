using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Custom.Bus {
	public abstract class SingleBehaviour<T, IT> : MonoBehaviour
		where T : SingleBehaviour<T, IT>, IT
		where IT : class {
		public static IT Instance {
			get {
				if ( _instance ) {
					return _instance;
				}

				_instance = FindObjectOfType<T>();
				if ( _instance ) {
					return _instance;
				}

				_instance = Create();
				return _instance;
			}
		}

		[CanBeNull]
		public static IT SafeInstance => !_instance ? null : _instance;

		public static bool HasInstance => _instance;

		protected static T Create() {
			var go = new GameObject($"[{typeof(T).Name}]");
			var i = go.AddComponent<T>();
			i.Init();
			return i;
		}

		protected static T _instance;

		protected virtual void Init() {}

		protected virtual void Awake() =>
			DontDestroyOnLoad(gameObject);
	}

	public abstract class SingleBehaviour<T> : SingleBehaviour<T, T>
		where T : SingleBehaviour<T, T> {
	}
}
