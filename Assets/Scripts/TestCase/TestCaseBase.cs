using System;
using UnityEngine;
using UnityEngine.Profiling;

public abstract class TestCaseBase : MonoBehaviour {
	enum ExecutionState {
		Unknown,
		SkipFrame1,
		PreWarm,
		SkipFrame2,
		Baseline1,
		SkipFrame3,
		Subscribe,
		SkipFrame4,
		Baseline2,
		SkipFrame5,
		SkipFrame6,
		Fire1,
		SkipFrame7,
		Baseline3,
		SkipFrame8,
		Fire2,
		SkipFrame9,
		Baseline4,
		SkipFrame10,
		Fire3,
		SkipFrame11,
		Baseline5,
		SkipFrame12,
		Unsubscribe,
		SkipFrame13,
		Complete,
	}

	[SerializeField]
	protected int SubjectCount = 500;

	[SerializeField]
	int _callCount1 = 1;

	[SerializeField]
	int _callCount2 = 10;

	[SerializeField]
	int _callCount3 = 100;

	int _startFrame;

	protected void Start() {
		_startFrame = transform.GetSiblingIndex() * 30 + Time.frameCount;
	}

	protected void Update() {
		var state = FrameToState(Time.frameCount - _startFrame);
		switch ( state ) {
			case ExecutionState.Baseline1:
			case ExecutionState.Baseline2:
			case ExecutionState.Baseline3:
			case ExecutionState.Baseline4:
			case ExecutionState.Baseline5:
				Run($"{GetType().Name}.Baseline_{SubjectCount}", Baseline);
				break;
			case ExecutionState.PreWarm:
				Run($"{GetType().Name}.PreWarm_Subscribe_{SubjectCount}", Subscribe);
				Run($"{GetType().Name}.PreWarm_Fire{_callCount1}", () => Fire(1));
				Assert(_callCount1);
				Run($"{GetType().Name}.PreWarm_Unsubscribe_{SubjectCount}", Unsubscribe);
				break;
			case ExecutionState.Subscribe:
				Run($"{GetType().Name}.Subscribe_{SubjectCount}", Subscribe);
				break;
			case ExecutionState.Fire1:
				Run($"{GetType().Name}.Fire_{_callCount1}", () => Fire(_callCount1));
				Assert(_callCount1);
				break;
			case ExecutionState.Fire2:
				Run($"{GetType().Name}.Fire_{_callCount2}", () => Fire(_callCount2));
				Assert(_callCount2);
				break;
			case ExecutionState.Fire3:
				Run($"{GetType().Name}.Fire_{_callCount3}", () => Fire(_callCount3));
				Assert(_callCount3);
				break;
			case ExecutionState.Unsubscribe:
				Run($"{GetType().Name}.Unsubscribe_{SubjectCount}", Unsubscribe);
				break;
			case ExecutionState.Complete:
				if ( transform.GetSiblingIndex() == (transform.parent.childCount - 1) ) {
					if ( Application.isEditor ) {
						Debug.Break();
					} else {
						Application.Quit();
					}
				}
				break;
		}
	}

	ExecutionState FrameToState(int frameCount) {
		if ( Enum.IsDefined(typeof(ExecutionState), frameCount) ) {
			return (ExecutionState)frameCount;
		}
		return ExecutionState.Unknown;
	}

	void Run(string operation, Action act) {
		Debug.Log($"Run {operation}");
		GC.Collect();
		Profiler.BeginSample(operation);
		act();
		Profiler.EndSample();
	}

	protected abstract void Baseline();
	protected abstract void Assert(int count);
	protected abstract void Subscribe();
	protected abstract void Fire(int count);
	protected abstract void Unsubscribe();
}