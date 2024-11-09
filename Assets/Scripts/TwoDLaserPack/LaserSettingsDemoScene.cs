using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoDLaserPack
{
	public class LaserSettingsDemoScene : MonoBehaviour
	{
		private void Start()
		{
			if (this.LineBasedLaser == null)
			{
				UnityEngine.Debug.LogError("You need to reference a valid LineBasedLaser on this script.");
			}
			this.toggleisActive.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserActiveChanged));
			this.toggleignoreCollisions.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserToggleCollisionsChanged));
			this.togglelaserRotationEnabled.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserAllowRotationChanged));
			this.togglelerpLaserRotation.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserLerpRotationChanged));
			this.toggleuseArc.onValueChanged.AddListener(new UnityAction<bool>(this.OnUseArcValueChanged));
			this.toggleTargetMouse.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleFollowMouse));
			this.slidertexOffsetSpeed.onValueChanged.AddListener(new UnityAction<float>(this.OnTextureOffsetSpeedChanged));
			this.sliderlaserArcMaxYDown.onValueChanged.AddListener(new UnityAction<float>(this.OnArcMaxYDownValueChanged));
			this.sliderlaserArcMaxYUp.onValueChanged.AddListener(new UnityAction<float>(this.OnArcMaxYUpValueChanged));
			this.slidermaxLaserRaycastDistance.onValueChanged.AddListener(new UnityAction<float>(this.OnLaserRaycastDistanceChanged));
			this.sliderturningRate.onValueChanged.AddListener(new UnityAction<float>(this.OnLaserTurningRateChanged));
			this.buttonSwitch.onClick.AddListener(new UnityAction(this.OnButtonClick));
			this.selectedMaterialIndex = 1;
			this.maxSelectedIndex = this.LaserMaterials.Length - 1;
		}

		private void OnToggleFollowMouse(bool followMouse)
		{
			this.targetShouldTrackMouse = followMouse;
			if (this.targetShouldTrackMouse)
			{
				this.FollowScript.enabled = false;
			}
			else
			{
				this.FollowScript.enabled = true;
			}
		}

		private void OnButtonClick()
		{
			if (this.selectedMaterialIndex < this.maxSelectedIndex)
			{
				this.selectedMaterialIndex++;
				this.LineBasedLaser.laserLineRenderer.material = this.LaserMaterials[this.selectedMaterialIndex];
				this.LineBasedLaser.laserLineRendererArc.material = this.LaserMaterials[this.selectedMaterialIndex];
				this.LineBasedLaser.hitSparkParticleSystem.GetComponent<Renderer>().material = this.LaserMaterials[this.selectedMaterialIndex];
			}
			else
			{
				this.selectedMaterialIndex = 0;
				this.LineBasedLaser.laserLineRenderer.material = this.LaserMaterials[this.selectedMaterialIndex];
				this.LineBasedLaser.laserLineRendererArc.material = this.LaserMaterials[this.selectedMaterialIndex];
				this.LineBasedLaser.hitSparkParticleSystem.GetComponent<Renderer>().material = this.LaserMaterials[this.selectedMaterialIndex];
			}
		}

		private void OnLaserTurningRateChanged(float turningRate)
		{
			this.LineBasedLaser.turningRate = turningRate;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser turning rate: " + Math.Round((double)turningRate, 2).ToString();
		}

		private void OnLaserRaycastDistanceChanged(float raycastDistance)
		{
			this.LineBasedLaser.maxLaserRaycastDistance = raycastDistance;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser raycast max distance: " + Math.Round((double)raycastDistance, 2).ToString();
		}

		private void OnArcMaxYUpValueChanged(float maxYValueUp)
		{
			this.LineBasedLaser.laserArcMaxYUp = maxYValueUp;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc maximum up arc height: " + Math.Round((double)maxYValueUp, 2).ToString();
		}

		private void OnArcMaxYDownValueChanged(float maxYValueDown)
		{
			this.LineBasedLaser.laserArcMaxYDown = maxYValueDown;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc maximum down arc height: " + Math.Round((double)maxYValueDown, 2).ToString();
		}

		private void OnTextureOffsetSpeedChanged(float offsetSpeed)
		{
			this.LineBasedLaser.laserTexOffsetSpeed = offsetSpeed;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser texture offset speed: " + Math.Round((double)offsetSpeed, 2).ToString();
		}

		private void OnUseArcValueChanged(bool useArc)
		{
			this.LineBasedLaser.useArc = useArc;
			this.sliderlaserArcMaxYDown.interactable = useArc;
			this.sliderlaserArcMaxYUp.interactable = useArc;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc enabled: " + useArc.ToString();
		}

		private void OnLaserLerpRotationChanged(bool lerpLaserRotation)
		{
			this.LineBasedLaser.lerpLaserRotation = lerpLaserRotation;
			this.sliderturningRate.interactable = lerpLaserRotation;
			this.textValue.color = Color.white;
			this.textValue.text = "Lerp laser rotation: " + lerpLaserRotation.ToString();
		}

		private void OnLaserAllowRotationChanged(bool allowRotation)
		{
			this.LineBasedLaser.laserRotationEnabled = allowRotation;
			this.togglelerpLaserRotation.interactable = allowRotation;
			this.sliderturningRate.interactable = allowRotation;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser rotation enabled: " + allowRotation.ToString();
		}

		private void OnLaserToggleCollisionsChanged(bool ignoreCollisions)
		{
			this.LineBasedLaser.ignoreCollisions = ignoreCollisions;
			this.textValue.color = Color.white;
			this.textValue.text = "Ignore laser collisions: " + ignoreCollisions.ToString();
		}

		private void OnLaserActiveChanged(bool state)
		{
			this.LineBasedLaser.SetLaserState(state);
			this.textValue.color = Color.white;
			this.textValue.text = "Laser active: " + state.ToString();
		}

		private void Update()
		{
			if (this.targetShouldTrackMouse)
			{
				Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
				Vector2 v = new Vector2(vector.x, vector.y);
				this.LineBasedLaser.targetGo.transform.position = v;
			}
		}

		public LineBasedLaser LineBasedLaser;

		public DemoFollowScript FollowScript;

		public Toggle toggleisActive;

		public Toggle toggleignoreCollisions;

		public Toggle togglelaserRotationEnabled;

		public Toggle togglelerpLaserRotation;

		public Toggle toggleuseArc;

		public Toggle toggleTargetMouse;

		public Slider slidertexOffsetSpeed;

		public Slider sliderlaserArcMaxYDown;

		public Slider sliderlaserArcMaxYUp;

		public Slider slidermaxLaserRaycastDistance;

		public Slider sliderturningRate;

		public Button buttonSwitch;

		public Text textValue;

		public Material[] LaserMaterials;

		private int selectedMaterialIndex;

		private int maxSelectedIndex;

		private bool targetShouldTrackMouse;
	}
}
