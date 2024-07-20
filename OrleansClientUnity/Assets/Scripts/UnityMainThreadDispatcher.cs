using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour {
	private static readonly Queue<Action> _executionQueue = new Queue<Action>();

	private void Update() {
		lock (_executionQueue) {
			while (_executionQueue.Count > 0) {
				_executionQueue.Dequeue().Invoke();
			}
		}
	}

	public void Enqueue(Action action) {
		lock (_executionQueue) {
			_executionQueue.Enqueue(action);
		}
	}

	private static UnityMainThreadDispatcher _instance;

	public static void Initialize() {
		if (_instance == null) {
			var obj = new GameObject("UnityMainThreadDispatcher");
			_instance = obj.AddComponent<UnityMainThreadDispatcher>();
			DontDestroyOnLoad(obj);
		}
	}

	public static void EnqueueAction(Action action) {
		if (_instance == null) {
			throw new Exception("UnityMainThreadDispatcher is not initialized. Call UnityMainThreadDispatcher.Initialize() first.");
		}
		_instance.Enqueue(action);
	}
}
