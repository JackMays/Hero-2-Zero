using UnityEngine;
using System.Collections;

public class Card
{
	#region Variables
	
	// The image for the card.
	int imageIndex = 0;
	
	// The title for the card.
	string title = "";
	
	// The card's description.
	string description = "";
	
	// What kind of card is it. 0: Nothing | 1: Fame change | 2: Gold change | 3: Item Change | 4: Choice |
	// 5: Teleport | 6: Health change | 7: Create Monster | 8: Monster | 9: Skip Turn | 10: Multiple Effects |
	// 11: Dice Effect | 12: Magic Effect
	int cardType = 0;

	#endregion
	
	// Use this for initialization
	public Card (int i, string ti, string d, int t)
	{
		imageIndex = i;
		title = ti;
		description = d;
		cardType = t;
	}
	
	#region Getters
	// Returns the index for the card's image.
	public int GetImageIndex()
	{
		return imageIndex;
	}
	
	// Returns the title of the card.
	public string GetTitle()
	{
		return title;
	}
	
	// Returns the description of the card.
	public string GetDescription()
	{
		return description;
	}
	
	// Returns the area where the card will appear.
	public int GetCardType()
	{
		return cardType;
	}
	#endregion
	
}
