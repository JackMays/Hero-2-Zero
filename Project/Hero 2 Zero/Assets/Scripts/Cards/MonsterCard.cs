using UnityEngine;
using System.Collections;

public class MonsterCard : Card
{
	#region Variables
	
	int monsterImgIndex;

	string name = "";

	int health = 0;
	int strength = 0;
	int defense = 0;
	int goldVictoryGain = 0;
	int fameGain = 0;
	int fameLose = 0;
	
	#endregion

	// Use this for initialization
	public MonsterCard (string nm, int monIm, int hp, int str, int def, int gain, int faga, int falo, int im, string de, int ty) : base (im, de, ty)
	{
		name = nm;

		// monster image
		monsterImgIndex = monIm;

		// Monster Health
		health = hp;
		// Attack strength
		strength = str;
		// Defense.
		defense = def;
		// gold earned on a win
		goldVictoryGain = gain;
		// Fame/glory given or taken away
		fameGain = faga;
		fameLose = falo;
		

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
	
	public int GetDefense()
	{
		return defense;
	}

	public int GetVictoryGold()
	{
		return goldVictoryGain;
	}

	public int GetFameMod(bool gain)
	{
		return (gain) ? fameGain : fameLose;
	}

	public bool HasDied()
	{
		return (health == 0);
	}

	public void TakeDamage(int dmg)
	{
		health -= dmg;

		Debug.Log ("Health: " + health);

		if (health < 0)
		{
			health = 0;
		}
	}
}
