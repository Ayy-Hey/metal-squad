using System;
using UnityEngine;

public class PathSplines : MonoBehaviour
{
	private void OnEnable()
	{
		this.cr = new LTSpline(new Vector3[]
		{
			this.trans[0].position,
			this.trans[1].position,
			this.trans[2].position,
			this.trans[3].position,
			this.trans[4].position
		});
	}

	private void Start()
	{
		this.avatar1 = GameObject.Find("Avatar1");
	}

	private void Update()
	{
		this.cr.place(this.avatar1.transform, this.iter);
		this.iter += Time.deltaTime * 0.7f;
		if (this.iter > 1f)
		{
			this.iter = 0f;
		}
	}

	private void OnDrawGizmos()
	{
		if (this.cr != null)
		{
			this.OnEnable();
		}
		Gizmos.color = Color.red;
		if (this.cr != null)
		{
			this.cr.gizmoDraw(-1f);
		}
	}

	public Transform[] trans;

	private LTSpline cr;

	private GameObject avatar1;

	private float iter;
}
