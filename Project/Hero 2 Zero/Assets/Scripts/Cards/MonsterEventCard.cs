using UnityEngine;
using System.Collections;

public class MonsterEventCard : Card
{
	#region variables
	// The name of the monster to draw.
	string name = "";
	
	#endregion
	
	// Constructor.
	public MonsterEventCard(string na, int im, string de) : base (im, de, 7)
	{
		// Sets the name of the monster.
		name = na;
	}
	
	// Returns the anme of the monster to summon.
	public string GetName()
	{
		return name;
	}
}