using System;
using com.dev.util.SecurityHelper;

[Serializable]
public class OnlineGift
{
	public void OnInit()
	{
		this.amountItem_security = new SecuredInt(this.amountItem);
	}

	public int ID
	{
		get
		{
			return this.id_security.Value;
		}
	}

	public int AmountItem
	{
		get
		{
			return this.amountItem_security.Value;
		}
	}

	public Item itemType;

	public int amountItem;

	private SecuredInt id_security;

	private SecuredInt amountItem_security;
}
