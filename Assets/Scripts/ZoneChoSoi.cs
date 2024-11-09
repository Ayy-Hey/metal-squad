using System;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class ZoneChoSoi : MonoBehaviour
{
	private void Start()
	{
		this.triggerBoundary.OnEnteredTrigger = delegate()
		{
			if (!this._isEnteredTrigger)
			{
				this._isEnteredTrigger = true;
				for (int i = 0; i < this.choSois.Count; i++)
				{
					if (this.choSois[i] && !this.choSois[i].isInit)
					{
						this.choSois[i].Init(this.levelChoSoi, delegate
						{
							for (int j = 0; j < this.choSois.Count; j++)
							{
								if (this.choSois[j] && this.choSois[j].isInit)
								{
									return;
								}
							}
							CameraController.Instance.NewCheckpoint(true, 15f);
						});
					}
				}
				this.triggerBoundary.NumericBoundaries.OnBoundariesTransitionFinished = delegate()
				{
					this.triggerBoundary.gameObject.SetActive(false);
				};
			}
		};
	}

	public ProCamera2DTriggerBoundaries triggerBoundary;

	public List<ChoSoi> choSois;

	public int levelChoSoi;

	private bool _isEnteredTrigger;
}
