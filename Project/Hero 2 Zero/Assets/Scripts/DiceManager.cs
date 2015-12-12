using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiceManager : MonoBehaviour
{
	#region Variables
	// List of Dice.
	public List<Dice> listDice;

	// The button to roll dice.
	string rollButton = "Fire1";

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
		
		// Loops through all the dice.
		foreach (Dice d in listDice) {
			count += d.GetResult();
		}
		
		// Returns the result.
		return count;
	}
	
	// Returns whether the dice have stopepd rolling.
	public bool IsRolling()
	{
		return isRolling;
	}
	
	// Rolls the dice.
	public void RollDice()
	{
		// Rolls all the dice.
		foreach (Dice d in listDice) {
			d.ApplyForce ();
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