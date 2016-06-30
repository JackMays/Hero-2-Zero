using UnityEngine;
using System.Collections;

public class DiceCard : Card
{
	#region Variables
	// The dice change value.
	int dice = 0;
	
	// The target of the card.
	int target = 0;
	
	#endregion
	
	// Constructor
	public DiceCard(int di, int ta, int im, string ti, string de) : base (im, ti, de, 11)
	{
		// Sets the fame.
		dice = di;
		
		// Sets the target.
		target = ta;
	}
	
	// Returns the fame.
	public int GetDice()
	{
		return dice;
	}
	
	// Returns the target.
	public int GetTarget()
	{
		return target;
	}
}
