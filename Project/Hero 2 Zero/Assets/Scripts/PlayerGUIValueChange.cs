using UnityEngine;
using System.Collections;

public class PlayerGUIValueChange
{
	#region Variables
	// The player index to change the value of.
	int playerIndex;
	
	// The child index to change.
	int childIndex;
	
	// The original value.
	int originalValue;
	
	// The target value.
	int targetValue;
	
	// The difference between the 2 values.
	int changeInValue;
	
	// How far through the change the value is.
	float timeProgression = 0;
	
	// The parent's transform of the value to be changed.
	Transform parent = null;
	
	// Holds whether the change has finished or not.
	bool hasFinished = false;
	
	#endregion
	
	// Constructor.
	public PlayerGUIValueChange(int play, int chil, int orig, int chan)
	{	
		playerIndex = play;
		childIndex = chil;
		originalValue = orig;
		targetValue = orig + chan;
		changeInValue = chan;
		
		// Keeps target is within bounds.
		if (targetValue < 0) {
			targetValue = 0;
		}
		// Keeping within health bounds.
		if (chil == 3) {
			if (targetValue > 20) {
				targetValue = 20;
			}
		}
		// Keeping within magic bounds.
		if (chil == 4) {
			if (targetValue > 5) {
				targetValue = 5;
			}
		}
		// Keeping within fame/gold bounds.
		if (targetValue > 999) {
			targetValue = 999;
		}
	}
	
	// Constructor 2.
	public PlayerGUIValueChange(int play, int chil, int orig, int chan, int targ)
	{
		playerIndex = play;
		childIndex = chil;
		originalValue = orig;
		targetValue = orig + chan;
		changeInValue = chan;
	}
	
	#region Getters
	public int GetPlayerIndex()
	{
		return playerIndex;
	}
	
	public int GetChildIndex()
	{
		return childIndex;
	}
	
	public int GetOriginalValue()
	{
		return originalValue;
	}
	
	public int GetTargetValue()
	{
		return targetValue;
	}
	
	public float GetTime()
	{
		return timeProgression;
	}
	
	public Transform GetParent()
	{
		return parent;
	}
	
	public bool GetHasFinished()
	{
		return hasFinished;
	}
	
	public int GetChangeValue()
	{
		return changeInValue;
	}
	
	#endregion
	
	// Changes time by the value given.
	public void ChangeTime(float t)
	{
		timeProgression += t;
		
		// Checks if the change is fame/gold or healh/magic.
		if (childIndex == 3 || childIndex == 4) {
			// Health/Magic ends at 1 second.
			if (timeProgression >= 1) {
				hasFinished = true;
			}
		}
		// Fame/Gold ends at 3.5 seconds.
		else {
			if (timeProgression >= 3.5f) {
				hasFinished = true;
			}
		}
	}
	
	// Sets the parent transform.
	public void SetParent(Transform p)
	{
		parent = p;
	}
}
