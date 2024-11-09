using System;

public class FxGrenadeBasic : FxGrenadeBase
{
	private void OnDisable()
	{
		try
		{
			if (!this.isPreview)
			{
				GameManager.Instance.fxManager.PoolGrenadeBasic.Store(this);
			}
			else
			{
				PreviewWeapon.Instance.PoolGrenadeBasic.Store(this);
			}
		}
		catch
		{
		}
	}
}
