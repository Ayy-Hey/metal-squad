using System;
using System.Collections;
using UnityEngine;

public class TestingEverything : MonoBehaviour
{
	private void Awake()
	{
		this.boxNoCollider = GameObject.CreatePrimitive(PrimitiveType.Cube);
		UnityEngine.Object.Destroy(this.boxNoCollider.GetComponent(typeof(BoxCollider)));
	}

	private void Start()
	{
		LeanTest.timeout = 45f;
		LeanTest.expected = 31;
		LeanTween.init(1210);
		LeanTween.addListener(this.cube1, 0, new Action<LTEvent>(this.eventGameObjectCalled));
		LeanTest.expect(!LeanTween.isTweening((GameObject)null), "NOTHING TWEEENING AT BEGINNING", null);
		LeanTest.expect(!LeanTween.isTweening(this.cube1), "OBJECT NOT TWEEENING AT BEGINNING", null);
		LeanTween.scaleX(this.cube4, 2f, 0f).setOnComplete(delegate()
		{
			LeanTest.expect(this.cube4.transform.localScale.x == 2f, "TWEENED WITH ZERO TIME", null);
		});
		LeanTween.dispatchEvent(0);
		LeanTest.expect(this.eventGameObjectWasCalled, "EVENT GAMEOBJECT RECEIVED", null);
		LeanTest.expect(!LeanTween.removeListener(this.cube2, 0, new Action<LTEvent>(this.eventGameObjectCalled)), "EVENT GAMEOBJECT NOT REMOVED", null);
		LeanTest.expect(LeanTween.removeListener(this.cube1, 0, new Action<LTEvent>(this.eventGameObjectCalled)), "EVENT GAMEOBJECT REMOVED", null);
		LeanTween.addListener(1, new Action<LTEvent>(this.eventGeneralCalled));
		LeanTween.dispatchEvent(1);
		LeanTest.expect(this.eventGeneralWasCalled, "EVENT ALL RECEIVED", null);
		LeanTest.expect(LeanTween.removeListener(1, new Action<LTEvent>(this.eventGeneralCalled)), "EVENT ALL REMOVED", null);
		this.lt1Id = LeanTween.move(this.cube1, new Vector3(3f, 2f, 0.5f), 1.1f).id;
		LeanTween.move(this.cube2, new Vector3(-3f, -2f, -0.5f), 1.1f);
		LeanTween.reset();
		LTSpline ltspline = new LTSpline(new Vector3[]
		{
			new Vector3(-1f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(4f, 0f, 0f),
			new Vector3(20f, 0f, 0f),
			new Vector3(30f, 0f, 0f)
		});
		ltspline.place(this.cube4.transform, 0.5f);
		LeanTest.expect(Vector3.Distance(this.cube4.transform.position, new Vector3(10f, 0f, 0f)) <= 0.7f, "SPLINE POSITIONING AT HALFWAY", "position is:" + this.cube4.transform.position + " but should be:(10f,0f,0f)");
		LeanTween.color(this.cube4, Color.green, 0.01f);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
		gameObject.name = "normalTimeScale";
		LeanTween.moveX(gameObject, 12f, 1f).setIgnoreTimeScale(false).setOnComplete(delegate()
		{
			this.timeElapsedNormalTimeScale = Time.time;
		});
		LTDescr[] array = LeanTween.descriptions(gameObject);
		LeanTest.expect(array.Length >= 0 && array[0].to.x == 12f, "WE CAN RETRIEVE A DESCRIPTION", null);
		gameObject = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
		gameObject.name = "ignoreTimeScale";
		LeanTween.moveX(gameObject, 5f, 1f).setIgnoreTimeScale(true).setOnComplete(delegate()
		{
			this.timeElapsedIgnoreTimeScale = Time.time;
		});
		base.StartCoroutine(this.timeBasedTesting());
	}

	private IEnumerator timeBasedTesting()
	{
		yield return new WaitForSeconds(1f);
		yield return new WaitForEndOfFrame();
		LeanTest.expect(Mathf.Abs(this.timeElapsedNormalTimeScale - this.timeElapsedIgnoreTimeScale) < 0.15f, "START IGNORE TIMING", string.Concat(new object[]
		{
			"timeElapsedIgnoreTimeScale:",
			this.timeElapsedIgnoreTimeScale,
			" timeElapsedNormalTimeScale:",
			this.timeElapsedNormalTimeScale
		}));
		Time.timeScale = 4f;
		int pauseCount = 0;
		LeanTween.value(base.gameObject, 0f, 1f, 1f).setOnUpdate(delegate(float val)
		{
			pauseCount++;
		}).pause();
		Vector3[] roundCirc = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(-9.1f, 25.1f, 0f),
			new Vector3(-1.2f, 15.9f, 0f),
			new Vector3(-25f, 25f, 0f),
			new Vector3(-25f, 25f, 0f),
			new Vector3(-50.1f, 15.9f, 0f),
			new Vector3(-40.9f, 25.1f, 0f),
			new Vector3(-50f, 0f, 0f),
			new Vector3(-50f, 0f, 0f),
			new Vector3(-40.9f, -25.1f, 0f),
			new Vector3(-50.1f, -15.9f, 0f),
			new Vector3(-25f, -25f, 0f),
			new Vector3(-25f, -25f, 0f),
			new Vector3(0f, -15.9f, 0f),
			new Vector3(-9.1f, -25.1f, 0f),
			new Vector3(0f, 0f, 0f)
		};
		GameObject cubeRound = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
		cubeRound.name = "bRound";
		Vector3 onStartPos = cubeRound.transform.position;
		LeanTween.moveLocal(cubeRound, roundCirc, 0.5f).setOnComplete(delegate()
		{
			LeanTest.expect(cubeRound.transform.position == onStartPos, "BEZIER CLOSED LOOP SHOULD END AT START", string.Concat(new object[]
			{
				"onStartPos:",
				onStartPos,
				" onEnd:",
				cubeRound.transform.position
			}));
		});
		Vector3[] roundSpline = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(0.9f, 2f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 0f)
		};
		GameObject cubeSpline = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
		cubeSpline.name = "bSpline";
		Vector3 onStartPosSpline = cubeSpline.transform.position;
		LeanTween.moveSplineLocal(cubeSpline, roundSpline, 0.5f).setOnComplete(delegate()
		{
			LeanTest.expect(Vector3.Distance(onStartPosSpline, cubeSpline.transform.position) <= 0.01f, "BEZIER CLOSED LOOP SHOULD END AT START", string.Concat(new object[]
			{
				"onStartPos:",
				onStartPosSpline,
				" onEnd:",
				cubeSpline.transform.position,
				" dist:",
				Vector3.Distance(onStartPosSpline, cubeSpline.transform.position)
			}));
		});
		this.groupTweens = new LTDescr[1200];
		this.groupGOs = new GameObject[this.groupTweens.Length];
		this.groupTweensCnt = 0;
		int descriptionMatchCount = 0;
		for (int i = 0; i < this.groupTweens.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
			gameObject.name = "c" + i;
			gameObject.transform.position = new Vector3(0f, 0f, (float)(i * 3));
			this.groupGOs[i] = gameObject;
		}
		yield return new WaitForEndOfFrame();
		bool hasGroupTweensCheckStarted = false;
		int setOnStartNum = 0;
		for (int j = 0; j < this.groupTweens.Length; j++)
		{
			this.groupTweens[j] = LeanTween.move(this.groupGOs[j], base.transform.position + Vector3.one * 3f, 3f).setOnStart(delegate
			{
				setOnStartNum++;
			}).setOnComplete(delegate()
			{
				if (!hasGroupTweensCheckStarted)
				{
					hasGroupTweensCheckStarted = true;
					LeanTween.delayedCall(this.gameObject, 0.1f, delegate()
					{
						LeanTest.expect(setOnStartNum == this.groupTweens.Length, "SETONSTART CALLS", string.Concat(new object[]
						{
							"expected:",
							this.groupTweens.Length,
							" was:",
							setOnStartNum
						}));
						LeanTest.expect(this.groupTweensCnt == this.groupTweens.Length, "GROUP FINISH", string.Concat(new object[]
						{
							"expected ",
							this.groupTweens.Length,
							" tweens but got ",
							this.groupTweensCnt
						}));
					});
				}
				this.groupTweensCnt++;
			});
			if (LeanTween.description(this.groupTweens[j].id).trans == this.groupTweens[j].trans)
			{
				descriptionMatchCount++;
			}
		}
		while (LeanTween.tweensRunning < this.groupTweens.Length)
		{
			yield return null;
		}
		LeanTest.expect(descriptionMatchCount == this.groupTweens.Length, "GROUP IDS MATCH", null);
		LeanTest.expect(LeanTween.maxSearch <= this.groupTweens.Length + 5, "MAX SEARCH OPTIMIZED", "maxSearch:" + LeanTween.maxSearch);
		LeanTest.expect(LeanTween.isTweening((GameObject)null), "SOMETHING IS TWEENING", null);
		float previousXlt4 = this.cube4.transform.position.x;
		this.lt4 = LeanTween.moveX(this.cube4, 5f, 1.1f).setOnComplete(delegate()
		{
			LeanTest.expect(this.cube4 != null && previousXlt4 != this.cube4.transform.position.x, "RESUME OUT OF ORDER", string.Concat(new object[]
			{
				"cube4:",
				this.cube4,
				" previousXlt4:",
				previousXlt4,
				" cube4.transform.position.x:",
				(!(this.cube4 != null)) ? 0f : this.cube4.transform.position.x
			}));
		});
		this.lt4.resume();
		this.rotateRepeat = (this.rotateRepeatAngle = 0);
		LeanTween.rotateAround(this.cube3, Vector3.forward, 360f, 0.1f).setRepeat(3).setOnComplete(new Action(this.rotateRepeatFinished)).setOnCompleteOnRepeat(true).setDestroyOnComplete(true);
		yield return new WaitForEndOfFrame();
		LeanTween.delayedCall(1.8f, new Action(this.rotateRepeatAllFinished));
		int countBeforeCancel = LeanTween.tweensRunning;
		LeanTween.cancel(this.lt1Id);
		LeanTest.expect(countBeforeCancel == LeanTween.tweensRunning, "CANCEL AFTER RESET SHOULD FAIL", string.Concat(new object[]
		{
			"expected ",
			countBeforeCancel,
			" but got ",
			LeanTween.tweensRunning
		}));
		LeanTween.cancel(this.cube2);
		int tweenCount = 0;
		for (int k = 0; k < this.groupTweens.Length; k++)
		{
			if (LeanTween.isTweening(this.groupGOs[k]))
			{
				tweenCount++;
			}
			if (k % 3 == 0)
			{
				LeanTween.pause(this.groupGOs[k]);
			}
			else if (k % 3 == 1)
			{
				this.groupTweens[k].pause();
			}
			else
			{
				LeanTween.pause(this.groupTweens[k].id);
			}
		}
		LeanTest.expect(tweenCount == this.groupTweens.Length, "GROUP ISTWEENING", string.Concat(new object[]
		{
			"expected ",
			this.groupTweens.Length,
			" tweens but got ",
			tweenCount
		}));
		yield return new WaitForEndOfFrame();
		tweenCount = 0;
		for (int l = 0; l < this.groupTweens.Length; l++)
		{
			if (l % 3 == 0)
			{
				LeanTween.resume(this.groupGOs[l]);
			}
			else if (l % 3 == 1)
			{
				this.groupTweens[l].resume();
			}
			else
			{
				LeanTween.resume(this.groupTweens[l].id);
			}
			if ((l % 2 != 0) ? LeanTween.isTweening(this.groupGOs[l]) : LeanTween.isTweening(this.groupTweens[l].id))
			{
				tweenCount++;
			}
		}
		LeanTest.expect(tweenCount == this.groupTweens.Length, "GROUP RESUME", null);
		LeanTest.expect(!LeanTween.isTweening(this.cube1), "CANCEL TWEEN LTDESCR", null);
		LeanTest.expect(!LeanTween.isTweening(this.cube2), "CANCEL TWEEN LEANTWEEN", null);
		LeanTest.expect(pauseCount == 0, "ON UPDATE NOT CALLED DURING PAUSE", "expect pause count of 0, but got " + pauseCount);
		yield return new WaitForEndOfFrame();
		Time.timeScale = 0.25f;
		float tweenTime = 0.2f;
		float expectedTime = tweenTime * (1f / Time.timeScale);
		float start = Time.realtimeSinceStartup;
		bool onUpdateWasCalled = false;
		LeanTween.moveX(this.cube1, -5f, tweenTime).setOnUpdate(delegate(float val)
		{
			onUpdateWasCalled = true;
		}).setOnComplete(delegate()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float num = realtimeSinceStartup - start;
			LeanTest.expect(Mathf.Abs(expectedTime - num) < 0.05f, "SCALED TIMING DIFFERENCE", string.Concat(new object[]
			{
				"expected to complete in roughly ",
				expectedTime,
				" but completed in ",
				num
			}));
			LeanTest.expect(Mathf.Approximately(this.cube1.transform.position.x, -5f), "SCALED ENDING POSITION", "expected to end at -5f, but it ended at " + this.cube1.transform.position.x);
			LeanTest.expect(onUpdateWasCalled, "ON UPDATE FIRED", null);
		});
		yield return new WaitForSeconds(expectedTime);
		Time.timeScale = 1f;
		int ltCount = 0;
		GameObject[] allGos = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject gameObject2 in allGos)
		{
			if (gameObject2.name == "~LeanTween")
			{
				ltCount++;
			}
		}
		LeanTest.expect(ltCount == 1, "RESET CORRECTLY CLEANS UP", null);
		this.lotsOfCancels();
		yield break;
	}

	private IEnumerator lotsOfCancels()
	{
		yield return new WaitForEndOfFrame();
		Time.timeScale = 4f;
		int cubeCount = 10;
		int[] tweensA = new int[cubeCount];
		GameObject[] aGOs = new GameObject[cubeCount];
		for (int i = 0; i < aGOs.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
			gameObject.transform.position = new Vector3(0f, 0f, (float)i * 2f);
			gameObject.name = "a" + i;
			aGOs[i] = gameObject;
			tweensA[i] = LeanTween.move(gameObject, gameObject.transform.position + new Vector3(10f, 0f, 0f), 0.5f + 1f * (1f / (float)aGOs.Length)).id;
			LeanTween.color(gameObject, Color.red, 0.01f);
		}
		yield return new WaitForSeconds(1f);
		int[] tweensB = new int[cubeCount];
		GameObject[] bGOs = new GameObject[cubeCount];
		for (int j = 0; j < bGOs.Length; j++)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.boxNoCollider);
			gameObject2.transform.position = new Vector3(0f, 0f, (float)j * 2f);
			gameObject2.name = "b" + j;
			bGOs[j] = gameObject2;
			tweensB[j] = LeanTween.move(gameObject2, gameObject2.transform.position + new Vector3(10f, 0f, 0f), 2f).id;
		}
		for (int k = 0; k < aGOs.Length; k++)
		{
			LeanTween.cancel(aGOs[k]);
			GameObject gameObject3 = aGOs[k];
			tweensA[k] = LeanTween.move(gameObject3, new Vector3(0f, 0f, (float)k * 2f), 2f).id;
		}
		yield return new WaitForSeconds(0.5f);
		for (int l = 0; l < aGOs.Length; l++)
		{
			LeanTween.cancel(aGOs[l]);
			GameObject gameObject4 = aGOs[l];
			tweensA[l] = LeanTween.move(gameObject4, new Vector3(0f, 0f, (float)l * 2f) + new Vector3(10f, 0f, 0f), 2f).id;
		}
		for (int m = 0; m < bGOs.Length; m++)
		{
			LeanTween.cancel(bGOs[m]);
			GameObject gameObject5 = bGOs[m];
			tweensB[m] = LeanTween.move(gameObject5, new Vector3(0f, 0f, (float)m * 2f), 2f).id;
		}
		yield return new WaitForSeconds(2.1f);
		bool inFinalPlace = true;
		for (int n = 0; n < aGOs.Length; n++)
		{
			if (Vector3.Distance(aGOs[n].transform.position, new Vector3(0f, 0f, (float)n * 2f) + new Vector3(10f, 0f, 0f)) > 0.1f)
			{
				inFinalPlace = false;
			}
		}
		for (int num = 0; num < bGOs.Length; num++)
		{
			if (Vector3.Distance(bGOs[num].transform.position, new Vector3(0f, 0f, (float)num * 2f)) > 0.1f)
			{
				inFinalPlace = false;
			}
		}
		LeanTest.expect(inFinalPlace, "AFTER LOTS OF CANCELS", null);
		yield break;
	}

	private void rotateRepeatFinished()
	{
		if (Mathf.Abs(this.cube3.transform.eulerAngles.z) < 0.0001f)
		{
			this.rotateRepeatAngle++;
		}
		this.rotateRepeat++;
	}

	private void rotateRepeatAllFinished()
	{
		LeanTest.expect(this.rotateRepeatAngle == 3, "ROTATE AROUND MULTIPLE", "expected 3 times received " + this.rotateRepeatAngle + " times");
		LeanTest.expect(this.rotateRepeat == 3, "ROTATE REPEAT", null);
		LeanTest.expect(this.cube3 == null, "DESTROY ON COMPLETE", "cube3:" + this.cube3);
	}

	private void eventGameObjectCalled(LTEvent e)
	{
		this.eventGameObjectWasCalled = true;
	}

	private void eventGeneralCalled(LTEvent e)
	{
		this.eventGeneralWasCalled = true;
	}

	public GameObject cube1;

	public GameObject cube2;

	public GameObject cube3;

	public GameObject cube4;

	private bool eventGameObjectWasCalled;

	private bool eventGeneralWasCalled;

	private int lt1Id;

	private LTDescr lt2;

	private LTDescr lt3;

	private LTDescr lt4;

	private LTDescr[] groupTweens;

	private GameObject[] groupGOs;

	private int groupTweensCnt;

	private int rotateRepeat;

	private int rotateRepeatAngle;

	private GameObject boxNoCollider;

	private float timeElapsedNormalTimeScale;

	private float timeElapsedIgnoreTimeScale;
}
