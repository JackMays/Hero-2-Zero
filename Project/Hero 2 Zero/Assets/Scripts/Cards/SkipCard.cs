using UnityEngine;
using System.Collections;

public class SkipCard : Card
{
	#region Variables
	// Number of turns to skip.
	int skipNum = 0;
	
	#endregion
	
	// Constructor.
	public SkipCard(int s, int im, string ti, string de) : base (im, ti, de, 9)
	{
		skipNum = s;
	}
	
	// Returns the number of turns to skip.
	public int GetSkip()
	{
		return skipNum;
	}
}
