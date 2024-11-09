using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using Spine;
using Spine.Unity;
using UnityEngine;

public class ThangMay : MonoBehaviour
{
	private void OnValidate()
	{
		if (!this.skeletonAnimationThangMay)
		{
			this.skeletonAnimationThangMay = base.GetComponentInChildren<SkeletonAnimation>();
			Spine.Animation[] items = this.skeletonAnimationThangMay.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
			this.anims = new string[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				this.anims[i] = items[i].Name;
			}
		}
	}

	private void OnEnable()
	{
		if (!this.isInit)
		{
			base.StartCoroutine(this.Init());
			this.isInit = true;
		}
	}

	private IEnumerator Init()
	{
		this._offsetBg = new Vector2(1f, 1f);
		this.skeletonAnimationThangMay.gameObject.SetActive(false);
		this.transGroup.gameObject.SetActive(false);
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		EnemyManager.Instance.Spawns[0].OnCompleted = delegate()
		{
			CameraController.Instance.StopMoveLeftBoundary = this._run;
		};
		this.PCTBStart.OnEnteredTrigger = new Action(this.OnEnteredTrigger);
		yield return new WaitUntil(() => Mathf.Abs(GameManager.Instance.player.transform.position.x - base.transform.position.x) < CameraController.Instance.Size().x * 3f);
		this.skeletonAnimationThangMay.gameObject.SetActive(true);
		this.transGroup.gameObject.SetActive(true);
		this.nextGroup.SetActive(false);
		yield return new WaitUntil(() => Mathf.Abs(base.transform.position.x - CameraController.Instance.parallaxLayer1.transform.position.x) <= CameraController.Instance.Size().x * 2f);
		CameraController.Instance.parallaxLayer1.isFlowCamera = false;
		yield break;
	}

	private void OnEnteredTrigger()
	{
		if (this._run)
		{
			return;
		}
		this._run = true;
		this.preGroup.SetActive(false);
		CameraController.Instance.StopMoveLeftBoundary = true;
		this.skeletonAnimationThangMay.state.SetAnimation(0, this.anims[0], true);
		base.StartCoroutine(this.UpdateThang());
	}

	private IEnumerator UpdateThang()
	{
		while (this._run)
		{
			yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
			float delta = Time.deltaTime;
			this._pos = this.transGroup.position;
			this._pos.y = Mathf.MoveTowards(this._pos.y, this.endY, this.speed * delta);
			this.transGroup.position = this._pos;
			if (this._pos.y == this.endY)
			{
				this._run = false;
				this.skeletonAnimationThangMay.state.ClearTracks();
				CameraController.Instance.StopMoveLeftBoundary = false;
				CameraController.Instance.NewCheckpoint(true, 5f);
				this.nextGroup.SetActive(true);
				float parallaxX = base.transform.position.x + CameraController.Instance.Size().x * 2f;
				CameraController.Instance.parallaxLayer1.transform.position = new Vector3(parallaxX, CameraController.Instance.transform.position.y, 0f);
				yield return new WaitUntil(() => CameraController.Instance.transform.position.x >= parallaxX);
				CameraController.Instance.parallaxLayer1.isFlowCamera = true;
				yield return new WaitUntil(() => CameraController.Instance.transform.position.x >= parallaxX + 2f);
				base.gameObject.SetActive(false);
			}
			yield return 0;
		}
		yield break;
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimationThangMay;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string[] anims;

	[SerializeField]
	private BaseTrigger PCTBStart;

	[SerializeField]
	private Transform transGroup;

	[SerializeField]
	private GameObject preGroup;

	[SerializeField]
	private GameObject nextGroup;

	[SerializeField]
	private float speed;

	[SerializeField]
	private float endY;

	private bool _run;

	private Vector3 _pos;

	private float oldRightBoundary;

	private Material _matBg;

	private Vector2 _offsetBg;

	private float _speedMoveBg;

	private bool isInit;
}
