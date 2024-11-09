using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class DemoEyeBot : DemoBaseObject
{
	private void OnEnable()
	{
		base.transform.position = this.start.position;
		base.Init();
		base.StartCoroutine(this.OnInit());
	}

	private IEnumerator OnInit()
	{
		yield return new WaitUntil(() => this.skeletonAnimation.state != null);
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.skeletonAnimation.state.SetAnimation(0, this.walkAnim1, false);
		LeanTween.move(base.gameObject, this.end.position, 2f).setOnComplete(delegate()
		{
			this.skeletonAnimation.state.SetAnimation(0, this.walkAnim3, false);
		}).setEase(LeanTweenType.easeOutCubic);
		yield break;
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		string name = trackEntry.Animation.Name;
		if (name != null)
		{
			if (!(name == "walk1-1"))
			{
				if (!(name == "walk1-3"))
				{
					if (name == "attack")
					{
						this.Shoot();
					}
				}
				else
				{
					this.skeletonAnimation.state.ClearTrack(trackEntry.TrackIndex);
					this.skeletonAnimation.state.SetAnimation(1, this.aimAnim, false);
					this.skeletonAnimation.state.SetAnimation(2, this.fireAnim, true);
					this.Shoot();
				}
			}
			else
			{
				this.skeletonAnimation.state.ClearTrack(trackEntry.TrackIndex);
				this.skeletonAnimation.state.SetAnimation(0, this.walkAnim2, true);
			}
		}
	}

	private void Shoot()
	{
		Vector2 vector = this.headGun1.position;
		Vector2 pos = this.headGun2.position;
		Vector2 normalized = ((Vector2)this.boneTarget.position - vector).normalized;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, normalized, 1000f);
		this.skeletonAnimation.state.SetAnimation(0, this.fireAnim, true);
		this.CreateBullet(vector, normalized);
		this.CreateBullet(pos, normalized);
	}

	private void CreateBullet(Vector2 pos, Vector2 Direction)
	{
		GameObject bullet;
		if (this.poolBullet.Count > 0)
		{
			bullet = this.poolBullet.Pop();
		}
		else
		{
			bullet = UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, base.transform);
			this.listBullet.Add(bullet);
		}
		bullet.gameObject.SetActive(true);
		bullet.transform.position = pos;
		Quaternion rotation = Quaternion.LookRotation(Direction, -Vector3.forward);
		rotation.x = 0f;
		rotation.y = 0f;
		rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);
		bullet.transform.rotation = rotation;
		Vector2 to = Direction.normalized * 5f + pos;
		LeanTween.move(bullet, to, 0.3f).setOnComplete(delegate()
		{
			bullet.SetActive(false);
			this.poolBullet.Push(bullet);
		});
	}

	private void OnDisable()
	{
		this.skeletonAnimation.state.ClearTracks();
		foreach (GameObject gameObject in this.listBullet)
		{
			if (gameObject.activeSelf && !this.poolBullet.Contains(gameObject))
			{
				LeanTween.cancel(gameObject);
				gameObject.gameObject.SetActive(false);
				this.poolBullet.Push(gameObject);
			}
		}
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	public string walkAnim1;

	[SpineAnimation("", "", true, false)]
	public string walkAnim2;

	[SpineAnimation("", "", true, false)]
	public string walkAnim3;

	[SpineAnimation("", "", true, false)]
	public string fireAnim;

	[SpineAnimation("", "", true, false)]
	public string aimAnim;

	[SerializeField]
	private Transform start;

	[SerializeField]
	private Transform end;

	[SerializeField]
	private Transform boneTarget;

	[SerializeField]
	private Transform headGun1;

	[SerializeField]
	private Transform headGun2;

	private float speed = 0.1f;

	private float timeAttack;

	private float TIME_ATTACK = 0.4f;

	private Stack<GameObject> poolBullet = new Stack<GameObject>();

	private List<GameObject> listBullet = new List<GameObject>();

	[SerializeField]
	private GameObject bulletPrefab;
}
