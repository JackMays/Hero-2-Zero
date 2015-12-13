using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class CardManager : MonoBehaviour
{
	enum Type
	{
		None,
		Fame,
		Gold,
		Item,
		Choice,
		Teleport,
		Health,
		Monster
	}

	#region Variables
	// Queues for each type of card.
	Queue<Card> laneCards = new Queue<Card>();
	Queue<Card> villageCards = new Queue<Card>();
	Queue<Card> fieldCards = new Queue<Card>();
	Queue<Card> forestCards = new Queue<Card>();
	Queue<Card> mountainCards = new Queue<Card>();
	Queue<Card> monsterCards = new Queue<Card>();
	
	// List of the queues. Makes it easier to access a certain list.
	List<Queue<Card>> cardList = new List<Queue<Card>>();
	
	// The physical card to be displayed to the player
	public Canvas cardObject;
	// The buttons on the card.
	GameObject choice1;
	GameObject choice2;
	
	// List of card images.
	public List<Sprite> imageList;
	public List<Sprite> monImageList;
	
	// The current drawn card.
	Card drawnCard;
	
	// Holds whether the player needs to make a choice.
	bool makeChoice = false;
	bool isMonsterDrawn = false;
	
	// Holds the selected choice.
	int chosenChoice = -1;
	
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		// Adds the queues to the list.
		cardList.Add(laneCards);
		cardList.Add(villageCards);
		cardList.Add(fieldCards);
		cardList.Add(forestCards);
		cardList.Add(mountainCards);
		cardList.Add(monsterCards);
		
		// Stores the buttons.
		choice1 = cardObject.transform.GetChild(3).gameObject;
		choice2 = cardObject.transform.GetChild(4).gameObject;
		
		// Hides the card.
		cardObject.enabled = false;
		
		// Creates the cards.
		CreateCards ();
		//ReadCards();
		
		// Loads the images.
		LoadImages();
	}
	
	// Creates the cards and adds them to their queues.
	void CreateCards()
	{	
		// Lane Cards
		laneCards.Enqueue(new FameCard(10, 0, 0, "You trip on a pothole", 1));
		laneCards.Enqueue(new Card(0, "You come across a goblin corpse.", 0));
		laneCards.Enqueue(new Card(0, "You kick a passing old lady. Bitch.", 0));
		laneCards.Enqueue(new Card(0, "You feel so joyful you start to skip. Like a fag.", 0));
		laneCards.Enqueue(new Card(0, "You find a stone and leave it.", 0));
		
		// Village Cards
		
		int[] c1E = new int[1] {1};
		int[] c1T = new int[1] {0};
		int[] c1V = new int[1] {10};
		int[] c2E = new int[1] {1};
		int[] c2T = new int[1] {0};
		int[] c2V = new int[1] {-10};
	
		villageCards.Enqueue(new ChoiceCard("Gain 10 Fame", "Lose 10 Fame", c1E, c1T, c1V, c2E, c2T, c2V, 1, "Do you want Fame?", 4));
		
		villageCards.Enqueue(new Card(1, "You knock on someone's door and headbutt them when they answer.", 0));
		villageCards.Enqueue(new Card(1, "You set someone's hut on fire. Pyromania YEAH.", 0));
		villageCards.Enqueue(new Card(1, "You window shop as you wander the streets.", 0));
		villageCards.Enqueue(new Card(1, "You find a big bearded man and start a public fight.", 0));
		villageCards.Enqueue(new Card(1, "A girl gives you a flower. You kick her.", 0));
		
		// Field Cards
		fieldCards.Enqueue(new Card(2, "You get hayfever because you're a pusseh.", 0));
		fieldCards.Enqueue(new Card(2, "A flower calls you a prick. You step on it.", 0));
		fieldCards.Enqueue(new Card(2, "A passing fairy grants you a wish. You wish for the fairy to die.", 0));
		fieldCards.Enqueue(new Card(2, "Grass. Pretty much it.", 0));
		fieldCards.Enqueue(new Card(2, "You lie on the grass and cloudwatch.", 0));
		
		// Forrest Cards
		forestCards.Enqueue(new Card(3, "All the trees look the same and you find yourself quickly lost.", 0));
		forestCards.Enqueue(new Card(3, "You find a piece of paper stuck to a tree. Suddenly you can hear drums.", 0));
		forestCards.Enqueue(new Card(3, "The elves invite you to a feast. You're the feast.", 0));
		forestCards.Enqueue(new Card(3, "You see a tree and kick it down. Turns out a tree falling in teh forrest does make a sound.", 0));
		forestCards.Enqueue(new Card(3, "You climb a tree to get a good vantage point.", 0));
		
		// Mountain Cards
		mountainCards.Enqueue(new Card(4, "You ride a mountain goat.", 0));
		mountainCards.Enqueue(new Card(4, "You build a snowman. Pretty snowman. And then you fireball it.", 0));
		mountainCards.Enqueue(new Card(4, "You shout at the mountain and cause an avalanche.", 0));
		mountainCards.Enqueue(new Card(4, "You push an old lady down the slopes.", 0));
		mountainCards.Enqueue(new Card(4, "You climb the mountain stairs and encounter a frost troll.", 0));

		// Monster Cards; 5th img element and 6th type enum
		monsterCards.Enqueue(new MonsterCard("Fucking Snowman",0, 10, 10, 10, 10, 5, "Monster!", 7));
	}
	
	void DebugList()
	{
		for (int i = 0; i < 6; ++i) {
			for (int j = 0; j < cardList[i].Count; ++j) {
				Debug.Log (cardList[i].Dequeue());
			}
		}
	}
	
	// Reads through all the cards in the file and adds them to the decks.
	List<Card> ReadCards()
	{
		// Opens the card file.
		StreamReader strm = new StreamReader("Assets/Resources/Cards.txt");
		
		// Holds the list of cards to return.
		List<Card> cards = new List<Card>();
		
		// Reads the first line.
		string text = strm.ReadLine();
		
		// Loops while there is still a card.
		while (text == "NEW CARD") {
			// Reads in the type of card.
			string type = strm.ReadLine();
			// Cuts the title from the line.
			type = type.Replace(" ", "");
			type = type.Substring(type.IndexOf(":") + 1);
			
			// Reads in the card's area.
			string a = strm.ReadLine();
			// Cuts the title from the line.
			a = a.Replace(" ", "");
			a = a.Substring(a.IndexOf(":") + 1);
			// Converts the string to an int.
			int area = int.Parse(a);
			
			// Creates a card for the given type.
			Card c = CreateCardFromType(strm, int.Parse(type));
			
			// Adds the card to the corresponding deck.
			AddCardToDeck(c, area);
			
			// Reads in the next line to see if there is another card.
			text = strm.ReadLine();
		}
		
		// Returns the list of cards.
		return cards;
	}
	
	
	
	// Uses the given type to create a card.
	Card CreateCardFromType(StreamReader strm, int type)
	{
		// Reads in the card's image.
		string value = strm.ReadLine();
		// Cuts the title from the line.
		value = value.Replace(" ", "");
		value = value.Substring(value.IndexOf(":") + 1);
		// Converts the string to an int.
		int i = int.Parse(value);
		
		// Reads in the card's description.
		value = strm.ReadLine();
		// Cuts the title from the line.
		value = value.Replace(" ", "");
		string d = value.Substring(value.IndexOf(":") + 1);

	
		// Holds the card.
		Card c = new Card(0, "", 0);
	
		// Checks if a blank card.
		if (type == 0) {			
			// Creates a new card.
			return new Card(i, d, type);
		}
		
		// Checks if a fame, gold or health card.
		if (type == 1 || type == 2 || type == 6) {
			// Reads in the change value.
			value = strm.ReadLine();
			// Cuts the title from the line.
			value = value.Replace(" ", "");
			value = value.Substring(value.IndexOf(":") + 1);
			// Converts the string to an int.
			int v = int.Parse(value);
			
			// Reads in the target.
			value = strm.ReadLine();
			// Cuts the title from the line.
			value = value.Replace(" ", "");
			value = value.Substring(value.IndexOf(":") + 1);
			// Converts the string to an int.
			int t = int.Parse(value);
			
			// Checks if a fame card.
			if (type == 1) {
				// Creates a new card.
				return new FameCard(v, t, i, d, type);
			}
			// Checks if a gold card.
			else if (type == 2) {
				// Creates a new card.
				return new GoldCard(v, t, i, d, type);
			}
			// Health card.
			else {
				// Creates a new card.
				return new DamageCard(v, t, i, d, type);
			}
		}
		
		// Checks if an item card.
		if (type == 3) {
		
		}
		
		// Checks if a choice card.
		if (type == 4) {
		
		}
		
		// Checks if a teleport card.
		if (type == 5) {
		
		}
		
		return new Card(0,"",0);
	}
	
	// Adds the card to the deck corresponding with the given area.
	void AddCardToDeck(Card c, int area)
	{
		// Checks which area the card appears in.
		switch (area) {
			// Lane.
			case 0:
				laneCards.Enqueue(c);
				break;
			// Village.
			case 1:
				villageCards.Enqueue(c);
				break;
			// Field.
			case 2:
				fieldCards.Enqueue(c);
				break;
			// Forest.
			case 3:
				forestCards.Enqueue(c);
				break;
			// Mountain.
			case 4:
				mountainCards.Enqueue(c);
				break;
			// Monster.
			case 5:
				monsterCards.Enqueue(c);
				break;		
		}
	
	}
	
	void LoadImages()
	{
	
	}
	
	// Pulls the first card and then puts it at the end of the pile.
	Card DrawCard(int pile)
	{
		// Takes the first card from the specified queue.
		Card c = cardList[pile].Dequeue();
		
		// Adds the card back to the end of the queue.
		cardList[pile].Enqueue(c);
		
		// Returns the card.
		return c;
	}
	
	// Displays the card to the player.
	public void DisplayCard(int area)
	{
		// Gets the first card in the designated area queue.
		drawnCard = DrawCard(area);

		// Set isMonsterDrawn if a monster is drawn, false if it isnr
		// Serves as a way to reset this boolean without additional functions or lines
		isMonsterDrawn = ((Type)drawnCard.GetCardType() == Type.Monster);

		// Sets the card image.
		// TEMP change image and text to monster image, will make timed reveal soon
		if (isMonsterDrawn)
		{
			MonsterCard mc = (MonsterCard)drawnCard;
			cardObject.transform.GetChild(1).GetComponent<Image>().sprite = monImageList[0];
			cardObject.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = mc.GetName();
		}
		else
		{
			cardObject.transform.GetChild(1).GetComponent<Image>().sprite = imageList[drawnCard.GetImageIndex()];
			// Changes the card description.
			cardObject.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = drawnCard.GetDescription();
		}
		
		// Checks if a choice card or not.
		if ((Type)drawnCard.GetCardType () == Type.Choice) {
			// Choice shit.
			ShowChoices();
		}
		else {
			// Hide the choices.
			choice1.SetActive(false);
			choice2.SetActive(false);
		}
		
		cardObject.enabled = true;
	}
	
	void ShowChoices()
	{
		// Casts the card as a choice card.
		ChoiceCard c = (ChoiceCard)drawnCard;
		
		// Sets the text for both choices.
		choice1.transform.GetChild(0).GetComponent<Text>().text = c.GetChoiceText(0);
		choice2.transform.GetChild(0).GetComponent<Text>().text = c.GetChoiceText(1);
		
		// Sets that the player needs to make a choice.
		makeChoice = true;
		
		// Resets selected choice.
		chosenChoice = -1;
		
		// Show the choices.
		choice1.SetActive(true);
		choice2.SetActive(true);
	}
	
	// Runs through the selected choice's effects and applies them.
	public void CheckChoices(int currentPlayer, List<Player> players)
	{
		// Casts the drawn card as a choice card.
		ChoiceCard c = (ChoiceCard)drawnCard;
		
		// Loops through the card's choices and applies the effects.
		for (int i = 0; i < c.GetChoiceEffects(chosenChoice).GetLength(0); ++i) {
			// Checks which type of effect to apply.
			
			// Gets the effect type.
			Type t = (Type)c.GetChoiceEffects(chosenChoice)[i];
			
			// Checks if fame effect.
			if (t == Type.Fame) {
				// Creates a fame card.
				drawnCard = new FameCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", 1);
			}
			
			// Checks if a gold effect.
			else if (t == Type.Gold) {
				// Creates a gold card.
				drawnCard = new GoldCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", 1);
			}
			
			// Checks if an item effect.
			else if (t == Type.Item) {
				// Creates a item card.
				//drawnCard = new GoldCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", 1);
			}
			
			// Checks if a Teleport effect.
			else if (t == Type.Teleport) {
				// Creates a teleport card.
				//drawnCard = new GoldCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", 1);
			}
			
			// Checks if a health effect.
			else if (t == Type.Health) {
				// Creates a health card.
				//drawnCard = new GoldCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", 1);
			}
			
			// Checks if a monster effect.
			else if (t == Type.Monster) {
				// Creates a monster card.
				//drawnCard = new GoldCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", 1);
			}
			
			
			
			// Applies the effect.
			ApplyEffect(currentPlayer, players);
		}
		
		makeChoice = false;
	}
	
	// Sets the selected choice.
	public void SetChosenChoice(int cho)
	{
		chosenChoice = cho;
	}

	// Returns whether a choice needs to be made.
	public bool NeedMakeChoice()
	{
		return makeChoice;
	}
	
	// Returns whetehr the player has made a choice.
	public bool HasMadeChoice()
	{
		// Checks if a choice has been selected.
		if (chosenChoice != -1) {
			// Returns that a choice has been made.
			return true;
		}
		
		// Returns that a choice hasn't been made.
		return false;
	}

	public bool HasMonEncountered()
	{

		return isMonsterDrawn;
	}

	public MonsterCard GetMonEncountered()
	{
		return (MonsterCard)drawnCard;
	}
	
	// Checks the card to see if it has an effect and then applies it.
	public void ApplyEffect(int currentPlayer, List<Player> players)
	{
		Type cardType = (Type)drawnCard.GetCardType();
	
		// Checks if the card is not a normal card.
		if (cardType != Type.None) {
			// Checks which type of card effect to apply.
			
			// Fame Change.
			if (cardType == Type.Fame) {
				ChangeFameEffect(currentPlayer, players);
			}
			// Gold Change.
			else if (cardType == Type.Gold) {
				ChangeGoldEffect(currentPlayer, players);
			}
			// Item Change.
			else if (cardType == Type.Item) {
				ChangeItemEffect(currentPlayer, players);
			}
			// Choice.
			else if (cardType == Type.Choice) {
				ApplyChoice(currentPlayer, players);
			}
			// Teleport
			else if (cardType == Type.Teleport) {
				TeleportPlayer(currentPlayer, players);
			}
			// Combat
			else if (cardType == Type.Monster) {
				TriggerEncounter();
			}
		}
	}
	
	
	
	#region Card Effects
	// Changes the fame value of selected player.
	void ChangeFameEffect(int currentPlayer, List<Player> players)
	{
		// Casts the drawn card as a fame card to access the functions.
		FameCard c = (FameCard)drawnCard;
	
		// Gets the target of the card.
		int target = c.GetTarget();
		
		// Finds the players which will be effected. 
		List<int> affectedPlayers = FindTargets(target, currentPlayer, players.Count);
		
		// Loops through all the players and changes the fame.
		foreach (int i in affectedPlayers) {
			players[i].ChangeFame(c.GetFame());
		}
	}
	
	// Changes the gold value of selected player.
	void ChangeGoldEffect(int currentPlayer, List<Player> players)
	{
		// Casts the drawn card as a gold card to access the functions.
		GoldCard c = (GoldCard)drawnCard;
		
		// Gets the target of the card.
		int target = c.GetTarget();
		
		// Finds the players which will be effected. 
		List<int> affectedPlayers = FindTargets(target, currentPlayer, players.Count);
		
		// Loops through all the players and changes the gold.
		foreach (int i in affectedPlayers) {
			players[i].ChangeFame(c.GetGold());
		}
	}
	
	// Changes the items of selected player.
	void ChangeItemEffect(int currentPlayer, List<Player> players)
	{
		
	}
	
	// Waits for the player to make a choice and then applies the choice.
	void ApplyChoice(int currentPlayer, List<Player> players)
	{
	
	}
	
	// Teleports selected players to set target.
	void TeleportPlayer(int currentPlayer, List<Player> players)
	{
	
	}
	// combat function for apply effect if brought back into use
	void TriggerEncounter()
	{
		isMonsterDrawn = true;
	}
	
	// Finds the players that the card will affect.
	List<int> FindTargets(int target, int current, int max)
	{
		// 0: Current Player | 1: Next Player | 2: All Other Players | 3: All Players
		// The list of players to return.
		List<int> returnList = new List<int>();
		
		// Checks the target value to choose which players to target.
		switch (target) {
		case 0:
			// Add the current player.
			returnList.Add(current);
			break;
		case 1:
			// Checks if the current player is last.
			if (current == max-1) {
				// Adds the first player.
				returnList.Add(0);
			}
			else {
				// Adds the next player.
				returnList.Add (current);
			}
			break;
		case 2:
			// Loops through all players.
			for (int i = 0; i < max; ++i) {
				// Skips the current player.
				if (i != current) {
					// Adds the player to the list.
					returnList.Add(i);
				}
			}
			break;
		case 3:
			// Loops through all players.
			for (int i = 0; i < max; ++i) {
				// Adds the player list.
				returnList.Add(i);
			}
			break;	
		};
		
		// Returns the filled list.
		return returnList;
	}
	#endregion
	
	
	// Returns whether the card is being shown.
	public bool IsShowingCard()
	{
		return cardObject.enabled;
	}
	
	// Hides the card.
	public void HideCard()
	{
		cardObject.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.A)) {
			DisplayCard(5);
			
			List<Player> plays = new List<Player>() {GameObject.Find("Totem").GetComponent<Player>(), GameObject.Find("Totem_Horned").GetComponent<Player>()};
			
			ApplyEffect(0, plays);
		}
		
		if (Input.GetKeyDown(KeyCode.S)) {
			DebugList ();
		}
	}
}
