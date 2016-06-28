using UnityEngine;
using System.Collections;

public class MultipleEffectCard : Card
{
	#region Variables
	// The effects of the card.
	int[] effects;
	
	// The targets of the effects.
	int[] targets;
	
	// The values of the effects.
	int[] values;
	
	#endregion
	
	// Constructor.
	public MultipleEffectCard(int[] e, int[] t, int[] v, int im, string ti, string de) : base(im, ti, de, 10)
	{
		// Sets the variables.
		effects = e;
		targets = t;
		values = v;
	}
	
	// Returns one of the int arrays.
	public int[] GetCardArray(int a)
	{
		// Returns a different array depending on the given value.
		
		// Returns the effect array.
		if (a == 0)
		{
			return effects;
		}
		
		// Returns the targets array.
		if (a == 1)
		{
			return targets;
		}
		
		// Returns the values array.
		return values;
	}
}
