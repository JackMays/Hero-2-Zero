using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour {

	List<ItemCard> potentialSaleItems = new List<ItemCard>();
	List<WeaponCard> potentialSaleWeapons = new List<WeaponCard>();

	List<Card> forSaleCards = new List<Card>();

	// Use this for initialization
	void Start () {
	
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

	public void BuyItem(int card, Player customer)
	{
		Card toBuy = forSaleCards[card];
		ItemCard boughtItem = (ItemCard)toBuy;
		WeaponCard boughtWeapon = (WeaponCard)toBuy;

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
