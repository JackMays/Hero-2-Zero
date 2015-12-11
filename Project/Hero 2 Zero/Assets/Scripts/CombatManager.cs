using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour {

	Player player;

	MonsterCard monster;

	int playerDiceRoll;
	int monsterDiceRoll;

	// Use this for initialization
	void Awake () 
	{
		player = null;
		monster = null;

		playerDiceRoll = 0;
		monsterDiceRoll = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ResolveCombat()
	{
		if (playerDiceRoll > monsterDiceRoll)
		{
			//player wins
		}
		else if (playerDiceRoll < monsterDiceRoll)
		{
			// monster wins
		}
	
		if (player.HasDied() || monster.HasDied())
		{
			// end combat
		}
	}

	public void EstablishCombat(Player p, MonsterCard m)
	{
		player = p;
		monster = m;
	}

	public void SetPlayerDiceRoll(int pRoll)
	{
		playerDiceRoll = pRoll + player.GetStrength();
	}

	public void SetMonsterDiceRoll(int mRoll)
	{
		playerDiceRoll = mRoll + monster.GetStrength();
	}
}
