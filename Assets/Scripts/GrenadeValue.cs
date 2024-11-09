using System;

public class GrenadeValue : GunValue
{
	public GrenadeValue()
	{
		this.gunName = this.name;
	}

	private float[] damages = new float[9];

	private float[] capacitys = new float[9];

	private float[] fireRates = new float[]
	{
		10f,
		30f,
		20f,
		40f,
		0f,
		10f,
		30f,
		0f,
		40f
	};

	private int[] coins = new int[10];

	private string name = "M4A1";

	private int price = 100;
}
