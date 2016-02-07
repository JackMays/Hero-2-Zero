using UnityEngine;
using System.Collections;

public class SpellCard : ItemCard
{
	#region Variables	
	// Holds the target of the spell. 0: Player | 1: Select Player | 2: Everyone | 3: Monster
	int target = 0;
	
	#endregion
	
	// Constructor.
	public SpellCard (int ta, string na, int co, int ef, int va, int us, int im, string de, int ty)
					: base (na, co, ef, va, us, ta, im, de, ty)
	{
		// Sets the values for the card.
		target = ta;		
	}
	
	// Returns the target of the card.
	public int GetTarget()
	{
		return target;
	}
}
