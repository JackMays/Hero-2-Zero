﻿using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour {

	public Map map;

	Player player;

	MonsterCard monster;

	int playerDiceRoll;
	int monsterDiceRoll;

	//bool isRoundResolved;
	bool isCombatEnded;

	// Use this for initialization
	void Awake () 
	{
		player = null;
		monster = null;

		playerDiceRoll = 0;
		monsterDiceRoll = 0;

		//isRoundResolved = false;
		isCombatEnded = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void ResolveCombat()
	{
		int damage = 0;

		// compare dice
		if (playerDiceRoll > monsterDiceRoll)
		{
			//player wins
			damage = playerDiceRoll - monsterDiceRoll;
			Debug.Log ("Monster Takes: " + playerDiceRoll + " + " + monsterDiceRoll + " = " + damage);
			monster.TakeDamage(damage);
		}
		else if (playerDiceRoll < monsterDiceRoll)
		{
			// monster wins
			damage = monsterDiceRoll - playerDiceRoll;
			Debug.Log ("Player Takes: " + monsterDiceRoll + " - " + playerDiceRoll + " = " + damage);
			player.TakeDamage(damage);
		}
		else
		{
			// DRAW
			Debug.Log ("Tie");
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
				// remove player for allotted turns, place monster card at area
				// Also decrease fame
				player.HandleDeath(monster.GetFameMod(false));

				Debug.Log ("Player's HP hit 0");
			}
			else if (monster.HasDied())
			{
				// Remove Card from area
				Debug.Log ("Monster's HP hit 0");
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

		// end combat
		isCombatEnded = true;
	}

	public void EstablishCombat(Player p, MonsterCard m, GameObject pr)
	{
		player = p;
		monster = m;

		// if there is no model, send it to be instantiated
		if (map.HasBlankModel((int)player.GetMapPosition().x, (int)player.GetMapPosition().y))
		{
			map.AddPrefabToTiles((int)player.GetMapPosition().x, (int)player.GetMapPosition().y, pr, player.transform.position);
		}
	}

	public void ResetCombat()
	{
		player = null;
		monster = null;
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

	public void SetMonsterDiceRoll(int mRoll)
	{

		monsterDiceRoll = mRoll + monster.GetStrength();
		Debug.Log ("Monster Total: " + mRoll + " (base) + " + monster.GetStrength() + " (str) = " + monsterDiceRoll);
	}
	/*public bool HasRoundResolved()
	{
		return isRoundResolved;
	}*/
	public bool HasCombatEnded()
	{
		return isCombatEnded;
	}
}
