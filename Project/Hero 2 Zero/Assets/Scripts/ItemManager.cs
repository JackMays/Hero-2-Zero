using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The item manager will be used to handle the effects of items and spells as well.
// Much easier to just combine these two concepts since they aren't much different functionality wise
// and it saves making another manager class.

public class ItemManager
{
	#region Variables
	// Reference to the list of players.
	List<Player> playersRef;
	
	// Holds whether a monster needs to be spawned.
	bool needSpawnMonster = false;
	
	// Holds whether a trap need to be laid.
	bool needLayTrap = false;
	
	// The camera to raycast from when selecting shit.
	public Camera rayCam;
	
	CardManager cardManager;
	Map map;
	Card activeCard = new Card(0,"",0);
	#endregion
	
	
	// Constructor.
	public ItemManager(List<Player> p, CardManager cMan, Map m)
	{
		playersRef = p;
		Debug.Log (playersRef[0].gameObject.name);
		Debug.Log (playersRef[1].gameObject.name);
		cardManager = cMan;
		map = m;
		//playerCam = GameObject.Find("Camera (Map)").GetComponent<Camera>();
		//rayCam = GameObject.Find ("Raycast Camera").GetComponent<Camera>();
		//rayCam.gameObject.SetActive(false);
	}
	
	#region Item Effects
	// Takes in the effect of the item, and the target from the target state
	// to apply the effect to.
	public void ApplyItemEffect(ItemCard card, int target)
	{
		Debug.Log ("applying targeted item effect");

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
				SpawnMonster();
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
	// Overload for Target Function situations
	public void ApplyItemEffect(ItemCard card, List<Player> players, int currentPlayer)
	{
		Debug.Log ("applying item effect");
		
		Debug.Log ("card effect: " + card.GetEffect().ToString() + " target type: " + card.GetTargetType().ToString() + " Value: " + card.GetValue ().ToString());
		
		// Holds the targets to get affected by card and the card's value.
		List<Player> targets = new List<Player>();
		int value = card.GetValue();
		
		// Checks if the card needs a spot selected.
		if (card.GetTargetType() == 5) {
			// Checks if a spot has not been selected yet.
			if (selectedTile == null) {
				// Sets the card as the active card.
				activeCard = card;
			
				// Enters raycast mode to select a tile.
				StartRaycasting();
			}
		}
		// Doesn't need a spot selected so it finds predetermined targets.
		else {
			targets = FindTargets(card.GetTargetType(), players, currentPlayer);
		}
		
		for (int i = 0; i < targets.Count; ++i)
		{
			// Checks the effect index and calls the appropriate function.
			switch (card.GetEffect()) {
				// Change Fame.
			case 0:
				targets[i].ChangeFame(value);
				break;
				// Change Gold.
			case 1:
				targets[i].ChangeGold(value);
				break;
				// Change Health.
			case 2:
				targets[i].ChangeHealth(value);
				break;
				// Change Dice.
			case 3:
				targets[i].ChangeDice(value);
				break;
				// Skip Monster.
			case 4:
				targets[i].SetSkipMonster(true);
				break;
				// Spawn Monster.
			case 5:
				needSpawnMonster = true;
				break;
				// Skip Turn.
			case 6:
				targets[i].ChangeTurnsToSkip(value);
				break;
				// Become Villain.
			case 7:
				targets[i].SetVillain(true);
				break;
			}
		}
	}
	
	// Changes the current fame of the player by the item's value.
	void ChangeFame(int value, int currentPlayer)
	{
		playersRef[currentPlayer].ChangeFame(value);
	}
	
	// Changes the current gold of the player by the item's value.
	void ChangeGold(int value, int currentPlayer)
	{
		playersRef[currentPlayer].ChangeGold(value);
	}
	
	// Changes the health of the player by the item's value.
	void ChangeHealth(int value, int currentPlayer)
	{
		playersRef[currentPlayer].ChangeHealth(value);
	}
	
	// Changes the number of dice the player can use.
	void ChangeDice(int value, int currentPlayer)
	{
		playersRef[currentPlayer].ChangeDice(value);
	}
	
	// Sets that the player skips the next monster they encounter.
	void SkipMonster(int currentPlayer)
	{
		playersRef[currentPlayer].SetSkipMonster(true);
	}
	
	// Spawns a monster on the selected tile.
	void SpawnMonster()
	{
		Debug.Log("SPAWNING");
	
		// Converts the tile to map position.
		Vector2 mapPos = new Vector2(Mathf.Abs(selectedTile.transform.localPosition.z) / 2, selectedTile.transform.localPosition.x / 2);
		// Position to spawn the monster at.
		Vector3 pos = selectedTile.transform.position;
		pos.y = 0;
		
		// Draws a monster card.
		MonsterCard mon = (MonsterCard)cardManager.DrawMonsterCard();
		
		// Adds the monster to the disgusting map arrays.
		map.AddMonsterToTile((int)mapPos.y, (int)mapPos.x, mon);
		map.AddPrefabToTiles((int)mapPos.y, (int)mapPos.x, cardManager.GetMonsterModel(mon.GetMonModel()), pos);
		
		EndRaycasting();
	}
	
	// Makes the current player skip the next turn.
	void SkipTurn(int value, int currentPlayer)
	{
		playersRef[currentPlayer].ChangeTurnsToSkip(value);
	}
	
	// Makes the current player the villain.
	void BecomeVillain(int currentPlayer)
	{
		playersRef[currentPlayer].SetVillain(true);
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
	
	
	#region Raycast
	// Whether the raycast camera is active.
	bool isRaycasting = false;
	
	// Holds the player's camera.
	public Camera playerCam;
	
	// The selection rectangle.
	public GameObject selectionSprite;
	
	// The layer mask to select only tiles.
	LayerMask lMask = 1 << 10;
	
	// Holds whether a tile is currently selected.
	GameObject selectedTile;
	
	public void SetCameras(Camera playC, Camera raycastC, GameObject s)
	{
		playerCam = playC;
		rayCam = raycastC;
		selectionSprite = s;
	}
	
	// Changes the active camera to the raycast camera and enters raycast mode.
	public void StartRaycasting()
	{
		// Changes the active camera.
		playerCam.gameObject.SetActive(false);
		rayCam.gameObject.SetActive(true);
		
		// Sets that the game should raycast.
		isRaycasting = true;
		
		// Sets that no tile has been selected.
		selectedTile = null;
	}
	
	// Casts the ray.
	public void RayCast()
	{
		Debug.Log("Casting");
	
		// Where to cast the ray from.
		Ray ray = rayCam.ScreenPointToRay(Input.mousePosition);
			
		// Holds the data on the hit target.
		RaycastHit hit;
			
		// Casts the ray.
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, lMask)) {
			// Checks if the hit tile is the same tile that was already selected. (Double click basically).
			if (hit.transform.gameObject == selectedTile) {
				// Activates the card's effect.
				ApplyItemEffect((ItemCard)activeCard, 5);
			}
			else {
				// Shows the selection sprite.
				selectionSprite.SetActive(true);
				// Moves the selection sprite to just above the targeted tile.
				selectionSprite.transform.position = new Vector3(hit.transform.position.x, 0.15f, hit.transform.position.z);
				// Stores the selected tile.
				selectedTile = hit.transform.gameObject;
				
				Debug.Log(new Vector2(Mathf.Abs(hit.transform.localPosition.z) / 2, hit.transform.localPosition.x / 2));
			}
		}
		else {
			// Hides the selection sprite.
			selectionSprite.SetActive(false);
			// Sets that no tile has been selected.
			selectedTile = null;
		}
	}

	
	// Changes the active camera back to the player camera and ends raycast mode.
	public void EndRaycasting()
	{
		// Changes the active camera.
		playerCam.gameObject.SetActive(true);
		rayCam.gameObject.SetActive(false);
		
		// Hides the selection object.
		selectionSprite.SetActive(false);
		
		// Sets that no tile is selected.
		selectedTile = null;
		
		// Sets no active card.
		activeCard = null;
		
		// Sets that the game should stop raycasting.
		isRaycasting = false;
	}
	
	// Returns whether in raycast mode or not.
	public bool IsInRaycastMode()
	{
		return isRaycasting;	
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
			for (int i = 0; i < players.Count; ++i) {

				Debug.Log ("Index: " + i.ToString() + " currentPlayer: " + currentPlayer.ToString());

				// Checks not the current player.
				if (i != currentPlayer) 
				{
					Debug.Log ("should not add player");
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