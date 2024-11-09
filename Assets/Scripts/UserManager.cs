using System;

public class UserManager
{
	public static UserManager User
	{
		get
		{
			if (UserManager.user == null)
			{
				UserManager.user = new UserManager();
			}
			return UserManager.user;
		}
	}

	public void YouLost()
	{
		this.CountLostRepeat++;
	}

	public void ResetCountRepeat()
	{
		this.CountLostRepeat = 0;
	}

	public bool RepeatLost
	{
		get
		{
			return this.CountLostRepeat > 0;
		}
	}

	public int CountLost
	{
		get
		{
			return this.CountLostRepeat;
		}
	}

	private static UserManager user;

	private int CountLostRepeat;
}
