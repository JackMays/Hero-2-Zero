﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	#region Variables	
	// Reference to the map.
	public Map map;
	
	// Reference to the card manager.
	public CardManager cardManager;
	
	// Reference to dice manager.
	public DiceManager diceManager;
	
	// Number of players.
	int numPlayers = 2;
	
	// List of the players.
	public List<Player> listPlayers;
	
	// The player whose turn it is.
	int currentPlayer = 0;
	
	// Current index in turn order.
	int turnIndex = 0;
	
	// The order in which player's take their turns.
	int[] turnOrder = new int[2] {0, 1};
	
	// State of player's turn. 0: Roll Dice | 1: Move Player | 2: Show Card | 3: Change player
	int turnState = 0;
	
	// Button to roll the dice.
	string rollButton = "Fire1";
	
	// Holds whether the dice have been rolled.
	bool diceRolled = false;
	
	#endregion
	
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Changes the player's turn.
	void ChangeTurns()
	{
		// Increments the index in turn order.
		++turnIndex;
		
		// Checks if all players have had their turn.
		if (turnIndex >= numPlayers) {
			// Resets turn index.
			turnIndex = 0;
		}
		
		// Sets which player's turn it is.
		currentPlayer = turnOrder[turnIndex];
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Rolling the dice stage.
		if (turnState == 0) {
			StateRollDice();
		}
		// Moving the player stage.
		else if (turnState == 1) {
			StateMovePlayer();
		}
		// Showing the card stage.
		else if (turnState == 2) {
			StateShowCard();
		}
		// Changing the player turn stage.
		else if (turnState == 3) {
			// Changes the player's turn.
			ChangeTurns();
			
			// Resets back to roll dice state.
			turnState = 0;
		}
	}
	
	// Waits for the player to roll the dice, then for them to stop and finally
	// passes the total die cast to the player.
	void StateRollDice()
	{
		// Checks if the dice have been rolled and if they have stopped.
		if (diceRolled && !diceManager.IsRolling()) {					
			// Resets dice rolled.
			diceRolled = false;
			
			// Starts teh player moving and updates the turn state.
			listPlayers[currentPlayer].StartMovement(diceManager.GetDiceResults());
			turnState = 1;
		}
		else if (diceRolled == false) {
			// Waits for the player to roll the dice.
			if (Input.GetButtonDown(rollButton)) {
				// Rolls the dice and sets to wait for result.
				diceManager.RollDice();
				diceRolled = true;
			}
		}
	}
	
	// Waits for the player to stop moving. (To be expanded)
	void StateMovePlayer()
	{
		// Checks if the player has stopped.
		if (!listPlayers[currentPlayer].GetMoving()) {
			// Updates the turn state to show the card.
			turnState = 2;
		}
	}
	
	// Gets the area tile that the player landed on and shows the first card
	// from the corresponding deck. (To be expanded)
	void StateShowCard()
	{
		// Checks if the card is already showing.
		if (cardManager.IsShowingCard()) {
			// Waits for the player to press the roll button to hide card and end turn.
			if (Input.GetButtonDown(rollButton)) {
				// Hides the card.
				cardManager.HideCard();
				
				// Changes turn state to change players.
				turnState = 3;
			}
		}
		else {
			// Gets the player's grid position.
			Vector2 playGrid = listPlayers[currentPlayer].GetPosition();
			
			// Gets the area type from the map at the player's position.
			int tileType = map.GetTile((int)playGrid.x, (int)playGrid.y)-1;
			
			// Shows the first card from the coresponding deck.
			cardManager.DisplayCard(tileType);
		}
	}
	
}
