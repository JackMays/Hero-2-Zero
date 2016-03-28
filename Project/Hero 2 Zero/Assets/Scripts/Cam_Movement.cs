using UnityEngine;
using System.Collections;

public class Cam_Movement : MonoBehaviour
{
	#region Variables
	public float speed;
	public Camera myCam;

	private float minFov = 15f;
	private float maxFov = 90f;
	
	private float zoomSens;
	
	// Holds whether the camera can move or not.
	public bool canMove = true;

	// Used to smooth the camera's movement.
	Vector3 velocity = Vector3.zero;

	// Holds the terrain layer.
	public LayerMask terrainMask;

	// The speed at which the camera will move when dragging.
	public float dragSpeedVelocity = 10;
	public float dragSpeedTranslate = 1;

	// Holds the original position of the mouse for dragging.
	Vector3 mousePos;

	// Holds whether the camera will come to a gradual stop or not when dragging.
	public bool gradualStop = true;
	#endregion

	void Start () {
		speed = 10f;
		zoomSens = 20f;
	}
	
	// Checks if the camer ahas moved out of the bounds.
	void checkCameraBounds() {
		// Holds the positions to move the camera back to.
		float newPosX = transform.position.x;
		float newPosZ = transform.position.z;
		
		// Checks if it's gone too far left.
		if (transform.position.x < 25) {
			newPosX = 25;
		}
		// Checks if too far right.
		else if (transform.position.x > 65) {
			newPosX = 65;
		}
		
		// Checks if it's gone too far forward.
		if (transform.position.z > -15) {
			newPosZ = -15;
		}
		// Checks if too far back.
		else if (transform.position.z < -45) {
			newPosZ = -45;
		}
		
		// Moves the camera.
		transform.position = new Vector3(newPosX, transform.position.y, newPosZ);
	}
	
	// Checks if the camera has moved out of the bounds.
	void CheckCameraBounds2()
	{
		// Holds the position of the camera.
		Vector3 newPos = transform.position;
		
		// Checks if too far left.
		if (newPos.x < 25) {
			newPos.x = 25;
		}
		else {
			// Only checks right if not too far left.
			if (newPos.x > 65) {
				newPos.x = 65;
			}
		}
		
		// Checks if too far forward.
		if (newPos.z > -15) {
			newPos.z = -15;
		}
		else {
			// Only checks back if not far forward.
			if (newPos.z < -45) {
				newPos.z = -45;
			}
		}
		
		// Sets the camera pos to the new position.
		transform.position = newPos;
	}

	// Gets the mouse's position in world space.
	Vector3 getMousePosition()
	{
		// Construct a ray from the current mouse coordinates
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		// Holds the data of where the ray hit the terrain.
		RaycastHit hit = new RaycastHit();
		
		// Casts a ray at the terrain.
		if (Physics.Raycast (ray, out hit, 100f, terrainMask)) {
			// Sets the position of where the ray hit.
			return hit.point;
		}

		// Returns an empty Vector3 as default.
		return Vector3.zero;
	}

	// Checks the state of dragging the screen.
	void checkDragScreen()
	{
		// Checks if the Middle mouse button has just been pressed.
		if (Input.GetMouseButtonDown(2)) {
			// Get the current mouse poisition.
			mousePos = Input.mousePosition;
		}

		float dragSpeed = 0;

		// Alters dragSpeed depending on using physics or not.
		if (gradualStop) {
			dragSpeed = dragSpeedVelocity;
		}
		else {
			dragSpeed = dragSpeedTranslate;
		}

		// Checks if the middle mouse button is still being pressed.
		if (Input.GetMouseButton(2)) {
			// Gets the new position of the mouse offset by the original position in world space.
			Vector3 pos = myCam.ScreenToViewportPoint(Input.mousePosition - mousePos);

			// Creates a vector with which to move the camera.
			Vector3 move = new Vector3(pos.x * -dragSpeed, 0, pos.y * -dragSpeed);

			// Checks if the the camera should gradually stop or flat stop.
			if (gradualStop) {
				// Uses velocity and physics to move the camera.
				GetComponent<Rigidbody>().velocity = move;
			}
			else {
				// Moves the camera through set position.
				transform.Translate(move, Space.World);
			}
		}
	}

	void Update ()
	{
		if (canMove)
		{
			// Allows the camera to be moved with middle mouse.
			checkDragScreen();

			// Move camera with WASD.
			if (Input.GetKey (KeyCode.W)) {
				transform.Translate (Vector3.forward * speed * Time.deltaTime, Space.World);
			}
			
			if (Input.GetKey (KeyCode.S)) {
				transform.Translate (Vector3.forward * -speed * Time.deltaTime, Space.World);		
			}
			if (Input.GetKey (KeyCode.A)) {
				transform.Translate (Vector3.right * -speed * Time.deltaTime, Space.World);		
			}
			if (Input.GetKey (KeyCode.D)) {
				transform.Translate (Vector3.right * speed * Time.deltaTime, Space.World);		
			}
			
			// Used for controllers.
			transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime, Space.World);
			transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime, Space.World);

			// Zoom.
			float fov = myCam.fieldOfView;
			fov -= Input.GetAxis ("Mouse ScrollWheel") * zoomSens;
			fov = Mathf.Clamp (fov, minFov, maxFov);
			myCam.fieldOfView = fov;
			
			// Checks if the camera has passed it's bounds.
			CheckCameraBounds2();
		}
	}
}
