using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour
{
	#region Variables
	// Holds whether the chest is open or not.
	bool isOpen = false;
	
	// Holds the chest's grid position.
	Vector2 gridPosition = Vector2.zero;
	
	// The chest's animations.
	Animation anims;
	
	// What the chest's item is.
	Card item;
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
	
	#endregion
	
	#region Setters
	// Assigns the chest's item to be what is given.
	public void SetItem(Card i)
	{
		item = i;
	}
	
	// Sets the chest's grid position.
	public void SetGridPosition(Vector2 pos)
	{
		gridPosition = pos;
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
		if (Input.GetKeyDown(KeyCode.C)) {
			// Checks if the chest is already open.
			if (isOpen) {
				// Opens the chest.
				anims.Play("Close");
				isOpen = false;
			}
			else {
				// Closes the chest.
				anims.Play("Open");
				isOpen = true;
			}
		}
	}
}
