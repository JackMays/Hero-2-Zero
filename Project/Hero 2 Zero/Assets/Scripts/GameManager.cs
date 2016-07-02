using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	#region Variables	
	#region Managers
	
	// Reference to the map.
	public Map map;
	
	// Reference to the card manager.
	public CardManager cardManager;
	
	// Reference to dice manager.
	public DiceManager diceManager;

	public CombatManager combatManager;

	public CameraManager cameraManager;
	
	// Reference to item manager.
	ItemManager itemManager;
	
	// The chest manager.
	ChestManager chestManager;
	
	public PlayerInfoGUI playerInfoGUI;
	
	public TurnGUI turnGUI;
	
	#endregion

	ItemCard selectedHandCard;
	
	// List of the players.
	public List<Player> listPlayers;
	public List<GameObject> listTargetButtons;

	// Number of players.
	int numPlayers = 4;
	
	// The player whose turn it is.
	int currentPlayer = 0;
	
	// Current index in turn order.
	int turnIndex = 0;

	int handIndex = 0;
	
	// The order in which player's take their turns.
	int[] turnOrder = new int[4] {0, 1, 2, 3};
	
	// State of player's turn. 0: Roll Dice | 1: Move Player | 2: Show Card | 3: Change player | 4: Combat | 5: Card targetting
	int turnState = 0;
	int prevTurnState = 0;
	
	// Button to roll the dice.
	string rollButton = "Fire2";
	
	// Holds whether the dice have been rolled.
	bool diceRolled = false;
	bool cmbPlayerRolled = false;
	bool cmbBothRolled = false;
	
	// Canvas for choosing a direction.
	public GameObject canvasDirection;

	public Camera playerCam;
	public Camera rayCam;
	public GameObject selectSprite;
	
	public int startPlayer = 0;
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		itemManager = new ItemManager(listPlayers, cardManager, map);
		itemManager.SetCameras(playerCam, rayCam, selectSprite);

		currentPlayer = startPlayer;

		cameraManager.SetActivePlayer(listPlayers[currentPlayer].gameObject);
		
		// Creates the chest manager.
		chestManager = new ChestManager(map, itemManager, playerInfoGUI);
		
		if (listPlayers[currentPlayer].GetItemHandLimit() != 0)
		{
			cardManager.PopulateHand(listPlayers[currentPlayer].GetCurrentItem(handIndex));
		}
		
		// Sets each player's index.
		for (int i = 0; i < listPlayers.Count; ++i) {
			listPlayers[i].SetIndex(i);
		}
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

		// update cam manager
		cameraManager.SetActivePlayer(listPlayers[currentPlayer].gameObject);
		
		// Sets the number of dice to roll. Putting here until setup state added later.
		diceManager.ShowDice(listPlayers[currentPlayer].GetDice());

		// Check new Players hand
		if (listPlayers[currentPlayer].GetItemHandLimit() != 0)
		{
			cardManager.PopulateHand(listPlayers[currentPlayer].GetCurrentItem(handIndex));
		}
	}

	// Checks if there are any players on the current tile.
	int CheckPlayerPositions(Vector2 mapPos, int skipIndex = -1)
	{
		// Loops through all the players.
		for (int i = 0; i < listPlayers.Count; ++i) {
			// Checks all but the skip index.
			if (i != skipIndex) {
				// Checks if the player's map position matches the search position.
				if (listPlayers[i].GetMapPosition() == mapPos) {
					// Returns the player index.
					return i;
				}
			}
		}
		
		// Returns that no player is on the tile.
		return -1;
	}
	
	// Does the work needed to show the turn GUI at the beginning or end of a turn.
	void SetupTurnGUI(bool beginning)
	{
		// Gets the player's grid position.
		Vector2 playGrid = listPlayers[currentPlayer].GetMapPosition();
		
		// Gets the area type from the map at the player's position.
		int tileType = map.GetTile((int)playGrid.x, (int)playGrid.y)-1;
		
		// Holds whether the player is on a village tile or not.
		bool inVillage = false;
		
		// Checks if the player is on a village tile.
		if (tileType == 1) {
			inVillage = true;
		}
	
		// Checks if beginning of turn.
		if (beginning) {
			turnGUI.StartNewPlayerTurn(listPlayers[currentPlayer], inVillage);
		}
		// Must be end of turn.
		else {
			turnGUI.EndPlayerTurn(inVillage);
		}
	}

	#region Hand
	public void IncrementHand()
	{
		if (listPlayers[currentPlayer].GetItemHandLimit() != 0)
		{
			if (handIndex != listPlayers[currentPlayer].GetItemHandLimit() - 1)
			{
				++handIndex;
			}
			else
			{
				handIndex = 0;
			}
			// test: will be a call to CardMaster passing in the handindex and currentplayer to draw the right card
			//GameObject.Find("HandIndexTest").GetComponent<Text>().text = handIndex.ToString();
			// Set active hand card with the current item
			cardManager.PopulateHand(listPlayers[currentPlayer].GetCurrentItem(handIndex));
		}


	}

	public void DecrementHand()
	{
		if (listPlayers[currentPlayer].GetItemHandLimit() != 0)
		{
			if (handIndex != 0)
			{
				--handIndex;
			}
			else
			{
				handIndex = listPlayers[currentPlayer].GetItemHandLimit() - 1;
			}
			// test: will be a call to CardMaster passing in the handindex and currentplayer to draw the right card
			//GameObject.Find("HandIndexTest").GetComponent<Text>().text = handIndex.ToString();
			// Set active hand card with the current item
			cardManager.PopulateHand(listPlayers[currentPlayer].GetCurrentItem(handIndex));
		}
	}

	public void ActivateHand()
	{
		selectedHandCard = listPlayers[currentPlayer].GetCurrentItem(handIndex);

		// check if selected card is in it area
		if (selectedHandCard.GetUsableArea() == 0)
		{
			if (turnState != 3)
			{
				Debug.Log ("Cant use Cards during Player Switch");
			}
			else
			{
				Debug.Log ("Activated Anywhere Card");
				prevTurnState = turnState;
				turnState = 5;
			}
		}
		else if (selectedHandCard.GetUsableArea() == 1)
		{
			if (turnState == 4 && !cmbPlayerRolled)
			{
				Debug.Log ("Activated In Combat Card");
				prevTurnState = turnState;
				turnState = 5;
			}
			else
			{
				Debug.Log ("Unavailable as not in Combat Start");
			}
		}
		else if (selectedHandCard.GetUsableArea() == 2)
		{
			if (turnState == 0)
			{
				Debug.Log ("Activated Board Card");
				prevTurnState = turnState;
				turnState = 5;
			}
			else
			{
				Debug.Log ("Unavailable when Dice has been rolled");
			}
		}
		// set the target buttons active if we are in the targetting phase
		if (turnState == 5 && selectedHandCard.GetTargetType() == 4)
		{
			for (int i = 0; i < listPlayers.Count; ++i)
			{
				listTargetButtons[i].SetActive(true);
			}
		}
		// if user target is noty needed skip to apply effect where a target is assigned based on targetType
		else if (turnState == 5 && selectedHandCard.GetTargetType() != 4)
		{
			itemManager.ApplyItemEffect(selectedHandCard, listPlayers, currentPlayer);
			turnState = prevTurnState;
		}

	}

	public void ExecuteTargetedHandEffect(int target)
	{
		// execurte effect from item manager then set turn state back to previous state
		itemManager.ApplyItemEffect(selectedHandCard, target);
		turnState = prevTurnState;
		for (int i = 0; i < listTargetButtons.Count; ++i)
		{
			listTargetButtons[i].SetActive(false);
		}
	}
	
	#endregion 
	
	#region Directions
	// Sets the direction that the player chose.
	public void SetDirection(int d)
	{
		// Hides the direction buttons.
		canvasDirection.SetActive(false);
	
		// Sets the player's direction.
		listPlayers[currentPlayer].SetDirection(d);
		
		// Sets teh player to move.
		listPlayers[currentPlayer].MoveTile();
		
		listPlayers[currentPlayer].Walk();
	}
	
	// Shows the direction canvas and activates the choosale buttons.
	void ShowDirectionCanvas(bool[] dirs)
	{
		// Loops through the buttons and ativates the possible choices.
		for (int i = 0; i < 4; ++i) {
			canvasDirection.transform.GetChild(i).GetComponent<Button>().interactable = dirs[i];
		}
	
		// Shows the canvas.
		canvasDirection.SetActive(true);
	}
	#endregion
	
	// Update is called once per frame
	void Update ()
	{		
		// Debugging Equipment.
		// Level up.
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			listPlayers[currentPlayer].ChangeLevel(1);
		}
		// Level down.
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			listPlayers[currentPlayer].ChangeLevel(-1);
		}
		
		// Raycast testing.
		if (Input.GetKeyDown(KeyCode.O)) {
			Debug.Log("Changing");
			if (!itemManager.IsInRaycastMode()) {
				itemManager.StartRaycasting();
			}
			else {
				itemManager.EndRaycasting();
			}
		}
		
		// Checks if a raycast is needed for item manager.
		if (itemManager.IsInRaycastMode()) {
			// Checks if the button has been pressed.
			if (Input.GetMouseButtonDown(0)) {
				itemManager.RayCast();
			}
		}
		// Usual shit.
		else {
			// Rolling the dice stage.
			if (turnState == 0) {

				if (listPlayers[currentPlayer].HasSkippedTurns())
				{					
					if ((listPlayers[currentPlayer].GetComponent<Animator>() && listPlayers[currentPlayer].HasIdleState()) ||
					    !listPlayers[currentPlayer].GetComponent<Animator>())
					{						
						StateRollDice();
					}
				}
				else
				{
					Debug.Log ("Skipped Player " + (currentPlayer + 1));
					turnState = 3;
				}
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

				if (cameraManager.HasCombatViewOn())
				{
					cameraManager.ToggleCombatView(false);
				}
				
				// Checks if any changes are being shown.
				if (!playerInfoGUI.GetShowingChanges()) {
					// Checks if the turn GUI is hidden.
					if (!turnGUI.GetShowing()) {
						SetupTurnGUI(false);
					}
				
					// Waits for the end turn button to be pressed or if the player skips.
					if (Input.GetButtonDown(rollButton) || turnGUI.GetPressedButton() == 0) {// || listPlayers[currentPlayer].GetSkippedTurn()) {
						// Resets the player's dice to standard. (In case they have more or less dice.)
						listPlayers[currentPlayer].ResetDiceCount();
						
						// Changes the player's turn.
						ChangeTurns();
						
						SetupTurnGUI(true);
						
						// Resets back to roll dice state.
						turnState = 0;
					}
				}
			}

			else if (turnState == 4)
			{
				if (!cameraManager.HasCombatViewOn())
				{
					cameraManager.ToggleCombatView(true);
				}

				StateCombat();
			}
		}	
	}
	
	public InputField input;
	
	#region Dice Rolling
	public void ForceDiceRoll()
	{
		Debug.Log(int.Parse(input.text));
		listPlayers[currentPlayer].StartMovement(int.Parse(input.text));
		listPlayers[currentPlayer].FindFinish();
		listPlayers[currentPlayer].Walk();
		turnState = 1;
	}
	
	// Waits for the player to roll the dice, then for them to stop and finally
	// passes the total die cast to the player.
	void StateRollDice()
	{
		// Checks if the dice have been rolled and if they have stopped.
		if (diceRolled && !diceManager.IsRolling()) {					
			// Resets dice rolled.
			diceRolled = false;
			
			// Starts the player moving and updates the turn state.
			listPlayers[currentPlayer].StartMovement(diceManager.GetDiceResults());
			listPlayers[currentPlayer].FindFinish();
			// start walking after dice is rolled and movement begins
			listPlayers[currentPlayer].Walk();
			turnState = 1;
		}
		else if (diceRolled == false) {
			// Sets the number of dice to roll.
			//diceManager.ShowDice(listPlayers[currentPlayer].GetDice());
			
			// Waits for the player to roll the dice.
			if (Input.GetButtonDown(rollButton) || turnGUI.GetPressedButton() == 0) {
				// Rolls the dice and sets to wait for result.
				diceManager.RollDice();
				diceRolled = true;
				
				// Hides the gui.
				turnGUI.ShowHideGUI(false);
			}
		}
	}
	
	#endregion
	
	#region Movement
	// Waits for the player to stop moving.
	void StateMovePlayer()
	{
		// Checks if the player is moving between tiles.
		if (listPlayers[currentPlayer].IsMovingBetweenTiles()) {
			// Continues the movement.
			listPlayers[currentPlayer].Move();
		}
		else {
			// Gets the player's map position.
			Vector2 playerPos = listPlayers[currentPlayer].GetMapPosition();
		
			// Checks if the player just stopped.
			if (listPlayers[currentPlayer].JustStoppedMoving()) {
				// Sets that the player has not stopped moving anymore.
				listPlayers[currentPlayer].SetJustStopped(false);
				
				// Gets the monster card on the current tile.
				MonsterCard mc = map.GetMonsterCardOnTile((int)playerPos.x, (int)playerPos.y);
				
				// Checks that a monster is on the tile.
				if (mc != null) {
					// Shows the monster card.
					cardManager.RevealMonCard(mc);
					// Starts combat.
					combatManager.EstablishMonCombat(listPlayers[currentPlayer], mc, cardManager.GetMonsterModel(mc.GetMonModelIndex()));
					listPlayers[currentPlayer].CombatIdle();
					// Updates state to combat.
					turnState = 4;
				}
				
				// Gets the index of a player on the current tile.
				int enemyPlayer = CheckPlayerPositions(playerPos, currentPlayer);
				
				// Checks if there is a player on the tile.
				if (enemyPlayer != -1) {
					// Do your combat initiallising here Jack. It would be currentPlayer versus enemyPlayer.
					combatManager.EstablishPvpCombat(listPlayers[currentPlayer], listPlayers[enemyPlayer]);
					listPlayers[currentPlayer].CombatIdle();
					listPlayers[enemyPlayer].CombatIdle();
					Debug.Log ("PVP BEGIN");
					
					// Updates state to combat.
					turnState = 4;
				}
				
				// Checks if the player cannot move anymore
				if (!listPlayers[currentPlayer].GetMoving()) {
					// Updates the turn state to show the card.
					// as long as the player isnt already in combat with a prexisting monster or player.
					if (turnState != 4)
					{
						// Checks if there is a chest on the tile.
						Chest chest = chestManager.CheckTileForChest(listPlayers[currentPlayer].GetMapPosition());
						if (chest != null) {
							chestManager.GivePlayerChestContents(listPlayers[currentPlayer], chest, currentPlayer);							
							
							turnState = 3;
						}
						else {	
							turnState = 2;
						}
						// idle when finished moving
						listPlayers[currentPlayer].Idle();
					}
				}
			}
				else {
					// Checks if the current tile is a choice tile.
					bool[] dirs = map.IsChoiceTile((int)playerPos.x, (int)playerPos.y);
					
					// Checks if the directions is not null.
					if (dirs != null) {
						// Shows the direction buttons.
						ShowDirectionCanvas(dirs);
						// idles when direction buttons are shown
						listPlayers[currentPlayer].Idle();
					}
					else {
						
						// Gets the direction the player must move in.
						int dir = map.IsForceTile((int)playerPos.x, (int)playerPos.y);
						
						// Checks if the player has to change direction.
						if (dir != -1) {
							// Changes the player's direction.
							listPlayers[currentPlayer].SetDirection(dir);
						}	
						
						// Finds the next tile to move to.
						listPlayers[currentPlayer].MoveTile();
					}
				}
		}
		
		// Right-click to skip movement and go straight to last tile.
		if (Input.GetMouseButtonDown(1)) {
			listPlayers[currentPlayer].SkipToFinish();
		}
	}
	
	#endregion
	
	// Gets the area tile that the player landed on and shows the first card
	// from the corresponding deck. (To be expanded)
	void StateShowCard()
	{
		// Checks if the card is already showing.
		if (cardManager.IsShowingCard()) {
			// Checks if a choice need to be made.
			if (cardManager.NeedMakeChoice()) {
				// Checks if a choice has been made.
				if (cardManager.HasMadeChoice()) {
					// Applies the card choice effects.
					cardManager.CheckChoices(currentPlayer, listPlayers);
					
					// Hides the card.
					cardManager.HideCard();
					
					// Changes turn state to change players.
					turnState = 3;
				}
			}
			else 
			{
				// Waits for the player to press the roll button to hide card and end turn.
				if (Input.GetButtonDown(rollButton)) 
				{
					// Applies the effect of the card.
					cardManager.ApplyEffect(currentPlayer, listPlayers);				
					// set up combat if a monster card is encountered
					if (cardManager.HasMonEncountered())
					{
						if (cardManager.HasMonRevealed())
						{
							MonsterCard monster = cardManager.GetMonEncountered();
							GameObject monModel = cardManager.GetMonsterModel(monster.GetMonModelIndex());

							combatManager.EstablishMonCombat(listPlayers[currentPlayer], monster, monModel);
							listPlayers[currentPlayer].CombatIdle();

							/*if (monModel.GetComponent<MonsterAnims>())
							{
								monModel.GetComponent<MonsterAnims>().CombatIdle();
							}*/

							Debug.Log ("COMBAT BEGIN");
							turnState = 4;
							cardManager.HideCard();
							cardManager.ResetMonsterFlags();
						}
						else
						{
							cardManager.RevealMonCard();
						}
					}
					else
					{
						// Hides the card.
						cardManager.HideCard();
						// Changes turn state to change players.
						turnState = 3;
					}
				}
			}
		}
		else 
		{
			// Gets the player's grid position.
			Vector2 playGrid = listPlayers[currentPlayer].GetMapPosition();
			
			// Gets the area type from the map at the player's position.
			int tileType = map.GetTile((int)playGrid.x, (int)playGrid.y)-1;
			
			// Checks if rest area.
			if (tileType == 6) {
				// Skip card drawing.
				turnState = 3;
				
				return;
			}
			
			// Shows the first card from the coresponding deck.
			cardManager.DisplayCard(tileType);
		}
	}

	void StateCombat()
	{
		if (!combatManager.HasCombatEnded())
		{
			// roll until both combatants are rolled
			if (!cmbBothRolled)
			{
				// Checks if the dice have been rolled and if they have stopped.
				if (diceRolled && !diceManager.IsRolling()) 
				{					
					// Resets dice rolled.
					diceRolled = false;

					if (!cmbPlayerRolled)
					{
						Debug.Log ("Player Rolled: " + diceManager.GetDiceResults());
						combatManager.SetPlayerDiceRoll(diceManager.GetDiceResults());
						cmbPlayerRolled = true;
					}
					else
					{
						if (combatManager.HasMonCombat())
						{
							Debug.Log ("Monster Rolled: " + diceManager.GetDiceResults());
							combatManager.SetMonsterDiceRoll(diceManager.GetDiceResults());
						}
						else if (combatManager.HasPlayerCombat())
						{
							Debug.Log ("Second Player Rolled: " + diceManager.GetDiceResults());
							combatManager.SetPlayerTwoDiceRoll(diceManager.GetDiceResults());
						}

						cmbBothRolled = true;
					}


				}
				else if (diceRolled == false) {
					// Waits for the player to roll the dice.
					if (Input.GetButtonDown(rollButton)) {

						if (!listPlayers[currentPlayer].HasSkippedMonster())
						{
							// Rolls the dice and sets to wait for result.
							diceManager.RollDice();
							diceRolled = true;
						}
						else
						{
							combatManager.ForceCombatEnd();
							listPlayers[currentPlayer].SetSkipMonster(false);
						}
					}
				}
			}
			else
			{
				// if both dice are rolled ask for a resolve then wait for the flag from combatManager
				/*if (combatManager.HasRoundResolved())
				{
					cmbPlayerRolled = false;
					cmbBothRolled = false;
					combatManager.ResetRound();
					Debug.Log ("should reset round now");
				}
				else
				{

				}*/

				combatManager.ResolveCombat();
			}

		}
		else
		{
			combatManager.ResetCombat();
			cmbPlayerRolled = false;
			cmbBothRolled = false;
			// change players
			turnState = 3;
			
			cardManager.HideMonsterCard();
			cardManager.ResetMonsterFlags();

			Debug.Log ("Combat Over");
		}


	}
}