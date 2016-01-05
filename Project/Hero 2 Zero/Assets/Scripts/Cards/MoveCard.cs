using UnityEngine;
using System.Collections;

public class MoveCard : Card
{
	#region Variables
	// Number of tiles for the target to move.
	int moveTiles = 0;
	
	// The designated tile for the target to move to.
	// 0: Lane | 1: Village | 2: Field | 3: Forest | 4: Mountain | 5: Monster |
	// 6: Next Player | 7: 2nd Player | 8: 3rd Player | 9: Random
	int movePosition = 0;
	
	// Holds whether the card is a teleport or movement card.
	bool isMovement = false;
	
	// The targets that the card will effect.
	int target = 0;
	#endregion
	
	// Use this for initialization
	public MoveCard (int mt, int mp, bool isM, int t, int im, string de) : base (im, de, 5)
	{
		// Sets the passed values.
		moveTiles = mt;
		movePosition = mp;
		isMovement = isM;
		target = t;
	}
	
	// Returns whether the card will move the target or teleport them.
	public bool DoesCardMove()
	{
		return isMovement;
	}

	// Returns the number of tiles for the target to move.
	public int GetMoveCount()
	{
		return moveTiles;
	}
	
	// Returns the target position for the target to move to.
	public int GetTeleportTarget()
	{
		return movePosition;
	}

	// Returns the target of the card.
	public int GetTarget()
	{
		return target;
	}
}
