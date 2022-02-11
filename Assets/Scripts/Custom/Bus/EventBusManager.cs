using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Custom.Bus {
	public sealed class EventBusManager : SingleBehaviour<EventBusManager> {
		const float CleanUpInterval = 5f;

		public static bool Exists => _instance;

		public readonly List<WeakReference<BaseEventBus>> EventBuses = new List<WeakReference<BaseEventBus>>();

		public readonly CustomTimer CleanUpTimer = new CustomTimer(CleanUpInterval);

		readonly HashSet<WeakReference<BaseEventBus>> _toRemove = new HashSet<WeakReference<BaseEventBus>>();

		void Update() =>
			TryCleanUp();

		public void TryRegister(BaseEventBus addEventBus) {
			foreach ( var wr in EventBuses ) {
				if ( wr.TryGetTarget(out var eventBus) && ReferenceEquals(eventBus, addEventBus) ) {
					return;
				}
			}
			EventBuses.Add(new WeakReference<BaseEventBus>(addEventBus));
		}

		public void TryUnregister(BaseEventBus eventBus) {
			foreach ( var wr in EventBuses ) {
				if ( wr.TryGetTarget(out var tmpEventBus) && ReferenceEquals(eventBus, tmpEventBus) ) {
					_toRemove.Add(wr);
					break;
				}
			}
		}

		protected override void Init() =>
			SceneManager.sceneLoaded += OnSceneLoaded;

		void OnSceneLoaded(Scene scene, LoadSceneMode mode) =>
			CheckEventBusesOnLoad();

		[ContextMenu("Fix Watchers")]
		void CheckEventBusesOnLoad() =>
			CheckEventBuses(x => x.FixWatchers());

		void TryCleanUp() {
			if ( CleanUpTimer.DeltaTick() ) {
				CleanUp();
			}
		}

		[ContextMenu("CleanUp")]
		void CleanUp() =>
			CheckEventBuses(x => x.CleanUp());

		void CheckEventBuses(Action<BaseEventBus> act) {
			foreach ( var wr in EventBuses ) {
				if ( wr.TryGetTarget(out var eventBus) ) {
					act?.Invoke(eventBus);
				} else {
					_toRemove.Add(wr);
				}
			}
			var removed = EventBuses.RemoveAll(x => _toRemove.Contains(x));
			if ( removed > 0 ) {
				Debug.LogFormat(this, "Cleared '{0}' event buses", removed);
			}
			_toRemove.Clear();
		}
	}
}