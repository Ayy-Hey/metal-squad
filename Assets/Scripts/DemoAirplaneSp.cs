using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class DemoAirplaneSp : DemoBaseObject
{
	private void OnEnable()
	{
		base.Init();
		base.Invoke("CallSupport", 0.5f);
	}

	private void OnDisable()
	{
		this.skeletonAnimation.state.Event -= this.HandleEvent;
		this.skeletonAnimation.state.Complete -= this.HandleComplete;
	}

	public void CallSupport()
	{
		this.isStart = true;
		this.Step = 0;
		this.TimeLife = 0f;
		this.skeletonAnimation.state.SetAnimation(0, this.runAnim, true);
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
		MonoBehaviour.print(string.Concat(new object[]
		{
			this.isStart,
			"_",
			this.Step,
			"_",
			this.TimeLife
		}));
	}

	private void Update()
	{
		if (this.isStart)
		{
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
							base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this.posTarget2, Time.deltaTime);
							float num = Vector2.Distance(this.posTarget2, base.transform.localPosition);
							if (num <= 0.1f)
							{
								this.isStart = false;
							}
						}
					}
				}
				else
				{
					this.TimeLife += Time.deltaTime;
					if (this.TimeLife >= 6f)
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
				base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this.posTarget, Time.deltaTime * 2f);
				float num2 = Vector2.Distance(this.posTarget, base.transform.localPosition);
				if (num2 <= 0.1f)
				{
					this.Step = 1;
					this.skeletonAnimation.state.SetAnimation(0, this.attackAnim, true);
				}
			}
		}
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		Vector2 v = new Vector2(this.GunTip1.WorldX - this.GunTip1_1.WorldX, this.GunTip1.WorldY - this.GunTip1_1.WorldY);
		Vector2 v2 = new Vector2(this.GunTip1.WorldX + base.transform.position.x, this.GunTip1.WorldY + base.transform.position.y);
		Vector2 v3 = new Vector2(this.GunTip2.WorldX - this.GunTip2_1.WorldX, this.GunTip2.WorldY - this.GunTip2_1.WorldY);
		Vector2 v4 = new Vector2(this.GunTip2.WorldX + base.transform.position.x, this.GunTip2.WorldY + base.transform.position.y);
		Vector2 v5 = new Vector2(this.GunTip3.WorldX - this.GunTip3_1.WorldX, this.GunTip3.WorldY - this.GunTip3_1.WorldY);
		Vector2 v6 = new Vector2(this.GunTip3.WorldX + base.transform.position.x, this.GunTip3.WorldY + base.transform.position.y);
		Vector2 v7 = new Vector2(this.GunTip4.WorldX - this.GunTip4_1.WorldX, this.GunTip4.WorldY - this.GunTip4_1.WorldY);
		Vector2 v8 = new Vector2(this.GunTip4.WorldX + base.transform.position.x, this.GunTip4.WorldY + base.transform.position.y);
		Vector2 v9 = new Vector2(this.GunTip5.WorldX - this.GunTip5_1.WorldX, this.GunTip5.WorldY - this.GunTip5_1.WorldY);
		Vector2 v10 = new Vector2(this.GunTip5.WorldX + base.transform.position.x, this.GunTip5.WorldY + base.transform.position.y);
		Vector2 v11 = new Vector2(this.GunTip6.WorldX - this.GunTip6_1.WorldX, this.GunTip6.WorldY - this.GunTip6_1.WorldY);
		Vector2 v12 = new Vector2(this.GunTip6.WorldX + base.transform.position.x, this.GunTip6.WorldY + base.transform.position.y);
		int num = UnityEngine.Random.Range(0, 5);
		this.bullet = DemoSpecialSkillManager.instance.bullet1Pool.New();
		if (this.bullet != null)
		{
			string name = e.Data.Name;
			switch (name)
			{
			case "shoot01":
				this.bullet.transform.rotation = this.MakeRotation(v);
				this.bullet.transform.position = v2;
				break;
			case "shoot02":
				this.bullet.transform.rotation = this.MakeRotation(v3);
				this.bullet.transform.position = v4;
				break;
			case "shoot03":
				this.bullet.transform.rotation = this.MakeRotation(v5);
				this.bullet.transform.position = v6;
				break;
			case "shoot04":
				this.bullet.transform.rotation = this.MakeRotation(v7);
				this.bullet.transform.position = v8;
				break;
			case "shoot05":
				this.bullet.transform.rotation = this.MakeRotation(v9);
				this.bullet.transform.position = v10;
				break;
			case "shoot06":
				this.bullet.transform.rotation = this.MakeRotation(v11);
				this.bullet.transform.position = v12;
				break;
			}
			this.bullet.gameObject.SetActive(true);
		}
		else
		{
			MonoBehaviour.print("null bullet");
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

	private Quaternion MakeRotation(Vector3 dir)
	{
		Quaternion result = Quaternion.LookRotation(dir);
		result.x = 0f;
		result.y = 0f;
		result.eulerAngles = new Vector3(0f, 0f, result.eulerAngles.z + 180f);
		return result;
	}

	private const int TIME_LIFE = 6;

	private bool isStart;

	private int Step;

	private float TimeLife;

	private float delayStep;

	[SpineAnimation("", "", true, false)]
	public string runAnim;

	[SpineAnimation("", "", true, false)]
	public string attackAnim;

	public SkeletonAnimation skeletonAnimation;

	public Vector3 posTarget;

	public Vector3 posTarget2;

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

	private DemoBullet1 bullet;
}
