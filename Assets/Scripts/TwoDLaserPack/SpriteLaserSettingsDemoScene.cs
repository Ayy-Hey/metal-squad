using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoDLaserPack
{
	public class SpriteLaserSettingsDemoScene : MonoBehaviour
	{
		private void Start()
		{
			if (this.SpriteBasedLaser == null)
			{
				UnityEngine.Debug.LogError("You need to reference a valid LineBasedLaser on this script.");
			}
			this.toggleisActive.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserActiveChanged));
			this.toggleignoreCollisions.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserToggleCollisionsChanged));
			this.togglelaserRotationEnabled.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserAllowRotationChanged));
			this.togglelerpLaserRotation.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserLerpRotationChanged));
			this.toggleuseArc.onValueChanged.AddListener(new UnityAction<bool>(this.OnUseArcValueChanged));
			this.toggleOscillateLaser.onValueChanged.AddListener(new UnityAction<bool>(this.OnOscillateLaserChanged));
			this.sliderlaserArcMaxYDown.onValueChanged.AddListener(new UnityAction<float>(this.OnArcMaxYDownValueChanged));
			this.sliderlaserArcMaxYUp.onValueChanged.AddListener(new UnityAction<float>(this.OnArcMaxYUpValueChanged));
			this.slidermaxLaserRaycastDistance.onValueChanged.AddListener(new UnityAction<float>(this.OnLaserRaycastDistanceChanged));
			this.sliderturningRate.onValueChanged.AddListener(new UnityAction<float>(this.OnLaserTurningRateChanged));
			this.sliderOscillationThreshold.onValueChanged.AddListener(new UnityAction<float>(this.OnOscillationThresholdChanged));
			this.sliderOscillationSpeed.onValueChanged.AddListener(new UnityAction<float>(this.OnOscillationSpeedChanged));
			this.buttonSwitch.onClick.AddListener(new UnityAction(this.OnButtonClick));
			this.selectedMaterialIndex = 1;
			this.maxSelectedIndex = this.LaserMaterials.Length - 1;
		}

		private void OnOscillationSpeedChanged(float oscillationSpeed)
		{
			this.SpriteBasedLaser.oscillationSpeed = oscillationSpeed;
		}

		private void OnOscillationThresholdChanged(float oscillationThreshold)
		{
			this.SpriteBasedLaser.oscillationThreshold = oscillationThreshold;
		}

		private void OnOscillateLaserChanged(bool oscillateLaser)
		{
			this.SpriteBasedLaser.oscillateLaser = oscillateLaser;
		}

		private void OnButtonClick()
		{
			if (this.selectedMaterialIndex < this.maxSelectedIndex)
			{
				this.selectedMaterialIndex++;
				this.SpriteBasedLaser.laserLineRendererArc.material = this.LaserMaterials[this.selectedMaterialIndex];
				this.SpriteBasedLaser.hitSparkParticleSystem.GetComponent<Renderer>().material = this.LaserMaterials[this.selectedMaterialIndex];
				this.SpriteBasedLaser.laserStartPiece = this.laserStartPieceRed;
				this.SpriteBasedLaser.laserMiddlePiece = this.laserMidPieceRed;
				this.SpriteBasedLaser.laserEndPiece = this.laserEndPieceRed;
			}
			else
			{
				this.selectedMaterialIndex = 0;
				this.SpriteBasedLaser.laserLineRendererArc.material = this.LaserMaterials[this.selectedMaterialIndex];
				this.SpriteBasedLaser.hitSparkParticleSystem.GetComponent<Renderer>().material = this.LaserMaterials[this.selectedMaterialIndex];
				this.SpriteBasedLaser.laserStartPiece = this.laserStartPieceBlue;
				this.SpriteBasedLaser.laserMiddlePiece = this.laserMidPieceBlue;
				this.SpriteBasedLaser.laserEndPiece = this.laserEndPieceBlue;
			}
			this.SpriteBasedLaser.DisableLaserGameObjectComponents();
		}

		private void OnLaserTurningRateChanged(float turningRate)
		{
			this.SpriteBasedLaser.turningRate = turningRate;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser turning rate: " + Math.Round((double)turningRate, 2).ToString();
		}

		private void OnLaserRaycastDistanceChanged(float raycastDistance)
		{
			this.SpriteBasedLaser.maxLaserRaycastDistance = raycastDistance;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser raycast max distance: " + Math.Round((double)raycastDistance, 2).ToString();
		}

		private void OnArcMaxYUpValueChanged(float maxYValueUp)
		{
			this.SpriteBasedLaser.laserArcMaxYUp = maxYValueUp;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc maximum up arc height: " + Math.Round((double)maxYValueUp, 2).ToString();
		}

		private void OnArcMaxYDownValueChanged(float maxYValueDown)
		{
			this.SpriteBasedLaser.laserArcMaxYDown = maxYValueDown;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc maximum down arc height: " + Math.Round((double)maxYValueDown, 2).ToString();
		}

		private void OnUseArcValueChanged(bool useArc)
		{
			this.SpriteBasedLaser.useArc = useArc;
			this.sliderlaserArcMaxYDown.interactable = useArc;
			this.sliderlaserArcMaxYUp.interactable = useArc;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc enabled: " + useArc.ToString();
		}

		private void OnLaserLerpRotationChanged(bool lerpLaserRotation)
		{
			this.SpriteBasedLaser.lerpLaserRotation = lerpLaserRotation;
			this.sliderturningRate.interactable = lerpLaserRotation;
			this.textValue.color = Color.white;
			this.textValue.text = "Lerp laser rotation: " + lerpLaserRotation.ToString();
		}

		private void OnLaserAllowRotationChanged(bool allowRotation)
		{
			this.SpriteBasedLaser.laserRotationEnabled = allowRotation;
			this.togglelerpLaserRotation.interactable = allowRotation;
			this.sliderturningRate.interactable = allowRotation;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser rotation enabled: " + allowRotation.ToString();
		}

		private void OnLaserToggleCollisionsChanged(bool ignoreCollisions)
		{
			this.SpriteBasedLaser.ignoreCollisions = ignoreCollisions;
			this.textValue.color = Color.white;
			this.textValue.text = "Ignore laser collisions: " + ignoreCollisions.ToString();
		}

		private void OnLaserActiveChanged(bool state)
		{
			this.SpriteBasedLaser.SetLaserState(state);
			this.textValue.color = Color.white;
			this.textValue.text = "Laser active: " + state.ToString();
		}

		private void Update()
		{
		}

		public SpriteBasedLaser SpriteBasedLaser;

		public Toggle toggleisActive;

		public Toggle toggleignoreCollisions;

		public Toggle togglelaserRotationEnabled;

		public Toggle togglelerpLaserRotation;

		public Toggle toggleuseArc;

		public Toggle toggleOscillateLaser;

		public Slider sliderlaserArcMaxYDown;

		public Slider sliderlaserArcMaxYUp;

		public Slider slidermaxLaserRaycastDistance;

		public Slider sliderturningRate;

		public Slider sliderOscillationThreshold;

		public Slider sliderOscillationSpeed;

		public Button buttonSwitch;

		public Text textValue;

		public Material[] LaserMaterials;

		public GameObject laserStartPieceBlue;

		public GameObject laserStartPieceRed;

		public GameObject laserMidPieceBlue;

		public GameObject laserMidPieceRed;

		public GameObject laserEndPieceBlue;

		public GameObject laserEndPieceRed;

		private int selectedMaterialIndex;

		private int maxSelectedIndex;
	}
}
