using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour {

	List<ItemCard> potentialSaleItems = new List<ItemCard>();
	List<WeaponCard> potentialSaleWeapons = new List<WeaponCard>();

	List<Card> forSaleCards = new List<Card>();

	Player customer = null;

	int card = 0;

	// Use this for initialization
	void Start () {
	
		forSaleCards.Add (new ItemCard("Test Item", 0, 4, 1, 2, 4, 1, "Shop test Item", 0));
		forSaleCards.Add (new WeaponCard("Test Weapon", -4, 4, 1, 0, "Shop Test Weapon", 0));

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PopulateSale()
	{
		for (int i = 0; i < 3; ++i)
		{
			int saleType = Random.Range(0, 2);
			int card = 0;

			if (saleType == 0)
			{
				card = Random.Range(0, potentialSaleItems.Count);

				ItemCard item = potentialSaleItems[card];

				forSaleCards.Add (item);

			}
			else
			{
				card = Random.Range(0, potentialSaleWeapons.Count);

				WeaponCard weapon = potentialSaleWeapons[card];

				forSaleCards.Add(weapon);
			}




		}


	}

	public void SetCustomer (Player player)
	{
		customer = player;
	}

	public void SetBoughtCard(int bought)
	{
		card = bought;
	}

	public void BuyItem(/*int card, Player customer*/)
	{
		Card toBuy = forSaleCards[card];
		ItemCard boughtItem = toBuy as ItemCard;
		WeaponCard boughtWeapon = toBuy as WeaponCard;

		if (boughtItem != null)
		{
			customer.ChangeItems(boughtItem, true);
		}
		else if (boughtWeapon != null)
		{
			customer.SetEquippedWeapon(boughtWeapon);
		}
	}
}
