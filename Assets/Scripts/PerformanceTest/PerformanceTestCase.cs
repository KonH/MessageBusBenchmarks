using NUnit.Framework;
using Unity.PerformanceTesting;

public class PerformanceTestCase {
	[Test, Performance]
	public void Fire100_EventManager() {
		var testCase = new CustomEventManagerPerformanceTestCase();
		testCase.Init();
		Measure.Method(() => testCase.Fire(100)).Run();
		testCase.Reset();
	}

	[Test, Performance]
	public void Fire100_EventBus() {
		var testCase = new CustomEventBusPerformanceTestCase();
		testCase.Init();
		Measure.Method(() => testCase.Fire(100)).Run();
		testCase.Reset();
	}

	[Test, Performance]
	public void Fire100_MessagePipe() {
		var testCase = new MessagePipePerformanceTestCase();
		testCase.Init();
		Measure.Method(() => testCase.Fire(100)).Run();
		testCase.Reset();
	}
}