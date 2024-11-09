using System;
using UnityEngine;

public class BayCauManager : MonoBehaviour
{
	public static BayCauManager Instance
	{
		get
		{
			if (BayCauManager.instance == null)
			{
				BayCauManager.instance = UnityEngine.Object.FindObjectOfType<BayCauManager>();
			}
			return BayCauManager.instance;
		}
	}

	private void Start()
	{
		this.InitBayCau();
		for (int i = 0; i < this.bayCaus.Length; i++)
		{
			this.bayCaus[i].OnStart();
		}
	}

	private void Update()
	{
		if (!GameManager.Instance || GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
		{
			for (int i = 0; i < this.bayCaus.Length; i++)
			{
				this.bayCaus[i].OnUpdate(Time.deltaTime);
			}
		}
	}

	public void InitBayCau()
	{
		for (int i = 0; i < this.bayCaus.Length; i++)
		{
			BayCau.Style style = this.bayCaus[i].style;
			if (style != BayCau.Style.Falling)
			{
				if (style != BayCau.Style.Moving)
				{
					if (style == BayCau.Style.Spinning)
					{
						this.AddFallingAction(this.spinningEff, this.bayCaus[i]);
					}
				}
				else
				{
					this.AddFallingAction(this.movingEff, this.bayCaus[i]);
				}
			}
			else
			{
				this.AddFallingAction(this.fallingEff, this.bayCaus[i]);
			}
		}
	}

	private void AddFallingAction(BayCauManager.E_Eff_Type type, BayCau bay)
	{
		if (type != BayCauManager.E_Eff_Type.None)
		{
			if (type != BayCauManager.E_Eff_Type.Eff_Spine)
			{
				bay.OnActionStart = delegate(BayCau bayCau)
				{
					GameManager.Instance.fxManager.ShowEffect((int)this.fallingEff, bayCau.transform.position, Vector3.one, true, true);
				};
			}
			else
			{
				bay.OnActionStart = delegate(BayCau bayCau)
				{
					GameManager.Instance.fxManager.ShowExplosionSpine(bayCau.transform.position, 0);
				};
			}
		}
	}

	private static BayCauManager instance;

	[SerializeField]
	private BayCauManager.E_Eff_Type fallingEff = BayCauManager.E_Eff_Type.None;

	[SerializeField]
	private BayCauManager.E_Eff_Type movingEff = BayCauManager.E_Eff_Type.None;

	[SerializeField]
	private BayCauManager.E_Eff_Type spinningEff = BayCauManager.E_Eff_Type.None;

	[SerializeField]
	private BayCau[] bayCaus;

	private enum E_Eff_Type
	{
		None = -1,
		Eff_0,
		Eff_3 = 3,
		Eff_4,
		Eff_5,
		Eff_6,
		Eff_8 = 8,
		Eff_Spine = 10
	}
}
