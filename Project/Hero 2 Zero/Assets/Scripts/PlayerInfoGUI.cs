using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfoGUI : MonoBehaviour
{
	#region Variables
	// List of the player segments.
	public List<Transform> playerSections = new List<Transform>();
	
	// Number of players in the game.
	int numPlayers = 0;
	
	// The child index of each player info part.
	int childIcon = 1;
	int childName = 2;
	int childHealth = 3;
	int childMagic = 4;
	int childFame = 5;
	int childGold = 6;
	
	// Holds all the value changes that need to be finished.
	List<PlayerGUIValueChange> valuesToChange = new List<PlayerGUIValueChange>();
	
	// List of the player icons. (Fame and Gold are at the end of this list so use count-1 or count-2 to get them.
	public List<Sprite> playerIcons = new List<Sprite>();
	
	// The parent for the big version of fame change.
	public GameObject fameParent;
	
	// Reference to the fame text that needs to be changed.
	public Text[] fameTexts = new Text[2];
	
	// The Text that show how much fame is being changed and which moves.
	public GameObject minusText;
	
	// The point at which the minus fame starts from.
	public Transform minusOrigin;
	
	// The time progress for the big fame difference.
	
	#endregion
	
	// Sets up the UI's players and icons.
	void Setup(int numP, int[] playI)
	{
		// Sets the number of player sections to show.
		numPlayers = numP;
		
		// Hides all player info sections and then shows the required amount.
		for (int i = 0; i < 4; ++i) {
			playerSections[i].gameObject.SetActive(false);
			
			if (i < numP) {
				playerSections[i].gameObject.SetActive(true);
				
				// Sets the correct player icon for each section.
				playerSections[i].GetChild(1).GetComponent<Image>().sprite = playerIcons[playI[i]];
			}
		}
	}
	
	// Takes the values needed to create a value change instance.
	/// To use this function pass in the player's index (which player is it),
	/// whether the value to be changed is health/magic/gold/fame (ONLY 1),
	/// the value before changing and the amount to change by. (If you're lowering the value make sure its a - number.)
	public void CreateValueChange(int playInd, bool health, bool magic, bool fame, bool gold, int original, int change)
	{
		// Finds the child object based on the bools.
		int child = 3;
		
		if (magic) {
			child = 4;
		}
		else if (fame) {
			child = 5;
		}
		else if (gold) {
			child = 6;
		}
		
		// Creates the value change and adds it to the list.
		valuesToChange.Add (new PlayerGUIValueChange(playInd, child, original, change));
	}
	
	#region Lerps
	// Lerps a int from one value to another over time.
	int LerpInt(int start, int end, float time)
	{
		// Checks if time is greater than 1.
		if (time >= 1) {
			// Keeps the value in bounds by returning the target and skipping the calculations.
			return end;
		}
		
		// Finds the difference between the start and end values.
		// The difference is then multiplied by time to get a current position through change.
		// The position is then minused from the start to move the value along.
		float diff = start - end;
		float progress = diff * time;
		return start - (int)progress;
	}
	
	// Lerps a float from one value to another over time.
	float LerpFloat(float start, float end, float time)
	{
		// Checks if time is greater than 1.
		if (time >= 1) {
			// Keeps the value in bounds by returning the target and skipping the calculations.
			return end;
		}
		
		// Finds the difference between the start and end values.
		// The difference is then multiplied by time to get a current position through change.
		// The position is then minused from the start to move the value along.
		float diff = start - end;
		float progress = diff * time;
		return start - (float)progress;
	}
	#endregion
	
	#region Change Loop Functions
	
	// Runs through details that needs to be changed and changes them.
	void ChangeValues()
	{
		// Holds whether a fame/gold change has already been started.
		bool fameChangeInProgress = false;
	
		// Loops through each detail change.
		foreach (PlayerGUIValueChange vc in valuesToChange) {
			// Checks if the value is health or magic.
			if (vc.GetChildIndex() == 3 || vc.GetChildIndex() == 4) {
				// Adds delta time to each value to progress the lerp.
				vc.ChangeTime(Time.deltaTime);
				
				ChangeBarValues(vc);
			}
			// Value must be fame or gold.
			else {
				// Checks if a fame/gold change is in progress.
				if (!fameChangeInProgress) {
					// Adds delta time to each value to progress the lerp.
					vc.ChangeTime(Time.deltaTime);
					
					ChangeTextValues(vc);
					
					// Sets that a fame/gold change is in progress.
					fameChangeInProgress = true;
				}
			}
		}
		
		// Removes all finished value changes.
		RemoveFinishedChanges();
	}
	
	// Loops through all the value changes and removes the ones that have finished.
	void RemoveFinishedChanges()
	{
		// Temporary list to hold value changes that will be removed.
		List<PlayerGUIValueChange> toRemove = new List<PlayerGUIValueChange>();
		
		// Loops through each detail change.
		foreach (PlayerGUIValueChange vc in valuesToChange) {
			// Checks if the value change has finished.
			if (vc.GetHasFinished()) {
				// Adds it to the removal list.
				toRemove.Add(vc);
			}
		}
		
		// Checks if any value changes need to be removed.
		if (toRemove.Count > 0) {
			// Loops through all the removal list.
			foreach(PlayerGUIValueChange vc in toRemove) {
				// Removes the value change from the main list.
				valuesToChange.Remove(vc);
			}
		}
	}
	
	#endregion
	
	#region Health/Magic Bar Functions
	
	// Changes a player's health or magic value.
	void ChangeBarValues(PlayerGUIValueChange vc)
	{
		// Checks if the value's parent has not been found.
		if (vc.GetParent() == null) {
			vc.SetParent(FindBarParent(vc.GetChildIndex(), vc.GetPlayerIndex(), vc.GetTargetValue()));
		}
		
		// Checks if health or magic.
		if (vc.GetChildIndex() == 3) {
			// Lerps the change fill.
			vc.GetParent().GetChild(0).GetComponent<Image>().fillAmount = LerpFloat(vc.GetOriginalValue(), vc.GetTargetValue(), vc.GetTime()) * 0.05f;
		}
		else {
			// Lerps the change fill.
			vc.GetParent().GetChild(0).GetComponent<Image>().fillAmount = LerpFloat(vc.GetOriginalValue(), vc.GetTargetValue(), vc.GetTime()) * 0.2f;
		}		
	}
	
	// Finds the bar transform which is parent of the value that needs to be changed.
	Transform FindBarParent(int index, int segment, int target)
	{
		// The parent bar reference.
		Transform parent = null;
		
		// Finds the value's parent.
		parent = playerSections[segment].GetChild(index);
		
		// Checks if a health or magic bar.
		if (index == 3) {		
			// Changes the health bar's colour.						
			parent.GetChild(1).GetComponent<Image>().color = ChangeHealthBarColour(target);
			
			// Changes the value's fill amount.
			parent.GetChild(1).GetComponent<Image>().fillAmount = target * 0.05f;
			
			// Changes the health bar's text.
			parent.GetChild(2).GetComponent<Text>().text = "HP : " + target + "/20";
		}
		else {
			// Changes the magic bar's colour.
			parent.GetChild(1).GetComponent<Image>().color = ChangeMagicBarColour(target);
		
			// Changes the value's fill amount.
			parent.GetChild(1).GetComponent<Image>().fillAmount = target * 0.2f;
		
			// Changes the magic bar's text.			
			parent.GetChild(2).GetComponent<Text>().text = "MP : " + target + "/5";
		}
		
		// Returns the parent.
		return parent;
	}
	
	// Returns the colour the health bar should be for the given value.
	Color ChangeHealthBarColour(int value)
	{
		// Creates a color to return.
		Color col = new Color(0f, 0f, 0f);
		
		// Stores the health percentage.
		float percent = value / (float)20;	
		
		// More than 50% health so red has to increase.
		if (percent >= 0.5f) {
			percent = (1 - percent) * 2;
			
			col = new Color(percent, 1f, 0f);
		}
		// Less than 50% health so green has to decrease.
		else {
			percent = percent * 2;
			
			col =  new Color(1f, percent, 0f);
		}
		
		// Returns the new health colour.
		return col;
	}
	
	// Returns the colour the magic bar should be for the given value.
	Color ChangeMagicBarColour(int value)
	{
		// Creates a color to return.
		Color col = new Color(0f, 0f, 0f);
		
		// Stores the health percentage.
		float percent = value / (float)5;	
		
		col = new Color(0f, percent, 1f);
		
		// Returns the new health colour.
		return col;
	}
	
	#endregion
	
	#region Fame/Gold Text Functions
	
	// Changes a player's fame or gold value.	
	void ChangeTextValues(PlayerGUIValueChange vc)
	{
		// Checks if the value's parent has not been found.
		if (vc.GetParent() == null) {
			vc.SetParent(playerSections[vc.GetPlayerIndex()].GetChild(vc.GetChildIndex()));
		}
		
		// Changes the big fame icon to be either fame or gold.
		if (vc.GetChildIndex() == 5) {
			fameParent.transform.GetChild(0).GetComponent<Image>().sprite = playerIcons[playerIcons.Count-2];
		}
		else {
			fameParent.transform.GetChild(0).GetComponent<Image>().sprite = playerIcons[playerIcons.Count-1];
		}
		
		ShowBigFameChange(vc);
	}
	
	void ShowBigFameChange(PlayerGUIValueChange vc)
	{
		// Gets the current time progression.
		float time = vc.GetTime();
		
		// The colour of the minus text.
		Color minusColour = new Color();
		
		// Gets the difference between the old and new fame values.
		int change = vc.GetChangeValue();
		
		// Creates the minus text to be shown and changes the colour.
		if (change > 0) {
			minusText.GetComponent<Text>().text = "+ " + change;
			minusText.GetComponent<Text>().color = Color.green;
			minusColour = Color.green;
		}
		else {
			minusText.GetComponent<Text>().text = "- " + Mathf.Abs(change);
			minusText.GetComponent<Text>().color = Color.red;
			minusColour = Color.red;
		}
		
		// Enlarges the fame parent.
		if (time < 0.5f) {
			minusText.SetActive(true);
			fameParent.SetActive(true);
			fameParent.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time * 2.5f);
			
			fameTexts[0].text = vc.GetOriginalValue().ToString();
		}
	
		// Moves the minus text downwards and lerps alpha to transparent.
		if (time >= 0.65f && time < 1.5f) {		
			Color trans = minusColour;
			trans.a = 0;
			
			minusText.transform.position = Vector3.Lerp(minusOrigin.position, fameTexts[0].transform.position, (time - 0.65f) * 1.25f);
			minusText.GetComponent<Text>().color = Color.Lerp(minusColour, trans, (time - 0.65f) * 1.25f);
		}
		// If the minus text has reached the fame text then start changing the fame text.
		if (time >= 1.5f && time <= 2.75f) {
			// Hides the minus text.
			minusText.SetActive(false);
			minusText.transform.position = minusOrigin.position;
			
			// Gets the lerp value for the current time.
			string newValue = LerpInt(vc.GetOriginalValue(), vc.GetTargetValue(), vc.GetTime()-1.5f).ToString();
			
			// Updates the big fame change value.
			fameTexts[0].text = newValue;
			
			// Updates the player's fame value.
			vc.GetParent().GetChild(1).GetComponent<Text>().text = newValue;
		}
		
		// Shrink the fame parent.
		if (time > 3f) {
			fameParent.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, (time - 3) * 2.5f);
		}
		
		// Hides the fame parent.
		if (time >= 3.5f) {
			fameParent.SetActive(false);
		}
	}
				
	#endregion
	
	// Update is called once per frame
	void Update ()
	{
		// TESTING
		if (Input.GetKeyDown(KeyCode.V)) {
			valuesToChange.Add(new PlayerGUIValueChange(0, 3, 20, Random.Range(-20,1)));
			valuesToChange.Add(new PlayerGUIValueChange(1, 3, 20, Random.Range(-20,1)));
			valuesToChange.Add(new PlayerGUIValueChange(2, 3, 20, Random.Range(-20,1)));
			valuesToChange.Add(new PlayerGUIValueChange(3, 3, 20, Random.Range(-20,1)));
			
			valuesToChange.Add(new PlayerGUIValueChange(0, 4, 5, Random.Range(-5,1)));
			valuesToChange.Add(new PlayerGUIValueChange(1, 4, 5, Random.Range(-5,1)));
			valuesToChange.Add(new PlayerGUIValueChange(2, 4, 5, Random.Range(-5,1)));
			valuesToChange.Add(new PlayerGUIValueChange(3, 4, 5, Random.Range(-5,1)));
			
			valuesToChange.Add(new PlayerGUIValueChange(0, 5, Random.Range(0,200), Random.Range(-100,100)));
			//valuesToChange.Add(new PlayerGUIValueChange(1, 5, Random.Range(0,200), Random.Range(-100,100)));
			valuesToChange.Add(new PlayerGUIValueChange(2, 5, Random.Range(0,200), Random.Range(-100,100)));
			//valuesToChange.Add(new PlayerGUIValueChange(3, 5, Random.Range(0,200), Random.Range(-100,100)));
			
			valuesToChange.Add(new PlayerGUIValueChange(0, 6, Random.Range(0,200), Random.Range(-100,100)));
			//valuesToChange.Add(new PlayerGUIValueChange(1, 6, Random.Range(0,200), Random.Range(-100,100)));
			//valuesToChange.Add(new PlayerGUIValueChange(2, 6, Random.Range(0,200), Random.Range(-100,100)));
			valuesToChange.Add(new PlayerGUIValueChange(3, 6, Random.Range(0,200), Random.Range(-100,100)));
		}
		
		if (Input.GetKeyDown(KeyCode.B)) {
			// Random number of canvases.
			int players = Random.Range(1,5);
			int[] icons = new int[4];
			
			for (int i = 0; i < icons.GetLength(0); ++i) {
				icons[i] = Random.Range(0,4);
			}
			
			Setup(players, icons);
		}
	
		// Checks if there are any details that need changing.
		if (valuesToChange.Count > 0) {
			ChangeValues();
		}
	}
}
