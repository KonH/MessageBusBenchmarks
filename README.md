# Message Bus Benchmarks

## Overview

The project provide benchmarks for message bus usage in Unity game engine.

Test operations:
- Use readonly struct as event argument (12 bytes)
- Subscribe 500 handlers to single event
- Unsubscribe 500 handlers from single event
- Publish event for 500 subscribers x1, x10, x100 times

Two approaches:
- **Basic** - using custom runner ([Assets/Scripts/TestCase/TestCaseBase.cs](Assets/Scripts/TestCase/TestCaseBase.cs))
- **Performance Testing Extension** - optimal way to run benchmarks, for publishing x100 case only (see more: https://docs.unity3d.com/Packages/com.unity.test-framework.performance@2.8/manual/index.html, https://blog.unity.com/technology/performance-benchmarking-in-unity-how-to-get-started)

Implementations: 
- **Zenject** - Signal from https://github.com/Mathijs-Bakker/Extenject
- **UniRx** - MessageBroker from https://github.com/neuecc/UniRx
- **MessagePipe** - https://github.com/Cysharp/MessagePipe
- **EventManager/EventBus** - some custom implementation

## Plots

![Android plots 1](Images/AndroidPlots1.png)
![Android plots 2](Images/AndroidPlots2.png)
![iOS plots 1](Images/iOSPlots1.png)
![iOS plots 2](Images/iOSPlots2.png)

## Basic Summary
![Summary](Images/Summary.png)

## Performance Testing Extension Summary
![Performance Testing Extension Summary](Images/PerformanceTestingExtensionSummary.png)
