using UnityEngine;
using System.Collections;

public class TrailerMoveWhirlwind : MonoBehaviour {

	float lerpTime = 0;
	public Transform target;
	public Transform origin;
	
	public bool dropWhirlwind = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (dropWhirlwind) {
			lerpTime += Time.deltaTime;
		
			transform.position = Vector3.Lerp(origin.position, target.position, lerpTime * 1.25f);
		}
	}
}
