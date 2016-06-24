using UnityEngine;
using System.Collections;

public class TESTMonsterSummon : MonoBehaviour
{
	#region Variables
	// The two circles that will reveal the monster.
	public Transform[] summonCircles;
	
	// The points used for moving the circles.
	public Transform[] circlePoints;
	
	// The card canvas.
	public GameObject cardCanvas;
	
	// The monster.
	public GameObject monster;
	
	// The mask to hide the card.
	public Transform cardMask;
	
	// Time used for moving the circles.
	float lerpTime = 0;
	
	// The speed the circles move at.
	float circleSpeed = 0.8f;
	
	// Whether the circles are moving.
	bool isMoving = false;
	#endregion
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	void Reset()
	{
		cardCanvas.SetActive(true);
		monster.SetActive(false);
		summonCircles[0].gameObject.SetActive(false);
		summonCircles[1].gameObject.SetActive(false);
		isMoving = false;
		lerpTime = 0;
		cardMask.gameObject.SetActive(false);
		cardMask.localScale = new Vector3(1.4f, 0, 0.1f);
	}
	
	void MoveCircle()
	{
		if (lerpTime * circleSpeed >= 1) {
			isMoving = false;
			summonCircles[0].gameObject.SetActive(false);
			summonCircles[1].gameObject.SetActive(false);
			cardCanvas.SetActive(false);
		}
		
		lerpTime += Time.deltaTime;
		
		summonCircles[0].position = Vector3.Lerp(circlePoints[0].position, circlePoints[1].position, lerpTime * circleSpeed);
		summonCircles[1].position = Vector3.Lerp(circlePoints[0].position, circlePoints[2].position, lerpTime * circleSpeed);
		
		summonCircles[0].GetChild(0).Rotate(Vector3.forward, 90);
		summonCircles[1].GetChild(0).Rotate (Vector3.forward, -90);
		
		cardMask.localScale = Vector3.Lerp(new Vector3(1.4f, 0, 0.1f), new Vector3(1.4f, 2.1f, 0.1f), lerpTime * circleSpeed);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isMoving) {
			MoveCircle();
		}
		
		if (Input.GetKeyDown(KeyCode.N)) {
			Reset();
		}
		
		if (Input.GetKeyDown(KeyCode.M)) {
			isMoving = true;
			lerpTime = 0;
			
			monster.SetActive(true);
			cardMask.gameObject.SetActive(true);
			summonCircles[0].gameObject.SetActive(true);
			summonCircles[1].gameObject.SetActive(true);
		}
	}
}
