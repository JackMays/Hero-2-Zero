using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
	#region Structs
	// Makes it a lot easier to store the tile position and the directions the player can choose.
	struct ChoiceTile {
		// The grid position of the tile.
		public Vector2 gridPos;
		
		// The directions the player ca move to.
		public bool[] directions;
		
		// Constructor.
		public ChoiceTile(Vector2 pos, bool[] dirs)
		{	
			gridPos = pos;
			directions = dirs;
		}
	}
	
	// Makes it easier to force the player's direction. (Stops players backtracking.)
	struct ForceTile {
		// The grid position of the tile.
		public Vector2 gridPos;
		
		// The direction the player has to move in.
		public int direction;
		
		// Constructor.
		public ForceTile(Vector2 pos, int dir)
		{	
			gridPos = pos;
			direction = dir;
		}
	}	
	#endregion
	
	#region Variables
	// The map.
	int[,] map = new int[9,9] {
		{1, 2, 3, 4, 1, 4, 5, 6, 1},
		{6, 0, 0, 5, 0, 3, 0, 0, 2},
		{5, 0, 0, 6, 1, 2, 0, 0, 3},
		{4, 3, 2, 0, 0, 0, 6, 5, 4},
		{4, 0, 1, 0, 0, 0, 1, 0, 2},
		{4, 5, 6, 0, 0, 0, 2, 3, 4},
		{3, 0, 0, 2, 1, 6, 0, 0, 5},
		{2, 0, 0, 3, 0, 5, 0, 0, 6},
		{1, 6, 5, 4, 3, 4, 3, 2, 1}		
	};
	
	
	// Holds the position of where the player needs to choose a path
	// and the directions the player can choose.
	List<ChoiceTile> choiceTiles = new List<ChoiceTile>() {
		new ChoiceTile(new Vector2(0, 3), new bool[4] {false, true, true, false}),
		new ChoiceTile(new Vector2(3, 8), new bool[4] {false, false, true, true}),
		new ChoiceTile(new Vector2(8, 5), new bool[4] {true, false, false, true}),
		new ChoiceTile(new Vector2(5, 0), new bool[4] {true, true, false, false})
	};
	 
	// Holds the position of where the player is forced to go in a
	// particular direction.
	List<ForceTile> forceTiles = new List<ForceTile>() {
		new ForceTile(new Vector2(0, 5), 1),
		new ForceTile(new Vector2(5, 8), 2),
		new ForceTile(new Vector2(8, 3), 3),
		new ForceTile(new Vector2(3, 0), 0)
	};
	
	// Monster Instances by Tile
	MonsterCard[,] tiledMonsterCards = new MonsterCard[9,9] {
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null}
	};
	
	// One 2D array I could let slide but 2 2D arrays which will have around 400 null pointers
	// is something that is gonna drive me insane. Thats 800 4/8 bit pointers for no reason.
	GameObject[,] tiledMonsterPrefabs = new GameObject[9,9] {
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null, null, null}
	};
	
	// Parent of all map tiles.
	public GameObject mapParent;
	
	// The tile prefab.
	public GameObject tilePrefab;
	
	// List of materials to change tile type.
	public Material[] mats;
	public GameObject[] tilePrefabs;
	
	// Camera overseeing map.
	public Camera cam;
	
	// Chest prefab.
	public GameObject chestPrefab;
	
	// List of chests.
	List<Chest> listChests = new List<Chest>();
	
	// Change to my map.
	public bool changeMap = false;
	#endregion
	
	#region Test
	public bool testSprites = false;
	
	public List<GameObject> sprites;
	#endregion
	
	// Use this for initialization
	void Awake ()
	{
		if (changeMap) {
			ChangeMap();
		}
		
		CreateMap();
		
		Vector3 moveCam = cam.transform.localPosition;
		moveCam.x = (map.GetLength(0) - 1) * 1.55f;
		moveCam.z = -map.GetLength(1) * 2f;
		cam.transform.localPosition = moveCam;
		
	}
	
	void ChangeMap()
	{
		map = new int[21, 30] {
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 1, 0, 4, 4, 2, 0, 0, 0, 0},
			{2, 2, 7, 6, 7, 2, 0, 0, 0, 0, 0, 0, 0, 0, 4, 7, 3, 6, 4, 4, 0, 7, 6, 1, 0, 7, 2, 0, 0, 0},
			{2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 4, 6, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0},
			{2, 0, 0, 0, 0, 2, 2, 1, 1, 6, 3, 7, 1, 4, 0, 0, 0, 0, 1, 4, 4, 7, 0, 0, 0, 4, 1, 0, 0, 0},
			{7, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 4, 0, 0, 0, 0},
			{7, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 1, 6, 3, 0, 0, 0, 0, 0, 4, 0, 0, 0, 7, 4, 4, 0, 0},
			{2, 2, 2, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 3, 1, 4, 7, 2, 2, 4, 6, 4, 4, 0, 0, 6, 0, 0},
			{0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 1, 1, 3, 4, 0, 0},
			{0, 0, 1, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 1, 3, 7, 0, 0, 0, 0, 0, 0, 0, 0},
			{0, 0, 1, 0, 0, 0, 0, 0, 6, 1, 7, 1, 4, 3, 0, 0, 0, 2, 1, 0, 0, 7, 6, 4, 3, 1, 0, 0, 0, 0},
			{7, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 6, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
			{7, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 0, 0, 0},
			{3, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0},
			{3, 0, 0, 6, 3, 3, 1, 3, 1, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0},
			{1, 3, 0, 0, 0, 0, 0, 0, 7, 6, 3, 1, 1, 7, 1, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 5, 5, 0},
			{0, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 1, 7, 0, 0, 0, 0, 0, 0, 5, 7, 0, 7, 0},
			{0, 0, 4, 6, 0, 0, 0, 0, 0, 0, 1, 1, 1, 6, 0, 0, 0, 0, 1, 1, 1, 7, 6, 1, 1, 5, 0, 0, 5, 5},
			{0, 0, 0, 6, 4, 1, 0, 0, 0, 0, 3, 0, 0, 3, 3, 1, 7, 3, 3, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 6},
			{0, 0, 0, 0, 0, 3, 2, 2, 2, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 7, 0, 0, 0, 0, 5, 6},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 2, 2, 2, 1, 0},
			{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 6, 5, 5, 1, 5, 0, 0, 0, 0}	
		};
		
		choiceTiles = new List<ChoiceTile>() {
			new ChoiceTile(new Vector2(3, 8), new bool[4] {false, true, true, false}),
			new ChoiceTile(new Vector2(3, 13), new bool[4] {true, false, true, false}),
			new ChoiceTile(new Vector2(1, 18), new bool[4] {false, true, true, false}),
			new ChoiceTile(new Vector2(14, 26), new bool[4] {false, true, true, false}),
			new ChoiceTile(new Vector2(16, 18), new bool[4] {true, false, true, false}),
			new ChoiceTile(new Vector2(10, 13), new bool[4] {false, true, true, false})
		};
		
		// Holds the position of where the player is forced to go in a
		// particular direction.
		forceTiles = new List<ForceTile>() {
			new ForceTile(new Vector2(6, 21), 2),
			new ForceTile(new Vector2(8, 21), 2),
			new ForceTile(new Vector2(16, 23), 3),
			new ForceTile(new Vector2(14, 13), 3),
			new ForceTile(new Vector2(10, 2), 0)
		};
		
		tiledMonsterCards = new MonsterCard[21,30];
		tiledMonsterPrefabs = new GameObject[21,30];
	}
	
	#region Create Map
	
	// Places the tiles around the map according to the array.
	void CreateMap()
	{
		// Holds available tile positions for chest placement.
		List<Vector2> viableTiles = new List<Vector2>();
	
		// Loops through the array.
		for (int i = 0; i < map.GetLength(0); ++i) {
			for (int j = 0; j < map.GetLength(1); ++j) {
				// Gets the tile value.
				int value = GetTile(j, i);
				
				// Checks if the space is a tile.
				if (value != 0) {
					// Creates the tile.
					if (!testSprites) {
						CreateTile2(i, j, value);
					}
					else {
						CreateSpriteTile(i,j,value);
					}
					
					// Adds the position to the list.
					viableTiles.Add(new Vector2(i, j));
				}
			}
		}
		
		// Places the chests using the tile list.
		PlaceChests(viableTiles);
	}
	
	// Places a tile at the specified position and changes its type.
	void CreateTile(int i, int j, int value)
	{
		// Places a tile, changes its name and assigns its parent.
		GameObject g = Instantiate<GameObject>(tilePrefab);
		g.name = "Tile : (" + i + ", " + j + ") : " + value;
		g.transform.parent = mapParent.transform;
		
		// Moves the tile locally to the correct position.
		g.transform.localPosition = new Vector3(j * 2, 0, (0 - i) * 2);
		
		// Changes the colour of the tile to change type.
		g.GetComponent<Renderer>().material = mats[value-1];
	}
	
	// Places a tile at the specified position and changes its type.
	void CreateTile2(int i, int j, int value)
	{	
		// Places a tile, changes its name and assigns its parent.
		GameObject g = Instantiate<GameObject>(tilePrefabs[value-1]);
		g.name = "Tile : (" + j + ", " + i + ") : " + value;
		g.transform.parent = mapParent.transform;
		
		// Moves the tile locally to the correct position.
		g.transform.localPosition = new Vector3(j * 2, 0, (0 - i) * 2);
	}
	
	// Places a tile at the specified position and changes its type.
	void CreateSpriteTile(int i, int j, int value)
	{	
		// Places a tile, changes its name and assigns its parent.
		int v = 0;
		
		if (value != 6 && value != 7) {
			v = 1;
		}
		else if (value == 6) {
			v = 2;
		}
		else if (value == 7) {
			v = 0;
		}
		
		GameObject g = Instantiate<GameObject>(sprites[value-1]);
		
		
		g.name = "Tile : (" + j + ", " + i + ") : " + value;
		g.transform.parent = mapParent.transform;
		
		// Moves the tile locally to the correct position.
		g.transform.localPosition = new Vector3(j * 2, 0.1f, (0 - i) * 2);
	}
	
	// Places chests on random tiles.
	void PlaceChests(List<Vector2> tiles)
	{		
		// Holds the random index.
		int index = Random.Range(0, tiles.Count);
		
		// Number of chests to place.
		int numChests = 10;
		
		// Holds whether the tile is already occupied.
		bool isOccupied = false;
		
		// Holds the maximum number of placement attempts.
		int max = 50;
		
		// Loops until all chests are placed.
		while (numChests > 0 && max > 0) {
			isOccupied = false;
		
			// Makes sure no other chests are on the tile.
			for (int i = 0; i < listChests.Count; ++i) {
			// Gets the chest's position.
				Vector2 pos = listChests[i].GetGridPosition();
				
				// Checks if the current indexed position is not too close to the current chest.
				//if (tiles[index] == pos) {
				if (tiles[index].x > pos.x - 4 && tiles[index].x < pos.x + 4) {
					if (tiles[index].y > pos.y - 4 && tiles[index].y < pos.y + 4) {
						Debug.Log("Occupied");
						isOccupied = true;
					}
				}	
			}
			
			// Places a chest if the tile is unoccupied.
			if (!isOccupied) {
				// Places a chest at the indexed tile's position and sets its parent.
				GameObject c = Instantiate<GameObject>(chestPrefab);
				c.transform.parent = mapParent.transform;
				c.transform.localPosition = new Vector3(tiles[index].y * 2, 0.062f, -tiles[index].x * 2);
				c.GetComponent<Chest>().SetGridPosition(tiles[index]);
			
				// Adds the chest to the list and decrements number to place.
				listChests.Add(c.GetComponent<Chest>());
				--numChests;
			}
			
			// Gets a new random index.
			index = Random.Range(0, tiles.Count);
			
			// Decrements max attempts.
			--max;
		}		
	}
	
	#endregion
	
	#region Getters
	
	// Returns the value of the requested tile.
	public int GetTile(int i, int j)
	{
		// Checks if the tile isn't out of bounds.
		if (i < 0 || j < 0 || i >= map.GetLength(1) || j >= map.GetLength(0)) {
			return 0;
		}
		
		// Return's the tile value.
		return map[j, i];
	}
	
	// Returns the directions the player can travel if a choice tile.
	public bool[] IsChoiceTile(int x, int y)
	{
		// Creates the vector to prevent multiple declarations in the loop.
		Vector2 pos = new Vector2(y, x);
		
		// Loops through the list.
		foreach(ChoiceTile p in choiceTiles) {
			// Checks if the given pos is the same as the tile's pos.
			if (pos == p.gridPos) {
				// Returns the directions the player can move.
				return p.directions;
			}
		}	
		
		// Returns the position is not a choice tile.
		return null;
	}
	
	// Returns the direction the player must travel if a force tile.
	public int IsForceTile(int x, int y)
	{
		// Creates the vector to prevent multiple declarations in the loop.
		Vector2 pos = new Vector2(y, x);
		
		// Loops through the list.
		foreach(ForceTile p in forceTiles) {
			// Checks if the given pos is the same as the tile's pos.
			if (pos == p.gridPos) {
				// Returns the directions the player must move.
				return p.direction;
			}
		}	
		
		// Returns the position is not a choice tile.
		return -1;
	}
	
	#endregion
	
	#region Monster Functions
	
	public void AddMonsterToTile(int i, int j, MonsterCard mon)
	{
		tiledMonsterCards[j, i] = mon;
	}
	
	public void AddPrefabToTiles(int i, int j, GameObject pr, Vector3 pos)
	{
		tiledMonsterPrefabs[j, i] = Instantiate(pr, pos, Quaternion.identity) as GameObject;
	}
	
	public void ClearMonsterTile(int i, int j)
	{
		tiledMonsterCards[j, i] = null;
	}
	
	public void ClearPrefabTile(int i, int j)
	{
		Destroy(tiledMonsterPrefabs[j, i]);
		
		tiledMonsterPrefabs[j, i] = null;
	}
	
	public MonsterCard GetMonsterOnTile(int i, int j)
	{
		// Checks if the tile isn't out of bounds.
		if (i < 0 || j < 0 || j >= map.GetLength(0) || i >= map.GetLength(1)) {
			return null;
		}
		
		// Return's the tile value.
		return tiledMonsterCards[j, i];
	}
	
	public bool HasBlankModel(int i, int j)
	{
		// Checks if the tile isn't out of bounds.
		if (i < 0 || j < 0 || j >= map.GetLength(0) || i >= map.GetLength(1)) {
			return false;
		}
		
		// return true if model is blank/null and false if not
		return (tiledMonsterPrefabs[j, i] == null);
	}
	
	#endregion
}