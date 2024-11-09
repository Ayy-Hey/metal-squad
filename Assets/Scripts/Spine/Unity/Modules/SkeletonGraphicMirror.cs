using System;
using UnityEngine;

namespace Spine.Unity.Modules
{
	public class SkeletonGraphicMirror : MonoBehaviour
	{
		private void Awake()
		{
			this.skeletonGraphic = base.GetComponent<SkeletonGraphic>();
		}

		private void Start()
		{
			if (this.mirrorOnStart)
			{
				this.StartMirroring();
			}
		}

		private void LateUpdate()
		{
			this.skeletonGraphic.UpdateMesh();
		}

		private void OnDisable()
		{
			if (this.restoreOnDisable)
			{
				this.RestoreIndependentSkeleton();
			}
		}

		public void StartMirroring()
		{
			if (this.source == null)
			{
				return;
			}
			if (this.skeletonGraphic == null)
			{
				return;
			}
			this.skeletonGraphic.startingAnimation = string.Empty;
			if (this.originalSkeleton == null)
			{
				this.originalSkeleton = this.skeletonGraphic.Skeleton;
				this.originalFreeze = this.skeletonGraphic.freeze;
			}
			//this.skeletonGraphic.Skeleton = this.source.skeleton;
			this.skeletonGraphic.freeze = true;
			if (this.overrideTexture != null)
			{
				this.skeletonGraphic.OverrideTexture = this.overrideTexture;
			}
		}

		public void UpdateTexture(Texture2D newOverrideTexture)
		{
			this.overrideTexture = newOverrideTexture;
			if (newOverrideTexture != null)
			{
				this.skeletonGraphic.OverrideTexture = this.overrideTexture;
			}
		}

		public void RestoreIndependentSkeleton()
		{
			if (this.originalSkeleton == null)
			{
				return;
			}
			//this.skeletonGraphic.Skeleton = this.originalSkeleton;
			this.skeletonGraphic.freeze = this.originalFreeze;
			this.skeletonGraphic.OverrideTexture = null;
			this.originalSkeleton = null;
		}

		public SkeletonRenderer source;

		public bool mirrorOnStart = true;

		public bool restoreOnDisable = true;

		private SkeletonGraphic skeletonGraphic;

		private Skeleton originalSkeleton;

		private bool originalFreeze;

		private Texture2D overrideTexture;
	}
}
