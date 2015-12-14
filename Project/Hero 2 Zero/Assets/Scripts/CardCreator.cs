using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class CardCreator : MonoBehaviour
{
	#region Variables
	// The list of card images.
	public List<Sprite> cardImages;
	
	// The object displaying the image.
	public Image image;
	
	// Card image to display.
	int imageIndex = 0;
	
	// The list of area toggles.
	public List<Toggle> areaToggles;
	
	// Holds the selected area.
	int currentArea = 0;
	
	// The list of type toggles.
	public List<Toggle> typeToggles;
	
	// Holds the selected card type.
	int currentType = 0;
	
	// Holds the description reference.
	public Text description;
	
	// References to the fame/gold/health panel and fields.
	#region Fame/Gold/Health Values
	// The panel that holds the fields.
	public GameObject FGHPanel;
	
	// The value of target to change.
	public InputField valueInput;
	
	// The list of targets.
	public List<Toggle> targetToggles;
	
	// The target panel.
	public GameObject targetPanel;
	
	// Current target of the card.
	int target = 0;
	#endregion
	
	
	// References to the choice panel.
	#region Choice Values
	// The panels that holds the fields.
	public GameObject choicePanel;
	
	
	
	#endregion
	
	
	// References to the teleport panel.
	#region Teleport Values
	
	
	#endregion
	
	
	#endregion 
	
	
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Changes the current location in the image list.
	public void ChangeImageIndex(bool next)
	{
		// Checks if the image needs to be next or previous.
		if (next) {
			// Checks if the image needs to reset to beginning.
			if (imageIndex == cardImages.Count - 1) {
				// Resets back to first image.
				imageIndex = 0;
			}
			else {
				// Increment index.
				++imageIndex;
			}
		}
		else {
			// Checks if the image needs to skip to end.
			if (imageIndex == 0) {
				// Skips to last image.
				imageIndex = cardImages.Count-1;
			}
			else {
				// Decrement index.
				--imageIndex;
			}
		}
		
		// Updates the displayed image.
		image.sprite = cardImages[imageIndex];
	}
	
	// Sets the chosen area and deselects the previous.
	public void SetSelectedArea(int a)
	{
		// Sets the old toggle to false.
		areaToggles[currentArea].isOn = false;
	
		// Saves the slected area.
		currentArea = a;
	}	
	
	// Sets the chosen card type and deselects the previous.
	public void SetSelectedType(int t)
	{
		// Sets the old toggle to false.
		typeToggles[currentType].isOn = false;
		
		// Saves the slected area.
		currentType = t;
		
		// Shows the partner panel to the selected type.
		ShowExtraPanel();
	}
	
	// Shows the panel of extra options for the current type.
	void ShowExtraPanel()
	{
		// Checks if Fame/Gold/Health type.
		if (currentType == 1 || currentType == 2 || currentType == 6) {
			// Checks if the panel is already showing.
			if (!FGHPanel.activeSelf) {
				// Hides all other panels.
				HideExtraPanels();
			}
			
			// Shows the fame/gold/health panel.
			FGHPanel.SetActive(true);
			
			// Shows the target panel.
			targetPanel.SetActive(true);
		}
		else {
			HideExtraPanels();
		}
	}
	
	// Hides all the extra option panels.
	void HideExtraPanels()
	{
		// Hides the fame/gold/health extra options.
		FGHPanel.SetActive(false);
		
		// Hides the target panel.
		targetPanel.SetActive(false);
	}
	
	// Sets the chosen target and deselects the previous.
	public void SetSelectedTarget(int t)
	{
		// Sets the old toggle to false.
		targetToggles[target].isOn = false;
		
		// Saves the slected area.
		target = t;
	}
	
	// Saves the current card in a readable format to be used in the main game.
	public void SaveCard()
	{
		// Holds the cards details.
		string cardDetails = "NEW CARD\n";
		
		// Adds the card type.
		cardDetails += "Type : " + currentType + "\n";
		
		// Adds the card's area.
		cardDetails += "Area : " + currentArea + "\n";
		
		// Adds the card's image.
		cardDetails += "Image : " + imageIndex + "\n";
		
		// Adds the card's description.
		cardDetails += "Description : " + description.text + "\n";
		
		// Checks which type of card it is.
		
		// Is a Fame/Gold/Health card.
		if (currentType == 1 || currentType == 2 || currentType == 6) {
			cardDetails += WriteFGHValues();
		}
		
		// Adds the card to the file.
		File.AppendAllText("Assets/Resources/Cards.txt", cardDetails);
			
	}
	
	// Writes the values for Fame/Gold/Health cards and returns them.
	string WriteFGHValues()
	{
		// Holds the card's details.
		string details = "";
		
		// Adds the card's change value.
		details += "Value : " + valueInput.text + "\n";
		
		// Adds the card's target.
		details += "Target : " + target + "\n";
		
		// Returns the new details.
		return details;
	}	
}