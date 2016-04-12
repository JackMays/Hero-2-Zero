using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	#region Variables
	// Typical player values.
	int health = 20;
	int maxHealth = 20;
	int strength = 5;
	int defence = 5;
	int gold = 10;
	int fame = 200;

	int deathSkipCap = 3;
	int turnSkipCap;
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
	public Vector2 mapPosition = new Vector2(0,0);

	Animator animatorCompo;
	
	// Holds whetehr the player is currently moving.
	bool isMoving = false;
	
	// The target position in 3D space where the player has to move to.
	Vector3 moveTarget;
	
	// Stores the tile position the player WAS standing on before moving to a new tile.
	Vector3 startJumpPosition;
	
	// Timer for steady movement between tiles.
	float moveTime = 0;
	
	// Whether the player is travelling between tiles or not.
	bool movingTile = false;
	
	// Holds whether the player has just stopped moving to a tile.
	bool justStopped = false;

	bool justFought = false;

	bool wasDead = false;
	
	// List of the item cards the player has.
	List<ItemCard> items = new List<ItemCard>();

	WeaponCard equippedWeapon = null;
	
	// Number of dice the player can throw.
	public int numDice = 2;
	
	// Holds whether the player can skip the next monster they encounter.
	bool skipMonster = false;
	
	// Holds whether the player is a villain or not.
	bool isVillain = false;
	
	// Stores the finish values for moving.
	Vector2 finishPosition = new Vector2();

	int finishDirection = 0;

	int idleID = Animator.StringToHash("isIdle");
	int walkID = Animator.StringToHash("isWalking");
	int combIdleID = Animator.StringToHash("isCombIdle");
	int combAttackID = Animator.StringToHash("isAttacking");
	int combWinID = Animator.StringToHash("isVictory");
	int combLoseID = Animator.StringToHash("isDefeated");
	int proneID = Animator.StringToHash("isProne");
	int getUpID = Animator.StringToHash("isGetUp");

	// Reference to the weapon so it can be hidden and shown.
	public List<GameObject> weapons;
	
	public Rigidbody chainEnd;
	
	#endregion
	
	// Use this for initialization
	void Awake ()
	{
		turnSkipCap = deathSkipCap;
		turnSkipCount = turnSkipCap;
		finishPosition = mapPosition;
		finishDirection = direction;

		animatorCompo = GetComponent<Animator>();

		equippedWeapon = new WeaponCard("Mace", -2, 5, 2, 0, "Mace for clubbing things", 0);

		items.Add (new ItemCard("Skip Monster (In Battle)", 0, 4, 0, 1, 0, 0, "Skip a Monster. Battle Only", 0));
		items.Add (new ItemCard("Summon Monster (On Board)", 0, 5, 0, 2, 5, 0, "Spawn a Monster on a tile. Board Only", 0));
		items.Add (new ItemCard("Player Turn Skip (On Board)", 0, 6, 1, 2, 4, 0, "Skip a Target Players turn. Board Only", 0));
		items.Add (new ItemCard("Money for all (On Board)", 0, 1, 100, 2, 3, 0, "MONEH!. Board Only", 0));
		items.Add (new ItemCard("Other Player HP loss (On Board)", 0, 2, -20, 2, 2, 0, "Digi... Nuke! cept active player. Board Only", 0));
	}
	
	#region Movement
	
	// Stores the spaces for the player to move and allows the player to move.
	public void StartMovement(int move)
	{
		movement = move;
		isMoving = true;
		justStopped = false;
	}
	
	// Sets the player up to move to the next tile.
	public void MoveTile()
	{		
		// Stores the current position in 3D space.
		startJumpPosition = new Vector3(map.TILEGAP * mapPosition.x, transform.position.y, 0 - (map.TILEGAP * mapPosition.y));
		
		// Find the direction the player needs to move in.
		FindDirection ();
		
		// Find the position the player needs to move to.
		FindMoveTarget();
		
		Move ();
		
		Debug.Log(chainEnd);
		if (chainEnd != null) {
			Debug.Log("FORCE");
			chainEnd.AddForce(-transform.forward*1000);
		}
		
		// Sets that the player is now moving.
		movingTile = true;
		justStopped = false;		
	}
	
	// Moves the player over time to the target space.
	public void Move()
	{		
		// Updates timer with time since last update.
		moveTime += Time.deltaTime;
		
		// 1 second to finish movement.
		transform.localPosition = Vector3.Lerp(startJumpPosition, moveTarget, moveTime / moveSpeed);
		
		Debug.Log(transform.localPosition);
		
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
			
			// Sets that the player has reached the target tile.
			movingTile = false;
			justStopped = true;
		}
	}
	
	// Finds the new direction for the player to move in.
	void ChangeDirection(bool checkForward)
	{
		// Checks if the player needs to look up/down or left/right.
		if (checkForward) {
			// Checks if the tile below is viable.
			if (map.GetTile((int)mapPosition.x, (int)mapPosition.y + 1) != 0) {
				direction = 2;
				transform.LookAt(transform.position + Vector3.back);
			}
			// Checks if the tile above is viable.
			if (map.GetTile((int)mapPosition.x, (int)mapPosition.y - 1) != 0) {
				direction = 0;
				transform.LookAt(transform.position + Vector3.forward);
			}
		}
		else {
			// Checks if the tile to the left is viable.
			if (map.GetTile((int)mapPosition.x - 1, (int)mapPosition.y) != 0) {
				direction = 3;
				transform.LookAt(transform.position + Vector3.left);
			}
			// Checks if the tile to the right is viable.
			if (map.GetTile((int)mapPosition.x + 1, (int)mapPosition.y) != 0) {
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
			if (map.GetTile((int)mapPosition.x, (int)mapPosition.y - 1) == 0) {
				ChangeDirection(false);
			}
		}
		else if (direction == 1) {
			if (map.GetTile((int)mapPosition.x + 1, (int)mapPosition.y) == 0) {
				ChangeDirection(true);
			}
		}
		else if (direction == 2) {
			if (map.GetTile((int)mapPosition.x, (int)mapPosition.y + 1) == 0) {
				ChangeDirection(false);
			}
		}
		else if (direction == 3) {
			if (map.GetTile((int)mapPosition.x - 1, (int)mapPosition.y) == 0) {
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
		
		// Holds the target to move to.
		Vector3 mTarget = new Vector3();
		
		if (direction == 0) {
			moveTarget = new Vector3(map.TILEGAP * mapPosition.x, transform.position.y, map.TILEGAP * (0 - (mapPosition.y-1)));
			mapPosition.y -= 1;
		}
		else if (direction == 1) {
			moveTarget = new Vector3(map.TILEGAP * (mapPosition.x+1), transform.position.y, 0 - (map.TILEGAP * mapPosition.y));
			mapPosition.x += 1;
		}
		else if (direction == 2) {
			moveTarget = new Vector3(map.TILEGAP * mapPosition.x, transform.position.y, map.TILEGAP * (0 - (mapPosition.y+1)));
			mapPosition.y += 1;
		}
		else if (direction == 3) {
			moveTarget = new Vector3(map.TILEGAP * (mapPosition.x-1), transform.position.y, 0 - (map.TILEGAP * mapPosition.y));
			mapPosition.x -= 1;
		}
	}
	
	#endregion
	
	#region Death
	// slight variation of Check Dead to allow for bonus fame loss on combat defeat
	public void HandleCombDeath(int floss)
	{
		ChangeFame(floss);
		CheckDead ();
	}
	
	// Resets the number of dice to be thrown back to default.
	public void ResetDiceCount()
	{
		numDice = 2;
	}
	
	// Checks whether the player has lost enough health to die.
	void CheckDead()
	{
		// Checks if health is below 0.
		if (health <= 0) {
			health = 0;
			ChangeTurnsToSkip(deathSkipCap);
			wasDead = true;
			//Prone();
		}
	}
	
	#endregion
	
	#region Skip Movement
	
	// This will find the tile the player will stop moving on.
	public void FindFinish()
	{
		// Makes a copy of movement, current position, current target and direction.
		int m = movement;
		Vector2 p = mapPosition;
		Vector2 t = moveTarget;
		int d = direction;
		
		
		// Loops until can move no more.
		while (m > 0) {
			// Finds the direction to move in.
			FindDirection();
			
			// Updates the map position.
			FindMoveTarget();
			
			// Decrements the number of tiles to move.
			--m;
		}
		
		// Stores the finish position and direction.
		finishPosition = mapPosition;
		finishDirection = direction;
		
		// Resets the original position, target and direction.
		mapPosition = p;
		moveTarget = t;
		direction = d;
		
		// Rotates the player back to original orientation.
		RotatePlayer(direction);
	}
	
	// Skips the player straight to the last tile.
	public void SkipToFinish()
	{
		//Debug.Log("mapPosition: " + finishPosition + ", Direction: " + finishDirection);
		
		// Sets the values to the finished values.
		mapPosition = finishPosition;
		direction = finishDirection;
		
		// Rotates the player to face the correct way.
		RotatePlayer(direction);
		
		// Moves the player in world space.
		transform.localPosition = new Vector3(mapPosition.x * map.TILEGAP, transform.position.y, 0 - (mapPosition.y * map.TILEGAP));
		
		// Sets movement to 0 and stops the player.
		movement = 0;
		moveTime = 0;
		isMoving = false;
		movingTile = false;
		justStopped = true;
	}

	public void Idle()
	{
		// reset everything when returning to idle
		if (animatorCompo)
		{
			animatorCompo.SetBool(walkID, false);
			animatorCompo.SetBool(combIdleID, false);
			animatorCompo.SetBool(combAttackID, false);
			animatorCompo.SetBool(combWinID, false);
			animatorCompo.SetBool(combLoseID, false);
			animatorCompo.SetBool(proneID, false);
			animatorCompo.SetBool(getUpID, false);
			animatorCompo.SetBool(idleID, true);
		}
	}

	public void Walk()
	{
		if (animatorCompo)
		{
			animatorCompo.SetBool(idleID, false);
			animatorCompo.SetBool(walkID, true);
		}
	}

	public void CombatIdle()
	{
		if (animatorCompo)
		{
			animatorCompo.SetBool(idleID, false);
			animatorCompo.SetBool(combIdleID, true);
		}
	}

	public void Attack()
	{
		if (animatorCompo)
		{
			animatorCompo.SetBool(combIdleID, false);
			animatorCompo.SetBool(combAttackID, true);
		}
	}
	// for anim event at the end of attack
	// Switch back to combatidle and flag an attack so combat state can move forward
	public void AttackEnd()
	{
		if (animatorCompo)
		{
			animatorCompo.SetBool(combAttackID, false);
			animatorCompo.SetBool(combIdleID, true);
		}

		justFought = true;
	}

	// for anim event at the end of attack
	// Switch back to combatidle and flag an attack so combat state can move forward
	public void DefeatEnd()
	{
		Prone();
		
		justFought = true;
	}

	public void Victory()
	{
		if (animatorCompo)
		{
			animatorCompo.SetBool(combWinID, true);
		}
	}

	public void Defeat()
	{
		if (animatorCompo)
		{
			animatorCompo.SetBool(combLoseID, true);
		}
	}

	public void Prone()
	{
		if (animatorCompo)
		{

			animatorCompo.SetBool(proneID, true);
		}
	}

	public void StandUp()
	{
		if (animatorCompo)
		{
			Debug.Log("Stand Up");

			animatorCompo.SetBool(getUpID, true);
		}
	}

	public void ResetPlayerCombat()
	{
		justFought = false;
	}
	
	// Rotates the player to a specified direction.
	void RotatePlayer(int dir)
	{
		// Checks which direction the player needs to face.
		switch (dir) {
			// Up.
		case 0:
			transform.LookAt(transform.position + Vector3.forward);
			break;
			// Right.
		case 1:
			transform.LookAt(transform.position + Vector3.right);
			break;
			// Down.
		case 2:
			transform.LookAt(transform.position + Vector3.back);
			break;
			// Left.
		case 3:
			transform.LookAt(transform.position + Vector3.left);
			break;			
		}
	}
	
	#endregion
	
	#region Changers
	
	// Changes the fame based on passed value.
	public void ChangeFame(int f)
	{
		Debug.Log("Fame has been changed by " + f + ". Fame was " + fame + ", and is now " + (fame + f));
		
		fame += f;

		if (fame <= 0)
		{
			Application.LoadLevel(3);
		}
	}
	
	// Changes the health based on passed value.
	public void ChangeHealth(int h)
	{
		Debug.Log("Health has been changed by " + h + ". Health was " + health + ", and is now " + (health + h));
	
		health += h;
		
		// Checks if the health went over the maximum.
		if (health > maxHealth) {
			health = maxHealth;
		}
		
		// Checks if the player died.
		CheckDead();
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
		
		//Defeat();

		health -= dmg;
		
		Debug.Log ("Health: " + health);
		
		if (health < 0)
		{
			health = 0;
		}
	}

	public void DecayWeaponDurability(int value)
	{
		equippedWeapon.ChangeDurability(value);

		if (equippedWeapon.GetDurability() == 0)
		{
			Debug.Log ("No more weapon");
			equippedWeapon = null;
		}
	}
	
	// Changes the gold based on passed value.
	public void ChangeGold(int g)
	{
		Debug.Log("Gold has been changed by " + g + ". Gold was " + gold + ", and is now " + (gold + g));
	
		gold += g;
	}
	
	// Changes the number of dice the player can throw.
	public void ChangeDice(int d)
	{
		numDice += d;
	}
	
	// Checks whether an item is to be added or removed and then does the appropriate action.
	public void ChangeItems(ItemCard c, bool add)
	{
		// Checks if the item is to be added.
		if (add) {
			// Adds the item.
			items.Add(c);
		}
		else {
			// Removes the item.
			items.Remove(c);
		}
	}
	
	// Changes the number of turns the player has to skip.
	public void ChangeTurnsToSkip(int t)
	{	

		Debug.Log(t.ToString());

		/*turnSkipCount += t;
		
		// Keeps number of skipped turns under cap.
		if (turnSkipCount > turnSkipCap) {
			turnSkipCount = turnSkipCap;
		}*/

		turnSkipCap = t;
		turnSkipCount = 0;
		
		// Keeps number of turns to skip out of negative.
		if (turnSkipCount < 0) {
			turnSkipCount = 0;
		}
	}
	
	#endregion
	
	#region Setters
	
	// Sets whether the player is moving.
	public void SetMoving(bool m)
	{
		isMoving = m;
	}
	
	// Sets whether the player can skip the next monster.
	public void SetSkipMonster(bool s)
	{
		skipMonster = s;
	}
	
	// Sets whether the player just stopped.
	public void SetJustStopped(bool b)
	{
		justStopped = b;
	}
	
	// Sets the direction of the player and rotates to face that way.
	public void SetDirection(int d)
	{
		// Sets the direction.
		direction = d;
		
		// Face up.
		if (d == 0) {
			transform.LookAt(transform.position + Vector3.forward);
			return;
		}
		
		// Face right.
		if (d == 1) {
			transform.LookAt(transform.position + Vector3.right);
			return;
		}
		
		// Face down.
		if (d == 2) {
			transform.LookAt(transform.position + Vector3.back);
			return;
		}
		
		// Face left.
		transform.LookAt(transform.position + Vector3.left);
	}

	public void SetEquippedWeapon(WeaponCard weapon)
	{
		equippedWeapon = new WeaponCard(weapon);
	}
	
	// Sets whether the player is a villain or not.
	public void SetVillain(bool v)
	{
		isVillain = v;
	}	
	
	// Switches whether the weapon should be shown or not.
	public void ShowWeapon(int show)
	{
		// Checks if the weapon is to be shown.
		if (show == 1) {
			foreach(GameObject go in weapons) {
				go.SetActive(true);
			}	
		}
		else {
			foreach(GameObject go in weapons) {
				go.SetActive(false);
			}	
		}
	}
	
	#endregion
	
	#region Getters
	
	// Returns whether the player is moving.
	public bool GetMoving()
	{
		return isMoving;
	}	
	
	// Returns current movement.
	public int GetMovement()
	{
		return movement;
	}
	
	// Returns the current health.
	public int GetHealth()
	{
		return health;
	}
	
	// Return attack value
	public int GetStrength()
	{
		if (equippedWeapon != null)
		{
			Debug.Log(" returned strength " + " + " + equippedWeapon.GetAttack() + " = " + (strength + equippedWeapon.GetAttack()));

			return strength + equippedWeapon.GetAttack();
		}
		else
		{
			Debug.Log ("returned base str");
			return strength;
		}
	}

	public int GetFame()
	{
		return fame;
	}
	
	// Returns the player's position in map space.
	public Vector2 GetMapPosition()
	{
		return mapPosition;
	}

	public Vector3 GetCombatDirection()
	{
		if (direction == 0)
		{
			return Vector3.back;
		}
		else if (direction == 1)
		{
			return Vector3.left;
		}
		else if (direction == 2)
		{
			return Vector3.forward;
		}
		else if (direction == 3)
		{
			return Vector3.right;
		}


		return Vector3.zero;
	}
	
	// Gets the number of dice the player can roll.
	public int GetDice()
	{
		return numDice;
	}

	public int GetItemHandLimit()
	{
		return items.Count;
	}

	public ItemCard GetCurrentItem(int index)
	{
		return items[index];
	}

	public WeaponCard GetWeapon()
	{
		return equippedWeapon;
	}
	
	// Returns whether the player is travelling between tiles.
	public bool IsMovingBetweenTiles()
	{
		return movingTile;
	}
	
	// Gets whether the player just stopped.
	public bool JustStoppedMoving()
	{
		return justStopped;
	}

	public bool HasFightAnimFinished()
	{
		return justFought;
	}

	public bool HasIdleState()
	{
		return animatorCompo.GetBool(idleID);
	}

	public bool HasProneState()
	{
		return animatorCompo.GetBool(proneID);
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
			// if skipping turns due to death
			// restore hp at the end
			if (wasDead)
			{
				ChangeHealth(maxHealth);
				wasDead = false;
			}
			if (!HasIdleState())
			{
				StandUp();
			}

			return true;
		}
	}

	public bool HasSkippedMonster()
	{
		return skipMonster;
	}
	
	// Gets whether the player is a villain or not.
	public bool GetVillain()
	{
		return isVillain;
	}
	
	#endregion
	
	// Update is called once per frame
	void Update ()
	{

	}
}