using UnityEngine;
using System.Collections;

public class SpellCard : ItemCard
{
	#region Variables
	// Holds the spell's effect.
	int effect = 0;
	
	// Holds the target of the spell. 0: Player | 1: Select Player | 2: Everyone | 3: Monster
	int target = 0;
	
	#endregion
	
	// Constructor.
	public SpellCard (int ef, int ta, string na, int co, int us, int im, string de, int ty)
					: base (na, co, us, im, de, ty)
	{
		// Sets the values for the card.
		effect = ef;
		target = ta;		
	}
	
	// Returns the card's effect index.
	public int GetEffect()
	{
		return effect;
	}
	
	// Returns the target of the card.
	public int GetTarget()
	{
		return target;
	}
}
