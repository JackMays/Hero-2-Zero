using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiceManager : MonoBehaviour
{
	#region Variables
	// List of Dice.
	public List<Dice> listDice;

	// The number of dice to throw.
	int numDice = 2;

	// Holds whether the dice are still rolling.
	bool isRolling = false;
	
	#endregion

	// Use this for initialization
	void Start ()
	{

	}
	
	// Checks to see if all the dice hasve stopped rolling.
	bool HaveDiceStopped()
	{
		// Holds number of dice that have stopped.
		int stopped = 0;
		
		// Loops through all the dice.
		foreach (Dice d in listDice) {
			// Checks if their result has been output.
			if (!d.isRolling) {
				// Increments the number of stopped dice.
				++stopped;
			}
		}
		
		// Checks if all dice have stopped.
		if (stopped == listDice.Count) {
			return true;
		}
		
		// Returns that not all dice have stopped.
		return false;
	}
	
	// Gets the result of each dice and returns the total.
	public int GetDiceResults()
	{
		// Holds the total result.
		int count = 0;
		
		// Loops through all the thrown dice.
		for (int i = 0; i < numDice; ++i) {
			count += listDice[i].GetResult();
		}
		
		// Returns the result.
		return count;
	}
	
	// Returns whether the dice have stopepd rolling.
	public bool IsRolling()
	{
		return isRolling;
	}
	
	// Hides/Shows dice based on the number of dice the player can throw.
	public void ShowDice(int numD)
	{
		// Checks if the already the correct number of dice.
		if (numDice == numD) {
			return;
		}
		
		// The number of dice is not correct so some need to be hidden/shown.
		numDice = numD;
		
		// Loops through the list of dice and shows the throwable while hiding the unused.
		for (int i = 0; i < listDice.Count; ++i) {
			// Checks if the current dice can be thrown.
			if (i < numD) {
				// Shows the dice.
				listDice[i].gameObject.SetActive(true);
			}
			else {
				// Hides the dice.
				listDice[i].gameObject.SetActive(false);
			}
		}
	}
	
	// Rolls the dice.
	public void RollDice()
	{
		// Loops through all the throwable dice.
		for (int i = 0; i < numDice; ++i) {
			listDice[i].ApplyForce ();
		}
		
		// Sets that the dice are rolling.
		isRolling = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Checks if the dice are rolling.
		if (isRolling) {
			// Checks if all the dice have stopped.
			if (HaveDiceStopped()) {
				// Gets the result of each dice.
				int count = GetDiceResults();
				
				// Outputs the total count.
				Debug.Log("Total Count : " + count);
				
				// Sets that the dice can be rolled again.
				isRolling = false;
			}
		}
	}
}