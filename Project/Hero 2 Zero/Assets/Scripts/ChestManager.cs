using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestManager
{
	#region Variables
	// List of the currently active chests.
	List<Chest> chests = new List<Chest>();
	
	// Parent of the chests.
	GameObject chestParent;
	
	// The chest prefab.
	public GameObject chestPrefab;
	
	// Reference to the Map.
	Map map;
	
	// Reference to the Item Manager.
	ItemManager itemManager;
	
	// Reference to the player info GUI.
	PlayerInfoGUI playerGUI;
	
	// Holds if a mimic needs to be spawned.
	bool spawnMimic = false;
	
	// Temp list of items. (Untilo cardmanager is safe)
	List<Card> items = new List<Card>();
	List<Card> weapons = new List<Card>();
	#endregion
	
	// Constructor.
	public ChestManager(Map m, ItemManager im, PlayerInfoGUI playInfo)
	{
		// Stores the map and item manager references.
		map = m;
		itemManager = im;
		playerGUI = playInfo; 
		
		// Loads the chest prefab for instantiating later.
		chestPrefab = Resources.Load<GameObject>("Prefabs/Chest");
		
		// Creates the parent gameobject and sets its position to align witht he tile's parent.
		chestParent = new GameObject("Chest Parent");
		chestParent.transform.position = GameObject.Find("Tiles").transform.position;
		
		// Attempts to place 10 chests in the world.
		PlaceChests(10, 50, false);
		
		items.Add(new ItemCard("Potion", 10, 2, 5, 2, 0, 0, "Heals 5 health.", 6));
		items.Add(new ItemCard("High Potion", 20, 2, 10, 2, 0, 0, "Heals 10 health.", 6));
		
		weapons.Add(new WeaponCard("Rusty Spoon", -2, 0, 5, 0, "A rusty spoon.", 0));
		weapons.Add(new WeaponCard("Grand Blade", 4, 0, 10, 0, "A powerful great sword.", 0));
	}
	
	
	// Places chests on random tiles.
	void PlaceChests(int numChestsToPlace, int maxAttempts, bool moveChest)
	{		
		// Holds the random index.
		int index = Random.Range(0, map.tilePositions.Count);
		
		// Holds whether the tile is already occupied.
		bool isOccupied = false;
		
		// Loops until all chests are placed.
		while (numChestsToPlace > 0 && maxAttempts > 0) {
			isOccupied = false;
			
			// Makes sure no other chests are on the tile.
			for (int i = 0; i < chests.Count; ++i) {
				// Gets the chest's position.
				Vector2 pos = chests[i].GetGridPosition();
				
				// Checks if the current indexed position is not too close to the current chest.
				if (map.tilePositions[index].x > pos.x - (3 * map.TILEGAP) && map.tilePositions[index].x < pos.x + (3 * map.TILEGAP)) {
					if (map.tilePositions[index].y > pos.y - (3 * map.TILEGAP) && map.tilePositions[index].y < pos.y + (3 * map.TILEGAP)) {
						isOccupied = true;
					}
				}	
			}
			
			// Places a chest if the tile is unoccupied.
			if (!isOccupied) {
				// Places a chest at the indexed tile's position and sets its parent.
				GameObject c = GameObject.Instantiate<GameObject>(chestPrefab);
				c.transform.parent = chestParent.transform;
				c.transform.localPosition = new Vector3(map.tilePositions[index].y * map.TILEGAP, 0.062f, -map.tilePositions[index].x * map.TILEGAP);
				c.GetComponent<Chest>().SetGridPosition(map.tilePositions[index]);
				SetChestReward(c.GetComponent<Chest>());
				
				// Adds the chest to the list and decrements number to place.
				chests.Add(c.GetComponent<Chest>());
				--numChestsToPlace;
			}
			
			// Gets a new random index.
			index = Random.Range(0, map.tilePositions.Count);
			
			// Decrements max attempts.
			--maxAttempts;
		}		
	}
	
	// Checks if a player has landed on a chest and if so, opens the chest and gives the player the chest's item.
	public Chest CheckChest(Vector2 playerPos)
	{
		// Checks if the given tile position has a chest and then returns it.
		return CheckTileForChest(playerPos);
	}
	
	// Checks if a chest is on the given tile and returns it.
	public Chest CheckTileForChest(Vector2 gridPos)
	{
		// Loops through all chests.
		foreach (Chest c in chests) {
			// Checks if the chest is closed.
			if (!c.GetIsOpen()) {
				// Checks if the chest's tile is the same as the one given.
				if (c.GetGridPosition() == new Vector2(gridPos.y, gridPos.x)) {
					// Returns the chest as it is on the tile and is closed.
					return c;
				}
			}
		}
		
		// Returns that no chest is on the tile.
		return null;
	}
	
	// Decreases the time before each open chest can close again.
	void AdvanceOpenChests()
	{
		// Loops through all chests.
		foreach(Chest c in chests) {
			// Checks if the chest is open.
			if (c.GetIsOpen()) {
				// Checks if the chest can be closed.
				if (c.GetOpenTurns() <= 0) {
					// Resets the chest to a new position.
					ResetChest(c);
				}
				else {
					// Decrease the number of turns left before the chest can close.
					c.SetOpenTurns(c.GetOpenTurns() - 1);
				}
			}
		}
	}
	
	int FindNewChestPosition(int maxAttempts = 25)
	{
		// Holds the random index.
		int index = Random.Range(0, map.tilePositions.Count);
		
		// Holds whether the tile is already occupied.
		bool isOccupied = false;
		
		// Holds the backup index if no actual position can be found.
		int backup = -1;
		
		// Loops until all chests are placed.
		while (maxAttempts > 0) {
			// Resets whether the tile is occupied by another chest.
			isOccupied = false;
			
			// Makes sure no other chests are on the tile.
			for (int i = 0; i < chests.Count; ++i) {
				// Gets the chest's position.
				Vector2 pos = chests[i].GetGridPosition();
				
				// Checks if the current indexed position is not too close to the current chest.
				if (map.tilePositions[index].x > pos.x - (4 * map.TILEGAP) && map.tilePositions[index].x < pos.x + (4 * map.TILEGAP)) {
					if (map.tilePositions[index].y > pos.y - (4 * map.TILEGAP) && map.tilePositions[index].y < pos.y + (4 * map.TILEGAP)) {
						isOccupied = true;
					}
				}
				
				// Checks if a backup has not yet been made.
				if (backup == -1) {
					// Creates a backup position despite near distance from other chests as long as not on a chest.
					if (map.tilePositions[index] != pos) {
						backup = index;
					}
				}
			}
			
			// Places a chest if the tile is unoccupied.
			if (!isOccupied) {
				// Returns the index of the tile to place the chest on.
				return index;
			}
			
			// Gets a new random index.
			index = Random.Range(0, map.tilePositions.Count);
			
			// Decrements max attempts.
			--maxAttempts;
		}
		
		// Returns the backup index since a suitable distance was not found.
		return backup;
	}
	
	// Moves a chest, closes it and gives it a new item.
	void ResetChest(Chest c)
	{
		// Finds a new tile to place the chest on.
		int index = FindNewChestPosition();
		
		// Moves the chest to its new position.
		c.transform.localPosition = new Vector3(map.tilePositions[index].y * map.TILEGAP, 0.062f, -map.tilePositions[index].x * map.TILEGAP);
		c.GetComponent<Chest>().SetGridPosition(map.tilePositions[index]);
		
		// Closes the chest.
		c.OpenCloseChest(false);
		
		// Gives the chest a new reward.
		SetChestReward(c);
	}
	
	// Gives the player the contents of the chest.
	public void GivePlayerChestContents(Player player, Chest chest, int index)
	{
		chest.OpenCloseChest(true);
		
		// Gets what the chest contains.
		int cType = chest.GetChestType();
		
		// Checks if the chest has gold.
		if (cType == 0) {
			// Adds the chest gold to the player.
			playerGUI.CreateValueChange(index, false, false, false, true, player.GetGold(), chest.getGold());
			player.ChangeGold(chest.getGold());
			return;
		}
		
		// Checks if the chest has an item.
		if (cType == 1) {
			// Adds the item card to the player's inventory.
			player.ChangeItems((ItemCard)chest.GetItem(), true);
			
			return;
		}
		
		// Checks if the chest has a weapon.
		if (cType == 2) {
			// Adds the item card to the player's inventory.
			//player.ChangeItems((WeaponCard)chest.GetItem(), true);
			
			return;
		}
		
		// Chest must be a Mimic.
		spawnMimic = true;
	}
	
	// Gives a given chest a random reward.
	void SetChestReward(Chest c)
	{
		// Gets a random number.
		int rand = Random.Range(0, 101);
		
		// 30% chance the chest will have gold.
		if (rand < 100) {
			c.SetGold(Random.Range(20, 50));
			return;
		}
		// 30% chance the chest will have an item.
		if (rand < 60) {
			
			return;
		}
		// 25% chance the chest will have a weapon.
		if (rand < 85) {
		
			return;
		}
		// 15% chance the chest will be a mimic.
		c.SetMimic();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Advances any open chests towards being moved.
		
	}
}