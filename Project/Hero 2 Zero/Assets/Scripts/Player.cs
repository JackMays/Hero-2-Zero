using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	#region Variables
	// Typical player values.
	int health = 20;
	int strength = 5;
	int defence = 5;
	int gold = 10;
	int fame = 200;

	int turnSkipCap = 3;
	int turnSkipCount;
	
	// The direction the player is moving in. 0 : up | 1 : right | 2 : down | 3 : left (Public because lazy).
	public int direction = 0;
	
	// The number of spaces the player has to move.
	int movement = 0;
	
	// Seconds taken to move between tiles.
	public float moveSpeed = 0.5f;
	
	// Reference to the map.
	public Map map;
	
	// Player's position in map coordinates. (Public for now cause I'm lazy.)
	public Vector2 position = new Vector2(0,0);
	
	// Holds whetehr the player is currently moving.
	bool isMoving = false;
	
	// The target position in 3D space where the player has to move to.
	Vector3 moveTarget;
	
	// Stores the tile position the player WAS standing on before moving to a new tile.
	Vector3 startJumpPosition;
	
	// Timer for steady movement between tiles.
	float moveTime = 0;
	
	// Whetehr the player is travelling between tiles or not.
	bool movingTile = false;
	
	#endregion
	
	// Use this for initialization
	void Awake ()
	{
		turnSkipCount = turnSkipCap;
	}
	
	// Moves the player over time to the target space.
	void Move()
	{
		// Debugs movement values.
		//Debug.Log(moveTime);
		//Debug.Log(startJumpPosition);
		//Debug.Log(moveTarget);
		
		// Updates timer with time sicne last update.
		moveTime += Time.deltaTime;
		
		// 1 second to finish movement.
		transform.localPosition = Vector3.Lerp(startJumpPosition, moveTarget, moveTime / moveSpeed);
		
		// Checks if player has finished moving.
		if (moveTime >= moveSpeed) {
			// Drecrements movement.
			--movement;
			
			// Checks if movement is finished.
			if (movement <= 0) {
				// Stops movement.
				isMoving = false;
			}
			
			// Resets the movement timer.
			moveTime = 0;
			
			// Sets that teh player has reached the target tile.
			movingTile = false;
		}
	}
	
	// Finds the new direction for the player to move in.
	void ChangeDirection(bool checkForward)
	{
		// Checks if the player needs to look up/down or left/right.
		if (checkForward) {
			// Checks if the tile below is viable.
			if (map.GetTile((int)position.x, (int)position.y - 1) != 0) {
				direction = 2;
				transform.LookAt(transform.position + Vector3.back);
			}
			// Checks if the tile above is viable.
			if (map.GetTile((int)position.x, (int)position.y + 1) != 0) {
				direction = 0;
				transform.LookAt(transform.position + Vector3.forward);
			}
		}
		else {
			// Checks if the tile to the left is viable.
			if (map.GetTile((int)position.x - 1, (int)position.y) != 0) {
				direction = 3;
				transform.LookAt(transform.position + Vector3.left);
			}
			// Checks if the tile to the right is viable.
			if (map.GetTile((int)position.x + 1, (int)position.y) != 0) {
				direction = 1;
				transform.LookAt(transform.position + Vector3.right);
			}
		}
	}
	
	// Finds the direction the player should move in. 
	void FindDirection()
	{
		// All these ifs are the same. It checks which direction the player is
		// currently moving and then checks to see if the next tile in that direction is
		// a valid target. Otherwise the player needs to change direction clockwise.
		
		if (direction == 0) {
			if (map.GetTile((int)position.x, (int)position.y + 1) == 0) {
				ChangeDirection(false);
			}
		}
		else if (direction == 1) {
			if (map.GetTile((int)position.x + 1, (int)position.y) == 0) {
				ChangeDirection(true);
			}
		}
		else if (direction == 2) {
			if (map.GetTile((int)position.x, (int)position.y - 1) == 0) {
				ChangeDirection(false);
			}
		}
		else if (direction == 3) {
			if (map.GetTile((int)position.x - 1, (int)position.y) == 0) {
				ChangeDirection(true);
			}
		}
	}
	
	// Finds the position to move to in relation to direction.
	void FindMoveTarget()
	{
		// These ifs are all the same. It checks which direction the player is moving
		// and finds the position of the next tile in that direction. It then updates
		// the player's map position.
	
		if (direction == 0) {
			moveTarget = new Vector3(2 * position.x, transform.position.y, 2 * (position.y+1));
			position.y += 1;
		}
		else if (direction == 1) {
			moveTarget = new Vector3(2 * (position.x+1), transform.position.y, 2 * position.y);
			position.x += 1;
		}
		else if (direction == 2) {
			moveTarget = new Vector3(2 * position.x, transform.position.y, 2 * (position.y-1));
			position.y -= 1;
		}
		else if (direction == 3) {
			moveTarget = new Vector3(2 * (position.x-1), transform.position.y, 2 * position.y);
			position.x -= 1;
		}
	}
	
	// Sets the player up to move to the next tile.
	void MoveTile()
	{
		// Stores the current position in 3D space.
		startJumpPosition = new Vector3(2 * position.x, transform.position.y, 2 * position.y);
		
		// Find the direction the player needs to move in.
		FindDirection ();
	
		// Find the position the player needs to move to.
		FindMoveTarget();
		
		// Sets that the player is now moving.
		movingTile = true;
	}

	public void ReplenishHealth(int hp)
	{
		// TEMP: hea between combat to resume flow
		health = hp;
	}


	
	// Returns whether the player is moving.
	public bool GetMoving()
	{
		return isMoving;
	}

	public bool HasDied()
	{
		return (health == 0);
	}

	public bool HasSkippedTurns()
	{
		if (turnSkipCount < turnSkipCap)
		{

			++turnSkipCount;
			return false;
		}
		else
		{
			ReplenishHealth(20);
			return true;
		}
	}
	
	// Returns current movement.
	public int GetMovement()
	{
		return movement;
	}

	public int GetHealth()
	{
		return health;
	}

	// Return attack value
	public int GetStrength()
	{
		return strength;
	}
	
	// Returns the player's position in map space.
	public Vector2 GetPosition()
	{
		return position;
	}
	
	// Sets whether the player is moving.
	public void SetMoving(bool m)
	{
		isMoving = m;
	}
	
	// Stores the spaces for the player to move and allows the player to move.
	public void StartMovement(int move)
	{
		movement = move;
		isMoving = true;
	}

	public void TakeDamage(int dmg)
	{
		// because fuck defense making me sit for an hour rolling dice
		// when it was eating all the damage
		/*int modifiedDmg = dmg - defence;
		if (modifiedDmg > 0)
		{
			health -= (dmg - defence);
		}*/

		health -= dmg;

		Debug.Log ("Health: " + health);

		if (health < 0)
		{
			health = 0;
		}
	}

	public void HandleDeath(int floss)
	{
		turnSkipCount = 0;
		ChangeFame(floss);
	}
	
	// Changes the fame based on passed value.
	public void ChangeFame(int f)
	{
		Debug.Log("Fame has been changed by " + f + ". Fame was " + fame + ", and is now " + (fame + f));
		
		fame += f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Checks if the player is currently moving.
		if (isMoving) {
			// Checks if the player is currently travelling between tiles.
			if (movingTile) {
				// Continues the movement.
				Move();
			}
			else {
				// Finds the next tile for the player to move to.
				MoveTile();
			}
		}
	}
}
