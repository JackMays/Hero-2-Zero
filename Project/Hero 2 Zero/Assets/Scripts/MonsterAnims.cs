using UnityEngine;
using System.Collections;

public class MonsterAnims : MonoBehaviour {

	Animator animatorCompo;

	int spawnID = Animator.StringToHash("isSpawning");
	int idleID = Animator.StringToHash("isIdle");
	int combIdleID = Animator.StringToHash("isCombatIdle");
	int combAttackID = Animator.StringToHash("isAttacking");
	int combWinID = Animator.StringToHash("isVictory");
	int combLoseID = Animator.StringToHash("isHit");
	int deadID = Animator.StringToHash("isDead");
	int runID = Animator.StringToHash("isRunning");

	bool justFought = false;

	// Use this for initialization
	void Awake () 
	{
		animatorCompo = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Idle()
	{
		// reset everything when returning to idle
		if (animatorCompo)
		{
			animatorCompo.SetBool(spawnID, false);
			animatorCompo.SetBool(combIdleID, false);
			animatorCompo.SetBool(combAttackID, false);
			animatorCompo.SetBool(combWinID, false);
			animatorCompo.SetBool(combLoseID, false);
			animatorCompo.SetBool(deadID, false);
			animatorCompo.SetBool(runID, false);
			animatorCompo.SetBool(idleID, true);
		}
	}
	
	/*public void Spawn()
	{
		if (animatorCompo)
		{
			animatorCompo.SetBool(spawnID, true);
		}
	}*/
	
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

			justFought = true;
		}
	}
	
	// for anim event at the end of attack
	// If attack kills combat needs to go forward to the right stage
	public void DefeatEnd()
	{
		if (animatorCompo)
		{
			animatorCompo.SetBool(combLoseID, false);
			animatorCompo.SetBool(combIdleID, true);
			
			justFought = true;
		}
	}
	
	public void Victory()
	{
		if (animatorCompo)
		{
			animatorCompo.SetBool(combWinID, true);
		}
	}
	
	public void Hit()
	{
		if (animatorCompo)
		{
			animatorCompo.SetBool(combLoseID, true);
			Debug.Log ("Hit");
		}
	}
	// Anim for if monster was hit and defeated
	public void Dead()
	{
		if (animatorCompo)
		{
			
			animatorCompo.SetBool(deadID, true);
		}
	}
	// run away anim if monster was hit but not defeated
	public void Run()
	{
		if (animatorCompo)
		{
			Debug.Log("Run Away!");
			
			animatorCompo.SetBool(runID, true);
		}
	}

	public bool HasFightAnimFinished()
	{
		return justFought;
	}

	public bool HasIdleState()
	{
		return animatorCompo.GetBool(idleID);
	}

	public bool HasDeadState()
	{
		return animatorCompo.GetBool(deadID);
	}
}
