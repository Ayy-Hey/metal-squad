using System;

public class OptionControl2 : OptionControl
{
	public override void Show()
	{
		base.Show();
	}

	public void MoveRambo(int direction)
	{
		if (GameManager.Instance.player.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		GameManager.Instance.player.EMovement = (BaseCharactor.EMovementBasic)direction;
	}

	public void TouchUpMove()
	{
		if (GameManager.Instance.player.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		GameManager.Instance.player.EMovement = BaseCharactor.EMovementBasic.Release;
	}
}
