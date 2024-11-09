using System;
using DG.Tweening;
using SWS;
using UnityEngine;
using UnityEngine.Events;

public class RuntimeDemo : MonoBehaviour
{
	private void OnGUI()
	{
		this.DrawExample1();
		this.DrawExample2();
		this.DrawExample3();
		this.DrawExample4();
		this.DrawExample5();
		this.DrawExample6();
		this.DrawExample7();
	}

	public void TestEvent(int code)
	{
		UnityEngine.Debug.Log("+++++++++++ Event code " + code);
	}

	private void DrawExample1()
	{
		GUI.Label(new Rect(10f, 10f, 20f, 20f), "1:");
		string name = "Walker (Path1)";
		Vector3 position = new Vector3(-25f, 0f, 10f);
		if (!this.example1.done && GUI.Button(new Rect(30f, 10f, 100f, 20f), "Instantiate"))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.example1.walkerPrefab, position, Quaternion.identity);
			gameObject.name = name;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.example1.pathPrefab, position, Quaternion.identity);
			gameObject.GetComponent<splineMove>().SetPath(WaypointManager.Paths[gameObject2.name]);
			this.example1.done = true;
		}
		if (this.example1.done && GUI.Button(new Rect(30f, 10f, 100f, 20f), "Destroy"))
		{
			UnityEngine.Object.Destroy(GameObject.Find(name));
			UnityEngine.Object.Destroy(GameObject.Find(this.example1.pathPrefab.name));
			this.example1.done = false;
		}
	}

	private void DrawExample2()
	{
		GUI.Label(new Rect(10f, 30f, 20f, 20f), "2:");
		if (GUI.Button(new Rect(30f, 30f, 100f, 20f), "Switch Path"))
		{
			string name = this.example2.moveRef.pathContainer.name;
			this.example2.moveRef.moveToPath = true;
			if (name == this.example2.pathName1)
			{
				this.example2.moveRef.SetPath(WaypointManager.Paths[this.example2.pathName2]);
			}
			else
			{
				this.example2.moveRef.SetPath(WaypointManager.Paths[this.example2.pathName1]);
			}
		}
	}

	private void DrawExample3()
	{
		GUI.Label(new Rect(10f, 50f, 20f, 20f), "3:");
		if (this.example3.moveRef.tween == null && GUI.Button(new Rect(30f, 50f, 100f, 20f), "Start"))
		{
			this.example3.moveRef.StartMove();
		}
		if (this.example3.moveRef.tween != null && GUI.Button(new Rect(30f, 50f, 100f, 20f), "Stop"))
		{
			this.example3.moveRef.Stop();
		}
		if (this.example3.moveRef.tween != null && GUI.Button(new Rect(30f, 70f, 100f, 20f), "Reset"))
		{
			this.example3.moveRef.ResetToStart();
		}
	}

	private void DrawExample4()
	{
		GUI.Label(new Rect(10f, 90f, 20f, 20f), "4:");
		if (this.example4.moveRef.tween != null && this.example4.moveRef.tween.IsPlaying() && GUI.Button(new Rect(30f, 90f, 100f, 20f), "Pause"))
		{
			this.example4.moveRef.Pause(0f);
		}
		if (this.example4.moveRef.tween != null && !this.example4.moveRef.tween.IsPlaying() && GUI.Button(new Rect(30f, 90f, 100f, 20f), "Resume"))
		{
			this.example4.moveRef.Resume();
		}
	}

	private void DrawExample5()
	{
		GUI.Label(new Rect(10f, 110f, 20f, 20f), "5:");
		if (GUI.Button(new Rect(30f, 110f, 100f, 20f), "Change Speed"))
		{
			float speed = this.example5.moveRef.speed;
			float num = 1.5f;
			if (speed == num)
			{
				num = 4f;
			}
			this.example5.moveRef.ChangeSpeed(num);
		}
	}

	private void DrawExample6()
	{
		GUI.Label(new Rect(10f, 130f, 20f, 20f), "6:");
		if (!this.example6.done && GUI.Button(new Rect(30f, 130f, 100f, 20f), "Add Event"))
		{
			EventReceiver receiver = this.example6.moveRef.GetComponent<EventReceiver>();
			UnityEvent unityEvent = this.example6.moveRef.events[1];
			unityEvent.RemoveAllListeners();
			unityEvent.AddListener(delegate()
			{
				receiver.ActivateForTime(this.example6.target);
			});
			this.example6.done = true;
		}
	}

	private void DrawExample7()
	{
		GUI.Label(new Rect(10f, 150f, 20f, 20f), "7:");
		if (!this.example7.done && GUI.Button(new Rect(30f, 150f, 100f, 20f), "Create Path"))
		{
			GameObject gameObject = new GameObject("Path7 (Runtime Creation)");
			PathManager pathManager = gameObject.AddComponent<PathManager>();
			Vector3[] array = new Vector3[]
			{
				new Vector3(-25f, 0f, -20f),
				new Vector3(-15f, 3f, -20f),
				new Vector3(-4f, 0f, -20f)
			};
			Transform[] array2 = new Transform[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject2 = new GameObject("Waypoint " + i);
				array2[i] = gameObject2.transform;
				array2[i].position = array[i];
			}
			pathManager.Create(array2, true);
			gameObject.AddComponent<PathRenderer>();
			gameObject.GetComponent<LineRenderer>().material = new Material(Shader.Find("Sprites/Default"));
			this.example7.done = true;
		}
	}

	public RuntimeDemo.ExampleClass1 example1;

	public RuntimeDemo.ExampleClass2 example2;

	public RuntimeDemo.ExampleClass3 example3;

	public RuntimeDemo.ExampleClass4 example4;

	public RuntimeDemo.ExampleClass5 example5;

	public RuntimeDemo.ExampleClass6 example6;

	public RuntimeDemo.ExampleClass6 example7;

	[Serializable]
	public class ExampleClass1
	{
		public GameObject walkerPrefab;

		public GameObject pathPrefab;

		public bool done;
	}

	[Serializable]
	public class ExampleClass2
	{
		public splineMove moveRef;

		public string pathName1;

		public string pathName2;
	}

	[Serializable]
	public class ExampleClass3
	{
		public splineMove moveRef;
	}

	[Serializable]
	public class ExampleClass4
	{
		public splineMove moveRef;
	}

	[Serializable]
	public class ExampleClass5
	{
		public splineMove moveRef;
	}

	[Serializable]
	public class ExampleClass6
	{
		public splineMove moveRef;

		public GameObject target;

		public bool done;
	}

	[Serializable]
	public class ExampleClass7
	{
		public bool done;
	}
}
