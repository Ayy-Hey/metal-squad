using System;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance
	{
		get
		{
			if (CameraController.instance == null)
			{
				CameraController.instance = UnityEngine.Object.FindObjectOfType<CameraController>();
			}
			return CameraController.instance;
		}
	}

	private void Awake()
	{
		this.isInit = true;
		this.MIN_X = base.transform.position.x;
		this.cameraChild = base.transform.GetChild(0).GetComponent<Camera>();
	}

	public void BeginCamera()
	{
		if (GameMode.Instance.modePlay != GameMode.ModePlay.Campaign)
		{
			this.isCompleted = true;
			return;
		}
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
		{
			this.isCompleted = true;
			return;
		}
		if (GameManager.Instance.StateManager.EState != EGamePlay.BEGIN)
		{
			return;
		}
		this.posMaxCamera.x = 0f;
		this.posMaxCamera.y = 0f;
		this.isCompleted = true;
	}

	public void ResetCamera()
	{
		this.NumericBoundaries.LeftBoundary = -10f;
		this.posMaxCamera = Vector2.zero;
		base.transform.position = new Vector3(0f, 0f, -10f);
		this.MIN_X = float.MinValue;
		this.MIN_Y = float.MinValue;
	}

	private void LateUpdate()
	{
		if (!this.isInit)
		{
			return;
		}
		this.CacheSizeAndViewPos();
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
		{
			Vector3 position = this.border[0].transform.position;
			position.x = base.transform.position.x - this.Size().x;
			this.border[0].transform.position = position;
			position = this.border[1].transform.position;
			position.x = base.transform.position.x + this.Size().x;
			this.border[1].transform.position = position;
			position = this.border[2].transform.position;
			position.y = base.transform.position.y + this.Size().y;
			this.border[2].transform.position = position;
			position = this.border[3].transform.position;
			position.y = base.transform.position.y - this.Size().y;
			this.border[3].transform.position = position;
			if (this.parallaxLayer1 != null && this.parallaxLayer1.tfChild != null)
			{
				this.parallaxLayer1.tfChild.localScale = Vector3.one * (this.mCamera.orthographicSize / 3.6f);
			}
			return;
		}
		if (this.cameraChild)
		{
			this.cameraChild.orthographicSize = this.mCamera.orthographicSize;
		}
		if (this.parallaxLayer1 != null && this.parallaxLayer1.tfChild != null)
		{
			this.parallaxLayer1.tfChild.localScale = Vector3.one * (this.mCamera.orthographicSize / 3.6f);
		}
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			Vector3 position2 = this.border[0].transform.position;
			position2.x = base.transform.position.x - this.Size().x;
			this.border[0].transform.position = position2;
			position2 = this.border[1].transform.position;
			position2.x = base.transform.position.x + this.Size().x;
			this.border[1].transform.position = position2;
			position2 = this.border[2].transform.position;
			position2.y = base.transform.position.y + this.Size().y;
			this.border[2].transform.position = position2;
			position2 = this.border[3].transform.position;
			position2.y = base.transform.position.y - this.Size().y;
			this.border[3].transform.position = position2;
			return;
		}
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING && !this.isMapEnded)
		{
			Vector3 position3 = this.border[0].transform.position;
			position3.x = base.transform.position.x - this.Size().x;
			this.border[0].transform.position = position3;
			position3 = this.border[1].transform.position;
			position3.x = base.transform.position.x + this.Size().x;
			this.border[1].transform.position = position3;
			position3 = this.border[2].transform.position;
			position3.y = base.transform.position.y + this.Size().y;
			this.border[2].transform.position = position3;
			position3 = this.border[3].transform.position;
			position3.y = base.transform.position.y - this.Size().y;
			this.border[3].transform.position = position3;
			if (GameManager.Instance.bossManager.Boss32 != null)
			{
				if (this.posMaxCamera.x < base.transform.position.x)
				{
					this.posMaxCamera.x = base.transform.position.x;
				}
				if (this.posMaxCamera.x > GameManager.Instance.bossManager.Boss32.GetPosition().x - 3f)
				{
					this.posMaxCamera.x = GameManager.Instance.bossManager.Boss32.GetPosition().x - 3f;
				}
				float leftBoundary = this.posMaxCamera.x - (this.Size().x + 2f);
				this.NumericBoundaries.LeftBoundary = leftBoundary;
				return;
			}
			GameMode.GameStyle style = GameMode.Instance.Style;
			if (style != GameMode.GameStyle.MultiPlayer)
			{
				if (style == GameMode.GameStyle.SinglPlayer)
				{
					CameraController.Orientation orientation = this.orientaltion;
					if (orientation != CameraController.Orientation.HORIZONTAL)
					{
						if (orientation == CameraController.Orientation.VERTICAL)
						{
							if (this.isVerticalDown)
							{
								float topBoundary = base.transform.position.y + (this.Size().y + 1f);
								this.NumericBoundaries.TopBoundary = topBoundary;
								return;
							}
							if (this.freeBottomNumeric)
							{
								return;
							}
							if (this.posMaxCamera.y < base.transform.position.y)
							{
								this.posMaxCamera.y = base.transform.position.y;
							}
							float bottomBoundary = this.posMaxCamera.y - (this.Size().y + 2f);
							this.NumericBoundaries.BottomBoundary = bottomBoundary;
						}
					}
					else
					{
						if (this.StopMoveLeftBoundary)
						{
							return;
						}
						if (this.posMaxCamera.x < base.transform.position.x)
						{
							this.posMaxCamera.x = base.transform.position.x;
						}
						float leftBoundary2 = this.posMaxCamera.x - (this.Size().x + 2f);
						this.NumericBoundaries.LeftBoundary = leftBoundary2;
					}
				}
			}
		}
	}

	public bool IsInView(Transform tfTarget)
	{
		if (!tfTarget.gameObject.activeSelf)
		{
			return false;
		}
		bool result = false;
		try
		{
			Vector3 vector = this.mCamera.WorldToViewportPoint(tfTarget.position);
			result = (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f);
		}
		catch (Exception ex)
		{
		}
		return result;
	}

	public Vector2 Position
	{
		get
		{
			return this.camPos;
		}
	}

	public void Shake(CameraController.ShakeType type = CameraController.ShakeType.BulletPlayer)
	{
		if (GameManager.Instance.StateManager.EState == EGamePlay.WIN || GameManager.Instance.StateManager.EState == EGamePlay.LOST || GameManager.Instance.StateManager.EState == EGamePlay.PAUSE)
		{
			return;
		}
		ShakePreset preset = ProCamera2DShake.Instance.ShakePresets[(int)type];
		ProCamera2DShake.Instance.Shake(preset);
	}

	public void ResetCameraPro()
	{
		if (this.isCompleted)
		{
			return;
		}
	}

	public void FadeCameraFx(Action callback, Action completed)
	{
		this.cameraFX.TransitionExit();
		this.cameraFX.OnTransitionExitEnded = delegate()
		{
			this.cameraFX.TransitionEnter();
			if (callback != null)
			{
				callback();
			}
		};
		this.cameraFX.OnTransitionEnterEnded = delegate()
		{
			if (completed != null)
			{
				completed();
			}
		};
	}

	public void AddPlayer(Transform tfPlayer, float offsetY = 1.5f)
	{
		ProCamera2D.Instance.AddCameraTarget(tfPlayer, 1f, 1f, 0f, default(Vector2));
		ProCamera2D.Instance.OffsetX = this.proCameraOffsetX;
		ProCamera2D.Instance.OffsetY = this.proCameraOffsetY;
		GameMode.GameStyle style = GameMode.Instance.Style;
		if (style != GameMode.GameStyle.MultiPlayer)
		{
			if (style == GameMode.GameStyle.SinglPlayer)
			{
				GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
				if (modePlay == GameMode.ModePlay.Campaign || modePlay == GameMode.ModePlay.Special_Campaign)
				{
					this.NewCheckpoint(true, 15f);
				}
			}
		}
	}

	public void NewCheckpoint(bool isTween = true, float v = 15f)
	{
		if (GameManager.Instance.bossManager.Boss32 != null || this.isSkipNextCheckPoint)
		{
			return;
		}
		int num = GameManager.Instance.CheckPoint;
		num = Mathf.Min(num, DataLoader.LevelDataCurrent.points.Count - 1);
		CameraController.Orientation orientation = this.orientaltion;
		if (orientation != CameraController.Orientation.HORIZONTAL)
		{
			if (orientation == CameraController.Orientation.VERTICAL)
			{
				if (this.isVerticalDown)
				{
					return;
				}
				float topBoundary = this.NumericBoundaries.TopBoundary;
				float checkpoint_pos_y = DataLoader.LevelDataCurrent.points[num].checkpoint_pos_y;
				if (GameManager.Instance.Level == ELevel.LEVEL_5 && checkpoint_pos_y < topBoundary)
				{
					return;
				}
				float num2 = Mathf.Abs(checkpoint_pos_y - topBoundary);
				if (isTween)
				{
					LeanTween.value(base.gameObject, topBoundary, checkpoint_pos_y, num2 / v).setOnUpdate(delegate(float value)
					{
						this.NumericBoundaries.TopBoundary = value;
					}).setEase(LeanTweenType.easeInOutSine);
				}
				else
				{
					LeanTween.cancel(base.gameObject);
					this.NumericBoundaries.TopBoundary = checkpoint_pos_y;
				}
			}
		}
		else
		{
			this.NewBoundaryRight(DataLoader.LevelDataCurrent.points[num].checkpoint_pos_x, isTween);
		}
	}

	public void NewBoundaryRight(float x, bool isTween = true)
	{
		float rightBoundary = this.NumericBoundaries.RightBoundary;
		float num = Mathf.Abs(x - rightBoundary);
		float num2 = 15f;
		if (isTween)
		{
			LeanTween.value(base.gameObject, rightBoundary, x, num / num2).setOnUpdate(delegate(float value)
			{
				this.NumericBoundaries.RightBoundary = value;
			}).setEase(LeanTweenType.easeInOutSine);
		}
		else
		{
			LeanTween.cancel(base.gameObject);
			this.NumericBoundaries.RightBoundary = x;
		}
	}

	public Vector2 Size()
	{
		return this._cameraSize;
	}

	public Vector2 GetPosEnemyRandomLeft()
	{
		return new Vector2
		{
			x = this.Position.x - this.Size().x - 1.5f,
			y = this.Position.y
		};
	}

	public Vector2 GetPosEnemyRandomRight()
	{
		return new Vector2
		{
			x = this.Position.x + this.Size().x + 1.5f,
			y = this.Position.y
		};
	}

	public float LeftCamera()
	{
		return this.NumericBoundaries.LeftBoundary;
	}

	public float TopCamera()
	{
		return this.NumericBoundaries.TopBoundary;
	}

	public float RightCamera()
	{
		return this.NumericBoundaries.RightBoundary;
	}

	public float BottomCamera()
	{
		return this.NumericBoundaries.BottomBoundary;
	}

	public void SetSize(float size)
	{
		this.mCamera.orthographicSize = size;
	}

	private void CacheSizeAndViewPos()
	{
		this._cameraSize.y = this.mCamera.orthographicSize;
		this._cameraSize.x = Mathf.Max(1f, (float)Screen.width / (float)Screen.height) * this._cameraSize.y;
		this.camPos = base.transform.position;
		this.viewPos.minX = this.camPos.x - this._cameraSize.x;
		this.viewPos.minY = this.camPos.y - this._cameraSize.y;
		this.viewPos.maxX = this.camPos.x + this._cameraSize.x;
		this.viewPos.maxY = this.camPos.y + this._cameraSize.y;
	}

	private static CameraController instance;

	[SerializeField]
	private Transform tfCheckEnemyLeft;

	[SerializeField]
	private Transform tfCheckEnemyRight;

	[HideInInspector]
	public bool isInit;

	public CameraController.Orientation orientaltion;

	public float proCameraOffsetX;

	public float proCameraOffsetY = 0.5f;

	public bool isVerticalDown;

	public LayerMask layerGround;

	private float MIN_X = float.MinValue;

	private float MIN_Y = float.MinValue;

	public Camera mCamera;

	public bool isCompleted;

	[SerializeField]
	private Transform[] border;

	public bool isMapEnded;

	[HideInInspector]
	public bool freeBottomNumeric;

	[Header("--------------CameraPro------------")]
	public bool isSkipNextCheckPoint;

	public Vector2 posMaxCamera;

	public ProCamera2DNumericBoundaries NumericBoundaries;

	public ProCamera2DTransitionsFX cameraFX;

	private Camera cameraChild;

	public ParallaxLayer parallaxLayer1;

	public bool StopMoveLeftBoundary;

	public ViewPos viewPos;

	public Vector3 camPos;

	private Vector2 _cameraSize;

	public enum Orientation
	{
		NONE,
		HORIZONTAL,
		VERTICAL
	}

	public enum ShakeType
	{
		BulletPlayer,
		GrenadePlayer,
		ShakeEnemy,
		Explosion,
		Strong
	}
}
