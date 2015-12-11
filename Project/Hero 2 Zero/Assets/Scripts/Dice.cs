using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
	#region Variables
	// Forces which will move and rotate dice.
	public float force = 10.0f;
	public float torgue = 10.0f;

	// The kind of force that will be applied.
	public ForceMode forceMode;

	// The dice's rigidbody.
	Rigidbody rigid;
	
	// Holds whether the dice is rolling.
	public bool isRolling = false;

	#endregion

	// Use this for initialization
	void Start ()
	{
		// Stores the dice's rigidbody.
		rigid = GetComponent<Rigidbody> ();
	}

	// Outputs the result of the roll.
	public int GetResult()
	{
		// Checks which vector is facing up. (fo = 1, up = 5, ba = 6, le = 3, ri = 4, bo = 2)

		// Forward is facing up.
		int o = 1;
		float current = transform.forward.y;

		// Back is facing up.
		if (-transform.forward.y > current) {
			o = 6;
			current = -transform.forward.y;
		}
		// Up is facing up.
		if (transform.up.y > current) {
			o = 5;
			current = transform.up.y;
		}
		// Down is facing up.
		if (-transform.up.y > current) {
			o = 2;
			current = -transform.up.y;
		}
		// Right is facing up.
		if (transform.right.y > current) {
			o = 4;
			current = transform.right.y;
		}
		// Left is facing up.
		if (-transform.right.y > current) {
			o = 3;
		}
		
		// Outputs dice result.
		//Debug.Log(name + " Result: " + o);
		
		// Returns the result.
		return o;
	}

	// Applies force to roll the dice.
	public void ApplyForce()
	{		
		// The direction to apply the force.
		Vector3 f = Vector3.down;
		
		// Hopefully stops downward force from appearing too much.
		while (f.y < -0.5f) {
			f = Random.onUnitSphere;
		}
		
		// Applies force randomly to one of the dice faces.
		rigid.AddForce (f * force, forceMode);
		rigid.AddTorque (Random.onUnitSphere * torgue, forceMode);
		
		// Sets that the dice is rolling.
		isRolling = true;
	}

	// Update is called once per frame
	void Update ()
	{	
		// Checks if the dice isn't moving.
		if (rigid.IsSleeping()) {
			// Sets that teh dice is no longer moving.
			isRolling = false;
		}
	}
}