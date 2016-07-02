using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnGUI : MonoBehaviour
{
	#region Variables
	// Parent of the canvas items.
	public Transform canvasParent;
	
	// Holds the index of the button that was pressed.
	int pressedButton = -1;
	
	// Holds the current player.
	Player currentPlayer = null;
	
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Stores the current player, changes the top button's text and shows the GUI. (Called at start of turn).
	public void StartNewPlayerTurn(Player play, bool inVillage)
	{
		// Stores the curent player.
		currentPlayer = play;
		
		// Changes the top button to say "Roll Dice".
		ChangeButtonText(true);
		
		// Changes the shop button to be pressable or not pressable.
		ChangeShopActive(inVillage);
		
		// Show the GUI>
		ShowHideGUI(true);
		
		// Resets the pressed button.
		pressedButton = -1;
	}
	
	// Changes the top button's text, changes interactability of shop buttonh and shows the GUI. (Called at end of turn).
	public void EndPlayerTurn(bool inVillage)
	{
		// Changes the top button to say "End Turn".
		ChangeButtonText(false);
		
		// Changes the shop button to be pressable or not pressable.
		ChangeShopActive(inVillage);
		
		// Show the GUI>
		ShowHideGUI(true);
		
		// Resets the pressed button.
		pressedButton = -1;
	}
	
	// Changes the text of the first button to either roll the dice or end turn.
	public void ChangeButtonText(bool roll)
	{
		// Checks if the button should say roll dice or not.
		if (roll) {
			canvasParent.GetChild(1).GetChild(1).GetComponent<Text>().text = "Roll Dice";
		}
		else {
			canvasParent.GetChild(1).GetChild(1).GetComponent<Text>().text = "End Turn";
		}	
	}
	
	// Changes whether the shop button can be pressed or not.
	public void ChangeShopActive(bool canPress)
	{
		canvasParent.GetChild(4).GetComponent<Button>().interactable = canPress;
	}
	
	// Returns which button was pressed.
	public int GetPressedButton()
	{
		return pressedButton;
	}	
	
	// Sets whether to show or hide the GUI.
	public void ShowHideGUI(bool show)
	{
		canvasParent.gameObject.SetActive(show);
	}
	
	// Returns whether the GUI is being shown.
	public bool GetShowing()
	{
		return canvasParent.gameObject.activeSelf;
	}
	
	#region Setters
	
	// Sets which button was pressed.
	public void SetPressedButton(int button)
	{
		pressedButton = button;
	}
	
	// Stores the current player for showing the inventory.
	public void SetCurrentPlayer(Player play)
	{
		currentPlayer = play;
	}
	
	#endregion
}