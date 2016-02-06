﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The item manager will be used to handle the effects of items and spells as well.
// Much easier to just combine these two concepts since they aren't much different functionality wise
// and it saves making another manager class.

public class ItemManager
{
	#region Variables
	// Reference to the list of players.
	List<Player> players;
	
	// Holds whether a monster needs to be spawned.
	bool needSpawnMonster = false;
	
	// Holds whether a trap need to be laid.
	bool needLayTrap = false;
	
	
	#endregion
	
	
	// Constructor.
	public ItemManager(List<Player> p)
	{
		players = p;
		Debug.Log (players[0].gameObject.name);
		Debug.Log (players[1].gameObject.name);
	}
	
	#region Item Effects
	// Takes in the effect of the item, the target of the effect and the player list
	// to apply the effect to.
	public void ApplyItemEffect(ItemCard card, int target)
	{
		Debug.Log ("applying item effect");

		Debug.Log ("card effect: " + card.GetEffect().ToString() + " target: " + target.ToString() + " Value: " + card.GetValue ().ToString());

		// Checks the effect index and calls the appropriate function.
		switch (card.GetEffect()) {
			// Change Fame.
			case 0:
				ChangeFame(card.GetValue(), target);
				break;
			// Change Gold.
			case 1:
				ChangeGold(card.GetValue(), target);
				break;
			// Change Health.
			case 2:
				ChangeHealth(card.GetValue(), target);
				break;
			// Change Dice.
			case 3:
				ChangeDice(card.GetValue(), target);
				break;
			// Skip Monster.
			case 4:
				SkipMonster(target);
				break;
			// Spawn Monster.
			case 5:
				needSpawnMonster = true;
				break;
			// Skip Turn.
			case 6:
				SkipTurn(card.GetValue(), target);
				break;
			// Become Villain.
			case 7:
				BecomeVillain(target);
				break;
		}
	}
	
	// Changes the current fame of the player by the item's value.
	void ChangeFame(int value, int currentPlayer)
	{
		players[currentPlayer].ChangeFame(value);
	}
	
	// Changes the current gold of the player by the item's value.
	void ChangeGold(int value, int currentPlayer)
	{
		players[currentPlayer].ChangeGold(value);
	}
	
	// Changes the health of the player by the item's value.
	void ChangeHealth(int value, int currentPlayer)
	{
		players[currentPlayer].ChangeHealth(value);
	}
	
	// Changes the number of dice the player can use.
	void ChangeDice(int value, int currentPlayer)
	{
		players[currentPlayer].ChangeDice(value);
	}
	
	// Sets that the player skips the next monster they encounter.
	void SkipMonster(int currentPlayer)
	{
		players[currentPlayer].SetSkipMonster(true);
	}
	
	// Makes the current player skip the next turn.
	void SkipTurn(int value, int currentPlayer)
	{
		players[currentPlayer].ChangeTurnsToSkip(value);
	}
	
	// Makes the current player the villain.
	void BecomeVillain(int currentPlayer)
	{
		players[currentPlayer].SetVillain(true);
	}
	#endregion
	
	
	#region Spell Effects
	// Takes in the effect of the item, the target of the effect and the player list
	// to apply the effect to.
	void ApplyItemEffect(int itemEffect, int itemTarget, List<Player> players, int currentPlayer)
	{
		// Gets the targets for the item to affect.
		List<Player> targets = FindTargets(itemTarget, players, currentPlayer);
		
		// Checks the effect index and calls the appropriate function.
		switch (itemEffect) {
			// Move To Tile.
			case 0:
			
				break;
			// Player Buff.
			case 1:
			
				break;
			// Lay Trap.
			case 2:
			
				break;
			// Change Dice.
			case 3:
			
				break;
			// Skip Monster.
			case 4:
			
				break;
			// Summon Monster.
			case 5:
			
				break;
			// Skip Turn.
			case 6:
			
				break;
		}
	}
	
	
	#endregion
	
	// Finds the players which will be affected by the item.
	List<Player> FindTargets(int target, List<Player> players, int currentPlayer)
	{
		// 0: Current Player | 1: Next Player | 2: Every other player | 3: Every player
		
		// Checks if affects the current player.
		if (target == 0) {
			// Returns just the current player.
			//plays.Add(players[currentPlayer]);
			return new List<Player> {players[currentPlayer]};
		}
		
		// Checks if affect only the next player.
		if (target == 1) {
			// Increments the curent player index.
			++currentPlayer;
			
			// Checks if the index needs to be reset.
			if (currentPlayer >= players.Count) {
				currentPlayer = 0;
			}
			
			// Returns just the next player.
			//plays.Add(players[currentPlayer]);
			return new List<Player> {players[currentPlayer]};
		}
		
		// Checks if affects all other players.
		if (target == 2) {
			// list of players to return.
			List<Player> plays = new List<Player>();
			
			// Loops through all players.
			for (int i = 0; i < plays.Count; ++i) {
				// Checks not the current player.
				if (i != currentPlayer) {
					// Adds player to list.
					plays.Add(players[i]);
				}
			}
			
			// Returns the list.
			return plays;
		}

		// Returns all the players.
		return players;
	}	
}