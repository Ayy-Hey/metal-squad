using System;

public class FxGrenadeIce : FxGrenadeBase
{
	private void OnDisable()
	{
		try
		{
			if (!this.isPreview)
			{
				GameManager.Instance.fxManager.PoolGrenadeIce.Store(this);
			}
			else
			{
				PreviewWeapon.Instance.PoolGrenadeIce.Store(this);
			}
		}
		catch
		{
		}
	}
}
