using System;
using UnityEngine;

public class TestingPunch : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Debug.Log("exported curve:" + this.curveToString(this.exportCurve));
	}

	private void Update()
	{
		LeanTween.dtManual = Time.deltaTime;
		if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
		{
			LeanTween.moveLocalX(base.gameObject, 5f, 1f).setOnComplete(delegate()
			{
				UnityEngine.Debug.Log("on complete move local X");
			}).setOnCompleteOnStart(true);
			GameObject gameObject = GameObject.Find("DirectionalLight");
			Light lt = gameObject.GetComponent<Light>();
			LeanTween.value(lt.gameObject, lt.intensity, 0f, 1.5f).setEase(LeanTweenType.linear).setLoopPingPong().setRepeat(-1).setOnUpdate(delegate(float val)
			{
				lt.intensity = val;
			});
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.S))
		{
			LeanTween.scale(base.gameObject, Vector3.one * 3f, 1f).setEase(LeanTweenType.punch);
			MonoBehaviour.print("scale punch!");
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.R))
		{
			LeanTween.rotateAroundLocal(base.gameObject, base.transform.forward, -80f, 5f).setPoint(new Vector3(1.25f, 0f, 0f));
			MonoBehaviour.print("rotate punch!");
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.M))
		{
			MonoBehaviour.print("move punch!");
			Time.timeScale = 0.25f;
			float start = Time.realtimeSinceStartup;
			LeanTween.moveX(base.gameObject, 1f, 1f).setOnComplete(new Action<object>(this.destroyOnComp)).setOnCompleteParam(base.gameObject).setOnComplete(delegate()
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				float num = realtimeSinceStartup - start;
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"start:",
					start,
					" end:",
					realtimeSinceStartup,
					" diff:",
					num,
					" x:",
					this.gameObject.transform.position.x
				}));
			}).setEase(LeanTweenType.easeInOutElastic).setOvershoot(8f).setPeriod(0.3f);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.C))
		{
			LeanTween.color(base.gameObject, new Color(1f, 0f, 0f, 0.5f), 1f);
			Color to = new Color(UnityEngine.Random.Range(0f, 1f), 0f, UnityEngine.Random.Range(0f, 1f), 0f);
			GameObject gameObject2 = GameObject.Find("LCharacter");
			LeanTween.color(gameObject2, to, 4f).setLoopPingPong(1).setEase(LeanTweenType.easeOutBounce);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.E))
		{
			LeanTween.delayedCall(base.gameObject, 0.3f, new Action<object>(this.delayedMethod)).setRepeat(4).setOnCompleteOnRepeat(true).setOnCompleteParam("hi");
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.V))
		{
			LeanTween.value(base.gameObject, new Action<Color>(this.updateColor), new Color(1f, 0f, 0f, 1f), Color.blue, 4f);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.P))
		{
			LeanTween.delayedCall(0.05f, new Action<object>(this.enterMiniGameStart)).setOnCompleteParam(new object[]
			{
				string.Empty + 5
			});
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.U))
		{
			LeanTween.value(base.gameObject, delegate(Vector2 val)
			{
				this.transform.position = new Vector3(val.x, this.transform.position.y, this.transform.position.z);
			}, new Vector2(0f, 0f), new Vector2(5f, 100f), 1f).setEase(LeanTweenType.easeOutBounce);
			GameObject l = GameObject.Find("LCharacter");
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"x:",
				l.transform.position.x,
				" y:",
				l.transform.position.y
			}));
			LeanTween.value(l, new Vector2(l.transform.position.x, l.transform.position.y), new Vector2(l.transform.position.x, l.transform.position.y + 5f), 1f).setOnUpdate(delegate(Vector2 val)
			{
				UnityEngine.Debug.Log("tweening vec2 val:" + val);
				l.transform.position = new Vector3(val.x, val.y, this.transform.position.z);
			}, null);
		}
	}

	private void enterMiniGameStart(object val)
	{
		object[] array = (object[])val;
		int num = int.Parse((string)array[0]);
		UnityEngine.Debug.Log("level:" + num);
	}

	private void updateColor(Color c)
	{
		GameObject gameObject = GameObject.Find("LCharacter");
		gameObject.GetComponent<Renderer>().material.color = c;
	}

	private void delayedMethod(object myVal)
	{
		string text = myVal as string;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"delayed call:",
			Time.time,
			" myVal:",
			text
		}));
	}

	private void destroyOnComp(object p)
	{
		GameObject obj = (GameObject)p;
		UnityEngine.Object.Destroy(obj);
	}

	private string curveToString(AnimationCurve curve)
	{
		string text = string.Empty;
		for (int i = 0; i < curve.length; i++)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"new Keyframe(",
				curve[i].time,
				"f, ",
				curve[i].value,
				"f)"
			});
			if (i < curve.length - 1)
			{
				text += ", ";
			}
		}
		return "new AnimationCurve( " + text + " )";
	}

	public AnimationCurve exportCurve;
}
