using UnityEngine;
using System.Collections;

public class TESTPLAYER4 : MonoBehaviour
{
	Animator anima;
	public ParticleSystem whirlwind;
	bool moveWhirlwind = false;
	public Transform[] whirlPoints;
	float whirlTime = 0;
	int whirlTarget = 0;
	Vector3 whirlStart = Vector3.zero;
	
	
	// Use this for initialization
	void Start () {
		anima = GetComponent<Animator>();
	}
	
	public void RevertIdle()
	{
		anima.SetBool("Fall Down", false);
	}
	
	public void StartWhirlwind()
	{
		whirlwind.Play();
	}
	
	public void StopParticles()
	{
		Debug.Log("Stopped");		
		whirlwind.Stop();
	}
	
	public void StartMovingWhirlwind(int target)
	{
		Debug.Log("MOVING");
		whirlTarget = target;
		whirlStart = whirlwind.transform.position;
		moveWhirlwind = true;
	}
	
	// Moves whirlwind up or down.
	public void MoveWhirlwind()
	{
		whirlTime += Time.deltaTime;
		
		if (whirlTime < 1) {
			whirlwind.transform.position = Vector3.Lerp(whirlStart, whirlPoints[whirlTarget].position, whirlTime);
		}
		else {
			whirlTime = 0;
			moveWhirlwind = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (moveWhirlwind) {
			MoveWhirlwind();
		}
		
		
		if (Input.GetKeyDown(KeyCode.V)) {
			anima.SetBool("Fall Down", true);
		}
	}
}
