using UnityEngine;
using System.Collections;

public class EndTrailer : MonoBehaviour
{
	
	public Transform player;
	public Transform slime;
	
	public Transform[] playerPoints;
	public Transform[] slimePoints;
	public Transform[] cameraPoints;
	public Transform[] textPoints;
	public Transform cam;

	bool play = false;
	bool growText = false;
	int textIndex = 0;
	string[] texts = new string[9] { "You are a Hero...",
										"Whose only desire...",
										"is to not be a Hero.",
										"Explore the board\n to lose fame",
										"Battle Monsters...",
										"And Lose",
										"De-Level to lose\n equipment",
										"Lose to your friends\n in online multiplayer",
										"Only 1 Hero can\n lose their status" };

	float lerpTime = 0;

	public bool isTextCam = false;
	public Transform text;
	public GameObject[] lights;

	// Use this for initialization
	void Start ()
	{
		player.GetComponent<Animator>().SetBool("isRunning", true);
		slime.GetComponent<Animator>().SetBool("isWalking", true);
		
		cam.position = cameraPoints[0].position;
		cam.rotation = cameraPoints[0].rotation;
		
		if (isTextCam) {
			cam.position = cameraPoints[2].position;
			cam.rotation = cameraPoints[2].rotation;
			lights[0].SetActive(false);
			lights[1].SetActive(true);
			cam.GetChild(0).gameObject.SetActive(true);
		}
		else {
			lights[0].SetActive(true);
			lights[1].SetActive(false);
			cam.GetChild(0).gameObject.SetActive(false);
		}
	}
	
	void GrowText()
	{
		lerpTime += Time.deltaTime;
		
		text.position = Vector3.Lerp(textPoints[0].position, textPoints[1].position, lerpTime / 1.5f);
		
		if (lerpTime >= 2) {
			++textIndex;
			lerpTime = 0;
			text.GetComponent<TextMesh>().text = texts[textIndex];
		}
	}	
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.A)) {
			play = true;
			lerpTime = 0;
		}
		
		if (Input.GetKeyDown(KeyCode.S)) {
			growText = true;
			lerpTime = 0;
			text.GetComponent<TextMesh>().text = texts[textIndex];
		}
		
		if (growText){
			GrowText();
		}
		
		if (play) {
			lerpTime += Time.deltaTime;
		
			if (lerpTime >= 0.75f) {
				cam.position = Vector3.Lerp(cameraPoints[0].position, cameraPoints[1].position, (lerpTime - 0.75f) / 3);
				cam.rotation = Quaternion.Lerp(cameraPoints[0].rotation, cameraPoints[1].rotation, (lerpTime - 0.75f) / 3);
			}
		
			player.position = Vector3.Lerp(playerPoints[0].position, playerPoints[1].position, lerpTime / 7);
			slime.position = Vector3.Lerp(slimePoints[0].position, slimePoints[1].position, lerpTime / 7);
		}
	}
}
