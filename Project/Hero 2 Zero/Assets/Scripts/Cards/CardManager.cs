using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
		mountainCards.Enqueue(new Card(4, "You ride a mountain goat.", 4));
		mountainCards.Enqueue(new Card(4, "You build a snowman. Pretty snowman. And then you fireball it.", 4));
		mountainCards.Enqueue(new Card(4, "You shout at the mountain and cause an avalanche.", 4));
		mountainCards.Enqueue(new Card(4, "You push an old lady down the slopes.", 4));
		mountainCards.Enqueue(new Card(4, "You climb the mountain stairs and encounter a frost troll.", 4));

		// Monster Cards; 5th img element and 6th type enum
		monsterCards.Enqueue(new MonsterCard("Fucking Snowman",0, 10, 10, 10, 10, 5, "Monster!", 6));
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

		// Sets the card image.
		// TEMP change image and text to monster image, will make timed reveal soon
		if ((Type)drawnCard.GetCardType() == Type.Monster)
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
			
			// Shows the choices.
			choice1.SetActive(true);
			choice2.SetActive(true);
		}
		else {
			// Hide the choices.
			choice1.SetActive(false);
			choice2.SetActive(false);
		}
		
		cardObject.enabled = true;
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
			else if (cardType == Type.Monster) {
				BeginCombat();
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

	void BeginCombat()
	{

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
	}
}
