using UnityEngine;
using System.Collections;

public class WeaponCard : Card
{
	#region Variables

	string name = "";

	// The attack value of the card.
	int attack = 0;
	
	// The defense value of the card.
	int defense = 0;
	
	// How many uses the weapon has.
	int durability = 0;
	
	#endregion
	
	// Constructor.
	public WeaponCard (string na, int at, int def, int du, int im, string des, int ty)
					: base (im, na, des, ty)
	{
		name = na;

		// Sets the values for the weapon.
		attack = at;
		defense = def;
		durability = du;
	}

	public WeaponCard (WeaponCard weap) : base (weap.GetImageIndex(), "", weap.GetDescription(), weap.GetCardType())
	{
		name = weap.GetName();
		
		// Sets the values for the weapon.
		attack = weap.GetAttack();
		defense = weap.GetDefense();
		durability = weap.GetDurability();
	}

	public string GetName()
	{
		return name;
	}
	
	// Returns the attack of the weapon.
	public int GetAttack()
	{
		return attack;
	}
	
	// Returns the defense of the weapon.
	public int GetDefense()
	{
		return defense;
	}
	
	// Returns the durability of the weapon.
	public int GetDurability()
	{
		return durability;
	}
	
	// Sets the durability of the weapon.
	public void SetDurability(int d)
	{
		durability = d;
	}
	
	// Changes the durability of the weapon.
	public void ChangeDurability(int d)
	{
		Debug.Log ("Decayed weapon Durability by: " + d);
		durability += d;
	}
}
