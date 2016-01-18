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
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		// Gets the animations.
		anims = GetComponent<Animation>();
	}
	
	// Returns the chest's grid position.
	public Vector2 GetGridPosition()
	{
		return gridPosition;
	}
	
	// Sets the chest's grid position.
	public void SetGridPosition(Vector2 pos)
	{
		gridPosition = pos;
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
