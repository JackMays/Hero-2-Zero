using UnityEngine;
using System.Collections;

public class GoldCard : Card
{
	#region Variables
	// The amount of gold that will be changed.
	int gold = 0;
	
	// The target of the card.
	int target = 0;
	
	#endregion
	
	// Constructor.
	public GoldCard(int go, int ta, int im, string ti, string de) : base (im, ti, de, 2)
	{
		// Sets the gold.
		gold = go;
		
		// Sets the target.
		target = ta;
	}
	
	// Returns the gold value of the card.
	public int GetGold()
	{
		return gold;
	}
	
	// Returns the target of the card.
	public int GetTarget()
	{
		return target;
	}
}
