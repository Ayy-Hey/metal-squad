using System;

public class FxGrenadeToxic : FxGrenadeBase
{
	private void OnDisable()
	{
		try
		{
			if (!this.isPreview)
			{
				GameManager.Instance.fxManager.PoolGrenadeToxic.Store(this);
			}
			else
			{
				PreviewWeapon.Instance.PoolGrenadeToxic.Store(this);
			}
		}
		catch
		{
		}
	}
}
