using UnityEngine;
using System.Collections;

public class ItemCard : Card
{
	#region Variables
	// Name of the item.
	string name = "";
	
	// Holds the cost of the item.
	int cost = 0;
	
	// Holds the effect index of the card.
	int effect = 0;
	
	// Holds the value for whatever the card does.
	int value = 0;
	
	// Holds where the item can be used. 0: Anywhere | 1: Battle Only | 2: Board Only
	int usableArea = 0;

	// Who the ard affects; 0: Current Player | 1: Next Player | 2: Every other player | 3: Every player | 4: Select Player | 5: Board Tile

	int targetType = 0;
	
	#endregion
	
	// Constructor.
	public ItemCard (string na, int co, int ef, int va, int us, int ta, int im, string de, int ty)
					: base (im, de, ty)
	{
		// Sets the item's values.
		name = na;
		cost = co;
		effect = ef;
		value = va;
		usableArea = us;
		targetType = ta;
	}
	
	// Returns the name of the item.
	public string GetName()
	{
		return name;
	}
	
	// Returns the cost of the item.
	public int GetCost()
	{
		return cost;
	}

	// Returns the effect of the card.
	public int GetEffect()
	{
		return effect;
	}
	
	// Returns the value of the item.
	public int GetValue()
	{
		return value;
	}

	// Returns where the item can be used.
	public int GetUsableArea()
	{
		return usableArea;
	}

	public int GetTargetType()
	{
		return targetType;
	}
}
