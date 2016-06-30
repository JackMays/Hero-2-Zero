using UnityEngine;
using System.Collections;

public class MagicCard : Card
{
	#region Variables
	// The magic change value.
	int magic = 0;
	
	// The target of the card.
	int target = 0;
	
	#endregion
	
	// Constructor
	public MagicCard(int ma, int ta, int im, string ti, string de) : base (im, ti, de, 12)
	{
		// Sets the fame.
		magic = ma;
		
		// Sets the target.
		target = ta;
	}
	
	// Returns the fame.
	public int GetMagic()
	{
		return magic;
	}
	
	// Returns the target.
	public int GetTarget()
	{
		return target;
	}
}
