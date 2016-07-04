using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TESTScrollRoll : MonoBehaviour
{
	#region Variables
	// The start and end position for moving the scroll.
	public Transform[] points;
	
	// Parent of the scroll and the depth mask.
	public Transform parent;
	
	// Referene to the scroll's transform.
	public Transform scroll;
	
	// The speed the scroll rolls across the csreen.
	float speed = 0.75f;
	
	// Time for moving the scroll across screen.
	float lerpTime = 0;
	
	// Holds wehther the scroll is rolling or not.
	bool isRolling = false;
	
	// Reference to the canvas background. (Background to get rid of a GetChild call.)
	public Transform canvasParent;
	
	// List of area images. (2nd last image is fame icon, last image is gold icon.)
	public Sprite[] images;
	#endregion
	
	// Changes the details of the event and sets the scroll to roll.
	public void CreateEvent(int ima, string title, string description, Vector2[] changes, bool hasChoices, ChoiceCard choiceCard)
	{
		// Changes the event details.
		ChangeDetails(ima, title, description, changes, hasChoices, choiceCard);
		
		// Shows the scroll.
		gameObject.SetActive(true);
		
		// Sets the position time back to 0.
		lerpTime = 0f;
		
		// Sets the scroll to start rolling.
		isRolling = true;
	}
	
	// Changes the details on the scroll. (The vector2 array holds the what needs to be changed and by how much.)
	void ChangeDetails(int ima, string title, string description, Vector2[] changes, bool hasChoice, ChoiceCard choice)
	{
		// Changes the area image.
		canvasParent.GetChild(0).GetComponent<Image>().sprite = images[0];//images[ima];
		
		// Changes the event title.
		canvasParent.GetChild(1).GetComponent<Text>().text = title;
		
		// Changes the event description.
		canvasParent.GetChild(2).GetComponent<Text>().text = description;
		
		// Checks if the event has a choice or not.
		if (hasChoice) {
			Debug.Log("Heading into Button Function");
			// Changes the button texts to show the choices.
			ChangeChoiceButtons(choice);
		}
		else {
			// Checks if there are no changes.
			if (changes == null) {
				canvasParent.GetChild(3).gameObject.SetActive(false);
				canvasParent.GetChild(4).gameObject.SetActive(false);
				
				return;
			}
		
			// Changes the effect icons and the accompanying strings.
			ChangeEffectIcons(changes);
		}
	}
	
	// Changes the effect icons and the accompanying strings.
	void ChangeEffectIcons(Vector2[] changes)
	{
		// Loops through the 2 change icons.
		for (int i = 0; i < 2; ++i) {
			// Checks if there is a change for the current icon.
			if (i < changes.Length) {
				// Shows the icon change. (It might be hidden.)
				canvasParent.GetChild(3 + i).gameObject.SetActive(true);
				
				// Changes the icon image.
				canvasParent.GetChild(3 + i).GetComponent<Image>().sprite = FindEffectIcon((int)changes[i].x);
				
				// Checks if the change is positive or negative.
				if (changes[i].y >= 0) {
					// Changes the icon text.
					canvasParent.GetChild(3 + i).GetChild(0).GetComponent<Text>().text = "+ " + changes[i].y.ToString();
				}
				else {
					// Changes the icon text.
					canvasParent.GetChild(3 + i).GetChild(0).GetComponent<Text>().text = "- " + (-changes[i].y).ToString();
				}
			}
			else {
				// There isn't a change for the current icon so hide it.
				canvasParent.GetChild(3 + i).gameObject.SetActive(false);
			}
		}
		
		// Hides the 2 choice buttons.
		canvasParent.GetChild(5).gameObject.SetActive(false);
		canvasParent.GetChild(6).gameObject.SetActive(false);
	}
	
	// Changes the choice buttons to show the choice and the rewards.
	void ChangeChoiceButtons(ChoiceCard choice)
	{
		// Shows the 2 choice buttons.
		canvasParent.GetChild(5).gameObject.SetActive(true);
		canvasParent.GetChild(6).gameObject.SetActive(true);
		
		// Hides the 2 effect icons.
		canvasParent.GetChild(3).gameObject.SetActive(false);
		canvasParent.GetChild(4).gameObject.SetActive(false);
		
		// Loops through the 2 buttons.
		for (int i = 0; i < 2; ++i) {
			// Changes the choice title.
			canvasParent.GetChild(5 + i).GetChild(0).GetComponent<Text>().text = choice.GetChoiceText(i);
			
			// Gets the choice effects and rewards.
			int[] effects = choice.GetChoiceEffects(i);
			int[] rewards = choice.GetChoiceValues(i);
			
			// String that will be assigned to the button's description.
			string desc = "";
			
			// Loops through the effects.
			for (int j = 0; j < effects.Length; ++j) {
				// Gets the correct sentence structure for the given effect.
				desc += GetEffectString(effects[j], rewards[j]);
				
				// Checks if there are 2 effects.
				if (effects.Length == 2 && j == 0) {
					// Adds a line break.
					desc += "\n";
				}
			}
			
			// Assigns the button's description to be the created string.
			canvasParent.GetChild(5 + i).GetChild(1).GetComponent<Text>().text = desc;
		}
	}
	
	// Creates a string that describes what is being lost and returns it.
	string GetEffectString(int effect, int reward)
	{
		// Checks if the effect is fame.
		if (effect == 1) {
			return "" + reward + " fame";
		}
		// Checks if the effect is gold.
		if (effect == 2) {
			return "" + reward + " gold";
		}
		// Checks if the effect is health.
		if (effect == 6) {
			return "" + reward + " health";
		}
		// Checks if the effect is magic.
		if (effect == 12) {
			return "" + reward + " magic";
		}
		// Checks if the effect is dice.
		if (effect == 11) {
			return "" + reward + " dice roll";
		}
		// Checks if the effect is skipping.
		if (effect == 9) {
			return "Skip next " + reward + " turns";
		}
		
		return "N/A";
	}
	
	
	// Temporary solution until type change later.
	Sprite FindEffectIcon(int index)
	{
		// Checks if a fame effect.
		if (index == 1) {
			return images[images.Length-1];
		}
		// Checks if a gold effect.
		if (index == 2) {
			return images[images.Length-2];
		}
		// Checks if a health effect.
		if (index == 6) {
			return images[images.Length-3];
		}
		// Checks if a dice effect.
		if (index == 11) {
			return images[images.Length-5];
		}
		// Checks if a magic effect.
		if (index == 12) {
			return images[images.Length-4];
		}		
		// Checks if a skip effect.
		return images[images.Length-6];
	}
	
	// Moves the scroll from point 1 to point 2.
	void RollScroll()
	{
		// Checks if the scroll has reached the end.
		if (lerpTime * speed >= 1) {
			isRolling = false;
		}
		
		// Increases position through movement.
		lerpTime += Time.deltaTime;
		
		// Moves the parent of the scroll and depth mask.
		parent.position = Vector3.Lerp(points[0].position, points[1].position, lerpTime * speed);
		
		// Rotates the scroll.
		scroll.Rotate(Vector3.up, -10);
	}
	
	// Skips the rolling.
	public void SkipRolling()
	{
		// Moves the scroll parent to the end.
		parent.position = points[1].position;
		
		// Rotates the scroll by a random number to help the illusion.
		scroll.Rotate(Vector3.up, Random.Range(1, 6) * -10);
	
		// Sets that the scroll has stopped rolling.
		isRolling = false;
	}
	
	// Hides the scroll, reset's its position and stops it rolling.
	public void ResetScroll()
	{
		// Hides the scroll.
		gameObject.SetActive(false);
		
		// Sets the scroll back to the beginning.
		parent.position = points[0].position;
		
		// Resets time back to 0.
		lerpTime = 0;
		
		// Sets teh scroll to stop rolling.
		isRolling = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.M)) {
			isRolling = true;
		}
	
		// Checks if the scroll is currently rolling.
		if (isRolling) {
			// Rolls the scroll.
			RollScroll();
		}
	}
}
