using UnityEngine;
using System.Collections;

public class HealthCard : Card
{
	#region Variables
	// The change in health.
	int health = 0;
	
	// The players to be effected.
	int target = 0;	
	
	#endregion
	
	// Constructor
	public HealthCard(int he, int ta, int im, string de) : base (im, de, 6)
	{
		// Sets the health change.
		health = he;
		
		// Sets teh target.
		target = ta;
	}
	
	// Returns teh health change.
	public int GetHealth()
	{
		return health;
	}
	
	// Returns the target.
	public int GetTarget()
	{
		return target;
	}
}
