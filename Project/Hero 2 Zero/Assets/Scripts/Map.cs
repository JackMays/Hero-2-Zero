using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
	#region Variables
	// The map.
	/*int[,] map = new int[5,5] {
	{1, 2, 3, 4, 5},
	{1, 0, 0, 0, 1},
	{5, 0, 0, 0, 2},
	{4, 0, 0, 0, 3},
	{3, 2, 1, 5, 4}
	};*/
	
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
	
	List<ChoiceTile> choiceTiles = new List<ChoiceTile>() {
		new ChoiceTile(new Vector2(0, 3), new bool[4] {false, true, true, false}),
		new ChoiceTile(new Vector2(3, 8), new bool[4] {false, false, true, true}),
		new ChoiceTile(new Vector2(8, 5), new bool[4] {true, false, false, true}),
		new ChoiceTile(new Vector2(5, 0), new bool[4] {true, true, false, false})
	};
	
	List<ForceTile> forceTiles = new List<ForceTile>() {
		new ForceTile(new Vector2(0, 5), 1),
		new ForceTile(new Vector2(5, 8), 2),
		new ForceTile(new Vector2(8, 3), 3),
		new ForceTile(new Vector2(3, 0), 0)
	};
	
	/*
	int[,] map = new int[7,7] {
		{6, 6, 6, 0, 6, 6, 6},
		{6, 0, 6, 6, 6, 0, 6},
		{6, 6, 0, 0, 0, 6, 6},
		{0, 6, 0, 0, 0, 6, 0},
		{6, 6, 0, 0, 0, 6, 6},
		{6, 0, 6, 6, 6, 0, 6},
		{6, 6, 6, 0, 6, 6, 6}
	};*/
	
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
	
	// Camera overseeing map.
	public Camera cam;
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		CreateMap();
		
		Vector3 moveCam = cam.transform.localPosition;
		moveCam.x = map.GetLength(0) - 1;
		cam.transform.localPosition = moveCam;
		
	}
	
	// Places the tiles around the map according to the array.
	void CreateMap()
	{
		// Loops through the array.
		for (int i = 0; i < map.GetLength(0); ++i) {
			for (int j = 0; j < map.GetLength(1); ++j) {
				// Gets the tile value.
				int value = GetTile(j, i);
				
				// Checks if the space is a tile.
				if (value != 0) {
					// Creates the tile.
					CreateTile(i, j, value);
				}
			}
		}
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
	
	public void AddMonsterToTile(int i, int j, MonsterCard mon)
	{
		tiledMonsterCards[i, j] = mon;
	}
	
	public void AddPrefabToTiles(int i, int j, GameObject pr, Vector3 pos)
	{
		tiledMonsterPrefabs[i, j] = Instantiate(pr, pos, Quaternion.identity) as GameObject; 
	}
	
	public void ClearMonsterTile(int i, int j)
	{
		tiledMonsterCards[i, j] = null;
	}
	
	public void ClearPrefabTile(int i, int j)
	{
		Destroy(tiledMonsterPrefabs[i, j]);
		
		tiledMonsterPrefabs[i, j] = null;
	}
	
	// Returns teh value of the requested tile.
	public int GetTile(int i, int j)
	{
		// Checks if the tile isn't out of bounds.
		if (i < 0 || j < 0 || i >= map.GetLength(1) || j >= map.GetLength(0)) {
			return 0;
		}
		
		// Return's the tile value.
		return map[j,i];
	}
	
	public MonsterCard GetMonsterOnTile(int i, int j)
	{
		// Checks if the tile isn't out of bounds.
		if (i < 0 || j < 0 || i >= map.GetLength(0) || j >= map.GetLength(1)) {
			return null;
		}
		
		// Return's the tile value.
		return tiledMonsterCards[i, j];
	}
	
	public bool HasBlankModel(int i, int j)
	{
		// Checks if the tile isn't out of bounds.
		if (i < 0 || j < 0 || i >= map.GetLength(0) || j >= map.GetLength(1)) {
			return false;
		}
		
		// return true if model is blank/null and false if not
		return (tiledMonsterPrefabs[i, j] == null);
	}
}