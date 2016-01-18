using UnityEngine;
using System.Collections;

public class ChestOpen : MonoBehaviour
{
	#region Variables
	// Holds whether the chest is open or not.
	bool isOpen = false;
	
	// The chest's animations.
	Animation anims;
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		// Gets the animations.
		anims = GetComponent<Animation>();
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
