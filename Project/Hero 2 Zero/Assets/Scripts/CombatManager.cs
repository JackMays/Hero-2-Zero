using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour {

	public Map map;

	Player player;
	Player player2;

	MonsterCard monster;

	int playerDiceRoll;
	int playerTwoDiceRoll;
	int monsterDiceRoll;

	bool isMonCombat;
	bool isPvpCombat;

	//bool isRoundResolved;
	bool isCombatEnded;
	// temp bool until monsters have implemented attacks
	bool MonsterAttackBypass = false;

	// Use this for initialization
	void Awake () 
	{
		player = null;
		player2 = null;
		monster = null;

		playerDiceRoll = 0;
		monsterDiceRoll = 0;

		isMonCombat = false;
		isPvpCombat = false;

		//isRoundResolved = false;
		isCombatEnded = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void ResolveMon()
	{
		int damage = 0;
		
		// compare dice
		if (playerDiceRoll > monsterDiceRoll)
		{
			//player wins
			damage = playerDiceRoll - monsterDiceRoll;
			Debug.Log ("Monster Takes: " + playerDiceRoll + " + " + monsterDiceRoll + " = " + damage);
			monster.TakeDamage(damage);
			//player.Victory();
			player.ChangeFame(monster.GetFameMod(true));


		}
		else if (playerDiceRoll < monsterDiceRoll)
		{
			// monster wins
			damage = monsterDiceRoll - playerDiceRoll;
			Debug.Log ("Player Takes: " + monsterDiceRoll + " - " + playerDiceRoll + " = " + damage);
			player.TakeDamage(damage);
			player.ChangeFame(monster.GetFameMod(false));
		}
		else
		{
			// DRAW
			Debug.Log ("Tie");
		}
		// lower weapon durability if one exists
		if (player.GetWeapon() != null)
		{
			player.DecayWeaponDurability(-1);
		}
		
		int tileX = (int)player.GetMapPosition().x;
		int tileY = (int)player.GetMapPosition().y;
		
		if (!monster.HasDied())
		{	
			map.AddMonsterToTile(tileX, tileY, monster);
			
			Debug.Log (" A " + map.GetMonsterOnTile(tileX, tileY).GetName() + " is here now.");
		}
		
		// if any have died, combat is over, if not its just a round
		if (player.HasDied() || monster.HasDied())
		{
			if (player.HasDied())
			{
				Debug.Log ("Player's HP hit 0");

				// remove player for allotted turns, place monster card at area
				// Also decrease fame more due to critical loss
				player.HandleCombDeath(monster.GetFameMod(false));
				

			}
			else if (monster.HasDied())
			{
				// Remove Card from area
				Debug.Log ("Monster's HP hit 0");
				// gain more fame as bonus for win
				player.ChangeFame(monster.GetFameMod(true));
				map.ClearMonsterTile(tileX, tileY);
				map.ClearPrefabTile(tileX, tileY);
			}


			
		}
		/*else
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
			//player.Victory();

			player.ChangeFame(10);
			player2.ChangeFame(-10);

			
		}
		else if (playerDiceRoll < playerTwoDiceRoll)
		{
			// player two wins
			damage = playerTwoDiceRoll - playerDiceRoll;
			Debug.Log ("Player Takes: " + playerTwoDiceRoll + " - " + playerDiceRoll + " = " + damage);
			player.TakeDamage(damage);
			//player2.Victory();

			player.ChangeFame(-10);
			player2.ChangeFame(10);
		}
		else
		{
			// DRAW
			Debug.Log ("Tie");
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
		if (player.HasDied() || player2.HasDied())
		{
			if (player.HasDied())
			{
				Debug.Log ("Player 1's HP hit 0");

				// remove losing player for allotted turns
				// gain/lose bonus fame for win & loss
				player.HandleCombDeath(-10);
				player2.ChangeFame(10);

			}
			else if (player2.HasDied())
			{
				Debug.Log ("Player 2's HP hit 0");

				// remove losing player for allotted turns
				// gain/lose bonus fame for win & loss
				player.ChangeFame(10);
				player2.HandleCombDeath(-10);
			}
			
		}
		/*else
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
			if ((player.JustStoppedAttacking() || MonsterAttackBypass) || 
			    (player.JustStoppedAttacking() && MonsterAttackBypass) ||
			    !player.GetComponent<Animator>())
			{
				ResolveMon();
				// end combat
				isCombatEnded = true;
			}
		}
		else if (isPvpCombat)
		{
			if (player.GetComponent<Animator>() && player2.GetComponent<Animator>())
			{
				if ((player.JustStoppedAttacking() || player2.JustStoppedAttacking()) || 
				    (player.JustStoppedAttacking() && player2.JustStoppedAttacking()))
				{

					ResolvePvp();

					// end combat
					isCombatEnded = true;
				}
			}
			else
			{
				ResolvePvp();
				
				// end combat
				isCombatEnded = true;
			}
		}
	}

	public void ExecuteAttackPhase()
	{
		if (isMonCombat)
		{
			if (playerDiceRoll > monsterDiceRoll)
			{
				player.Attack();
			}
			else if (playerDiceRoll < monsterDiceRoll)
			{
				MonsterAttackBypass = true;
				player.Defeat();
			}
			else
			{
				player.Attack();
				MonsterAttackBypass = true;
			}
		}
		else if (isPvpCombat)
		{
			// compare dice
			if (playerDiceRoll > playerTwoDiceRoll)
			{
				player.Attack();
				player2.Defeat();
			}
			else if (playerDiceRoll < playerTwoDiceRoll)
			{
				player2.Attack();
				player.Defeat();
			}
			else
			{
				player.Attack();
				player2.Attack();
			}
		}
	}

	public void EstablishMonCombat(Player p, MonsterCard m, GameObject pr)
	{
		player = p;
		monster = m;

		isMonCombat = true;

		// if there is no model, send it to be instantiated
		if (map.HasBlankModel((int)player.GetMapPosition().x, (int)player.GetMapPosition().y))
		{
			// Gets the player's world position.
			Vector3 pos = player.transform.position;
			// Removes the y coordinate.
			pos.y = 0;
			// Spawns the monster.
			map.AddPrefabToTiles((int)player.GetMapPosition().x, (int)player.GetMapPosition().y, pr, pos);
		}
	}

	public void EstablishPvpCombat(Player p1, Player p2)
	{
		player = p1;
		player2 = p2;

		isPvpCombat = true;
	}

	public void ResetCombat()
	{
		player = null;
		monster = null;

		isMonCombat = false;
		isPvpCombat = false;
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
		
		Debug.Log (" A " + map.GetMonsterOnTile(tileX, tileY).GetName() + " is here now CAUSE SKIP.");

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
