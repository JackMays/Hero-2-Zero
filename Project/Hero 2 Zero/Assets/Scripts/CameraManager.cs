using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	GameObject mainCam;

	GameObject currentPlayer = null;
	
	Vector3 camMapViewPos;

	bool mapView = false;
	
	int camState = 0;
	
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
			
			if (camState == 2) {
				camState = 0;
			}
			else {
				++camState;
			}
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
		
		Debug.Log(playerPos);
		
		if (mainCam.transform.localPosition != playerPos)
		{
			//mainCam.transform.localPosition = new Vector3 (playerPos.x, playerPos.y + 5, playerPos.z - 5);
			
			mainCam.transform.localPosition = Vector3.Lerp (mainCam.transform.localPosition, 
			                                                new Vector3 (playerPos.x, playerPos.y + 5, playerPos.z - 5), 
			                                                Time.deltaTime * camSpeed);
		}
	}

	public void SetActivePlayer(GameObject p)
	{
		currentPlayer = p;
	}
}
