﻿using UnityEngine;
using System.Collections;

public class ChoiceCard : Card
{
	#region Variables
	// Text in first choice.
	string choice1Text = "";
	
	// Text in second choice.
	string choice2Text = "";
	
	// Each choice can have max 2 effects.
	// 0: Nothing happens | 1: Change fame | 2: Change gold | 3: Change items | 4: Teleport | 5: Change health
	// 6: Battle | 7: Become vaillain
	
	// Contains the effects of choice 1.
	int[] choice1Effects;
	// Contains the targets of choice 1.
	int[] choice1Targets;
	// Contains the values of choice 1.
	int[] choice1Values;
	// Applies on to Item and Move effects. Holds whether the choice adds or removes items.
	bool[] choice1Extra;
	
	// Contains the effects of choice 2.
	int[] choice2Effects;
	// Contains the targets of choice 2.
	int[] choice2Targets;
	// Contains the values of choice 2.
	int[] choice2Values;
	// Applies on to Item and Move effects. Holds whether the choice adds or removes items or move / teleport.
	bool[] choice2Extra;
	
	#endregion
	
	// Constructor
	public ChoiceCard(string c1, string c2, int[] c1E, int[] c1T, int[] c1V, 
						int[] c2E, int[] c2T, int[] c2V, int im, string de)
						: base(im, de, 4)
	{
		// Sets the choice 1 values.
		choice1Text = c1;
		choice1Effects = c1E;
		choice1Targets = c1T;
		choice1Values = c1V;
		
		// Sets the choice 2 values.
		choice2Text = c2;
		choice2Effects = c2E;
		choice2Targets = c2T;
		choice2Values = c2V;
	}
	
	// Returns the text for the given choice.
	public string GetChoiceText(int c)
	{
		// Checks if the first choice is required.
		if (c == 0) {
			// Returns the text for choice 1.
			return choice1Text;
		}
		
		// Returns the text for choice 2.
		return choice2Text;
	}
	
	// Returns the effects for the given choice.
	public int[] GetChoiceEffects(int c)
	{
		// Checks if the first choice is required.
		if (c == 0) {
			// Returns the effects for choice 1.
			return choice1Effects;
		}
		
		// Returns the effects for choice 2.
		return choice2Effects;
	}
	
	// Returns the targets for the given choice.
	public int[] GetChoiceTargets(int c)
	{
		// Checks if the first choice is required.
		if (c == 0) {
			// Returns the targets for choice 1.
			return choice1Targets;
		}
		
		// Returns the targets for choice 2.
		return choice2Targets;
	}
	
	// Returns the values for the given choice.
	public int[] GetChoiceValues(int c)
	{
		// Checks if the first choice is required.
		if (c == 0) {
			// Returns the values for choice 1.
			return choice1Values;
		}
		
		// Returns the values for choice 2.
		return choice2Values;
	}
	
	// Returns the whether the choice should remove or add an item.
	public bool[] GetChoiceExtra(int c)
	{
		// Checks if the first choice is required.
		if (c == 0) {
			// Returns the values for choice 1.
			return choice1Extra;
		}
		
		// Returns the values for choice 2.
		return choice2Extra;
	}
}
