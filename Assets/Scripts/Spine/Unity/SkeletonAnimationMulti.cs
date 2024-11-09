using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	public class SkeletonAnimationMulti : MonoBehaviour
	{
		private void Clear()
		{
			foreach (SkeletonAnimation skeletonAnimation in this.skeletonAnimations)
			{
				UnityEngine.Object.Destroy(skeletonAnimation.gameObject);
			}
			this.skeletonAnimations.Clear();
			this.animationNameTable.Clear();
			this.animationSkeletonTable.Clear();
		}

		private void SetActiveSkeleton(SkeletonAnimation skeletonAnimation)
		{
			foreach (SkeletonAnimation skeletonAnimation2 in this.skeletonAnimations)
			{
				skeletonAnimation2.gameObject.SetActive(skeletonAnimation2 == skeletonAnimation);
			}
			this.currentSkeletonAnimation = skeletonAnimation;
		}

		private void Awake()
		{
			this.Initialize(false);
		}

		public Dictionary<Animation, SkeletonAnimation> AnimationSkeletonTable
		{
			get
			{
				return this.animationSkeletonTable;
			}
		}

		public Dictionary<string, Animation> AnimationNameTable
		{
			get
			{
				return this.animationNameTable;
			}
		}

		public SkeletonAnimation CurrentSkeletonAnimation
		{
			get
			{
				return this.currentSkeletonAnimation;
			}
		}

		public void Initialize(bool overwrite)
		{
			if (this.skeletonAnimations.Count != 0 && !overwrite)
			{
				return;
			}
			this.Clear();
			MeshGenerator.Settings meshSettings = this.meshGeneratorSettings;
			Transform transform = base.transform;
			foreach (SkeletonDataAsset skeletonDataAsset in this.skeletonDataAssets)
			{
				SkeletonAnimation skeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(skeletonDataAsset);
				skeletonAnimation.transform.SetParent(transform, false);
				skeletonAnimation.SetMeshSettings(meshSettings);
				skeletonAnimation.initialFlipX = this.initialFlipX;
				skeletonAnimation.initialFlipY = this.initialFlipY;
				Skeleton skeleton = skeletonAnimation.skeleton;
				skeleton.FlipX = this.initialFlipX;
				skeleton.FlipY = this.initialFlipY;
				skeletonAnimation.Initialize(false);
				this.skeletonAnimations.Add(skeletonAnimation);
			}
			Dictionary<string, Animation> dictionary = this.animationNameTable;
			Dictionary<Animation, SkeletonAnimation> dictionary2 = this.animationSkeletonTable;
			foreach (SkeletonAnimation skeletonAnimation2 in this.skeletonAnimations)
			{
				foreach (Animation animation in skeletonAnimation2.Skeleton.Data.Animations)
				{
					dictionary[animation.Name] = animation;
					dictionary2[animation] = skeletonAnimation2;
				}
			}
			this.SetActiveSkeleton(this.skeletonAnimations[0]);
			this.SetAnimation(this.initialAnimation, this.initialLoop);
		}

		public Animation FindAnimation(string animationName)
		{
			Animation result;
			this.animationNameTable.TryGetValue(animationName, out result);
			return result;
		}

		public TrackEntry SetAnimation(string animationName, bool loop)
		{
			return this.SetAnimation(this.FindAnimation(animationName), loop);
		}

		public TrackEntry SetAnimation(Animation animation, bool loop)
		{
			if (animation == null)
			{
				return null;
			}
			SkeletonAnimation skeletonAnimation;
			this.animationSkeletonTable.TryGetValue(animation, out skeletonAnimation);
			if (skeletonAnimation != null)
			{
				this.SetActiveSkeleton(skeletonAnimation);
				skeletonAnimation.skeleton.SetToSetupPose();
				return skeletonAnimation.state.SetAnimation(0, animation, loop);
			}
			return null;
		}

		public void SetEmptyAnimation(float mixDuration)
		{
			this.currentSkeletonAnimation.state.SetEmptyAnimation(0, mixDuration);
		}

		public void ClearAnimation()
		{
			this.currentSkeletonAnimation.state.ClearTrack(0);
		}

		public TrackEntry GetCurrent()
		{
			return this.currentSkeletonAnimation.state.GetCurrent(0);
		}

		private const int MainTrackIndex = 0;

		public bool initialFlipX;

		public bool initialFlipY;

		public string initialAnimation;

		public bool initialLoop;

		[Space]
		public List<SkeletonDataAsset> skeletonDataAssets = new List<SkeletonDataAsset>();

		[Header("Settings")]
		public MeshGenerator.Settings meshGeneratorSettings = MeshGenerator.Settings.Default;

		private readonly List<SkeletonAnimation> skeletonAnimations = new List<SkeletonAnimation>();

		private readonly Dictionary<string, Animation> animationNameTable = new Dictionary<string, Animation>();

		private readonly Dictionary<Animation, SkeletonAnimation> animationSkeletonTable = new Dictionary<Animation, SkeletonAnimation>();

		private SkeletonAnimation currentSkeletonAnimation;
	}
}
