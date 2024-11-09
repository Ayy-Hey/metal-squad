using System;
using System.Collections.Generic;
using Spine.Unity.Modules.AttachmentTools;
using UnityEngine;

namespace Spine.Unity.Modules
{
	public class AtlasRegionAttacher : MonoBehaviour
	{
		private void Awake()
		{
			SkeletonRenderer component = base.GetComponent<SkeletonRenderer>();
			component.OnRebuild += this.Apply;
			if (component.valid)
			{
				this.Apply(component);
			}
		}

		private void Start()
		{
		}

		private void Apply(SkeletonRenderer skeletonRenderer)
		{
			if (!base.enabled)
			{
				return;
			}
			this.atlas = this.atlasAsset.GetAtlas();
			if (this.atlas == null)
			{
				return;
			}
			float scale = skeletonRenderer.skeletonDataAsset.scale;
			foreach (AtlasRegionAttacher.SlotRegionPair slotRegionPair in this.attachments)
			{
				Slot slot = skeletonRenderer.Skeleton.FindSlot(slotRegionPair.slot);
				Attachment attachment = slot.Attachment;
				AtlasRegion atlasRegion = this.atlas.FindRegion(slotRegionPair.region);
				if (atlasRegion == null)
				{
					slot.Attachment = null;
				}
				else if (this.inheritProperties && attachment != null)
				{
					slot.Attachment = attachment.GetRemappedClone(atlasRegion, true, true, scale);
				}
				else
				{
					RegionAttachment attachment2 = atlasRegion.ToRegionAttachment(atlasRegion.name, scale, 0f);
					slot.Attachment = attachment2;
				}
			}
		}

		[SerializeField]
		protected AtlasAsset atlasAsset;

		[SerializeField]
		protected bool inheritProperties = true;

		[SerializeField]
		protected List<AtlasRegionAttacher.SlotRegionPair> attachments = new List<AtlasRegionAttacher.SlotRegionPair>();

		private Atlas atlas;

		[Serializable]
		public class SlotRegionPair
		{
			[SpineSlot("", "", false, true, false)]
			public string slot;

			[SpineAtlasRegion("")]
			public string region;
		}
	}
}
