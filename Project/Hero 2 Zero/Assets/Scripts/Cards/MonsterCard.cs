using UnityEngine;
using System.Collections;

public class MonsterCard : Card{

	int monsterImgIndex;

	string name = "";

	int health = 0;
	int strength = 0;
	int goldVictoryGain = 0;
	int fameModifier = 0;

	// Use this for initialization
	public MonsterCard (string nm, int monIm, int hp, int str, int gain, int famod, int im, string de, int ty) : base (im, de, ty)
	{
		name = nm;

		// monster image
		monsterImgIndex = monIm;

		// Monster Health
		health = hp;
		// Attack strength
		strength = str;
		// gold earned on a win
		goldVictoryGain = gain;
		// Fame/glory given or taken away
		fameModifier = famod;
		

	}

	public string GetName()
	{
		return name;
	}

	public int GetMonImg()
	{
		return monsterImgIndex;
	}
	

	public int GetHealth()
	{
		return health;
	}

	public int GetStrength()
	{
		return strength;
	}

	public int GetVictoryGold()
	{
		return goldVictoryGain;
	}

	public int GetFameMod()
	{
		return fameModifier;
	}

	public bool HasDied()
	{
		return (health == 0);
	}
}
