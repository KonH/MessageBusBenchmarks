using System;
using System.Collections.Generic;
using Custom.Shared;
using UnityEngine;

namespace Custom.Manager {
	public sealed class EventManager : SingleInstance<EventManager> {
		public const float CleanUpInterval = 10.0f;

		public Dictionary<Type, HandlerBase> Handlers => _handlers;

		readonly Dictionary<Type, HandlerBase> _handlers = new Dictionary<Type, HandlerBase>(100);

		public EventManager() => AddHelper();

		public void CheckHandlersOnLoad() {
			foreach ( var handler in _handlers ) {
				handler.Value.FixWatchers();
			}
		}

		public void CleanUp() {
			foreach ( var handler in _handlers ) {
				handler.Value.CleanUp();
			}
		}

		void Sub<T>(object watcher, Action<T> action) {
			var tHandler = GetOrCreateHandler<T>();
			if ( tHandler != null ) {
				tHandler.Subscribe(watcher, action);
			}
		}

		void Unsub<T>(Action<T> action) {
			if ( !_handlers.TryGetValue(typeof(T), out var handler) ) {
				return;
			}
			if ( handler is Handler<T> tHandler ) {
				tHandler.Unsubscribe(action);
			}
		}

		void FireEvent<T>(T args) {
			var tHandler = GetOrCreateHandler<T>();
			tHandler?.Fire(args);
		}

		Handler<T> GetOrCreateHandler<T>() {
			if ( !_handlers.TryGetValue(typeof(T), out var handler) ) {
				handler = new Handler<T>();
				_handlers.Add(typeof(T), handler);
			}
			return handler as Handler<T>;
		}

		bool HasWatchersDirect<T>() where T : struct {
			if ( _handlers.TryGetValue(typeof(T), out var container) ) {
				return (container.Watchers.Count > 0);
			}
			return false;
		}

		void AddHelper() {
			var go = new GameObject("[EventHelper]");
			go.AddComponent<EventHelper>();
		}

		public static void Subscribe<T>(object watcher, Action<T> action) where T : struct =>
			Instance.Sub(watcher, action);

		public static void Unsubscribe<T>(Action<T> action) where T : struct {
			if ( _instance != null ) {
				Instance.Unsub(action);
			}
		}

		public static void Fire<T>(T args) where T : struct =>
			Instance.FireEvent(args);

		public static bool HasWatchers<T>() where T : struct =>
			Instance.HasWatchersDirect<T>();
	}
}