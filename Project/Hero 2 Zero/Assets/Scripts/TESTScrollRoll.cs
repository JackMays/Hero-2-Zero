using UnityEngine;
using System.Collections;

public class TESTScrollRoll : MonoBehaviour
{
	public Transform[] points;
	public Transform parent;
	public Transform scroll;
	float lerpTime = 0;
	float speed = 0.75f;
	
	bool isRolling = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isRolling) {
			if (lerpTime * speed >= 1) {
				isRolling = false;
			}
			
			lerpTime += Time.deltaTime;
			
			parent.position = Vector3.Lerp(points[0].position, points[1].position, lerpTime * speed);
			
			scroll.Rotate(Vector3.up, -10);
		}
		
		if (Input.GetKeyDown(KeyCode.L)) {
			isRolling = true;
			
			lerpTime = 0;
		}
	}
}
