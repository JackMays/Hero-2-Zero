using UnityEngine;
using System.Collections;

public class PauseGUI : MonoBehaviour
{
	#region Variables
	// Holds the buttons needed to pause.
	KeyCode pauseButton = KeyCode.Escape;
	
	// The pause canvas.
	public GameObject pauseCanvas;
	
	// Holds if the game has been paused or not.
	bool isPaused = false;
	
	#endregion
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Unpauses the game.
	public void Unpause()
	{
		// Starts time running.
		Time.timeScale = 1;
		
		// Hides the pause canvas.
		pauseCanvas.SetActive(false);
		
		// Sets the game is unpaused.
		isPaused = false;
	}
	
	// Pauses the game.
	void Pause()
	{
		// Stops time.
		Time.timeScale = 0;
		
		// Shows the pause canvas.
		pauseCanvas.SetActive(true);
		
		// Sets thatt he game is paused.
		isPaused = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Checks if the game has paused or not.
		if (isPaused) {
			
		}
		
		// Checks if the pause button ha been pressed.
		if (Input.GetKeyDown(pauseButton)) {
			// Checks if the game ha been paused.
			if (isPaused) {
				// Unpauses the game.
				Unpause();
			}
			else {
				// Pauses the game.
				Pause();
			}
		}
	}
}
