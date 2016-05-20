using UnityEngine;
using System.Collections;

public class ReceiveAnimEvent : MonoBehaviour
{
	public bool isTrailer = true;
	public bool isStage4 = true;
	public bool isStage5 = true;
	public bool isStage6 = true;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void KnockDown()
	{
		if (isTrailer) {
			GameObject.Find("Camera Points").GetComponent<CameraControl>().KnockDown();
		}
	}
	
	public void LoseArmour()
	{
		if (isStage4) {
			GameObject.Find("Camera Points").GetComponent<CameraControl>().LoseArmour();
		}
	}
	
	public void ChangeSwords()
	{
		if (isStage5 && !isStage6) {
			GameObject.Find("Camera Points").GetComponent<CameraControl>().ChangeSwords();
		}
	}
	
	public void DealDamage()
	{
		if (isStage5) {
			GameObject.Find("Camera Points").GetComponent<CameraControl>().DealDamage();
		}
	}
	
	public void ShowCard()
	{
		if (isStage5) {
			GameObject.Find("Camera Points").GetComponent<CameraControl>().ShowCard();
		}
	}	
	
	public void AttackPlayer()
	{
		if (isStage6) {
			GameObject.Find("Camera Points").GetComponent<CameraControl>().AttackPlayer();
		}
	}
	
	public void ShowFireball()
	{
		if (isStage6) {
			GameObject.Find("Camera Points").GetComponent<CameraControl>().fireball.SetActive(true);
			GameObject.Find("Camera Points").GetComponent<CameraControl>().fireCircle.SetActive(false);
			GameObject.Find("Camera Points").GetComponent<CameraControl>().moveFireball = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
