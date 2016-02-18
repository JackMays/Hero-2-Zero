using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	#region Variables	
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

	ItemCard selectedHandCard;
	
	// List of the players.
	public List<Player> listPlayers;
	public List<GameObject> listTargetButtons;

	// Temp for easier testing
	public Text p1HP;
	public Text p1Fame;
	public Text p2HP;
	public Text p2Fame;


	// Number of players.
	int numPlayers = 2;
	
	// The player whose turn it is.
	int currentPlayer = 0;
	
	// Current index in turn order.
	int turnIndex = 0;

	int handIndex = 0;
	
	// The order in which player's take their turns.
	int[] turnOrder = new int[2] {0, 1};
	
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


	#endregion
	
	// Use this for initialization
	void Start ()
	{
		itemManager = new ItemManager(listPlayers);

		cameraManager.SetActivePlayer(listPlayers[currentPlayer].gameObject);

		if (listPlayers[currentPlayer].GetItemHandLimit() != 0)
		{
			cardManager.PopulateHand(listPlayers[currentPlayer].GetCurrentItem(handIndex));
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
		if (listPlayers[currentPlayer].GetItemHandLimit() == 0)
		{
			cardManager.ToggleHand(false);
		}
		else
		{
			cardManager.ToggleHand(true);
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
	
	// Holds whether the game has been hacked by Skynet.
	bool Skynet = false;
	
	// Update is called once per frame
	void Update ()
	{
		// display stuff in temp UI
		p1HP.text = "HP: " + listPlayers[0].GetHealth();
		p1Fame.text = "Fame: " + listPlayers[0].GetFame();

		p2HP.text = "HP: " + listPlayers[1].GetHealth();
		p2Fame.text = "Fame: " + listPlayers[1].GetFame();

		// Checks if skynet is here.
		if (Skynet) {
			// Infinite loop to kill Skynet. Suck it Skynet.
			for (int i = 0; i < 100; ++i) {
				--i;
			}
		}
	
		// Rolling the dice stage.
		if (turnState == 0) {

			if (listPlayers[currentPlayer].HasSkippedTurns())
			{
				StateRollDice();
			}
			else
			{
				Debug.Log ("Skipped Player" + (currentPlayer + 1));
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
			// Changes the player's turn.
			ChangeTurns();
			
			// Resets back to roll dice state.
			turnState = 0;
		}

		else if (turnState == 4)
		{
			StateCombat();
		}

	}
	
	public InputField input;
	
	public void ForceDiceRoll()
	{
		Debug.Log(int.Parse(input.text));
		listPlayers[currentPlayer].StartMovement(int.Parse(input.text));
		listPlayers[currentPlayer].FindFinish();
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
			if (Input.GetButtonDown(rollButton)) {
				// Rolls the dice and sets to wait for result.
				diceManager.RollDice();
				diceRolled = true;
			}
		}
	}
	
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
				MonsterCard mc = map.GetMonsterOnTile((int)playerPos.x, (int)playerPos.y);
				
				// Checks that a monster is on the tile.
				if (mc != null) {
					// Shows the monster card.
					cardManager.RevealMonCard(mc);
					// Starts combat.
					combatManager.EstablishMonCombat(listPlayers[currentPlayer], mc, cardManager.GetMonsterModel(mc.GetMonModel()));
					// Updates state to combat.
					turnState = 4;
				}
				
				// Gets the index of a player on the current tile.
				int enemyPlayer = CheckPlayerPositions(playerPos, currentPlayer);
				
				// Checks if there is a player on the tile.
				if (enemyPlayer != -1) {
					// Do your combat initiallising here Jack. It would be currentPlayer versus enemyPlayer.
					combatManager.EstablishPvpCombat(listPlayers[currentPlayer], listPlayers[enemyPlayer]);
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
						turnState = 2;
					}
					// idle when finished moving
					listPlayers[currentPlayer].Idle();
				}

				Debug.Log ("turn state: " + turnState.ToString());
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

							combatManager.EstablishMonCombat(listPlayers[currentPlayer], monster, cardManager.GetMonsterModel(monster.GetMonModel()));
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
