using UnityEngine;
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
	
	// Contains the effects of choice 2.
	int[] choice2Effects;
	// Contains the targets of choice 2.
	int[] choice2Targets;
	
	#endregion
	
	// Constructor
	public ChoiceCard(string c1, string c2, int[] c1E, int[] c1T,
						int[] c2E, int[] c2T, int im, string de, int ty)
						: base(im, de, ty)
	{
		// Sets the choice 1 values.
		choice1Text = c1;
		choice1Effects = c1E;
		choice1Targets = c1T;
		
		// Sets the choice 2 values.
		choice2Text = c2;
		choice2Effects = c2E;
		choice2Targets = c2T;
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
}
