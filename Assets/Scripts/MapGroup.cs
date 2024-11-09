using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGroup : MonoBehaviour
{
	private void LateUpdate()
	{
		if (CameraController.Instance.isInit)
		{
			for (int i = 0; i < this.maps.Count; i++)
			{
				this.maps[i].CheckMapView();
			}
		}
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
		if (modePlay == GameMode.ModePlay.Campaign || modePlay == GameMode.ModePlay.Special_Campaign || modePlay == GameMode.ModePlay.Endless)
		{
			this.OnUpdate();
		}
	}

	public void OnUpdate()
	{
		for (int i = 0; i < this.maps.Count; i++)
		{
			this.maps[i].OnUpdate(this.offsetSize);
		}
	}

	public List<MapElement> maps;

	private Vector2 offsetSize = new Vector2(2f, 2f);
}
