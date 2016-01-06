using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayHotseat()
	{
		// Dice Rolling Scene
		Application.LoadLevel(2);
	}

	public void GoToMulti()
	{
		Application.LoadLevel(1);
	}

	public void Quit()
	{
		// exit app, ignored in editor
		Application.Quit();
	}

	public void ReturnToMenu()
	{
		Application.LoadLevel(0);
	}
}
