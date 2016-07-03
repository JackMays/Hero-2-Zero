using UnityEngine;
using System.Collections;

public class CombatGUI : MonoBehaviour
{
	#region Variables
	// Parent of the canvas items.
	public Transform canvasParent;
	
	// Holds the index of the button that was pressed.
	int pressedButton = -1;
	
	// Holds the current player.
	Player currentPlayer = null;
	
	#endregion
	
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
	
	#endregion
}