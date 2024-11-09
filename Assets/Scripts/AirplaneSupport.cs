using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class AirplaneSupport : MonoBehaviour
{
	public void CallSupport()
	{
		this.isShowDemo = false;
		this.posTarget.x = CameraController.Instance.transform.position.x - 1.66666663f * CameraController.Instance.Size().x;
		this.posTarget.y = CameraController.Instance.transform.position.y + 0.6666667f * CameraController.Instance.Size().y;
		base.transform.position = this.posTarget;
		this.isStart = true;
		this.Step = 0;
		this.countAnim = 0;
		this.skeletonAnimation.state.SetAnimation(0, this.runAnim, true);
		this.TimeLife = 0f;
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this.GunTip1 = this.skeletonAnimation.skeleton.FindBone("Gun_tip01");
		this.GunTip1_1 = this.skeletonAnimation.skeleton.FindBone("Gun_tip01_01");
		this.GunTip2 = this.skeletonAnimation.skeleton.FindBone("Gun_tip02");
		this.GunTip2_1 = this.skeletonAnimation.skeleton.FindBone("Gun_tip02_02");
		this.GunTip3 = this.skeletonAnimation.skeleton.FindBone("Gun_tip03");
		this.GunTip3_1 = this.skeletonAnimation.skeleton.FindBone("Gun_tip03_03");
		this.GunTip4 = this.skeletonAnimation.skeleton.FindBone("Gun_tip04");
		this.GunTip4_1 = this.skeletonAnimation.skeleton.FindBone("Gun_tip04_04");
		this.GunTip5 = this.skeletonAnimation.skeleton.FindBone("Gun_tip05");
		this.GunTip5_1 = this.skeletonAnimation.skeleton.FindBone("Gun_tip05_05");
		this.GunTip6 = this.skeletonAnimation.skeleton.FindBone("Gun_tip06");
		this.GunTip6_1 = this.skeletonAnimation.skeleton.FindBone("Gun_tip06_07");
		this.GunTip7 = this.skeletonAnimation.skeleton.FindBone("Gun_tip07");
		this.GunTip7_1 = this.skeletonAnimation.skeleton.FindBone("Gun_tip07_07");
		if (ProfileManager.settingProfile.IsSound)
		{
			this.mAudio.Play();
		}
	}

	private void Update()
	{
		if (!this.isStart || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		this.posTarget.x = CameraController.Instance.transform.position.x - 0.5f * CameraController.Instance.Size().x;
		this.posTarget.y = CameraController.Instance.transform.position.y + 0.6666667f * CameraController.Instance.Size().y;
		int step = this.Step;
		if (step != 0)
		{
			if (step != 1)
			{
				if (step == 2)
				{
					this.delayStep += Time.deltaTime;
					if (this.delayStep >= 1f)
					{
						Vector2 v = default(Vector2);
						v.x = CameraController.Instance.transform.position.x + 2f * CameraController.Instance.Size().x;
						v.y = CameraController.Instance.transform.position.y + 0.6666667f * CameraController.Instance.Size().y;
						base.transform.position = Vector3.Lerp(base.transform.position, v, Time.deltaTime * 3f);
						if (!CameraController.Instance.IsInView(base.transform))
						{
							this.isStart = false;
							UnityEngine.Object.Destroy(base.gameObject);
						}
					}
				}
			}
			else
			{
				base.transform.position = this.posTarget;
				this.TimeLife += Time.deltaTime;
				if (this.TimeLife >= 10f && !this.isShowDemo)
				{
					this.Step = 2;
					this.delayStep = 0f;
					this.skeletonAnimation.state.ClearTracks();
					this.skeletonAnimation.state.SetAnimation(0, this.runAnim, true);
				}
			}
		}
		else
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.posTarget, Time.deltaTime * 5f);
			float num = Vector2.Distance(this.posTarget, base.transform.position);
			if (num <= 0.1f)
			{
				this.Step = 1;
				this.skeletonAnimation.state.SetAnimation(0, this.attackAnim, true);
			}
		}
	}

	public void SetCompleted()
	{
		this.isShowDemo = false;
		this.Step = 2;
		this.delayStep = 0f;
		this.skeletonAnimation.state.ClearTracks();
		this.skeletonAnimation.state.SetAnimation(0, this.runAnim, true);
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		Vector2 direction = new Vector2(this.GunTip1.WorldX - this.GunTip1_1.WorldX, this.GunTip1.WorldY - this.GunTip1_1.WorldY);
		Vector2 v = new Vector2(this.GunTip1.WorldX + base.transform.position.x, this.GunTip1.WorldY + base.transform.position.y);
		Vector2 direction2 = new Vector2(this.GunTip2.WorldX - this.GunTip2_1.WorldX, this.GunTip2.WorldY - this.GunTip2_1.WorldY);
		Vector2 v2 = new Vector2(this.GunTip2.WorldX + base.transform.position.x, this.GunTip2.WorldY + base.transform.position.y);
		Vector2 direction3 = new Vector2(this.GunTip3.WorldX - this.GunTip3_1.WorldX, this.GunTip3.WorldY - this.GunTip3_1.WorldY);
		Vector2 v3 = new Vector2(this.GunTip3.WorldX + base.transform.position.x, this.GunTip3.WorldY + base.transform.position.y);
		Vector2 direction4 = new Vector2(this.GunTip4.WorldX - this.GunTip4_1.WorldX, this.GunTip4.WorldY - this.GunTip4_1.WorldY);
		Vector2 v4 = new Vector2(this.GunTip4.WorldX + base.transform.position.x, this.GunTip4.WorldY + base.transform.position.y);
		Vector2 direction5 = new Vector2(this.GunTip6.WorldX - this.GunTip6_1.WorldX, this.GunTip6.WorldY - this.GunTip6_1.WorldY);
		Vector2 v5 = new Vector2(this.GunTip6.WorldX + base.transform.position.x, this.GunTip6.WorldY + base.transform.position.y);
		int num = UnityEngine.Random.Range(0, 5);
		float damage = ProfileManager.airplaneSkillProfile.Damaged;
		int num2 = ProfileManager.airplaneSkillProfile.NumberBullet;
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
		{
			num2 = 10;
			damage = 5f;
		}
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "shoot01"))
			{
				if (!(name == "shoot02"))
				{
					if (!(name == "shoot03"))
					{
						if (!(name == "shoot04"))
						{
							if (!(name == "shoot06"))
							{
								if (!(name == "Death"))
								{
								}
							}
							else if (num2 > 4)
							{
								BulletWithTail bulletWithTail = GameManager.Instance.bulletManager.CreatBulletWithTail(direction5, v5, damage, (float)UnityEngine.Random.Range(10, 15));
								bulletWithTail.transform.position = v5;
							}
						}
						else if (num2 > 3)
						{
							if (num == 0)
							{
								BulletWithTail bulletWithTail2 = GameManager.Instance.bulletManager.CreatBulletWithTail2(direction4, v4, damage, (float)UnityEngine.Random.Range(8, 15));
								bulletWithTail2.transform.position = v4;
							}
							else
							{
								BulletWithTail bulletWithTail3 = GameManager.Instance.bulletManager.CreatBulletWithTail(direction4, v4, damage, (float)UnityEngine.Random.Range(8, 15));
								bulletWithTail3.transform.position = v4;
							}
						}
					}
					else if (num == 0)
					{
						BulletWithTail bulletWithTail4 = GameManager.Instance.bulletManager.CreatBulletWithTail2(direction3, v3, damage, (float)UnityEngine.Random.Range(8, 15));
						bulletWithTail4.transform.position = v3;
					}
					else
					{
						BulletWithTail bulletWithTail5 = GameManager.Instance.bulletManager.CreatBulletWithTail(direction3, v3, damage, (float)UnityEngine.Random.Range(8, 15));
						bulletWithTail5.transform.position = v3;
					}
				}
				else if (num == 0)
				{
					BulletWithTail bulletWithTail6 = GameManager.Instance.bulletManager.CreatBulletWithTail2(direction2, v2, damage, (float)UnityEngine.Random.Range(8, 15));
					bulletWithTail6.transform.position = v2;
				}
				else
				{
					BulletWithTail bulletWithTail7 = GameManager.Instance.bulletManager.CreatBulletWithTail(direction2, v2, damage, (float)UnityEngine.Random.Range(8, 15));
					bulletWithTail7.transform.position = v2;
				}
			}
			else if (num == 0)
			{
				BulletWithTail bulletWithTail8 = GameManager.Instance.bulletManager.CreatBulletWithTail2(direction, v, damage, (float)UnityEngine.Random.Range(8, 15));
				bulletWithTail8.transform.position = v;
			}
			else
			{
				BulletWithTail bulletWithTail9 = GameManager.Instance.bulletManager.CreatBulletWithTail(direction, v, damage, (float)UnityEngine.Random.Range(8, 15));
				bulletWithTail9.transform.position = v;
			}
		}
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		string text = entry.ToString();
		if (text != null)
		{
			if (!(text == "attack"))
			{
				if (!(text == "death"))
				{
				}
			}
		}
	}

	private const int TIME_LIFE = 10;

	private bool isStart;

	private int Step;

	private float TimeLife;

	private float delayStep;

	[SpineAnimation("", "", true, false)]
	public string runAnim;

	[SpineAnimation("", "", true, false)]
	public string attackAnim;

	public SkeletonAnimation skeletonAnimation;

	public Bone GunTip1;

	public Bone GunTip1_1;

	public Bone GunTip2;

	public Bone GunTip2_1;

	public Bone GunTip3;

	public Bone GunTip3_1;

	public Bone GunTip4;

	public Bone GunTip4_1;

	public Bone GunTip5;

	public Bone GunTip5_1;

	public Bone GunTip6;

	public Bone GunTip6_1;

	public Bone GunTip7;

	public Bone GunTip7_1;

	public AudioSource mAudio;

	private int countAnim;

	public bool isShowDemo;

	private Vector2 posTarget;
}
