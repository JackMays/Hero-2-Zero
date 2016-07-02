using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerSelectGUI : MonoBehaviour
{
	#region Variables
	// Holds the parents for each camera's players.
	public List<GameObject> cameraPlayers = new List<GameObject>();
	
	#region Player Variables
	// Holds the current player index of each camera.
	public int[] cameraPlayerIndex = new int[4];
	
	// Holds the positions for each index. (First and last used for looping)
	Vector3[] indexPositions = new Vector3[6] { new Vector3(3f, 0, 2),
											    new Vector3(0, 0, 2),
											    new Vector3(-3, 0, 2),
											    new Vector3(-6, 0, 2),
											    new Vector3(-9, 0, 2),
											    new Vector3(-12f, 0, 2) };
	
	// Speed that the player's shouold move at.
	float moveSpeed = 0.5f;
	
	// List of times used for lerping player movement.
	float[] lerpTimes = new float[4];
	
	// List of prevous positions for moving player parents.
	int[] moveStarts = new int[4];
	
	// List of targets for when moving player parents.
	int[] moveTargets = new int[4];
	
	// List of which player parents should be moving.
	bool[] isMoving = new bool[4];
	
	// List of which players are playing.
	bool[] isPlaying = new bool[4];
	#endregion
	
	#region Hat Variables
	// The number of hats available.
	int numHats = 2;
	
	// List of the current selected hat for each player section.
	int[] hatIndex = new int[4];
	
	// The parents of each hat object.
	public List<Transform> hatParents = new List<Transform>();
	
	// Player 4's default hat.
	public List<GameObject> player4DefaultHats = new List<GameObject>();
	
	// The text objects that display the current index.
	public Text[] hatTexts = new Text[4];
	#endregion
	
	#region Colour Variables
	// The colour of each player.
	int[] playerColourIndex = new int[4];
	
	// The number of available colours.
	int numColours = 2;
	
	// Holds the colour materials for each player.
	public List<List<Material>> listPlayerMaterials = new List<List<Material>>();
	
	// Holds the colour images for each player. (THIS IS UNTIL PLAYER CHILDREN GET COMBINED).
	public List<List<Texture>> listPlayerTextures = new List<List<Texture>>();
	public List<Material> player1ColourTextures = new List<Material>();
	public List<Material> player2ColourTextures = new List<Material>();
	public List<Material> player3ColourTextures = new List<Material>();
	public List<Material> player4ColourTextures = new List<Material>();
	
	// The text objects that display the current index.
	public Text[] colourTexts = new Text[4];
	#endregion
	
	#region AI Variables
	// Holds which players are going to be AI.
	bool[] isAI = new bool[4];
	
	// Holds the level of each AI player.
	int[] playerAIIndex = new int[4] { 1, 1, 1, 1 };
	
	// Holds the text value for each AI level.
	string[] aiLevels = new string[3] { "Easy", "Medium", "Hard" } ;
	
	// List of the AI difficulty panels.
	public GameObject[] aiPanels = new GameObject[4];
	
	// The text objects that display the current index.
	public Text[] aiDifficultyTexts = new Text[4];
	#endregion
	
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		listPlayerMaterials.Add(player1ColourTextures);
		listPlayerMaterials.Add(player2ColourTextures);
		listPlayerMaterials.Add(player3ColourTextures);
		listPlayerMaterials.Add(player4ColourTextures);
	}
	
	#region Add/Remove Players
	
	// Removes a player from the selection.
	public void AddRemovePlayer(string details)
	{
		// Gets whether the player should be added or removed.
		string boo = details.Substring(details.IndexOf(",") + 1);
		// Removes the bool to leave just the index.
		details = details.Remove(details.IndexOf(","));
		
		// Converts the index to an int and the add/remove to a bool.
		int index = int.Parse(details);
		bool add = bool.Parse(boo);
		
		// Hides the player window and player camera at the given index.
		transform.GetChild(index).GetChild(0).gameObject.SetActive(add);
		transform.GetChild(index).GetChild(1).gameObject.SetActive(!add);
		cameraPlayers[index].SetActive(add);
		
		// Sets that the player at the index is not playing.
		isPlaying[index] = add;		
	}
	
	// Locks a player in and states that they are ready to begin.
	public void ConfirmPlayer(string details)
	{
		// Gets whether the player should be locked or unlocked
		string boo = details.Substring(details.IndexOf(",") + 1);
		// Removes the bool to leave just the index.
		details = details.Remove(details.IndexOf(","));
		
		// Converts the index to an int and the lock/unlock to a bool.
		int index = int.Parse(details);
		bool add = bool.Parse(boo);
		
		// Sets the player to show 
	}
	
	#endregion
	
	#region Change Players
	
	// Sets the player parent at the index given to move left.
	public void MovePlayerLeft(string index)
	{
		// Converts the string to an int.
		int ind = int.Parse(index);
		
		// Checks if the player parent is not already moving.
		if (!isMoving[ind]) {
			// Rotates the parent's children to face right.
			RotatePlayers(ind, 1);
		
			FindPlayerParentMovementTarget(ind, false);
		}
	}
	
	// Sets the player parent at the index given to move right.
	public void MovePlayerRight(string index)
	{
		// Converts the string to an int.
		int ind = int.Parse(index);
		
		// Checks if the player parent is not already moving.
		if (!isMoving[ind]) {
			// Rotates the parent's children to face left.
			RotatePlayers(ind, 0);
		
			FindPlayerParentMovementTarget(ind, true);
		}
	}
	
	// Rotates a player parent's children to face left, right or forward.
	void RotatePlayers(int index, int dir)
	{
		// The direction to face.
		Vector3 direction = Vector3.left;
		
		// Checks if the direction should be right.
		if (dir == 1) {
			direction = Vector3.right;
		}
		// Checks if the direction should be forward.
		else if (dir == 2) {
			direction = Vector3.back;
		}
		
		// Loops through all the parent's children and rotates them.
		for (int i = 0; i < 4; ++i) {
			cameraPlayers[index].transform.GetChild(i).forward = direction;
		}
	}
	
	// Finds the target for the player parent to move from and towards.
	void FindPlayerParentMovementTarget(int index, bool right)
	{		
		// Checks if moving right or left.
		if (right) {
			// Checks if the camera is already on the last player.
			if (cameraPlayerIndex[index] == 7) {
				
			}
			else {
				// Stores the beginning and target values.
				moveStarts[index] = cameraPlayerIndex[index];
				moveTargets[index] = moveStarts[index] + 1;
				
				// Increments position in player list.
				++cameraPlayerIndex[index];
				
				// Sets all players of the parent to start walking.
				ChangeAnimations(index, 1);
				
				// Resets movement time.
				lerpTimes[index] = 0;
				
				// Sets that the player parent should start moving.
				isMoving[index] = true;
			}
		}
		else {
			// Checks if the camera is already on the first player.
			if (cameraPlayerIndex[index] == -1) {
				
			}
			else {
				// Stores the beginning and target values.
				moveStarts[index] = cameraPlayerIndex[index];
				moveTargets[index] = moveStarts[index] - 1;
				
				// Decrement position in player list.
				--cameraPlayerIndex[index];
				
				// Sets all players of the parent to start walking.
				ChangeAnimations(index, 1);
				
				// Resets movement time.
				lerpTimes[index] = 0;
				
				// Sets that the player parent should start moving.
				isMoving[index] = true;
			}
		}
	}
	
	// Moves a player parent from one point to another.
	void MovePlayerParent(int index)
	{
		// Checks if the movement is finished.
		if (lerpTimes[index] * moveSpeed >= 1) {
			// Sets the parent at the target and ends movement.
			cameraPlayers[index].transform.localPosition = indexPositions[moveTargets[index]];
			isMoving[index] = false;
			
			// Sets all player children back to idle and makes them face forward.
			ChangeAnimations(index, 0);
			RotatePlayers(index, 2);
			
			// Keeps player index in bounds.
			if (cameraPlayerIndex[index] > 4) {
				cameraPlayerIndex[index] = 1;
			}
			else if (cameraPlayerIndex[index] < 1) {
				cameraPlayerIndex[index] = 4;
			}
		}
		
		// Checks if the target is the first player. (Loops to beginning.)
		if (moveTargets[index] == indexPositions.Length-1) {
			// Checks if the progression is more than halfway.
			if (lerpTimes[index] * moveSpeed >= 0.5f) {
				// Changes the start and target to left of first player and first player.
				moveStarts[index] = 0;
				moveTargets[index] = 1;
			}
		}
		
		// Checks if the target is the last player. (Loops to ending.)
		if (moveTargets[index] == 0) {
			// Checks if the progression is more than halfway.
			if (lerpTimes[index] * moveSpeed >= 0.5f) {
				// Changes the start and target to right of last player and last player.
				moveStarts[index] = indexPositions.Length-1;
				moveTargets[index] = indexPositions.Length-2;
			}
		}
		
		// Lerps from one point to another based on the time passed.
		cameraPlayers[index].transform.localPosition = 
			Vector3.Lerp(indexPositions[moveStarts[index]], indexPositions[moveTargets[index]], lerpTimes[index] * moveSpeed);
	}
	
	// Changes the animations for each of the given camera's players.
	// Animation :: 0 - Idle, 1 - Walking, 2 - Victory
	void ChangeAnimations(int index, int animation)
	{
		// Loops through all the parent's children.
		foreach(Player play in cameraPlayers[index].GetComponentsInChildren<Player>()) {
			// Checks if the player should be idle.
			if (animation == 0) {
				play.Idle();
			}
			// Checks if the player should be walking.
			else if (animation == 1) {
				play.Walk();
			}
			// Player should show victory.
			else {
				play.Victory();
			}
		}
	}
	
	#endregion
	
	#region Hats
	
	// Changes the current hat equipped to the player's of the parent at the given index.
	public void ChangeHats(string details)
	{
		// Gets the direction string from the given details.
		string dir = details.Substring(details.IndexOf(",") + 1);
		// Removes the direction to leave just the index.
		details = details.Remove(details.IndexOf(","));
		
		// Converts the index to an int and the direction to a bool.
		int index = int.Parse(details);
		bool direction = bool.Parse(dir);
		
		// Hides the current hat if there is one.
		if (hatIndex[index] != 0) {
			HideShowHats(index, hatIndex[index], false);
		}
		
		// Checks if the hat index is being incremented or decremented.
		if (direction) {
			++hatIndex[index];
			
			// If at the end of the list, loop back to beginning.
			if (hatIndex[index] > numHats) {
				hatIndex[index] = 0;
			}
		}
		else {
			--hatIndex[index];
			
			// If at the beginning of the list, loop back to end.
			if (hatIndex[index] < 0) {
				hatIndex[index] = numHats;
			}
		}
		
		// Checks that a hat was selected. (0 means the hat is a lie).
		if (hatIndex[index] != 0) {
			// Loops through all the parent's children and shows the newly selected hat.
			HideShowHats(index, hatIndex[index], true);
			
			// Hides player 4's default hat.
			player4DefaultHats[index].SetActive(false);
		}
		else {
			// Shows player 4's default hat.
			player4DefaultHats[index].SetActive(true);
		}
		
		// Updates the hat index text.
		ChangePlayerDetails(index, 0, hatIndex[index].ToString());
	}
	
	// Loops through a given parent's children and either hides or shows the passed hat index.
	void HideShowHats(int index, int hat, bool show)
	{
		Debug.Log(hat);
		// Loops through the children.
		for (int i = 0; i < 4; ++i) {
			// Hats are stored under a hat parent which is the first child of every player.
			hatParents[(index * 4) + i].GetChild(hat-1).gameObject.SetActive(show);
		}
	}
	#endregion
	
	#region Colour
	// Changes the colour index for the given player index.
	public void ChangeColourIndex(string details)
	{
		// Gets the direction string from the given details.
		string rai = details.Substring(details.IndexOf(",") + 1);
		// Removes the direction to leave just the index.
		details = details.Remove(details.IndexOf(","));
		
		// Converts the index to an int and the direction to a bool.
		int index = int.Parse(details);
		bool raise = bool.Parse(rai);
		
		// Checks if the colour index needs to be raised or lowered.
		if (raise) {
			// If already at the end of the list, loop back to the beginning.
			if (playerColourIndex[index] >= numColours-1) {
				playerColourIndex[index] = 0;
			}
			else {
				// Increments the colour index.
				++playerColourIndex[index];
			}
		}
		else {
			// If already at beginning of list, loop back to the end.
			if (playerColourIndex[index] == 0) {
				playerColourIndex[index] = numColours-1;
			}
			else {
				// Increments the colour index.
				--playerColourIndex[index];
			}
		}
		
		// Updates all the parent's players with the new colour scheme.
		ChangePlayerColours(index);
		
		// Changes the player's colour string to reflect the current colour index.
		ChangePlayerDetails(index, 1, playerColourIndex[index].ToString());
	}
	
	// Loops through the specified child of a player parent and updates all the materials to the new colour pallette.
	void ChangePlayerColours(int index)
	{
		// Gets a list of renderers for all the player's children.
		Renderer[] childRenderers = cameraPlayers[index].transform.GetChild(cameraPlayerIndex[index]-1).GetComponentsInChildren<Renderer>();
		
		Debug.Log(playerColourIndex[index]);
		
		// Loops through the renderers.
		for (int i = 0; i < childRenderers.Length; ++i) {
			if (childRenderers[i].material.name.Contains("Lower Level Clothes")) {
				childRenderers[i].material = listPlayerMaterials[cameraPlayerIndex[index]-1][(playerColourIndex[index] * 3)];
			}
			else if (childRenderers[i].material.name.Contains("Robe")) {
				childRenderers[i].material = listPlayerMaterials[cameraPlayerIndex[index]-1][(playerColourIndex[index] * 3) + 1];
			}
			else if (childRenderers[i].material.name.Contains("Hair")) {
				childRenderers[i].material = listPlayerMaterials[cameraPlayerIndex[index]-1][(playerColourIndex[index] * 3) + 2];
			}
		}
	}
	#endregion
	
	#region AI
	// Changes the AI level for the given player index.
	public void ChangeAILevel(string details)
	{
		// Gets the direction string from the given details.
		string rai = details.Substring(details.IndexOf(",") + 1);
		// Removes the direction to leave just the index.
		details = details.Remove(details.IndexOf(","));
		
		// Converts the index to an int and the direction to a bool.
		int index = int.Parse(details);
		bool raise = bool.Parse(rai);
	
		// Checks if the AI level needs to be raised or lowered.
		if (raise) {
			// If already at top level AI, loop back to lowest level.
			if (playerAIIndex[index] == aiLevels.Length-1) {
				playerAIIndex[index] = 0;
			}
			else {
				// Increments the AI level.
				++playerAIIndex[index];
			}
		}
		else {
			// If already at lowest level AI, loop back to highest level.
			if (playerAIIndex[index] == 0) {
				playerAIIndex[index] = aiLevels.Length-1;
			}
			else {
				// Increments the AI level.
				--playerAIIndex[index];
			}
		}
		
		// Changes the player's AI difficulty string to reflect the current AI level.
		ChangePlayerDetails(index, 2, aiLevels[playerAIIndex[index]]);
	}
	
	// Changes a given player's text value.
	void ChangePlayerDetails(int index, int detail, string value)
	{
		// Checks if the text to change is hat.
		if (detail == 0) {
			hatTexts[index].text = value;
		}
		// Checks if the text to change is colour.
		else if (detail == 1) {
			colourTexts[index].text = value;
		}
		// Checks if the text to change is AI difficulty.
		else if (detail == 2) {
			aiDifficultyTexts[index].text = value;
		}
	}
	#endregion
	
	// Update is called once per frame
	void Update ()
	{
		// Loops through all the player parents and sees if they need to be moved.
		for (int i = 0; i < 4; ++i) {
			if (isMoving[i]) {
				// Progresses the parent's movement.
				lerpTimes[i] += Time.deltaTime;
				
				// Moves the parent.
				MovePlayerParent(i);
			}
		}
	}
}