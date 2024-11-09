using System;
using UnityEngine;

public class GateCampaign : MonoBehaviour
{
	private void Start()
	{
		this.isStartRun = false;
	}

	private void LateUpdate()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.PAUSE)
		{
			return;
		}
		if (this.isStartRun)
		{
			GameManager.Instance.player.OnMovement(BaseCharactor.EMovementBasic.Right);
		}
	}

	public void ShowNewStart()
	{
        Debug.Log("ShowNewStart 1");
		if (!this.isStartRun)
		{
			return;
		}
        Debug.Log("ShowNewStart 2");
        CameraController.Instance.isSkipNextCheckPoint = this.isSkipCameraPro;
		this.isStartRun = false;
		GameManager.Instance.player.isAutoRun = false;
		CameraController.Instance.FadeCameraFx(delegate
		{
            Debug.Log("ShowNewStart 3");
            CameraController.Instance.NewCheckpoint(false, 15f);
			CameraController.Instance.posMaxCamera.x = this.posMaxCamera;
			GameManager.Instance.player.transform.position = this.RestartPos.position;
			MapPath2 mapPath = UnityEngine.Object.Instantiate(Resources.Load(this.pathMap, typeof(MapPath2))) as MapPath2;
			mapPath.transform.parent = this.tfParentPath2.parent;
			for (int i = 0; i < mapPath.Maps.Count; i++)
			{
				this.mapGroup.maps.Add(mapPath.Maps[i]);
			}
			GameManager.Instance.player._PlayerSpine.OnIdle(true);
			if (this.OnOpenNewMapStarted != null)
			{
				this.OnOpenNewMapStarted();
			}
			GameManager.Instance.StateManager.SetPreview();
			CameraController.Instance.parallaxLayer1.OnDisableBG();
		}, delegate
		{
            Debug.Log("ShowNewStart 4");
            if (this.OnOpenNewMapEnded != null)
			{
				this.OnOpenNewMapEnded();
			}
			else
			{
                Debug.Log("ShowNewStart 5");
                GameManager.Instance.StateManager.EState = EGamePlay.RUNNING;
				GameManager.Instance.hudManager.ShowControl(1.1f);
			}
		});
	}

	public Action OnOpenNewMapEnded;

	public Action OnOpenNewMapStarted;

	[SerializeField]
	private Transform RestartPos;

	public bool isStartRun;

	[SerializeField]
	private float posMaxCamera;

	[SerializeField]
	private MapGroup mapGroup;

	[SerializeField]
	private Transform tfParentPath2;

	[SerializeField]
	private string pathMap = "Map/Map1/Path2";

	[SerializeField]
	private bool isSkipCameraPro;
}
