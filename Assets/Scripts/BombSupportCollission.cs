using System;
using UnityEngine;

public class BombSupportCollission : CachingMonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!this.parrent.isInit)
		{
			return;
		}
		bool flag = other.CompareTag("Ground");
		if (CameraController.Instance.orientaltion == CameraController.Orientation.VERTICAL)
		{
			flag = other.CompareTag("Obstacle");
		}
		if (flag)
		{
			CameraController.Instance.Shake(CameraController.ShakeType.ShakeEnemy);
			GameManager.Instance.fxManager.ShowEffect(0, this.parrent.transform.position, Vector3.one, true, true);
			this.parrent.box2D.enabled = false;
			this.parrent.isInit = false;
			this.parrent.gameObject.SetActive(false);
		}
	}

	public BombSupport parrent;
}
