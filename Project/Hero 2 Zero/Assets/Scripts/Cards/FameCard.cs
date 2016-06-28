using UnityEngine;
using System.Collections;

public class FameCard : Card
{
	#region Variables
	// The fame change value.
	int fame = 0;
	
	// The target of the card.
	int target = 0;
	
	#endregion
	
	// Constructor
	public FameCard(int fa, int ta, int im, string ti, string de) : base (im, ti, de, 1)
	{
		// Sets the fame.
		fame = fa;
		
		// Sets the target.
		target = ta;
	}

	// Returns the fame.
	public int GetFame()
	{
		return fame;
	}
	
	// Returns the target.
	public int GetTarget()
	{
		return target;
	}
}
