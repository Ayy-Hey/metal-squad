using System;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
	public void Start()
	{
		if (this.isInit)
		{
			return;
		}
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Endless)
		{
			CameraController.Instance.parallaxLayer1 = base.GetComponent<ParallaxLayer>();
		}
		base.gameObject.SetActive(true);
		if (this.triggerCameraStop != null)
		{
			TriggerCamera triggerCamera = this.triggerCameraStop;
			triggerCamera.OnEnteredTrigger = (Action)Delegate.Combine(triggerCamera.OnEnteredTrigger, new Action(delegate()
			{
				this.isRunning = false;
			}));
		}
		this.preX = CameraController.Instance.Position.x;
		this.preY = CameraController.Instance.Position.y;
		if (this.textures != null)
		{
			for (int i = 0; i < this.textures.Length; i++)
			{
				if (this.textures[i] != null)
				{
					this.materialParallaxes[i].renderer.material.mainTexture = this.textures[i];
				}
			}
		}
		if (this.cloudTextures != null)
		{
			for (int j = 0; j < this.cloudTextures.Length; j++)
			{
				if (this.cloudTextures[j])
				{
					this.materialClouds[j].renderer.material.mainTexture = this.cloudTextures[j];
					this.materialClouds[j].renderer.material.SetColor("_TintColor", this.cloudColor);
				}
			}
		}
		this.bgMaxSpeed = this.materialParallaxes[0].speed;
		for (int k = 1; k < this.materialParallaxes.Length; k++)
		{
			this.bgMaxSpeed = ((this.bgMaxSpeed >= this.materialParallaxes[k].speed) ? this.bgMaxSpeed : this.materialParallaxes[k].speed);
		}
		this.isInit = true;
	}

	public void OnDisableBG()
	{
		base.gameObject.SetActive(false);
	}

	public void StartAuto()
	{
		this.isAuto = true;
	}

	public void ActiveAllObject()
	{
		foreach (MaterialParallax materialParallax in this.materialParallaxes)
		{
			materialParallax.renderer.gameObject.SetActive(true);
		}
	}

	public void StopAuto()
	{
		this.isAuto = false;
		this.materialParallaxes[1].speed = 0.00333333f;
		this.materialParallaxes[2].speed = 0.005f;
		this.materialParallaxes[3].speed = 0.02f;
		this.materialParallaxes[4].speed = 0.03f;
		this.materialParallaxes[5].speed = 0.00333333f;
	}

	private void Update()
	{
		if (!this.isFlowCamera || !this.isInit)
		{
			return;
		}
		CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
		if (orientaltion != CameraController.Orientation.VERTICAL)
		{
			float num = CameraController.Instance.Position.x - this.preX;
			this.preX = CameraController.Instance.Position.x;
			for (int i = 0; i < this.materialParallaxes.Length; i++)
			{
				Vector2 mainTextureOffset = this.materialParallaxes[i].renderer.material.mainTextureOffset;
				if (this.isAuto)
				{
					mainTextureOffset.x = ((!this.materialParallaxes[i].isRepeat) ? (mainTextureOffset.x + this.materialParallaxes[i].speed) : Mathf.Repeat(mainTextureOffset.x + this.materialParallaxes[i].speed, 1f));
				}
				else
				{
					mainTextureOffset.x = ((!this.materialParallaxes[i].isRepeat) ? (mainTextureOffset.x + num * this.materialParallaxes[i].speed) : Mathf.Repeat(mainTextureOffset.x + num * this.materialParallaxes[i].speed, 1f));
				}
				this.materialParallaxes[i].renderer.material.mainTextureOffset = mainTextureOffset;
			}
			if (this.materialClouds != null)
			{
				for (int j = 0; j < this.materialClouds.Length; j++)
				{
					Vector2 mainTextureOffset2 = this.materialClouds[j].renderer.material.mainTextureOffset;
					mainTextureOffset2.x = mainTextureOffset2.x + this.materialClouds[j].speed + num * this.bgMaxSpeed;
					this.materialClouds[j].renderer.material.mainTextureOffset = mainTextureOffset2;
				}
			}
		}
		else
		{
			float num = CameraController.Instance.Position.y - this.preY;
			this.preY = CameraController.Instance.Position.y;
			for (int k = 0; k < this.materialParallaxes.Length; k++)
			{
				Vector2 mainTextureOffset3 = this.materialParallaxes[k].renderer.material.mainTextureOffset;
				if (this.isAuto)
				{
					mainTextureOffset3.y = ((!this.materialParallaxes[k].isRepeat) ? (mainTextureOffset3.x + this.materialParallaxes[k].speed) : Mathf.Repeat(mainTextureOffset3.y + this.materialParallaxes[k].speed, 1f));
				}
				else
				{
					mainTextureOffset3.y = ((!this.materialParallaxes[k].isRepeat) ? (mainTextureOffset3.x + num * this.materialParallaxes[k].speed) : Mathf.Repeat(mainTextureOffset3.y + num * this.materialParallaxes[k].speed, 1f));
				}
				this.materialParallaxes[k].renderer.material.mainTextureOffset = mainTextureOffset3;
			}
			if (this.materialClouds != null)
			{
				for (int l = 0; l < this.materialClouds.Length; l++)
				{
					Vector2 mainTextureOffset4 = this.materialClouds[l].renderer.material.mainTextureOffset;
					mainTextureOffset4.y += this.materialClouds[l].speed;
					this.materialClouds[l].renderer.material.mainTextureOffset = mainTextureOffset4;
				}
			}
		}
	}

	private void LateUpdate()
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.isFlowCamera)
		{
			Vector3 position = base.transform.position;
			CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
			if (orientaltion != CameraController.Orientation.VERTICAL)
			{
				position.x = CameraController.Instance.camPos.x;
			}
			else
			{
				position.y = CameraController.Instance.camPos.y;
			}
			base.transform.position = position;
		}
		if (this.tfbackground != null)
		{
			Vector3 position2 = this.tfbackground.position;
			CameraController.Orientation orientaltion2 = CameraController.Instance.orientaltion;
			if (orientaltion2 != CameraController.Orientation.VERTICAL)
			{
				position2.y = CameraController.Instance.Position.y;
			}
			else
			{
				position2.x = CameraController.Instance.Position.x;
			}
			this.tfbackground.position = position2;
		}
	}

	private void OnDisable()
	{
		this.isInit = false;
	}

	[SerializeField]
	private MaterialParallax[] materialParallaxes;

	[SerializeField]
	private MaterialParallax[] materialClouds;

	[SerializeField]
	private Texture[] textures;

	[SerializeField]
	private Texture[] cloudTextures;

	[SerializeField]
	private Color cloudColor;

	[SerializeField]
	private bool isRunning;

	private float timePlay;

	[SerializeField]
	private TriggerCamera triggerCameraStop;

	[SerializeField]
	public bool isFlowCamera = true;

	[SerializeField]
	private bool isAuto = true;

	private bool isInit;

	[SerializeField]
	private Transform tfbackground;

	public Transform tfChild;

	private float bgMaxSpeed;

	private float preX;

	private float preY;
}
