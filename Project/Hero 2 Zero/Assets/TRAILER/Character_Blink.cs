using UnityEngine;
using System.Collections;

public class Character_Blink : MonoBehaviour
{
	#region Variables
	
	// The renderer that shows the eye texture.
	public Renderer eyes;
	
	// The rendered that shows the mouth.
	public Renderer mouth;
	
	// The list of eye textures.
	public Texture[] eyeTextures;
	
	// The list of mouth textures.
	public Texture[] mouthTextures;
	
	// Time since last update.
	float lerpTime = 0;
	
	// Whether the character is blinking or not.
	public bool isBlinking = false;

	// Current stage of blinking.
	int blinkStage = 0;
	
	// Whether the player has 2 eye colours.
	public bool has2Colours = false;
	
	#endregion
	
	#region Functions
	// Changes the eye textures to the texture at the given index.
	public void ChangeEyes(int i)
	{
		// Checks if the character has 2 eye colours.
		if (has2Colours) {
			eyes.materials[0].mainTexture = eyeTextures[i];
			eyes.materials[1].mainTexture = eyeTextures[i+5];
		}
		else {
			eyes.material.mainTexture = eyeTextures[i];
		}
	}
	
	// Changes the mouth texture to the texture at the given index.
	public void ChangeMouth(int i)
	{
		mouth.material.mainTexture = mouthTextures[i];
	}
	
	// Sets default values for blinking.
	public void StartBlinking()
	{
		lerpTime = 0;
		blinkStage = 0;
		isBlinking = true;
	}
	
	// Runs through the blinking textures once.
	void Blink()
	{
		// Waits a little before changing image.
		if (lerpTime >= 0.035f) {
			// If the blink is finished, return to normal texture.
			if (blinkStage == 4) {
				eyes.material.mainTexture = eyeTextures[0];
				blinkStage = 0;
				lerpTime = 0;
				isBlinking = false;
				return;
			}
			
			// If the eye is closed, open it halfway.
			if (blinkStage == 3) {
				eyes.material.mainTexture = eyeTextures[2];
			}
			// Close the eye.
			else {
				eyes.material.mainTexture = eyeTextures[1 + blinkStage];
			}
			
			// Reset time for next image change.
			lerpTime = 0;
			
			// Advance the blinking stage.
			++blinkStage;
		}
	}
	
	// Runs through the blinking textures for both eyes.
	void Blink2()
	{
		// Waits a little before changing image.
		if (lerpTime >= 0.035f) {
			// If the blinking is finished, return to normal texture.
			if (blinkStage == 4) {
				eyes.materials[0].mainTexture = eyeTextures[0];
				eyes.materials[1].mainTexture = eyeTextures[5];
				blinkStage = 0;
				lerpTime = 0;
				isBlinking = false;
				return;
			}
			
			// If the eye is closed, open it halfway.
			if (blinkStage == 3) {
				eyes.materials[0].mainTexture = eyeTextures[2];
				eyes.materials[1].mainTexture = eyeTextures[6];
			}
			// Close the eye.
			else {
				eyes.materials[0].mainTexture = eyeTextures[1 + blinkStage];
				eyes.materials[1].mainTexture = eyeTextures[1 + blinkStage + 5];
			}
			
			// Reset time for next image change.
			lerpTime = 0;
			
			// Advance the blinking stage.
			++blinkStage;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*
		if (Input.GetKey(KeyCode.A) && !isBlinking) {
			isBlinking = true;
		}
	
		if (Input.GetKeyDown(KeyCode.S)) {
			ChangeEyes(4);
		}
		
		if (Input.GetKeyDown(KeyCode.D)) {
			ChangeEyes(3);
		}
		
		if (Input.GetKeyDown(KeyCode.Q)) {
			ChangeMouth(0);
		}
		
		if (Input.GetKeyDown(KeyCode.W)) {
			ChangeMouth(1);
		}
		
		if (Input.GetKeyDown(KeyCode.E)) {
			ChangeMouth(2);
		}
		
		if (Input.GetKeyDown(KeyCode.R)) {
			ChangeMouth(3);
		}
		*/
		// Checks if the character is blinking.
		if (isBlinking) {
			// Advance time to change image.
			lerpTime += Time.deltaTime;
		
			// Checks if the character has 2 different eye colours.
			if (has2Colours) {
				
				Blink2();
			}
			else {
		
				Blink();
			}
		}
	}
	
	#endregion
}
