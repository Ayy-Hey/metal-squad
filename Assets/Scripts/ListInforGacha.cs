using System;
using System.Collections.Generic;

public class ListInforGacha
{
	public void Create(int amountMax)
	{
		this.inforSpin = new InforGacha[amountMax];
		this.inforDisplay = new Dictionary<string, Item>();
	}

	public InforGacha[] inforSpin;

	public Dictionary<string, Item> inforDisplay;
}
