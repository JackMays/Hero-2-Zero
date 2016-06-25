using UnityEngine;
using System.Collections;

public class Butterfly : MonoBehaviour
{
	#region Variables
	
	// Reference to the butterfly's transform for optimal movement and rotation.
	Transform trans;
	
	// Reference to the butterfly's animator.
	Animator anim;
	
	// Speed the butterfly will move at.
	float moveSpeed = 0.25f;
	
	// Time used for moving the butterfly.
	float lerpTme = 0;
	
	// The positions the butterfly is moving from and to and the very start position.
	Vector3 oldPos = Vector3.zero;
	Vector3 newPos = Vector3.zero;
	Vector3 startPos = Vector3.zero;
	
	// The maximum distacne the butterfly can travel from the start position.
	float maxDistance = 10;
	
	// Holds whether the butterfly is flying or has landed.
	bool isFlying = false;
	
	// Holds whether the butterfly is going to land.
	bool isLanding = false;
	
	// Time used to decide how long the butterfly will land for.
	float landTime = 0;
	
	#endregion
	
	// Use this for initialization
	void Start ()
	{
		// Stores the butteerfly's transform and animator.
		trans = transform;
		anim = GetComponent<Animator>();
		
		// Stores the curent position as the start position.
		startPos = trans.position;
		newPos = startPos;
		oldPos = startPos;
		
		// Sets the butterfly to start flying.
		StartFlying();
	}
	
	// Changes the butterfly's animator depending on the given bool.
	void ChangeAnimator(bool fly)
	{
		anim.SetBool("isFlying", fly);
	}
	
	// Sets the butterfly to start flying.
	void StartFlying()
	{
		// Sets the butterfly to start flapping its wings.
		ChangeAnimator(true);
		
		// Finds a target for the butterfly to fly to.
		FindTarget();
		
		// Resets the movement time.
		lerpTme = 0;
		
		// Sets that the butterfly is now flying.
		isFlying = true;
	}
	
	// Finds a target for the butterfly to fly towards.
	void FindTarget()
	{
		// A new vector 3 to hold the target.
		Vector3 target = new Vector3();
		
		// Finds a random position in x,y,z within a certain boundary from the current position and the starting position.
		
		// Finds a new X position.
		target.x = FindPoint(false, newPos.x, startPos.x);
		
		// Checks if the butterfly is going to land.
		if (isLanding) {
			// The y position is set just above the ground.
			target.y = 0.5f;
		}
		// Find a normal position in y.
		else {
			// Finds a new Y position.
			target.y = FindPoint(true, newPos.y, startPos.y);
		}
		
		// Finds a new Z position.
		target.z = FindPoint(false, newPos.z, startPos.z);
		
		// Sets the current position to be the old position.
		oldPos = newPos;
		
		// Sets the new position to the target.
		newPos = target;
		
		// Sets a random speed for the butterfly to move at.
		moveSpeed = Random.Range(0.15f, 0.3f);
		
		// Makes the butterfly face toward the target point.
		FaceTarget();
	}
	
	// Finds a given random offset froma given point. (Y needs a separate check.)
	float FindPoint(bool yAxis, float curPos, float staPos)
	{
		// Bool used to keep break the loop.
		bool breakLoop = false;
		
		// Int for stopping endless loop.
		int maxChecks = 20;
		
		// The value to return.
		float point = 0;
		
		// Loops until a point is found or 20 checks have been made.
		while(!breakLoop) {
			// Finds a random distance from the current position.
			point = curPos + Random.Range(-2, 3);
		
			// Decrements number of remaining checks.
			--maxChecks;
			
			// Checks if the axis is Y.
			if (yAxis) {
				// Checks that the point is not higher than maximum or lower than the ground.
				if (point > 0.5f && point < startPos.y + 2) {
					// The loop can be broken.
					breakLoop = true;
				}
			}
			// Is the X or Z axis.
			else {
				// Checks that the point is within bounds.
				if (point > staPos - maxDistance && point < staPos + maxDistance) {
					// The loop can be broken.
					breakLoop = true;
				}
			}
			
			// If 20 checks has passed, the loop has to break.
			if (maxChecks <= 0) {
				breakLoop = true;
			}
		}
	
		// Returns the point.
		return point;
	}
	
	// Makes the butterfly face the point it is flying towards.
	void FaceTarget()
	{
		//trans.LookAt(newPos);
		trans.LookAt(2 * trans.position - newPos);
	}
	
	// Lerps the butterfly towards the target point over time.
	void Fly()
	{
		// Checks if the butterfly has reached the target position.
		if (lerpTme * moveSpeed >= 1) {
			// Checks if the butterfly needs to land.
			if (isLanding) {
				// Lands the butterfly.
				StopFlying();
				return;
			}
			
			// 10% chance the butterfly will want to land.
			if (Random.Range(0, 101) <= 10) {
				// Sets the butterfly to start landing.
				isLanding = true;
			}
			
			// Finds a new target for the butterfly to fly to.
			FindTarget();
			
			// Resets the time left to travel.
			lerpTme = 0;
			
			return;
		}
		
		// Increases time the butterfly has been flying towards point.
		lerpTme += Time.deltaTime;
		
		// Moves the butterfly closer towards the target depending on time and speed.
		trans.position = Vector3.Lerp(oldPos, newPos, lerpTme * moveSpeed);
	}
	
	// Stops the butterfly from flying.
	void StopFlying()
	{
		// Sets the butterfly to stop flapping.
		ChangeAnimator(false);
		
		// Sets a random time to spend on the ground.
		landTime = Random.Range(3,10);
		
		// Sets that the butterfly is no longer flying or landing.
		isFlying = false;
		isLanding = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Checks if the butterfly is currently flying.
		if (isFlying) {
			Fly();
		}
		// Butterfly has landed.
		else {
			// Checks if the butterfly is ready to fly again.
			if (landTime <= 0) {
				// Sets the butterfly to start flying again.
				StartFlying();
			}
			else {
				// Decreases the time before the butterfly will fly again.
				landTime -= Time.deltaTime;
			}
		}
	}
}