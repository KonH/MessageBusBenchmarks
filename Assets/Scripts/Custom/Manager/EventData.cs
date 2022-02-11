using System;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Manager {
	[Serializable]
	public sealed class EventData {
		public string Name;
		public Type   Type;

		public List<MonoBehaviour> MonoWatchers  = new List<MonoBehaviour>(100);
		public List<string>        OtherWatchers = new List<string>(100);

		public EventData(Type type) {
			Type = type;
			Name = type.ToString();
		}
	}
}