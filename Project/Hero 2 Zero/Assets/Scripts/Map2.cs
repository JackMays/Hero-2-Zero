﻿using UnityEngine;
using System.Collections;

public class Map2 : MonoBehaviour
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
	MonsterCard[,] tiledMonsterCards = new MonsterCard[7,7] {
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null}
	};
	
	// One 2D array I could let slide but 2 2D arrays which will have around 400 null pointers
	// is something that is gonna drive me insane. Thats 800 4/8 bit pointers for no reason.
	GameObject[,] tiledMonsterPrefabs = new GameObject[7,7] {
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null},
		{null, null, null, null, null, null, null}
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
	
	public void AddPrefabToTiles(int i, int j, GameObject pr, Vector3 pos)
	{
		tiledMonsterPrefabs[i, j] = Instantiate(pr, pos, Quaternion.identity) as GameObject; 
	}
	
	/*public void AddMonsteredFlag(int i, int j)
	{
		monsteredTiles[i, j] = true;
	}*/
	
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
	
	public bool HasBlankModel(int i, int j)
	{
		// Checks if the tile isn't out of bounds.
		if (i < 0 || j < 0 || i >= map.GetLength(0) || j >= map.GetLength(1)) {
			return false;
		}
		
		// return true if model is blank/null and false if not
		return (tiledMonsterPrefabs[i, j] == null);
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