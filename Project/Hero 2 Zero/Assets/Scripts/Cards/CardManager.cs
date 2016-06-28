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
		CreateMonster,
		Monster,
		Skip,
		MultipleEffect
	}

	#region Variables
	// Queues for each type of card.
	Queue<Card> laneCards = new Queue<Card>();
	Queue<Card> villageCards = new Queue<Card>();
	Queue<Card> fieldCards = new Queue<Card>();
	Queue<Card> forestCards = new Queue<Card>();
	Queue<Card> mountainCards = new Queue<Card>();
	List<MonsterCard> monsterCards = new List<MonsterCard>();
	
	// Items are a list because specific cards may need to be drawn at certain times.
	List<ItemCard> itemCards = new List<ItemCard>();
	
	// List of the queues. Makes it easier to access a certain list.
	List<Queue<Card>> cardList = new List<Queue<Card>>();
	
	// The physical card to be displayed to the player
	public Canvas cardObject;
	public Canvas monsterCardObject;
	public Canvas handCardObject;

	public Button showHand;
	public Button hideHand;
	public Button leftHand;
	public Button rightHand;
	// The buttons on the card.
	GameObject choice1;
	GameObject choice2;
	
	// List of card images.
	public List<Sprite> imageList;
	public List<Sprite> monImageList;
	public List<GameObject> monPrefabList;
	
	// The current drawn card.
	Card drawnCard;
	
	// Holds whether the player needs to make a choice.
	bool makeChoice = false;
	bool isMonsterDrawn = false;
	bool isMonsterRevealed = false;
	
	// Holds the selected choice.
	int chosenChoice = -1;
	
	// Holds whether a card is being shown.
	bool cardShowing = false;
	
	// Reference to the player info GUI.
	public PlayerInfoGUI playerInfoGUI;
	
	// Reference to the scroll.
	public TESTScrollRoll scrollEvent; 
	#endregion
	
	#region Start Shit
	// Use this for initialization
	void Start ()
	{
		// Adds the queues to the list.
		cardList.Add(laneCards);
		cardList.Add(villageCards);
		cardList.Add(fieldCards);
		cardList.Add(forestCards);
		cardList.Add(mountainCards);
		
		// Stores the buttons.
		choice1 = cardObject.transform.GetChild(3).gameObject;
		choice2 = cardObject.transform.GetChild(4).gameObject;
		
		// Hides the card.
		cardObject.enabled = false;
		monsterCardObject.enabled = false;

		ToggleHand(false);
		
		// Creates the cards.
		CreateCards ();
		//ReadCards();
		
		// Loads the images.
		//LoadImages();
		
		// Shuffles the decks.
		for (int i =0; i < 5; ++i) {
			ShuffleNormalCards(i);
		}
	}
	
	// Creates the cards and adds them to their queues.
	void CreateCards()
	{	
		int[] c1E = new int[2] {1, 2};
		int[] c1T = new int[2] {0, 0};
		int[] c1V = new int[2] {10, 10};
		int[] c2E = new int[1] {1};
		int[] c2T = new int[1] {0};
		int[] c2V = new int[1] {-10};
	
		// Lane Cards: 10
		laneCards.Enqueue(new ChoiceCard("Help", "Leave", c1E, c1T, c1V, c2E, c2T, c2V, 0, "Billy and the Well", "Billy is stuck in a well."));
		laneCards.Enqueue(new FameCard(10, 0, 0, "Goblin Corpse", "You come across a goblin corpse and passing villagers congratulate your victory."));
		laneCards.Enqueue(new FameCard(-5, 0, 0, "Roadsign Rearrange", "You rearrange the directions of some roadsigns."));
		laneCards.Enqueue(new FameCard(-10, 0, 0, "Bitchy Old Lady", "You kick a passing old lady. Bitch."));
		laneCards.Enqueue(new FameCard(10, 0, 0, "Carriage Attack", "You assault a innocent looking carriage travelling the path. It's cargo is Kidnapped children."));
		laneCards.Enqueue(new Card(0, "Like a Fag", "You feel so joyful you start to skip. Like a fag.", 0));
		laneCards.Enqueue(new Card(0, "Stones", "You find a stone and leave it.", 0));
		laneCards.Enqueue(new HealthCard(-5, 0, 0, "Trip-idation of Potholes", "You trip on a pothole"));
		laneCards.Enqueue(new SkipCard(1, 0, "Tree Blocked", "A fallen tree is blocking your path. Skip next turn."));
		
		c1E = new int[2] {1, 2};
		c1T = new int[2] {0, 0};
		c1V = new int[2] {-10, -20};
		
		laneCards.Enqueue(new MultipleEffectCard(c1E, c1T, c1V, 0, "Bandit Attack", "You are ambushed by a group of bandits."));
		
		// Village Cards: 10
		
		c1E = new int[1] {1};
		c1T = new int[1] {0};
		c1V = new int[1] {10};
		c2E = new int[1] {1};
		c2T = new int[1] {0};
		c2V = new int[1] {-10};
	
		villageCards.Enqueue(new ChoiceCard("Gain 10 Fame", "Lose 10 Fame", c1E, c1T, c1V, c2E, c2T, c2V, 1, "Simple Question", "Do you want Fame?"));
		villageCards.Enqueue(new FameCard(-10, 0, 1, "Cropped", "You trample a villager's crops."));
		villageCards.Enqueue(new FameCard(-25, 0, 1, "Zinedine Zidane at the Door", "You knock on someone's door and headbutt them when they answer."));
		villageCards.Enqueue(new Card(1, "Pyromaniac", "You set someone's hut on fire. Pyromania YEAH.", 0));
		villageCards.Enqueue(new Card(1, "Window Shopper", "You window shop as you wander the streets.", 0));
		villageCards.Enqueue(new Card(1, "Observer", "You take some time to people watch in the Village Square.", 0));
		villageCards.Enqueue(new FameCard(-15, 0, 1, "Beard Brawl", "You find a big bearded man and start a public fight."));
		villageCards.Enqueue(new FameCard(-5, 0, 1, "Flower Girl Punt", "A girl gives you a flower. You kick her."));
		villageCards.Enqueue(new GoldCard(-20, 0, 1, "Childish Thievery", "A passing gang of kids steal money from you. -20 Gold."));
		
		c1E = new int[2] {2, 6};
		c1T = new int[2] {0, 0};
		c1V = new int[2] {-10, 20};
		
		villageCards.Enqueue(new MultipleEffectCard(c1E, c1T, c1V, 1, "Nap Time", "You spend the night at a local inn."));
		
		c1E = new int[1] {1};
		c1T = new int[1] {0};
		c1V = new int[1] {-20};
		c1E = new int[1] {1};
		c2T = new int[1] {0};
		c2V = new int[1] {20};
		
		villageCards.Enqueue(new ChoiceCard("Join in", "Stop it", c1E, c1T, c1V, c2E, c2T, c2V, 1, "Bar Brawl", "A bar brawl has broken out where you are drinking."));
		
		
		// Field Cards: 10 (sans the commented out ones)
		fieldCards.Enqueue(new MonsterEventCard("Slime", 2, "A Slimy Encounter", "You encounter a friendly slime. Now slay it."));
		fieldCards.Enqueue(new FameCard(-10, 0, 2, "Don't Believe in Fairies", "A passing fairy grants you a wish. You wish for the fairy to die."));
		fieldCards.Enqueue(new FameCard(10, 0, 0, "Sexy Butterflies", "Ripped apart a Butterfly. You saved Butterfly kind from evil Dictator!"));
		fieldCards.Enqueue(new Card(2, "Hayfever", "You get hayfever because you're a pusseh.", 0));
		//fieldCards.Enqueue(new Card(2, "A flower calls you a prick. You step on it.", 0));
		fieldCards.Enqueue(new Card(2, "Grass", "Grass. Pretty much it.", 0));
		fieldCards.Enqueue(new Card(2, "Cloudwatching", "You lie on the grass and cloudwatch.", 0));
		villageCards.Enqueue(new GoldCard(20, 0, 2, "Buried Gold", "You spot an oddly out of place dirt mound in the grass. Digging it unearths a small pouch."));
		fieldCards.Enqueue(new HealthCard(20, 0, 2, "Lazy Nap", "You lie in the field and relax under the warm sun."));
		fieldCards.Enqueue(new HealthCard(10, 0, 2, "Picnic Time", "A group of villagers invite you to join their picnic"));
		
		c1E = new int[2] {1, 6};
		c1T = new int[2] {0, 0};
		c1V = new int[2] {-10, -20};
		c2E = new int[1] {1};
		c2T = new int[1] {0};
		c2V = new int[1] {10};
		
		fieldCards.Enqueue(new ChoiceCard("Destroy it", "Report it", c1E, c1T, c1V, c2E, c2T, c2V, 2, "Botony", "You come across a rare breed of poison ivy."));
		
		// Forest Cards: 10
		forestCards.Enqueue(new HealthCard(10, 0, 3, "Yummy Apple", "You pick an apple from a tree and eat it."));
		forestCards.Enqueue(new SkipCard(1, 3, "Feast with Elves", "You are invited to a feast by the local elves. Skip a turn."));
		forestCards.Enqueue(new SkipCard(2, 3, "Lost", "All the trees look the same and you find yourself quickly lost."));
		forestCards.Enqueue(new Card(3, "Slender Forest", "You find a piece of paper stuck to a tree. Suddenly you can hear drums.", 0));
		forestCards.Enqueue(new Card(3, "Timber", "You see a tree and kick it down. Turns out a tree falling in the forest does make a sound.", 0));
		forestCards.Enqueue(new Card(3, "Vantage Point", "You climb a tree to get a good vantage point.", 0));
		forestCards.Enqueue(new FameCard(-20, 0, 3, "Sacred Destruction", "You trip over a sacred statue and break it."));
		forestCards.Enqueue(new FameCard(-20, 0, 3, "Magical Unicorn", "You come across a unicorn that will grant you 3 wishes. You scissor kick it."));
		forestCards.Enqueue(new FameCard(-20, 0, 3, "Racism", "You are EXTREMELY racist to an Elf. Like, wow, can we even put that in this game?"));
		forestCards.Enqueue(new GoldCard(50, 0, 3, "Forest Chest", "In a forest clearing you an important looking chest. As usual."));
		
		c1E = new int[2] {1, 9};
		c1T = new int[2] {0, 0};
		c1V = new int[2] {-10, 1};
		
		// Mountain Cards: 10
		mountainCards.Enqueue(new MultipleEffectCard(c1E, c1T, c1V, 4, "Snowy Shortcut", "You take a shortcut through a snowy pass, only to find it deeper than expected. -10 Health, Skip next turn."));
		mountainCards.Enqueue(new Card(4, "Mountain Goat", "You ride a mountain goat.", 0));
		mountainCards.Enqueue(new Card(4, "Wanna build a Snowman", "You build a snowman. Pretty snowman. And then you fireball it.", 0));
		mountainCards.Enqueue(new Card(4, "Avalanche", "You shout at the mountain and cause an avalanche.", 0));
		mountainCards.Enqueue(new FameCard(-10, 0, 4, "Dead Old Lady", "You push an old lady down the slopes."));
		mountainCards.Enqueue(new FameCard(-10, 0, 4, "Mountain Bear Prank", "You sneak up on a mountain bear and place a sign on his neck, 'Free Hugs'"));
		mountainCards.Enqueue(new MonsterEventCard("Frost Troll", 4, "Forest Troll", "You climb the mountain stairs and encounter a frost troll."));
		mountainCards.Enqueue(new HealthCard(-20, 0, 4, "Slip 'n' Slide", "You slip in the snow and fall down the mountain."));
		
		c1E = new int[2] {1, 2};
		c1T = new int[2] {0, 0};
		c1V = new int[2] {10, 20};
		
		mountainCards.Enqueue(new MultipleEffectCard(c1E, c1T, c1V, 4, "Snowman Contest", "You win the snowman building contest."));

		c1E = new int[2] {1, 2};
		c1T = new int[2] {0, 0};
		c1V = new int[2] {10, 30};
		
		mountainCards.Enqueue(new MultipleEffectCard(c1E, c1T, c1V, 4, "Bounty Hunter", "You help capture a criminal and claim their bounty."));

		// Monster Cards; 5th img element and 6th type enum: 3
		// name, card image, model, HP, Strength, Defence, gold gain, fame gain, fame loss, base card image, description
		/*monsterCards.Add(new MonsterCard("Fucking Snowman", 0, 0, 10, 10, 3, 10, 10, -10, 5, "Monster!"));
		monsterCards.Add(new MonsterCard("Slime", 1, 1, 10, 4, 1, 5, 5, -15, 5, "Monster!"));
		monsterCards.Add(new MonsterCard("Frost Troll", 2, 0, 15, 8, 4, 20, 10, -10, 5, "Monster!"));*/
		monsterCards.Add(new MonsterCard("Goblin", 0, 2, 1 /*15*/, 3 /*8*/, 4, 5, 10, -10, 5, "Monster!"));
	}
	
	#endregion
	
	#region Loading Cards
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
		Card c = new Card(0, "", "", 0);
	
		// Checks if a blank card.
		if (type == 0) {			
			// Creates a new card.
			return new Card(i, "Title", d, type);
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
				return new FameCard(v, t, i, "Title", d);
			}
			// Checks if a gold card.
			else if (type == 2) {
				// Creates a new card.
				return new GoldCard(v, t, i, "Title", d);
			}
			// Health card.
			else {
				// Creates a new card.
				return new HealthCard(v, t, i, "Title", d);
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
		
		return new Card(0,"","",0);
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
				monsterCards.Add((MonsterCard) c);
				break;		
		}
	}
	
	void LoadImages()
	{
	
	}
	#endregion
	
	#region Draw & Display Card
	// Displays the card to the player.
	public void DisplayCard(int area)
	{
		// Gets the first card in the designated area queue.
		// Checks if the area is a monster area.
		if (area == 5) {
			// Draws a monster card.
			drawnCard = DrawMonsterCard();
		} else 
		{
			// Draws a normal card.
			drawnCard = DrawCard(area);
			
			DisplayNormalCard(true);
			
			cardShowing = true;
			
			return;
		}

		// Set isMonsterDrawn if a monster is drawn, false if it isnr
		// Serves as a way to reset this boolean without additional functions or lines
		isMonsterDrawn = ((Type)drawnCard.GetCardType() == Type.Monster);
		
		cardObject.transform.GetChild(1).GetComponent<Image>().sprite = imageList[drawnCard.GetImageIndex()];
		// Changes the card description.
		cardObject.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = drawnCard.GetDescription();
		
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
		
		cardShowing = true;
	}
	
	// Shows the normal card and updates its values.
	void DisplayNormalCard(bool other)
	{
		Vector2[] changes = new Vector2[1];
		
		// Checks if the card is a normal card, an effect card or a multiple effect card.
		if (drawnCard.GetCardType() == 1 || drawnCard.GetCardType() == 2 || drawnCard.GetCardType() == 6) {
			changes = new Vector2[1] { new Vector2(drawnCard.GetCardType(), GetCardTypeValue()) };
		}
		else if (drawnCard.GetCardType() == 10) {
			// Casts the card as a multiple effect card.
			MultipleEffectCard mCard = (MultipleEffectCard)drawnCard;
			
			changes = new Vector2[2] { new Vector2(mCard.GetCardArray(0)[0], mCard.GetCardArray(2)[0]),
				new Vector2(mCard.GetCardArray(0)[1], mCard.GetCardArray(2)[1]) };
		}
		else {
			changes = null;
		}
		
		scrollEvent.CreateEvent(drawnCard.GetImageIndex(), drawnCard.GetTitle(), drawnCard.GetDescription(), changes); 
	}
	
	// Pulls the first card and then puts it at the end of the pile.
	Card DrawCard(int pile)
	{
		if (pile > 6) {
			pile = 1;
		}
	
		// Takes the first card from the specified queue.
		Card c = cardList[pile].Dequeue();
		
		// Adds the card back to the end of the queue.
		cardList[pile].Enqueue(c);
		
		// Returns the card.
		return c;
	}
	
	// Use this method to draw a monster card.
	public Card DrawMonsterCard()
	{
		// Takes the first card from the list.
		MonsterCard mon = monsterCards[0];
		
		// Shuffles the monster cards.
		ShuffleMonsterCards();
		
		// Sets the drawn card to a deep copy of the monster card.
		return new MonsterCard(mon);
	}
	
	// Shuffles a normal card deck.
	void ShuffleNormalCards(int index)
	{
		// Creates a list to temporarily store the deck.
		List<Card> cards = new List<Card>();
		
		// Loops through the deck.
		while (cardList[index].Count > 0) {
			// Adds the card to the temp list.
			cards.Add(cardList[index].Dequeue());
		}
		
		// Declaring the int here so as to not keep declaring in the loop.
		int rand = 0;
		
		// Loops through the list and randomly adds cards back to the queue.
		while (cards.Count > 0) {
			rand = Random.Range(0, cards.Count);
			cardList[index].Enqueue(cards[rand]);
			cards.RemoveAt(rand);
		}
	}
	
	// Shuffles the monster cards.
	void ShuffleMonsterCards()
	{
		// Creates a second list to temporarily hold the randomised list.
		List<MonsterCard> temp = new List<MonsterCard>();
		
		// Declaring int here to prevent numerous declaring in the loop.
		int rand = 0;
		
		// Loops through the list.
		while (monsterCards.Count > 0) {
			// Gets a random idnex.
			rand = Random.Range(0, monsterCards.Count);
			// Adds a random index to the temp list.
			temp.Add(monsterCards[rand]);
			// Removes the card.
			monsterCards.RemoveAt(rand);
		}
		
		// Sets the monster card list to the new shuffled list.
		monsterCards = temp;
	}
	
	// Shows the normal card and updates its values.
	void DisplayNormalCard()
	{
		// Sets the card's image.
		cardObject.transform.GetChild(1).GetComponent<Image>().sprite = imageList[drawnCard.GetImageIndex()];
		
		// Changes the card description.
		cardObject.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = drawnCard.GetDescription();
		
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
		
		// Shows the card.
		cardObject.enabled = true;
	}
	
	int GetCardTypeValue()
	{
		// Checks if the card is a fame, gold, health or magic card.
		if (drawnCard.GetCardType() == 1) {
			// Casts the card as a fame card and returns the fame value.
			FameCard card = (FameCard)drawnCard;
			return card.GetFame();
		}
		
		if (drawnCard.GetCardType() == 2) {
			// Casts the card as a gold card and returns the gold value.
			GoldCard card = (GoldCard)drawnCard;
			return card.GetGold();
		}
		
		if (drawnCard.GetCardType() == 6) {
			// Casts the card as a health card and returns the health value.
			HealthCard card = (HealthCard)drawnCard;
			return card.GetHealth();
		}
		
		// Ignore Magic for now.
		return 0;
	}
	
	public void RevealMonCard()
	{
		// Casts the drawn card as a monster card.
		MonsterCard mon = (MonsterCard)drawnCard;
		
		// Sets the card's image.
		monsterCardObject.transform.GetChild(1).GetComponent<Image>().sprite = monImageList[mon.GetMonImg()];
		
		// Sets the card's name.
		monsterCardObject.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = mon.GetName();
		// Sets the card's attack.
		monsterCardObject.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = mon.GetStrength().ToString();
		// Sets the card's health.
		monsterCardObject.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = mon.GetHealth().ToString();
		// Sets the card's defense.
		monsterCardObject.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = mon.GetDefense().ToString();
		// Sets the card's fame gain.
		monsterCardObject.transform.GetChild(6).GetChild(1).GetComponent<Text>().text = mon.GetFameMod(true).ToString();
		// Sets the card's fame loss.
		monsterCardObject.transform.GetChild(7).GetChild(1).GetComponent<Text>().text = mon.GetFameMod(false).ToString();
		// Sets the card's gold gain.
		monsterCardObject.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = mon.GetVictoryGold().ToString();
		
		// Hides the normal card.
		cardObject.enabled = false;
		
		// Shows the monster card.
		monsterCardObject.enabled = true;
		
		// Sets that the card has been shown.
		isMonsterRevealed = true;

		Debug.Log ("revealed (Drawn): " + mon.GetName());
	}
	
	// For when landing on a persisting monster.
	public void RevealMonCard(MonsterCard mon)
	{
		// Sets the card's image.
		monsterCardObject.transform.GetChild(1).GetComponent<Image>().sprite = monImageList[mon.GetMonImg()];
		
		// Sets the card's name.
		monsterCardObject.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = mon.GetName();
		// Sets the card's attack.
		monsterCardObject.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = mon.GetStrength().ToString();
		// Sets the card's health.
		monsterCardObject.transform.GetChild(4).GetChild(1).GetComponent<Text>().text = mon.GetHealth().ToString();
		// Sets the card's defense.
		monsterCardObject.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = mon.GetDefense().ToString();
		// Sets the card's fame gain.
		monsterCardObject.transform.GetChild(6).GetChild(1).GetComponent<Text>().text = mon.GetFameMod(true).ToString();
		// Sets the card's fame loss.
		monsterCardObject.transform.GetChild(7).GetChild(1).GetComponent<Text>().text = mon.GetFameMod(false).ToString();
		// Sets the card's gold gain.
		monsterCardObject.transform.GetChild(8).GetChild(1).GetComponent<Text>().text = mon.GetVictoryGold().ToString();
		
		// Hides the normal card.
		cardObject.enabled = false;
		
		// Shows the monster card.
		monsterCardObject.enabled = true;
		
		// Sets that the card has been shown.
		isMonsterRevealed = true;

		Debug.Log ("revealed (Persistant): " + mon.GetName());
	}
	#endregion
	
	#region Choices	
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
				Debug.Log("Fame Effect");
				// Creates a fame card.
				drawnCard = new FameCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", "");
			}
			
			// Checks if a gold effect.
			else if (t == Type.Gold) {
				Debug.Log("Gold Effect");
				// Creates a gold card.
				drawnCard = new GoldCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", "");
			}
			
			// Checks if an item effect.
			else if (t == Type.Item) {
				// Creates a item card.
				drawnCard = new ItemEventCard(c.GetChoiceExtra(chosenChoice)[i], c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", "");
			}
			
			// Checks if a Teleport effect.
			else if (t == Type.Teleport) {
				// Creates a teleport card.
				drawnCard = new MoveCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceValues(chosenChoice)[i], c.GetChoiceExtra(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", "");
			}
			
			// Checks if a health effect.
			else if (t == Type.Health) {
				// Creates a health card.
				drawnCard = new HealthCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "", "");
			}
			
			// Checks if a monster effect.
			else if (t == Type.Monster) {
				// Creates a monster card.
				//drawnCard = new MonsterCard(c.GetChoiceValues(chosenChoice)[i], c.GetChoiceTargets(chosenChoice)[i], 1, "");
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
	
	#endregion
	
	#region Card Effects
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
			// Teleport
			else if (cardType == Type.Teleport) {
				TeleportPlayer(currentPlayer, players);
			}
			// Health
			else if (cardType == Type.Health) {
				ChangeHealthEffect(currentPlayer, players);
			}
			// Create Monster
			else if (cardType == Type.CreateMonster) {
				CreateMonster();
			}
			// Combat
			else if (cardType == Type.Monster) {
				
			}
			// Skip Turn
			else if (cardType == Type.Skip) {
				ChangeTurnSkip(currentPlayer, players);
			}
			// Multiple Effects
			else if (cardType == Type.MultipleEffect) {
				CastMultipleEffects(currentPlayer, players);
			}
		}
	}
	
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
			players[i].ChangeGold(c.GetGold());
		}
	}
	
	// Changes the items of selected player.
	void ChangeItemEffect(int currentPlayer, List<Player> players)
	{
		// Casts the drawn card as a gold card to access the functions.
		ItemEventCard c = (ItemEventCard)drawnCard;
		
		// Gets the target of the card.
		int target = c.GetTarget();
		
		// Finds the players which will be effected. 
		List<int> affectedPlayers = FindTargets(target, currentPlayer, players.Count);
		
		// Loops through all the players and changes the gold.
		foreach (int i in affectedPlayers) {
			players[i].ChangeItems(itemCards[c.GetItemIndex()], c.GetAddRemove());
		}
	}
	
	// Teleports selected players to set target.
	void TeleportPlayer(int currentPlayer, List<Player> players)
	{
	
	}
	
	// Changes the gold value of selected player.
	void ChangeHealthEffect(int currentPlayer, List<Player> players)
	{
		// Casts the drawn card as a gold card to access the functions.
		HealthCard c = (HealthCard)drawnCard;
		
		// Gets the target of the card.
		int target = c.GetTarget();
		
		// Finds the players which will be effected. 
		List<int> affectedPlayers = FindTargets(target, currentPlayer, players.Count);
		
		// Loops through all the players and changes the gold.
		foreach (int i in affectedPlayers) {
			players[i].ChangeHealth(c.GetHealth());
		}
	}
	
	// Spawns a monster on the current tile.
	void CreateMonster()
	{
		// Casts the current card as a monster event card.
		MonsterEventCard card = (MonsterEventCard)drawnCard;
		
		// Holds the monster card from the list.
		MonsterCard mon = new MonsterCard(monsterCards[0]);
		
		// Loops through the list.
		for (int i = 0; i < monsterCards.Count; ++i) {
			// Checks if the current monster's name matches the searching name.
			if (monsterCards[i].GetName() == card.GetName()) {
				// Stores the monster card and ends the loop.
				mon = monsterCards[i];
				i = monsterCards.Count;
			}
		}
		
		// Performs a deep copy of the monster card.
		drawnCard = new MonsterCard(mon);
		
		// Sets that a monster has been encountered.
		isMonsterDrawn = true;
	}
	
	// Combat function for apply effect if brought back into use
	void TriggerEncounter()
	{
		isMonsterDrawn = true;
	}
	
	// Changes the number of turns the player has to skip.
	void ChangeTurnSkip(int current, List<Player> players)
	{
		SkipCard s = (SkipCard)drawnCard;
	
		Debug.Log("SKIP 2!");
			
		players[current].ChangeTurnsToSkip(s.GetSkip());
	}
	
	// Creates cards depending on the multiple effects and calls the correct frunctions.
	void CastMultipleEffects(int current, List<Player> players)
	{
		// Casts the card as a multiple effect card.
		MultipleEffectCard m = (MultipleEffectCard)drawnCard;
		
		// Gets the arrays.
		int[] effects = m.GetCardArray(0);
		int[] targets = m.GetCardArray(1);
		int[] values = m.GetCardArray(2);
		
		//Loops through the effects.
		for (int i = 0; i < effects.GetLength(0); ++i) {
			// Creates a card for the current effect.
			switch (effects[i]) {
				// Fame Card.
				case 1:
					drawnCard = new FameCard(values[i], targets[i], 0, "", "");
					break;
				// Gold Card.
				case 2:
					drawnCard = new GoldCard(values[i], targets[i], 0, "", "");
					break;
				// Item Card.
				case 3:
					drawnCard = new ItemEventCard(true, values[i], targets[i], 0, "", "");
					break;
				// Teleport Card.
				case 5:
					// Not supported currently.
					
				// Health Card.	
				case 6:
					drawnCard = new HealthCard(values[i], targets[i], 0, "", "");
					break;
				// Creatre Monster.
				case 7:
					// Not supported currently.
					
				// Skip Turn.	
				case 9:
					Debug.Log("SKIP 1!");
					drawnCard = new SkipCard(values[i], 0, "", "");
					break;
			}
			
			// Applies the new card effect.
			ApplyEffect(current, players);
		}
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
	
	#region Small Monster Functions
	
	public bool HasMonEncountered()
	{
		return isMonsterDrawn;
	}
	
	public bool HasMonRevealed()
	{
		return isMonsterRevealed;
	}
	
	public MonsterCard GetMonEncountered()
	{
		return (MonsterCard)drawnCard;
	}	
	
	public GameObject GetMonsterModel(int index)
	{
		return monPrefabList[index];
	}
	
	public void ResetMonsterFlags()
	{
		isMonsterDrawn = false;
		isMonsterRevealed = false;
	}
	
	public void HideMonsterCard()
	{
		monsterCardObject.enabled = false;
		cardShowing = false;
	}
	
	#endregion
	
	#region Show/Hide Card
	// Returns whether the card is being shown.
	public bool IsShowingCard()
	{
		return cardShowing;
	}

	// Hides the card.
	public void HideCard()
	{
		cardObject.enabled = false;
		cardShowing = false;
		scrollEvent.ResetScroll();
	}
	#endregion

	#region Hand
	// called on turn switch
	public void ToggleHand(bool enable)
	{
		// enable and, scroll and hide buttons
		handCardObject.enabled = enable;
		leftHand.gameObject.SetActive(enable);
		rightHand.gameObject.SetActive(enable);
		hideHand.gameObject.SetActive(enable);
		// Enable show hand when the rest isnt
		showHand.gameObject.SetActive(!enable);

	}

	public void PopulateHand(ItemCard card)
	{
		handCardObject.transform.GetChild(1).GetComponent<Image>().sprite = imageList[card.GetImageIndex()];
		// Changes the card description.
		handCardObject.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = card.GetDescription();
	}

	#endregion
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.A)) {
			
		}
	}
}