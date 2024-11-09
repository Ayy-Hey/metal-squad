using System;
using System.Collections.Generic;
using Spine.Unity.Modules.AttachmentTools;
using UnityEngine;

namespace Spine.Unity.Modules
{
	public class SpriteAttacher : MonoBehaviour
	{
		private static AtlasPage GetPageFor(Texture texture, Shader shader)
		{
			if (SpriteAttacher.atlasPageCache == null)
			{
				SpriteAttacher.atlasPageCache = new Dictionary<Texture, AtlasPage>();
			}
			AtlasPage atlasPage;
			SpriteAttacher.atlasPageCache.TryGetValue(texture, out atlasPage);
			if (atlasPage == null)
			{
				Material m = new Material(shader);
				atlasPage = m.ToSpineAtlasPage();
				SpriteAttacher.atlasPageCache[texture] = atlasPage;
			}
			return atlasPage;
		}

		private void Start()
		{
			this.Initialize(false);
			if (this.attachOnStart)
			{
				this.Attach();
			}
		}

		private void AnimationOverrideSpriteAttach(ISkeletonAnimation animated)
		{
			if (this.overrideAnimation && base.isActiveAndEnabled)
			{
				this.Attach();
			}
		}

		public void Initialize(bool overwrite = true)
		{
			if (overwrite || this.attachment == null)
			{
				ISkeletonComponent component = base.GetComponent<ISkeletonComponent>();
				SkeletonRenderer skeletonRenderer = component as SkeletonRenderer;
				if (skeletonRenderer != null)
				{
					this.applyPMA = skeletonRenderer.pmaVertexColors;
				}
				else
				{
					SkeletonGraphic skeletonGraphic = component as SkeletonGraphic;
					if (skeletonGraphic != null)
					{
						this.applyPMA = skeletonGraphic.MeshGenerator.settings.pmaVertexColors;
					}
				}
				if (this.overrideAnimation)
				{
					ISkeletonAnimation skeletonAnimation = component as ISkeletonAnimation;
					if (skeletonAnimation != null)
					{
						skeletonAnimation.UpdateComplete -= this.AnimationOverrideSpriteAttach;
						skeletonAnimation.UpdateComplete += this.AnimationOverrideSpriteAttach;
					}
				}
				this.spineSlot = (this.spineSlot ?? component.Skeleton.FindSlot(this.slot));
				Shader shader = (!this.applyPMA) ? Shader.Find("Sprites/Default") : Shader.Find("Spine/Skeleton");
				this.attachment = ((!this.applyPMA) ? this.sprite.ToRegionAttachment(SpriteAttacher.GetPageFor(this.sprite.texture, shader), 0f) : this.sprite.ToRegionAttachmentPMAClone(shader, TextureFormat.RGBA32, false, null, 0f));
			}
		}

		private void OnDestroy()
		{
			ISkeletonAnimation component = base.GetComponent<ISkeletonAnimation>();
			if (component != null)
			{
				component.UpdateComplete -= this.AnimationOverrideSpriteAttach;
			}
		}

		public void Attach()
		{
			if (this.spineSlot != null)
			{
				this.spineSlot.Attachment = this.attachment;
			}
		}

		public const string DefaultPMAShader = "Spine/Skeleton";

		public const string DefaultStraightAlphaShader = "Sprites/Default";

		public bool attachOnStart = true;

		public bool overrideAnimation = true;

		public Sprite sprite;

		[SpineSlot("", "", false, true, false)]
		public string slot;

		private RegionAttachment attachment;

		private Slot spineSlot;

		private bool applyPMA;

		private static Dictionary<Texture, AtlasPage> atlasPageCache;
	}
}
