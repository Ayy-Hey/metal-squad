using System;
using UnityEngine;

public class GeneralEventsListenersCS : MonoBehaviour
{
	private void Awake()
	{
		LeanTween.LISTENERS_MAX = 100;
		LeanTween.EVENTS_MAX = 2;
		this.fromColor = base.GetComponent<Renderer>().material.color;
	}

	private void Start()
	{
		LeanTween.addListener(base.gameObject, 0, new Action<LTEvent>(this.changeColor));
		LeanTween.addListener(base.gameObject, 1, new Action<LTEvent>(this.jumpUp));
	}

	private void jumpUp(LTEvent e)
	{
		base.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 300f);
	}

	private void changeColor(LTEvent e)
	{
		Transform transform = (Transform)e.data;
		float num = Vector3.Distance(transform.position, base.transform.position);
		Color to = new Color(UnityEngine.Random.Range(0f, 1f), 0f, UnityEngine.Random.Range(0f, 1f));
		LeanTween.value(base.gameObject, this.fromColor, to, 0.8f).setLoopPingPong(1).setDelay(num * 0.05f).setOnUpdate(delegate(Color col)
		{
			base.GetComponent<Renderer>().material.color = col;
		});
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer != 2)
		{
			this.towardsRotation = new Vector3(0f, (float)UnityEngine.Random.Range(-180, 180), 0f);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.layer != 2)
		{
			this.turnForIter = 0f;
			this.turnForLength = UnityEngine.Random.Range(0.5f, 1.5f);
		}
	}

	private void FixedUpdate()
	{
		if (this.turnForIter < this.turnForLength)
		{
			base.GetComponent<Rigidbody>().MoveRotation(base.GetComponent<Rigidbody>().rotation * Quaternion.Euler(this.towardsRotation * Time.deltaTime));
			this.turnForIter += Time.deltaTime;
		}
		base.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 4.5f);
	}

	private void OnMouseDown()
	{
		if (UnityEngine.Input.GetKey(KeyCode.J))
		{
			LeanTween.dispatchEvent(1);
		}
		else
		{
			LeanTween.dispatchEvent(0, base.transform);
		}
	}

	private Vector3 towardsRotation;

	private float turnForLength = 0.5f;

	private float turnForIter;

	private Color fromColor;

	public enum MyEvents
	{
		CHANGE_COLOR,
		JUMP,
		LENGTH
	}
}
