using UnityEngine;
using System.Collections;

public class ItemEventCard : Card
{
	#region Variables
	// Holds whether the item will be added or removed.
	bool addItem = false;
	
	// Holds the item index.
	int itemIndex = 0;
	
	// Holds the target of the card.
	int target = 0;
	#endregion
	
	// Use this for initialization
	public ItemEventCard (bool ad, int ii, int t, int im, string ti, string de) : base (im, ti, de, 3)
	{
		// Sets the index for the item, the target and whetehr the item is to be removed or added.
		itemIndex = ii;
		
		target = t;
		
		addItem = ad;
	}
	
	// Returns whether the item should be added or not.
	public bool GetAddRemove()
	{
		return addItem;
	}
	
	// Returns the index where the item is.
	public int GetItemIndex()
	{
		return itemIndex;
	}
	
	// Returns the target of the card.
	public int GetTarget()
	{
		return target;
	}
}
