using UnityEngine;
using System.Collections;

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
	
	int[,] map = new int[7,7] {
		{1, 2, 3, 0, 1, 2, 3},
		{2, 0, 4, 5, 6, 0, 4},
		{1, 6, 0, 0, 0, 6, 5},
		{0, 5, 0, 0, 0, 1, 0},
		{3, 4, 0, 0, 0, 2, 3},
		{2, 0, 4, 3, 2, 0, 4},
		{1, 6, 5, 0, 1, 6, 5}
	};
	
	// This... (I can't wait until we put the actual map sizes in and this array is filled
	// with over 400 useless null pointers all taking up space.
	MonsterCard[,] tiledMonsterCards = new MonsterCard[7,7] {
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null}
	};

	/*bool[,] monsteredTiles = new bool[7,7] {
		{false, false, false, false, false, false, false},
		{false, false, false, false, false, false, false},
		{false, false, false, false, false, false, false},
		{false, false, false, false, false, false, false},
		{false, false, false, false, false, false, false},
		{false, false, false, false, false, false, false},
		{false, false, false, false, false, false, false}
	};*/
	
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
				int value = GetTile(i, j);
				
				// Checks if the space is a tile.
				if (value != 0) {
					// Creates the tile.
					CreateTile(i, j, value);
				}
			}
		}
	}
	
	// Places a tile at the specified position adn changes its type.
	void CreateTile(int i, int j, int value)
	{
		// Places a tile, changes its name and assigns its parent.
		GameObject g = Instantiate<GameObject>(tilePrefab);
		g.name = "Tile : (" + i + ", " + j + ") : " + value;
		g.transform.parent = mapParent.transform;
		
		// Moves the tile locally to the correct position.
		g.transform.localPosition = new Vector3(i * 2, 0, j * 2);
		
		// Changes the colour of the tile to change type.
		g.GetComponent<Renderer>().material = mats[value-1];
	}

	public void AddMonsterToTile(int i, int j, MonsterCard mon)
	{
		tiledMonsterCards[i, j] = mon;
	}

	/*public void AddMonsteredFlag(int i, int j)
	{
		monsteredTiles[i, j] = true;
	}*/

	public void ClearMonsterTile(int i, int j)
	{
		tiledMonsterCards[i, j] = null;
	}
	
	// Returns teh value of the requested tile.
	public int GetTile(int i, int j)
	{
		// Checks if the tile isn't out of bounds.
		if (i < 0 || j < 0 || i >= map.GetLength(0) || j >= map.GetLength(1)) {
			return 0;
		}
		
		// Return's the tile value.
		return map[i,j];
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

	/*public bool HasTileBeenMonstered(int i, int j)
	{
		// Checks if the tile isn't out of bounds.
		if (i < 0 || j < 0 || i >= map.GetLength(0) || j >= map.GetLength(1)) {
			return false;
		}
		
		// Return's the tile value.
		return monsteredTiles[i, j];
	}*/
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
