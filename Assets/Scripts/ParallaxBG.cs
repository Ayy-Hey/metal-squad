using System;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
	private void Start()
	{
		foreach (MaterialParallax materialParallax in this.materialParallaxes)
		{
			materialParallax.savedOffset = materialParallax.renderer.material.GetTextureOffset("_MainTex");
		}
		TriggerCamera triggerCamera = this.triggerCameraStop;
		triggerCamera.OnEnteredTrigger = (Action)Delegate.Combine(triggerCamera.OnEnteredTrigger, new Action(delegate()
		{
			this.isRunning = false;
		}));
	}

	private void Update()
	{
		if (!this.isFlowCamera)
		{
			return;
		}
		foreach (MaterialParallax materialParallax in this.materialParallaxes)
		{
			materialParallax.savedOffset = materialParallax.renderer.material.GetTextureOffset("_MainTex");
			if (!this.isRunning)
			{
				if (materialParallax.speed > 0f)
				{
					materialParallax.speed -= 5E-05f;
					materialParallax.speed = Mathf.Clamp(materialParallax.speed, 0f, float.MaxValue);
				}
				else
				{
					this.isFlowCamera = false;
					GameManager.Instance.hudManager.ShowControl(1.1f);
					if (!this.isClone)
					{
						this.CloneBackground();
						this.isClone = true;
					}
				}
			}
			float x = Mathf.Repeat(materialParallax.savedOffset.x + materialParallax.speed, 1f);
			Vector2 value = new Vector2(x, materialParallax.savedOffset.y);
			materialParallax.renderer.material.SetTextureOffset("_MainTex", value);
		}
	}

	private void CloneBackground()
	{
		for (int i = 1; i < 4; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.cloneObjs[1], this.cloneObjs[1].transform.parent);
			this.posNextBG = gameObject.transform.localScale.x;
			Vector3 position = gameObject.transform.position;
			position = this.cloneObjs[1].transform.position + Vector3.right * this.posNextBG * (float)i;
			position.z = 9f;
			gameObject.transform.position = position;
			gameObject.transform.localScale = this.cloneObjs[1].transform.localScale;
		}
		this.cloneObjs[0].gameObject.SetActive(false);
	}

	private void LateUpdate()
	{
		if (this.isFlowCamera)
		{
			Vector3 position = base.transform.position;
			position.x = CameraController.Instance.transform.position.x;
			base.transform.position = position;
		}
	}

	private void OnDisable()
	{
		foreach (MaterialParallax materialParallax in this.materialParallaxes)
		{
			materialParallax.renderer.material.SetTextureOffset("_MainTex", materialParallax.savedOffset);
		}
	}

	[SerializeField]
	private MaterialParallax[] materialParallaxes;

	[SerializeField]
	private bool isRunning;

	private float timePlay;

	[SerializeField]
	private TriggerCamera triggerCameraStop;

	[SerializeField]
	private bool isFlowCamera = true;

	[SerializeField]
	private GameObject[] cloneObjs;

	private bool isClone;

	private float posNextBG;
}
