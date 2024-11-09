using System;
using UnityEngine;

public class LineLaserPlayer : MonoBehaviour
{
	private void OnEnable()
	{
		int num;
		if (GameManager.Instance)
		{
			num = this.player.GunCurrent.WeaponCurrent.cacheGunProfile.RankUpgrade;
		}
		else
		{
			num = ((!ProfileManager.weaponsSpecial[3].GetGunBuy()) ? 2 : (ProfileManager.weaponsSpecial[3].GetLevelUpgrade() / 10));
		}
		this.laser.SetMainMaterials(num);
		float size = 0.8f + 0.3f * (float)num;
		this.laser.SetSize(size);
		this.laser.Tilling = new Vector2(0.5f, 1f);
		for (int i = 0; i < this.particles.Length; i++)
		{
			this.particles[i].startColor = this.colors[num];
		}
		this._isShowEff = false;
		this.laser.ActiveEffStart(true);
	}

	private void OnDisable()
	{
		this.laser.DisableEff();
		this.laser.Off();
	}

	private void Update()
	{
		if (!this._isShowEff)
		{
			this._isShowEff = true;
			this.laser.useFx = true;
			this.laser.ActiveEffEnd(true);
		}
		this.startPos = base.transform.position;
		this.endPos = base.transform.right * this.maxLaserRaycastDistance + this.startPos;
		this.hits = Physics2D.RaycastAll(base.transform.position, base.transform.right, this.maxLaserRaycastDistance);
		for (int i = 0; i < this.hits.Length; i++)
		{
			if (this.hits[i].collider)
			{
				try
				{
					IHealth component = this.hits[i].collider.GetComponent<IHealth>();
					if (component != null && !this.hits[i].collider.CompareTag("Rambo"))
					{
						float num = 0f;
						bool flag = false;
						this.player.GunCurrent.WeaponCurrent.cacheGunProfile.GetTrueDamage(out num, out flag);
						if (this.player != null && this.player.IsRemotePlayer)
						{
							num = 0f;
						}
						if (flag)
						{
							GameManager.Instance.fxManager.CreateCritical(this.hits[i].collider.transform.position);
							flag = false;
						}
						component.AddHealthPoint(-num, EWeapon.LASER);
					}
				}
				catch
				{
				}
				if (this.hits[i].collider.gameObject.layer == 20 || this.hits[i].collider.gameObject.layer == 19 || this.hits[i].collider.CompareTag("Boss") || this.hits[i].collider.CompareTag("Tank"))
				{
					this.endPos = this.hits[i].point;
					break;
				}
			}
		}
		this.laser.OnShow(Time.fixedDeltaTime, this.startPos, this.endPos);
	}

	public LaserHandMade laser;

	public ParticleSystem[] particles;

	public Color[] colors;

	public float maxLaserRaycastDistance = 20f;

	private Vector3 startPos;

	private Vector3 endPos;

	private bool _isShowEff;

	private RaycastHit2D[] hits;

	public PlayerMain player;
}
