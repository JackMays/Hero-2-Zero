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
	float camCombatPlayerYOffset = 3.5f;
	
	public Transform bananaPosition;

	// Use this for initialization
	void Start () 
	{
		mainCam = GameObject.FindWithTag("MainCamera");

		initRot = mainCam.transform.localRotation;

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

	Quaternion newRot;
	Quaternion initRot;
	bool setRot = false;
	
	void SetNewRotation()
	{
		// Holds the current rotation of the camera.
		Quaternion tempRot = mainCam.transform.localRotation;

		// Makes the camera face the player's left.
		mainCam.transform.forward = currentPlayer.transform.right;
		
		// Rotates the camera to face downwards a little.
		mainCam.transform.Rotate (new Vector3(35,0,0), Space.Self);
		
		// Stores the current camera rotation as the target.
		newRot = mainCam.transform.localRotation;
		
		// Resets camera to original rotation.
		mainCam.transform.localRotation = tempRot;
		
		// For the if else.
		setRot = true;
	}

	void UpdateCamera()
	{
		Vector3 playerPos = currentPlayer.transform.localPosition;
		float camSpeed = 2.0f;
		
		//Debug.Log(playerPos);
		
		if (combatView)
		{
			Vector3 offsetPlayerPos = playerPos + (-currentPlayer.transform.right * camCombatViewOffset);
			offsetPlayerPos.y = camCombatPlayerYOffset;
			// This should be in and if else but the lerp didnt allow it.
			SetNewRotation();			
			
			Debug.Log(offsetPlayerPos);
			
			if (mainCam.transform.localPosition != offsetPlayerPos)
			{	
				mainCam.transform.localPosition = Vector3.Lerp (mainCam.transform.localPosition, 
				                                                offsetPlayerPos, 
				                                                Time.deltaTime * camSpeed);
				                                                
				mainCam.transform.localRotation = Quaternion.Lerp(mainCam.transform.localRotation, newRot, Time.deltaTime * camSpeed);
			}
		}
		else
		{
			if (mainCam.transform.localPosition != playerPos)
			{
				//mainCam.transform.localPosition = new Vector3 (playerPos.x, playerPos.y + 5, playerPos.z - 5);
				
				mainCam.transform.localPosition = Vector3.Lerp (mainCam.transform.localPosition, 
				                                                new Vector3 (playerPos.x, playerPos.y + 5, playerPos.z - 5), 
				                                                Time.deltaTime * camSpeed);

				mainCam.transform.localRotation = Quaternion.Lerp(mainCam.transform.localRotation, initRot, Time.deltaTime * camSpeed);
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
