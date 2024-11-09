using System;
using Spine.Unity;

[Serializable]
public class UISpriteBoss
{
	public SkeletonGraphic skeleton_Boss;

	[SpineAnimation("", "", true, false)]
	public string str_nameAni_On;

	[SpineAnimation("", "", true, false)]
	public string str_nameAni_Off;
}
