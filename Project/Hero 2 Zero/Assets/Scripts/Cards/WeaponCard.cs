using UnityEngine;
using System.Collections;

public class WeaponCard : ItemCard
{
	#region Variables
	// The attack value of the card.
	int attack = 0;
	
	// The defense value of the card.
	int defense = 0;
	
	// How many uses the weapon has.
	int durability = 0;
	
	#endregion
	
	// Constructor.
	public WeaponCard (int at, int def, int du, string na, int co, int ef, int va, int us, int im, string des, int ty)
					: base (na, co, ef, va, us, im, des, ty)
	{
		// Sets the values for the weapon.
		attack = at;
		defense = def;
		durability = du;
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
		durability += d;
	}
}
