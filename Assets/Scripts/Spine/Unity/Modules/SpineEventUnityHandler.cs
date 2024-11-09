using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Spine.Unity.Modules
{
	public class SpineEventUnityHandler : MonoBehaviour
	{
		private void Start()
		{
			this.skeletonComponent = (this.skeletonComponent ?? base.GetComponent<ISkeletonComponent>());
			if (this.skeletonComponent == null)
			{
				return;
			}
			this.animationStateComponent = (this.animationStateComponent ?? (this.skeletonComponent as IAnimationStateComponent));
			if (this.animationStateComponent == null)
			{
				return;
			}
			Skeleton skeleton = this.skeletonComponent.Skeleton;
			if (skeleton == null)
			{
				return;
			}
			SkeletonData data = skeleton.Data;
			AnimationState animationState = this.animationStateComponent.AnimationState;
			using (List<SpineEventUnityHandler.EventPair>.Enumerator enumerator = this.events.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{

                    SpineEventUnityHandler.EventPair ep = enumerator.Current;
					EventData eventData = data.FindEvent(ep.spineEvent);
					ep.eventDelegate = (ep.eventDelegate ?? delegate(TrackEntry trackEntry, Event e)
					{
						if (e.Data == eventData)
						{
							ep.unityHandler.Invoke();
						}
					});
					animationState.Event += ep.eventDelegate;
				}
			}
		}

		private void OnDestroy()
		{
			this.animationStateComponent = (this.animationStateComponent ?? base.GetComponent<IAnimationStateComponent>());
			if (this.animationStateComponent == null)
			{
				return;
			}
			AnimationState animationState = this.animationStateComponent.AnimationState;
			foreach (SpineEventUnityHandler.EventPair eventPair in this.events)
			{
				if (eventPair.eventDelegate != null)
				{
					animationState.Event -= eventPair.eventDelegate;
				}
				eventPair.eventDelegate = null;
			}
		}

		public List<SpineEventUnityHandler.EventPair> events = new List<SpineEventUnityHandler.EventPair>();

		private ISkeletonComponent skeletonComponent;

		private IAnimationStateComponent animationStateComponent;

		[Serializable]
		public class EventPair
		{
			[SpineEvent("", "", true, false)]
			public string spineEvent;

			public UnityEvent unityHandler;

			public AnimationState.TrackEntryEventDelegate eventDelegate;
		}
	}
}
