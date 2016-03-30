using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	GameObject mainCam;

	GameObject currentPlayer = null;
	
	Vector3 camMapViewPos;
	Vector3 camCombatViewPos = Vector3.zero;

	bool mapView = false;
	bool combatView = false;
	
	int camState = 0;

	float camCombatViewOffset = 3.0f;
	float camCombatPlayerYOffset = 2.5f;
	
	public Transform bananaPosition;

	// Use this for initialization
	void Start () 
	{
		mainCam = GameObject.FindWithTag("MainCamera");

		camMapViewPos = mainCam.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			//mapView = !mapView;
			
			/*if (camState == 2) {
				camState = 0;
			}
			else {
				++camState;
			}*/

			combatView = !combatView;
		}
		
		if (camState == 1)
		{
			if (mainCam.transform.localPosition != camMapViewPos)
			{
				mainCam.transform.localPosition = camMapViewPos;
			}
		}
		else if (camState == 2) {
			mainCam.transform.localPosition = bananaPosition.localPosition;
		}
		else
		{
			UpdateCamera();
		}	
	}

	void UpdateCamera()
	{
		Vector3 playerPos = currentPlayer.transform.localPosition;
		float camSpeed = 2.0f;
		
		//Debug.Log(playerPos);

		if (combatView)
		{
			Vector3 offsetPlayerPos = playerPos + (currentPlayer.transform.right * camCombatViewOffset);
			offsetPlayerPos.y = playerPos.y * camCombatPlayerYOffset;

			if (mainCam.transform.localPosition != offsetPlayerPos)
			{	
				mainCam.transform.localPosition = Vector3.Lerp (mainCam.transform.localPosition, 
				                                                offsetPlayerPos, 
				                                                Time.deltaTime * camSpeed);
			}

			//mainCam.transform.forward = -currentPlayer.transform.right;
		}
		else
		{
			if (mainCam.transform.localPosition != playerPos)
			{
				//mainCam.transform.localPosition = new Vector3 (playerPos.x, playerPos.y + 5, playerPos.z - 5);
				
				mainCam.transform.localPosition = Vector3.Lerp (mainCam.transform.localPosition, 
				                                                new Vector3 (playerPos.x, playerPos.y + 5, playerPos.z - 5), 
				                                                Time.deltaTime * camSpeed);
			}
		}
	}

	public bool HasCombatViewOn()
	{
		return combatView;
	}

	public void SetActivePlayer(GameObject p)
	{
		currentPlayer = p;
	}

	public void ToggleCombatView(bool toggle)
	{
		combatView = toggle;
	}
}
