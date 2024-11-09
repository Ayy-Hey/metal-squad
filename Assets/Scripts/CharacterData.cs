using System;

public class CharacterData
{
	public CharacterData()
	{
		this.skills = new Skill[ProfileManager.CHARACTER_MAX_SKILL];
		for (int i = 0; i < this.skills.Length; i++)
		{
			this.skills[i] = new Skill();
		}
	}

	public Skill[] skills;
}
