using UnityEngine;
using System.Collections;

public class MonsterCard : Card
{
	#region Variables
	
	int monsterImgIndex;
	int monsterModelIndex;

	string name = "";

	int health = 0;
	int strength = 0;
	int defense = 0;
	int goldVictoryGain = 0;
	int fameGain = 0;
	int fameLose = 0;
	int expMod = 0;
	
	#endregion

	// Use this for initialization
	public MonsterCard (string nm, int monIm, int model, int hp, int str, int def, int gain, int faga, int falo, int exp, int im, string de) : base (im, nm, de, 8)
	{
		name = nm;

		// monster image
		monsterImgIndex = monIm;
		monsterModelIndex = model;


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
		expMod = exp;

		Debug.Log ("expMod: " + expMod);
	}
	
	public MonsterCard (MonsterCard mon) : base (mon.GetImageIndex(), mon.name, mon.GetDescription(), mon.GetCardType())
	{
		monsterImgIndex = mon.monsterImgIndex;
		monsterModelIndex = mon.monsterModelIndex;
		name = mon.name;
		health = mon.health;
		strength = mon.strength;
		defense = mon.defense;
		goldVictoryGain = mon.goldVictoryGain;
		fameGain = mon.fameGain;
		fameLose = mon.fameLose;
		expMod = mon.expMod;
	}
	
	// This will allow to make a deep copy of another monster instance.
	public void CloneMonster(MonsterCard mon)
	{
		monsterImgIndex = mon.monsterImgIndex;
		monsterModelIndex = mon.monsterModelIndex;
		name = mon.name;
		health = mon.health;
		strength = mon.strength;
		defense = mon.defense;
		goldVictoryGain = mon.goldVictoryGain;
		fameGain = mon.fameGain;
		fameLose = mon.fameLose;		
	}
	
	public void SetValues(string n, int s, int h)
	{
		name = n;
		strength = s;
		health = h;
	}
	
	public string GetName()
	{
		return name;
	}

	public int GetMonImg()
	{
		return monsterImgIndex;
	}

	public int GetMonModelIndex()
	{
		return monsterModelIndex;
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
		if (gain) {
			return fameGain;
		}
		else {
			return fameLose;
		}
	}

	public int GetExpMod(bool gain)
	{
		if (gain)
		{
			return expMod;
		}
		else
		{
			return -expMod;
		}
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
