using System;
using System.Collections;
using UnityEngine;

public class EffectExplosion : MonoBehaviour
{
	public IEnumerator Show(int type, Vector3 pos, Vector3 scale, bool isShake, bool isPlaySound = true)
	{
		this._animator.SetTrigger(this.EFFECT_NAME[type]);
		Vector3 rot = new Vector3(0f, 0f, 0f);
		switch (type)
		{
		case 0:
			if (GameManager.Instance.StateManager.EState != EGamePlay.LOST && GameManager.Instance.StateManager.EState != EGamePlay.WIN && isShake)
			{
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			}
			pos.y += 1f;
			if (isPlaySound)
			{
				GameManager.Instance.audioManager.PlayBoom();
			}
			break;
		case 1:
			if (GameManager.Instance.StateManager.EState != EGamePlay.LOST && GameManager.Instance.StateManager.EState != EGamePlay.WIN && isShake)
			{
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			}
			pos.y += 0.7f;
			if (isPlaySound)
			{
				GameManager.Instance.audioManager.PlayBoom();
			}
			break;
		case 2:
			if (GameManager.Instance.StateManager.EState != EGamePlay.LOST && GameManager.Instance.StateManager.EState != EGamePlay.WIN && isShake)
			{
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			}
			if (isPlaySound)
			{
				GameManager.Instance.audioManager.PlayBoom();
			}
			break;
		case 4:
			if (GameManager.Instance.StateManager.EState != EGamePlay.LOST && GameManager.Instance.StateManager.EState != EGamePlay.WIN && isShake)
			{
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			}
			if (isPlaySound)
			{
				GameManager.Instance.audioManager.PlayBoom();
			}
			break;
		case 5:
			if (GameManager.Instance.StateManager.EState != EGamePlay.LOST && GameManager.Instance.StateManager.EState != EGamePlay.WIN && isShake)
			{
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			}
			if (isPlaySound)
			{
				GameManager.Instance.audioManager.PlayBoom();
			}
			break;
		case 6:
			if (GameManager.Instance.StateManager.EState != EGamePlay.LOST && GameManager.Instance.StateManager.EState != EGamePlay.WIN && isShake)
			{
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			}
			if (isPlaySound)
			{
				GameManager.Instance.audioManager.PlayBoom();
			}
			break;
		case 7:
			if (GameManager.Instance.StateManager.EState != EGamePlay.LOST && GameManager.Instance.StateManager.EState != EGamePlay.WIN && isShake)
			{
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			}
			if (isPlaySound)
			{
				GameManager.Instance.audioManager.PlayBoom();
			}
			break;
		}
		base.transform.position = pos;
		base.transform.localScale = scale;
		this._animator.Play(0);
		yield return this.TimeHide[type];
		base.gameObject.SetActive(false);
		yield break;
	}

	private void OnDisable()
	{
		GameManager.Instance.fxManager.EffectExplosionPool.Store(this);
	}

	private string[] EFFECT_NAME = new string[]
	{
		"no1",
		"no2",
		"no3",
		"no4",
		"no5",
		"no6",
		"no7",
		"no8",
		"no9"
	};

	public Animator _animator;

	private WaitForSeconds[] TimeHide = new WaitForSeconds[]
	{
		new WaitForSeconds(0.8f),
		new WaitForSeconds(0.7f),
		new WaitForSeconds(0.4f),
		new WaitForSeconds(0.3f),
		new WaitForSeconds(0.5f),
		new WaitForSeconds(0.5f),
		new WaitForSeconds(0.5f),
		new WaitForSeconds(0.5f),
		new WaitForSeconds(0.5f)
	};
}
