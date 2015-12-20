using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The item manager will be used to handle the effects of items and spells as well.
// Much easier to just combine these two concepts since they aren't much different functionality wise
// and it saves making another manager class.

public class ItemManager
{
	#region Variables
	
	
	
	#endregion
	
	
	#region Item Effects
	// Takes in the effect of the item, the target of the effect and the player list
	// to apply the effect to.
	void ApplyItemEffect(int itemEffect, int itemTarget, List<Player> players, int currentPlayer)
	{
		// Gets the targets for the item to affect.
	}
	
	
	#endregion
	
	
	#region Spell Effects
	
	
	#endregion
	
	// Finds the players which will be affected by the item.
	List<Player> FindTargets(int target, List<Player> players, int currentPlayer)
	{
		// 0: Current Player | 1: Next Player | 2: Every other player | 3: Every player
		// The list of players to return.
		//List<Player> plays = new List<Player>();
		
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