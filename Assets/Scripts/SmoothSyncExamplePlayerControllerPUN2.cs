using System;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Smooth;
using UnityEngine;

public class SmoothSyncExamplePlayerControllerPUN2 : MonoBehaviourPunCallbacks
{
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.rb2D = base.GetComponent<Rigidbody2D>();
		this.smoothSync = base.GetComponent<SmoothSyncPUN2>();
		this.childSmoothSync = base.transform.GetChild(0).GetComponent<SmoothSyncPUN2>();
		if (this.smoothSync)
		{
			SmoothSyncPUN2 smoothSyncPUN = this.smoothSync;
			if (SmoothSyncExamplePlayerControllerPUN2._003C_003Ef__mg_0024cache0 == null)
			{
				SmoothSyncExamplePlayerControllerPUN2._003C_003Ef__mg_0024cache0 = new SmoothSyncPUN2.validateStateDelegate(SmoothSyncExamplePlayerControllerPUN2.validateStateOfPlayer);
			}
			smoothSyncPUN.validateStateMethod = SmoothSyncExamplePlayerControllerPUN2._003C_003Ef__mg_0024cache0;
		}
	}

	private void Update()
	{
		if (!base.photonView.IsMine)
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.T))
		{
			base.transform.position = base.transform.position + Vector3.right * 18f;
			this.smoothSync.teleport();
			this.childSmoothSync.teleport();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.F))
		{
			this.smoothSync.forceStateSendNextOnPhotonSerializeView();
		}
		float d = this.transformMovementSpeed * Time.deltaTime;
		if (UnityEngine.Input.GetKey(KeyCode.LeftShift) && UnityEngine.Input.GetKey(KeyCode.Equals))
		{
			base.transform.localScale = base.transform.localScale + new Vector3(1f, 1f, 1f) * d * 0.2f;
		}
		if (UnityEngine.Input.GetKey(KeyCode.LeftShift) && UnityEngine.Input.GetKey(KeyCode.Minus))
		{
			base.transform.localScale = base.transform.localScale - new Vector3(1f, 1f, 1f) * d * 0.2f;
		}
		if (this.childObjectToControl)
		{
			if (UnityEngine.Input.GetKey(KeyCode.RightShift) && UnityEngine.Input.GetKey(KeyCode.Equals))
			{
				this.childObjectToControl.transform.localScale = this.childObjectToControl.transform.localScale + new Vector3(1f, 1f, 1f) * d * 0.2f;
			}
			if (UnityEngine.Input.GetKey(KeyCode.RightShift) && UnityEngine.Input.GetKey(KeyCode.Minus))
			{
				this.childObjectToControl.transform.localScale = this.childObjectToControl.transform.localScale - new Vector3(1f, 1f, 1f) * d * 0.2f;
			}
		}
		if (this.childObjectToControl)
		{
			if (UnityEngine.Input.GetKey(KeyCode.S))
			{
				this.childObjectToControl.transform.position = this.childObjectToControl.transform.position + new Vector3(0f, -1.5f, -1f) * d;
			}
			if (UnityEngine.Input.GetKey(KeyCode.W))
			{
				this.childObjectToControl.transform.position = this.childObjectToControl.transform.position + new Vector3(0f, 1.5f, 1f) * d;
			}
			if (UnityEngine.Input.GetKey(KeyCode.A))
			{
				this.childObjectToControl.transform.position = this.childObjectToControl.transform.position + new Vector3(-1f, 0f, 0f) * d;
			}
			if (UnityEngine.Input.GetKey(KeyCode.D))
			{
				this.childObjectToControl.transform.position = this.childObjectToControl.transform.position + new Vector3(1f, 0f, 0f) * d;
			}
		}
		if (this.rb)
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0))
			{
				this.rb.velocity = Vector3.zero;
				this.rb.angularVelocity = Vector3.zero;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.rb.AddForce(new Vector3(0f, -1.5f, -1f) * this.rigidbodyMovementForce);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.rb.AddForce(new Vector3(0f, 1.5f, 1f) * this.rigidbodyMovementForce);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.rb.AddForce(new Vector3(-1f, 0f, 0f) * this.rigidbodyMovementForce);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.rb.AddForce(new Vector3(1f, 0f, 0f) * this.rigidbodyMovementForce);
			}
		}
		else if (this.rb2D)
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0))
			{
				this.rb2D.velocity = Vector3.zero;
				this.rb2D.angularVelocity = 0f;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.rb2D.AddForce(new Vector3(0f, -1.5f, -1f) * this.rigidbodyMovementForce);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.rb2D.AddForce(new Vector3(0f, 1.5f, 1f) * this.rigidbodyMovementForce);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.rb2D.AddForce(new Vector3(-1f, 0f, 0f) * this.rigidbodyMovementForce);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.rb2D.AddForce(new Vector3(1f, 0f, 0f) * this.rigidbodyMovementForce);
			}
		}
		else
		{
			if (UnityEngine.Input.GetKey(KeyCode.DownArrow))
			{
				base.transform.position = base.transform.position + new Vector3(0f, -1.5f, -1f) * d;
			}
			if (UnityEngine.Input.GetKey(KeyCode.UpArrow))
			{
				base.transform.position = base.transform.position + new Vector3(0f, 1.5f, 1f) * d;
			}
			if (UnityEngine.Input.GetKey(KeyCode.LeftArrow))
			{
				base.transform.position = base.transform.position + new Vector3(-1f, 0f, 0f) * d;
			}
			if (UnityEngine.Input.GetKey(KeyCode.RightArrow))
			{
				base.transform.position = base.transform.position + new Vector3(1f, 0f, 0f) * d;
			}
		}
	}

	public static bool validateStateOfPlayer(StatePUN2 latestReceivedState, StatePUN2 latestValidatedState)
	{
		return Vector3.Distance(latestReceivedState.position, latestValidatedState.position) <= 9000f || latestReceivedState.ownerTimestamp - latestValidatedState.receivedOnServerTimestamp >= 0.5f;
	}

	private Rigidbody rb;

	private Rigidbody2D rb2D;

	private SmoothSyncPUN2 smoothSync;

	private SmoothSyncPUN2 childSmoothSync;

	public float transformMovementSpeed = 30f;

	public float rigidbodyMovementForce = 500f;

	public GameObject childObjectToControl;

	[CompilerGenerated]
	private static SmoothSyncPUN2.validateStateDelegate _003C_003Ef__mg_0024cache0;
}
