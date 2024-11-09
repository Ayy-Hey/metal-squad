using System;

public class Charactor
{
	public Charactor(float Hp, float Speed)
	{
		this.HP = Hp;
		this.Speed = Speed;
		this.Level = 1;
		this.Radius_Bomb = 55;
	}

	public int Level;

	public float Damage;

	public float Def;

	public float HP;

	public float Speed;

	public int Radius_Bomb;
}
