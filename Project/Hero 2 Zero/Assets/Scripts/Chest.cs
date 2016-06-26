using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour
{
	#region Variables
	// Holds whether the chest is open or not.
	bool isOpen = false;
	
	// Holds the chest's grid position.
	public Vector2 gridPosition = Vector2.zero;
	
	// The chest's animations.
	Animation anims;
	
	// What the chest's item is.
	Card item;
	
	// Holds whether the chest has Gold, Items, Weapons or is a Mimic.
	int chestType = 0;
	
	// Holds the amount of gold the chest contains.
	int gold = 0;
	
	// The amount of turns before the chest can be closed again.
	int turnsToClose = 0;
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		// Gets the animations.
		anims = GetComponent<Animation>();
	}
	
	#region Getters
	// Returns the chest's item.
	public Card GetItem()
	{
		return item;
	}
	
	// Returns the chest's grid position.
	public Vector2 GetGridPosition()
	{
		return gridPosition;
	}
	
	// Returns whether the chest is open.
	public bool GetIsOpen()
	{
		return isOpen;
	}
	
	// Returns how much gold is in the chest.
	public int getGold()
	{
		return gold;
	}
	
	// Returns what the chest contains.
	public int GetChestType()
	{
		return chestType;
	}
	
	// Returns the number of turns before the chest can close.
	public int GetOpenTurns()
	{
		return turnsToClose;
	}
	
	#endregion
	
	#region Setters
	// Assigns the chest's item to be what is given.
	public void SetItem(ItemCard i)
	{
		item = i;
		
		chestType = 1;
	}
	
	// Sets the chest to have a set amount of gold inside.
	public void SetGold(int g)
	{
		gold = g;
		
		chestType = 0;
	}
	
	// Sets the chest to have a weapon.
	public void SetWeapon(WeaponCard w)
	{
		item = w;
		
		chestType = 2;
	}
	
	// Sets teh chest to be a mimic.
	public void SetMimic()
	{
		chestType = 3;
	}
	
	// Sets the chest's grid position.
	public void SetGridPosition(Vector2 pos)
	{
		gridPosition = pos;
	}
	
	// Sets the number of turns the chest must stay open.
	public void SetOpenTurns(int t)
	{
		turnsToClose = t;
	}
	#endregion
	
	// Opens or closes the chest.
	public void OpenCloseChest(bool open)
	{
		// Checks if the chest needs to be opened or closed.
		if (open) {
			// Opens the chest.
			anims.Play("Open");
			isOpen = true;
		}
		else {
			// Closes the chest.
			anims.Play("Close");
			isOpen = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
