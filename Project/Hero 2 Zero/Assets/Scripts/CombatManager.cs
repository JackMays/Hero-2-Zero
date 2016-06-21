using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour {

	public Map map;

	Player player;
	Player player2;

	MonsterCard monster;
	MonsterAnims monAnims;

	int playerDiceRoll;
	int playerTwoDiceRoll;
	int monsterDiceRoll;

	float combatPosOffset = 0.8f;

	bool isMonCombat = false;
	bool isPvpCombat = false;

	bool isAttackPhaseExec = false;
	bool isResolveOnce = false;
	//bool isRoundResolved;
	bool isCombatEnded = false;
	// temp bool until monsters have implemented attacks
	bool MonsterAttackBypass = false;

	Vector3 oriPlayerPos = Vector3.zero;
	Vector3 oriPlayer2Pos = Vector3.zero;
	Vector3 oriMonsterPos = Vector3.zero;
	// Use this for initialization
	void Awake () 
	{
		player = null;
		player2 = null;
		monster = null;
		monAnims = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void ResolveMon()
	{
		int damage = 0;

		int tileX = (int)player.GetMapPosition().x;
		int tileY = (int)player.GetMapPosition().y;
		
		// compare dice
		if (playerDiceRoll > monsterDiceRoll)
		{
			//player wins
			damage = playerDiceRoll - monsterDiceRoll;
			Debug.Log ("Monster Takes: " + playerDiceRoll + " + " + monsterDiceRoll + " = " + damage);
			monster.TakeDamage(damage);
			player.Victory();
			player.ChangeFame(monster.GetFameMod(true));

			if (monster.HasDied())
			{
				// Remove Card from area
				Debug.Log ("Monster's HP hit 0");
				// gain more fame as bonus for win
				player.ChangeFame(monster.GetFameMod(true));
				map.ClearMonsterTile(tileX, tileY);
				map.ClearPrefabTile(tileX, tileY);
			}


		}
		else if (playerDiceRoll < monsterDiceRoll)
		{
			// monster wins
			damage = monsterDiceRoll - playerDiceRoll;
			Debug.Log ("Player Takes: " + monsterDiceRoll + " - " + playerDiceRoll + " = " + damage);
			player.TakeDamage(damage);
			player.ChangeFame(monster.GetFameMod(false));

			if (player.HasDied())
			{
				Debug.Log ("Player's HP hit 0");
				
				// remove player for allotted turns, place monster card at area
				// Also decrease fame more due to critical loss
				player.HandleCombDeath(monster.GetFameMod(false));
			}
			/*else
			{
				player.StandUp();
			}*/
		}
		else
		{
			// DRAW
			Debug.Log ("Tie");

			player.Idle();
		}
		// lower weapon durability if one exists
		if (player.GetWeapon() != null)
		{
			player.DecayWeaponDurability(-1);
		}
		// If monster is still alive from win or tie put on tile
		if (!monster.HasDied())
		{	
			map.AddMonsterToTile(tileX, tileY, monster);
			
			Debug.Log (" A " + map.GetMonsterCardOnTile(tileX, tileY).GetName() + " is here now.");
		}
		
		// if any have died, combat is over, if not its just a round
		/*if (player.HasDied() || monster.HasDied())
		{
			if (player.HasDied())
			{
				Debug.Log ("Player's HP hit 0");

				// remove player for allotted turns, place monster card at area
				// Also decrease fame more due to critical loss
				player.HandleCombDeath(monster.GetFameMod(false));
				

			}
			else
			{
				player.StandUp();
			}

			if (monster.HasDied())
			{
				// Remove Card from area
				Debug.Log ("Monster's HP hit 0");
				// gain more fame as bonus for win
				player.ChangeFame(monster.GetFameMod(true));
				map.ClearMonsterTile(tileX, tileY);
				map.ClearPrefabTile(tileX, tileY);
			}


			
		}
		else
		{
			isRoundResolved = true;
			Debug.Log("round resolved flagged");
		}*/


		


	}

	void ResolvePvp()
	{
		int damage = 0;
		
		// compare dice
		if (playerDiceRoll > playerTwoDiceRoll)
		{
			//player one wins
			damage = playerDiceRoll - playerTwoDiceRoll;
			Debug.Log ("Player 2 Takes: " + playerDiceRoll + " - " + playerTwoDiceRoll + " = " + damage);
			player2.TakeDamage(damage);
			player.Victory();

			player.ChangeFame(10);
			player2.ChangeFame(-10);

			if (player2.HasDied())
			{
				Debug.Log ("Player 2's HP hit 0");
				
				// remove losing player for allotted turns
				// gain/lose bonus fame for win & loss
				player.ChangeFame(10);
				player2.HandleCombDeath(-10);
			}
			/*else
			{
				player2.StandUp();
			}*/

			
		}
		else if (playerDiceRoll < playerTwoDiceRoll)
		{
			// player two wins
			damage = playerTwoDiceRoll - playerDiceRoll;
			Debug.Log ("Player Takes: " + playerTwoDiceRoll + " - " + playerDiceRoll + " = " + damage);
			player.TakeDamage(damage);
			player2.Victory();

			player.ChangeFame(-10);
			player2.ChangeFame(10);

			if (player.HasDied())
			{
				Debug.Log ("Player 1's HP hit 0");
				
				// remove losing player for allotted turns
				// gain/lose bonus fame for win & loss
				player.HandleCombDeath(-10);
				player2.ChangeFame(10);
				
			}
			/*else
			{
				player.StandUp();
			}*/
		}
		else
		{
			// DRAW
			Debug.Log ("Tie");

			player.Idle();
			player2.Idle();
		}

		if (player.GetWeapon() != null)
		{
			player.DecayWeaponDurability(-1);
		}

		if (player2.GetWeapon() != null)
		{
			player2.DecayWeaponDurability(-1);
		}

		// if any have died, combat is over, if not its just a round
		/*if (player.HasDied() || player2.HasDied())
		{
			if (player.HasDied())
			{
				Debug.Log ("Player 1's HP hit 0");

				// remove losing player for allotted turns
				// gain/lose bonus fame for win & loss
				player.HandleCombDeath(-10);
				player2.ChangeFame(10);

			}
			else
			{
				player.StandUp();
			}

			if (player2.HasDied())
			{
				Debug.Log ("Player 2's HP hit 0");

				// remove losing player for allotted turns
				// gain/lose bonus fame for win & loss
				player.ChangeFame(10);
				player2.HandleCombDeath(-10);
			}
			else
			{
				player2.StandUp();
			}
			
		}
		else
		{
			isRoundResolved = true;
			Debug.Log("round resolved flagged");
		}*/
		

		

	}

	public void ResolveCombat()
	{

		ExecuteAttackPhase();
		

		if (isMonCombat)
		{
			if (/*(player.HasFightAnimFinished() || MonsterAttackBypass) ||*/ 
			    (player.HasFightAnimFinished() && monAnims.HasFightAnimFinished()) ||
			    !player.GetComponent<Animator>() || !monAnims)
			{
				if (!isResolveOnce)
				{
					ResolveMon();
					isResolveOnce = true;
				}

				int tileX = (int)player.GetMapPosition().x;
				int tileY = (int)player.GetMapPosition().y;

				if (player.GetComponent<Animator>() && monAnims)
				{
					// player/monster is Idle (win/draw) or has gone to prone/dead respectively (loss)
					if ((player.HasIdleState() && monAnims.HasIdleState()) ||
					    (player.HasProneState() || monAnims.HasDeadState()) /*|| player.HasDied()*/)
					{	
						player.transform.position = oriPlayerPos;
						map.GetMonsterPrefabOnTile(tileX, tileY).transform.position = oriMonsterPos;

						// end combat
						isCombatEnded = true;
						player.ResetPlayerCombat();
					}
				}
				else
				{
					player.transform.position = oriPlayerPos;
					map.GetMonsterPrefabOnTile(tileX, tileY).transform.position = oriMonsterPos;
					isCombatEnded = true;
					player.ResetPlayerCombat();
				}
			}
		}
		else if (isPvpCombat)
		{
			if (player.GetComponent<Animator>() && player2.GetComponent<Animator>())
			{
				if (/*(player.HasFightAnimFinished() || player2.HasFightAnimFinished()) ||*/ 
				    (player.HasFightAnimFinished() && player2.HasFightAnimFinished()))
				{
					if (!isResolveOnce)
					{
						ResolvePvp();
						isResolveOnce = true;
					}
					// Both are standing (Tie), One is down (win/loss)
					if ((player.HasIdleState() && player2.HasIdleState()) || 
					    (player.HasProneState() || player2.HasProneState()) /*||
					    (player.HasDied() || player2.HasDied())*/)
					{

						player.transform.position = oriPlayerPos;
						player2.transform.position = oriPlayer2Pos;

						// end combat
						isCombatEnded = true;
						player.ResetPlayerCombat();
						player2.ResetPlayerCombat();
					}
				}
			}
			else
			{
				if (!isResolveOnce)
				{
					ResolvePvp();
					isResolveOnce = true;
				}

				player.transform.position = oriPlayerPos;
				player2.transform.position = oriPlayer2Pos;

				// end combat
				isCombatEnded = true;
				player.ResetPlayerCombat();
				player2.ResetPlayerCombat();
				
			}
		}
	}

	public void ExecuteAttackPhase()
	{
		if (isMonCombat)
		{
			if (!isAttackPhaseExec)
			{
				if (playerDiceRoll > monsterDiceRoll)
				{
					player.Attack();
					//MonsterAttackBypass = true;

				}
				else if (playerDiceRoll < monsterDiceRoll)
				{
					//MonsterAttackBypass = true;
					monAnims.Attack();
				}
				else
				{
					player.Attack();
					monAnims.Attack();
					//MonsterAttackBypass = true;
				}

				isAttackPhaseExec = true;

			}
			else
			{
				if (playerDiceRoll > monsterDiceRoll)
				{
					if (monster.HasDied())
					{
						monAnims.Dead();
					}
					else
					{
						monAnims.Hit();
					}

					// Mon defeat
				}
				else if (playerDiceRoll < monsterDiceRoll)
				{
					player.Defeat();

				}

			}
		}
		else if (isPvpCombat)
		{
			if (!isAttackPhaseExec)
			{
				// compare dice
				if (playerDiceRoll > playerTwoDiceRoll)
				{
					player.Attack();
				}
				else if (playerDiceRoll < playerTwoDiceRoll)
				{
					player2.Attack();
				}
				else
				{
					player.Attack();
					player2.Attack();
				}

				isAttackPhaseExec = true;
			}
			else
			{
				if (playerDiceRoll > playerTwoDiceRoll)
				{

					if (player.HasFightAnimFinished())
					{
						player2.Defeat();
					}
				}
				else if (playerDiceRoll < playerTwoDiceRoll)
				{
					if (player2.HasFightAnimFinished())
					{
						player.Defeat();
					}
				}

			}
		}


	}

	public void EstablishMonCombat(Player p, MonsterCard m, GameObject pr)
	{
		player = p;
		monster = m;

		isMonCombat = true;

		int tileX = (int)player.GetMapPosition().x;
		int tileY = (int)player.GetMapPosition().y;

		// if there is no model, send it to be instantiated
		if (map.HasBlankModel(tileX, tileY))
		{
			// Gets the player's world position.
			Vector3 pos = player.transform.position;
			// Removes the y coordinate.
			pos.y = 0;
			// Spawns the monster.
			map.AddPrefabToTiles(tileX, tileY, pr, pos);
		}

		GameObject monPrefab = map.GetMonsterPrefabOnTile((int)player.GetMapPosition().x, (int)player.GetMapPosition().y);

		oriPlayerPos = player.transform.position;
		oriMonsterPos = monPrefab.transform.position;

		monAnims = monPrefab.GetComponent<MonsterAnims>();

		monPrefab.transform.forward = -player.transform.forward;

		player.transform.position += player.GetCombatDirection() * combatPosOffset;
		monPrefab.transform.position += -player.GetCombatDirection() * combatPosOffset;
	}

	public void EstablishPvpCombat(Player p1, Player p2)
	{
		player = p1;
		player2 = p2;

		isPvpCombat = true;

		oriPlayerPos = player.transform.position;
		oriPlayer2Pos = player2.transform.position;

		player2.transform.forward = -player.transform.forward;
		
		player.transform.position += player.GetCombatDirection() * combatPosOffset;
		player2.transform.position += -player.GetCombatDirection() * combatPosOffset;
	}

	public void ResetCombat()
	{
		player = null;
		player2 = null;
		monster = null;

		oriPlayerPos = Vector3.zero;
		oriPlayer2Pos = Vector3.zero;
		oriMonsterPos = Vector3.zero;


		isMonCombat = false;
		isPvpCombat = false;
		isAttackPhaseExec = false;
		isResolveOnce = false;
		isCombatEnded = false;
		// make sure round is reset
		//ResetRound();
	}

	/*public void ResetRound()
	{
		isRoundResolved = false;
		playerDiceRoll = 0;
		monsterDiceRoll = 0;
	}*/

	public void SetPlayerDiceRoll(int pRoll)
	{
		playerDiceRoll = pRoll + player.GetStrength();
		Debug.Log ("Player Total: " + pRoll + " (base) + " + player.GetStrength() + " (str) = " + playerDiceRoll);
	}

	public void SetPlayerTwoDiceRoll(int p2Roll)
	{
		playerTwoDiceRoll = p2Roll + player2.GetStrength();

		Debug.Log ("Second Player Total: " + p2Roll + " (base) + " + player2.GetStrength() + " (str) = " + playerTwoDiceRoll);
	}

	public void SetMonsterDiceRoll(int mRoll)
	{

		monsterDiceRoll = mRoll + monster.GetStrength();
		Debug.Log ("Monster Total: " + mRoll + " (base) + " + monster.GetStrength() + " (str) = " + monsterDiceRoll);
	}

	public void ForceCombatEnd()
	{
		int tileX = (int)player.GetMapPosition().x;
		int tileY = (int)player.GetMapPosition().y;

		map.AddMonsterToTile(tileX, tileY, monster);
		
		Debug.Log (" A " + map.GetMonsterCardOnTile(tileX, tileY).GetName() + " is here now CAUSE SKIP.");

		player.transform.position = oriPlayerPos;
		map.GetMonsterPrefabOnTile(tileX, tileY).transform.position = oriMonsterPos;

		isCombatEnded = true;
	}

	/*public bool HasRoundResolved()
	{
		return isRoundResolved;
	}*/

	public bool HasMonCombat()
	{
		return isMonCombat;
	}
	public bool HasPlayerCombat()
	{
		return isPvpCombat;
	}

	public bool HasCombatEnded()
	{
		return isCombatEnded;
	}
}
