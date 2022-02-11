using System;
using System.Collections.Generic;
using Custom.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Custom.Manager {
	public sealed class EventHelper : MonoBehaviour {
		static bool AutoFill => false;

		public List<EventData> Events = new List<EventData>(100);

		readonly Dictionary<Type, string> _typeCache = new Dictionary<Type, string>();

		float _cleanupTimer;

		void Awake() {
			DontDestroyOnLoad(gameObject);
			SubscribeToLogged();
		}

		void OnEnable() =>
			SceneManager.sceneLoaded += OnSceneLoaded;

		void OnDisable() =>
			SceneManager.sceneLoaded -= OnSceneLoaded;

		void OnSceneLoaded(Scene scene, LoadSceneMode mode) =>
			EventManager.Instance.CheckHandlersOnLoad();

		void SubscribeToLogged() {}

#pragma warning disable SA1001
		// ReSharper disable once UnusedMember.Local
		void SubscribeToLog<T>() where T : struct =>
			EventManager.Subscribe<T>(this, OnLog);
#pragma warning restore SA1001

		void OnLog<T>(T ev) where T : struct =>
			Debug.LogFormat("Event: {0}", typeof(T));

		void Update() {
			TryCleanUp();
			if(AutoFill) {
				Fill();
			}
		}

		[ContextMenu("CheckEventHandlers")]
		public void CheckEventHandlers() {
			var handlers = EventManager.Instance.Handlers;
			foreach ( var handler in handlers ) {
				if ( handler.Value.Watchers.Count > 0 ) {
					Debug.Log(handler.Key);
					foreach ( var watcher in handler.Value.Watchers ) {
						Debug.Log(handler.Key + " => " + watcher.GetType());
					}
				}
			}
		}

		[ContextMenu("ClearEventHandlers")]
		public void ClearEventHandlers() =>
			EventManager.Instance.Handlers.Clear();

		void TryCleanUp() {
			if ( _cleanupTimer > EventManager.CleanUpInterval ) {
				EventManager.Instance.CleanUp();
				_cleanupTimer = 0;
			} else {
				_cleanupTimer += Time.deltaTime;
			}
		}

		[ContextMenu("Fill")]
		public void Fill() {
			var handlers = EventManager.Instance.Handlers;
			foreach ( var pair in handlers ) {
				var eventData = GetEventData(pair.Key);
				if ( eventData == null ) {
					eventData = new EventData(pair.Key);
					Events.Add(eventData);
				}
				FillEvent(pair.Value, eventData);
			}
		}

		void FillEvent(HandlerBase handler, EventData data) {
			data.MonoWatchers.Clear();
			data.OtherWatchers.Clear();
			foreach ( var item in handler.Watchers) {
				if ( item is MonoBehaviour behaviour ) {
					data.MonoWatchers.Add(behaviour);
				} else {
					data.OtherWatchers.Add((item != null) ? GetTypeNameFromCache(item.GetType()) : "null");
				}
			}
		}

		EventData GetEventData(Type type) {
			foreach ( var ev in Events ) {
				if ( ev.Type == type ) {
					return ev;
				}
			}
			return null;
		}

		string GetTypeNameFromCache(Type type) {
			if ( !_typeCache.TryGetValue(type, out var typeName) ) {
				typeName = type.ToString();
				_typeCache.Add(type, typeName);
			}
			return typeName;
		}
	}
}