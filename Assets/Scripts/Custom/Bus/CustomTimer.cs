using System;
using UnityEngine;

namespace Custom.Bus {
	public sealed class CustomTimer {
		public float Timer    { get; private set; }
		public float Interval { get; private set; }

		public CustomTimer(float interval) {
			Timer    = 0.0f;
			Interval = interval;
		}

		public CustomTimer(TimeSpan interval) {
			Timer    = 0.0f;
			Interval = FromTimeSpan(interval);
		}

		public void Reset() =>
			Timer = 0.0f;

		public void Reset(float newInterval) =>
			Reset(0f, newInterval);

		public void Reset(float newTimer, float newInterval) {
			Timer    = newTimer;
			Interval = newInterval;
		}

		public void Reset(float newTimer, TimeSpan newInterval) {
			Timer    = newTimer;
			Interval = FromTimeSpan(newInterval);
		}

		public bool Tick(float delta) {
			Timer += delta;
			if ( Timer > Interval ) {
				Timer -= Interval;
				return true;
			}
			return false;
		}

		public bool DeltaTick() => Tick(Time.deltaTime);

		public bool UnscaledDeltaTick() => Tick(Time.unscaledDeltaTime);

		float FromTimeSpan(TimeSpan interval) => (float)interval.TotalSeconds;
	}
}