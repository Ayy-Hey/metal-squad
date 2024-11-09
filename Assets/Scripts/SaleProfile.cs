using System;

public class SaleProfile
{
	public SaleProfile(string saleName)
	{
		this.saleTime = new IntProfileData(this.strSaleGun, 0);
	}

	public int SaleTime
	{
		get
		{
			return this.saleTime.Data;
		}
		set
		{
			this.saleTime.setValue(value);
		}
	}

	private string strSaleGun = "com.metal.squad.sale.time";

	private IntProfileData saleTime;
}
