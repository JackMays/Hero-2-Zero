using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class CameraControl : MonoBehaviour
{
	#region Variables
	
	public Transform cam;
	public Transform player;
	public Transform slime;
	
	float lerpTime = 0;
	float slimeLerpTime = 0;
	
	public float camSpeed = 1;
	public float slimeSpeed = 1;
	
	public List<Transform> cameraPoints = new List<Transform>();
	public List<Transform> slimePoints = new List<Transform>();
	public List<Transform> playerPoints = new List<Transform>();
	
	public int stage = 0;
	
	bool moveCamera = false;
	
	int[] slimeHash = new int[4] {Animator.StringToHash("isIdle"), Animator.StringToHash("isSurprised"),
									Animator.StringToHash("isScared"), Animator.StringToHash("isEyeOpen") };
	
	Animator sAnimator;
	
	public GameObject[] swords = new GameObject[2];
	
	#endregion
	
	#region Start
	// Use this for initialization
	void Start ()
	{
		if (stage == 0) {
			sAnimator = slime.GetComponent<Animator>();
		
			cam.position = cameraPoints[0].position;
			cam.rotation = cameraPoints[0].rotation;
		
			slime.position = slimePoints[0].position;
			slime.rotation = slimePoints[0].rotation;
			ChangeSlimeAnimator(0);
		
			player.position = playerPoints[0].position;
		
			swords[1].SetActive(false);
		}
		
		if (stage == 1) {
			cam.position = cameraPoints[0].position;
			cam.rotation = cameraPoints[0].rotation;
		}
	}
	#endregion
	
	#region Stage 1
	
	void ChangeSlimeAnimator(int a)
	{
		for (int i = 0; i < slimeHash.GetLength(0); ++i) {
			if (i == a) {
				sAnimator.SetBool(slimeHash[i], true);
			}
			else {
				sAnimator.SetBool(slimeHash[i], false);
			}
		}
	}
	
	void MoveCameraToPoint()
	{
		lerpTime += Time.deltaTime;
		
		cam.position = Vector3.Lerp(cameraPoints[0].position, cameraPoints[1].position, lerpTime * camSpeed);
		cam.rotation = Quaternion.Lerp(cameraPoints[0].rotation, cameraPoints[1].rotation, lerpTime * camSpeed);
		
		if (lerpTime * camSpeed > 0.5f && lerpTime * camSpeed < 0.55f) {
			if (!player.GetComponent<Character_Blink>().isBlinking) {
				player.GetComponent<Character_Blink>().StartBlinking();
			}
		}
		
		if (lerpTime * camSpeed > 0.6f) {	
			slimeLerpTime += Time.deltaTime;
			
			slime.position = Vector3.Lerp(slimePoints[0].position, slimePoints[1].position, slimeLerpTime * slimeSpeed);
			ChangeSlimeAnimator(5);
		}
		
		player.position = Vector3.Lerp(playerPoints[0].position, playerPoints[1].position, lerpTime * camSpeed);
		player.GetComponent<Player>().Walk();
		
		//if (inRange(cam.transform.position, cameraPoints[toPoint].position, range)) {
		if (lerpTime * camSpeed >= 1) {
			//currentStage = nextStage;
			lerpTime = 0;
			slimeLerpTime = 0;
			moveCamera = false;
			player.GetComponent<Player>().Idle();
			ChangeSlimeAnimator(0);
			rotateSlime = true;
			player.GetComponent<Character_Blink>().StartBlinking();
		}
	}
	
	bool rotateSlime = false;
	bool player2Idle = false;
	
	
	void RotateSlime()
	{
		slimeLerpTime += Time.deltaTime;
		
		slime.rotation = Quaternion.Lerp(slime.rotation, slimePoints[2].rotation, slimeLerpTime * 2);

		slime.position = Vector3.Lerp(slimePoints[1].position, slimePoints[2].position, slimeLerpTime * 3);
		
		player.rotation = Quaternion.Lerp(player.rotation, playerPoints[1].rotation, slimeLerpTime * 1);
		
		if (slimeLerpTime * 1.5 >= 1) {
			rotateSlime = false;
			slimeLerpTime = 0;
			player2Idle = true;
		}
	}
	
	bool waitToRun = false;
	bool runAway = false;
	
	void RunAway()
	{
		lerpTime += Time.deltaTime;
		
		player.LookAt(playerPoints[2].position);
		
		player.position = Vector3.Lerp(playerPoints[1].position, playerPoints[2].position, lerpTime / 4);
		cam.position = Vector3.Lerp(cameraPoints[2].position, cameraPoints[3].position, lerpTime / 1.5f);
		cam.rotation = Quaternion.Lerp(cameraPoints[2].rotation, cameraPoints[3].rotation, lerpTime / 1.5f);
		
		
		
		if (lerpTime / 4 >= 0.30f) {
			slimeLerpTime += Time.deltaTime;
			if (slimeLerpTime < 1.5f) {
				ChangeSlimeAnimator(0);
				slime.rotation = Quaternion.Lerp(slimePoints[2].rotation, slimePoints[3].rotation, slimeLerpTime / 1.5f);
			}
		}
		
		if (lerpTime / 4 >= 1) {
			runAway = false;
			lerpTime = 0;
			slimeLerpTime = 0;
			cam.position = cameraPoints[2].position;
			cam.rotation = cameraPoints[2].rotation;
			slimeFaceCamera = true;
		}
	}
	
	bool slimeSurprised = false;
	bool slimeFaceCamera = false;
	
	void SlimeFaceCamera()
	{
		lerpTime += Time.deltaTime;
		
		if (lerpTime > 0.5f) {
			slimeLerpTime += Time.deltaTime;
			
			slime.rotation = Quaternion.Lerp(slimePoints[3].rotation, slimePoints[2].rotation, slimeLerpTime * 1.75f);
			
			if (slimeLerpTime * 1.75f >= 1) {
				lerpTime = 0;
				slimeLerpTime = 0;
				slimeFaceCamera = false;
			}
		}
	}
	
	void Stage1Update()
	{
		// Initial movement.
		if (Input.GetKeyDown(KeyCode.A)) {
			moveCamera = true;
			slime.position = slimePoints[0].position;
			slime.rotation = slimePoints[0].rotation;
			player.rotation = playerPoints[0].rotation;
			player.GetComponent<Player>().StopRunning();
			swords[0].SetActive(true);
			swords[1].SetActive(false);
		}
		
		if (rotateSlime) {
			RotateSlime();
		}
		
		if (player2Idle) {
			lerpTime += Time.deltaTime;
			
			if (lerpTime >= 1) {
				player.GetComponent<Player>().CombatIdle();
				lerpTime = 0;
				player2Idle = false;
				slimeSurprised = true;
			}
		}
		
		if (slimeSurprised) {
			lerpTime += Time.deltaTime;
			
			if (lerpTime > 1.8f) {
				ChangeSlimeAnimator(1);
				lerpTime = 0;
				slimeSurprised = false;
				waitToRun = true;
			}
		}
		
		if (waitToRun) {
			lerpTime += Time.deltaTime;
			
			if (lerpTime >= 1f && lerpTime <= 4.5f) {
				ChangeSlimeAnimator(2);
			}
			
			if (lerpTime >= 1.5f && lerpTime <= 2f) {
				player.GetComponent<Character_Blink>().StartBlinking();
			}
			
			if (lerpTime >= 3f) {
				cam.position = cameraPoints[2].position;
				cam.rotation = cameraPoints[2].rotation;
			}
			
			if (lerpTime >= 4.5f) {
				ChangeSlimeAnimator(3);
			}
			
			if (lerpTime >= 6) {
				waitToRun = false;
				runAway = true;
				lerpTime = 0;
				slimeLerpTime = 0;
				swords[0].SetActive(false);
				swords[1].SetActive(true);
				player.GetComponent<Player>().RunAway();
			}
		}
		
		if (runAway) {
			RunAway();
		}
		
		if (slimeFaceCamera) {
			SlimeFaceCamera();
		}
		
		if (moveCamera) {
			MoveCameraToPoint();
		}		
	}
	
	#endregion
	
	#region Stage 2
	bool moveToSign = false;
	bool watchSign = false;
	bool loseFame = false;
	
	float textTime = 0;
	bool showText = true;
	bool hideText = false;
	
	bool changeFameCounter = false;
	bool scaleFameParent = false;
	
	public Material signMat;
	public Material newSignMat;
	public Renderer signRend;
	
	public GameObject fameParent;
	public Text[] fameTexts = new Text[2];
	public GameObject minusText;
	public Transform minusOrigin;
	
	bool changeAnimator = false;
	bool changePlayerAnimator = false;
	
	void Stage2Update()
	{
		if (Input.GetKeyDown(KeyCode.A)) {
			moveToSign = true;
			watchSign = false;
			player.GetComponent<Player>().Idle();
			changePlayerAnimator = true;
			player.position = playerPoints[0].position;
			player.rotation = playerPoints[0].rotation;
			cam.position = cameraPoints[0].position;
			cam.rotation = cameraPoints[0].rotation;
			lerpTime = 0;
			slimeLerpTime = 0;
			textTime = 0;
			showText = true;
			hideText = false;
			scaleFameParent = false;
			fameTexts[0].text = " : 100";
			fameTexts[1].text = "100";
			signRend.material = signMat;
			minusText.transform.position = minusOrigin.position;
		}
		
		if (Input.GetKeyDown(KeyCode.S)) {
			loseFame = true;
			textTime = 0;
			showText = true;
			hideText = false;
			scaleFameParent = false;
			fameTexts[0].text = " : 100";
			fameTexts[1].text = "100";
			minusText.GetComponent<Text>().color = Color.red;
			minusText.transform.position = minusOrigin.position;
		}
		
		if (loseFame) {
			LoseFame();
		}
		
		if (moveToSign) {
			MoveToSign();
		}
		
		if (watchSign) {
			WatchSign();
		}
		
	}
	
	void MoveToSign()
	{
		lerpTime += Time.deltaTime;
		
		cam.position = Vector3.Lerp(cameraPoints[0].position, cameraPoints[1].position, lerpTime / 4);
		cam.rotation = Quaternion.Lerp(cameraPoints[0].rotation, cameraPoints[1].rotation, lerpTime / 4);
		
		player.position = Vector3.Lerp(playerPoints[0].position, playerPoints[1].position, lerpTime / 4);
		if (changePlayerAnimator) {
			player.GetComponent<Player>().Walk();
			changePlayerAnimator = false;
		}
		
		if (lerpTime / 4 > 1) {
			player.GetComponent<Player>().Idle();
			moveToSign = false;
			lerpTime = 0;
			slimeLerpTime = 0;
			watchSign = true;
		}
	}
	
	void WatchSign()
	{
		lerpTime += Time.deltaTime;
		
		if (lerpTime > 1f) {
			cam.position = cameraPoints[2].position;
		}
		
		if (lerpTime > 1.75f && lerpTime < 4.5f && player.position != playerPoints[2].position) {
			slimeLerpTime += Time.deltaTime;
			player.position = Vector3.Lerp(playerPoints[1].position, playerPoints[2].position, slimeLerpTime * 2);
			player.GetComponent<Player>().Walk();
			changePlayerAnimator = true;
		}
		
		if (changePlayerAnimator && player.position == playerPoints[2].position) {
			player.GetComponent<Player>().Idle();
			player.GetComponent<Player>().ChangingSign(true);
			changePlayerAnimator = false;
		}
		
		if (lerpTime > 3f && lerpTime < 7) {
			cam.position = cameraPoints[3].position;
			cam.rotation = cameraPoints[3].rotation;
		}
		
		if (lerpTime > 3.75f && lerpTime < 4.25f) {
			player.GetComponent<Character_Blink>().StartBlinking();
		}
		
		if (lerpTime > 6.5 && lerpTime < 7f) {
			cam.position = cameraPoints[2].position;
			cam.rotation = cameraPoints[2].rotation;
			slimeLerpTime = 0;
			player.GetComponent<Player>().ChangingSign(false);
			
			signRend.material = newSignMat;
		}
		
		if (lerpTime > 7.1f) {
			LoseFame();
		}
		
		if (lerpTime > 10f && lerpTime < 13) {
			slimeLerpTime += Time.deltaTime;
			player.GetComponent<Player>().Walk();
			player.position = Vector3.Lerp(playerPoints[2].position, playerPoints[3].position, slimeLerpTime / 4);
			player.rotation = Quaternion.Lerp(playerPoints[2].rotation, playerPoints[3].rotation, slimeLerpTime * 2);
		}
	}
	
	void LoseFame()
	{
		textTime += Time.deltaTime;
		
		if (textTime < 0.5f && showText) {
			minusText.SetActive(true);
			fameParent.SetActive(true);
			fameParent.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, textTime * 2.5f);
		}
		
		if (textTime > 0.5f && showText) {
			showText = false;
			textTime = 0;
		}
		
		if (!showText && !hideText && !changeFameCounter) {
			if (textTime * 1.25f < 1) {		
				Color trans = Color.red;
				trans.a = 0;
		
				minusText.transform.position = Vector3.Lerp(minusOrigin.position, fameTexts[0].transform.position, textTime * 1.25f);
				minusText.GetComponent<Text>().color = Color.Lerp(Color.red, trans, textTime * 1f);
			}
			else {
				changeFameCounter = true;
				minusText.SetActive(false);
				textTime = 0;
			}
		}
		
		if (changeFameCounter) {
			string t = "" + LerpInt(100, 80, textTime);	
			string t2 = " : " + LerpInt(100, 80, textTime);
				
			fameTexts[0].text = t2;
			fameTexts[1].text = t;
			if (textTime >= 1) {
				hideText = true;
				textTime = 0;
				changeFameCounter = false;
			}
		}
		
		if (hideText && !scaleFameParent && textTime > 0.5f) {
			textTime = 0;
			scaleFameParent = true;
		}
		
		if (scaleFameParent) {
			fameParent.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, textTime * 2.5f);
		}
		
		if (hideText && textTime > 1f) {
			minusText.SetActive(false);
			fameParent.SetActive(false);
			scaleFameParent = false;
		}	
	}
	
	int LerpInt(int start, int end, float time)
	{
		int diff = start - end;
		float progress = diff * time;
		return start - (int)progress;
	}
	
	float LerpFloat(float start, float end, float time)
	{
		float diff = start - end;
		float progress = diff * time;
		return start - (float)progress;
	}
	
	#endregion
	
	#region Stage 3
	
	bool[] showMonster = new bool[3];
	bool showSpawnAnims = false;
	bool showAttacking = false;
	public Transform[] circles = new Transform[2];
	public Transform[] circlePoints = new Transform[3];
	public GameObject[] monsters = new GameObject[3];
	public GameObject[] splitCameras = new GameObject[3];
	public GameObject textCanvas;
	bool[] showCanvasText = new bool[2];
	public GameObject[] players = new GameObject[3];
	bool hasAttacked = false;
	bool knockedDown = false;
	
	public GameObject UICanvas;
	public Sprite[] playerFaces = new Sprite[3];
	
	void Stage3Update()
	{
		if (Input.GetKeyDown(KeyCode.A)) {
			//showMonster[0] = true;
			showSpawnAnims = false;
			lerpTime = 0;
			monsters[0].gameObject.SetActive(true);
			circles[0].gameObject.SetActive(true);
			circles[1].gameObject.SetActive(true);
			circles[2].gameObject.SetActive(true);
			circles[3].gameObject.SetActive(true);
			circles[4].gameObject.SetActive(true);
			circles[5].gameObject.SetActive(true);
			cam.position = cameraPoints[0].position;
			cam.rotation = cameraPoints[0].rotation;
			
			cam.gameObject.SetActive(true);
			splitCameras[0].SetActive(false);
			splitCameras[1].SetActive(false);
			splitCameras[2].SetActive(false);
			
			showCanvasText[0] = true;
			UICanvas.SetActive(false);
		}
		
		if (showCanvasText[0]) {
			lerpTime += Time.deltaTime;
		
			if (lerpTime < 2) {
				textCanvas.SetActive(true);
				textCanvas.transform.GetChild(2).GetComponent<Text>().text = "Battle Monsters...";
			}
			else {
				textCanvas.SetActive(false);
				showCanvasText[0] = false;
				showMonster[0] = true;
				lerpTime = 0;
			}
		}
		
		if (!showAttacking) {
			if (showMonster[0] ) {
				ShowMonster(0);
			}
			if (showMonster[1]) {
				ShowMonster(1);
			}
			if (showMonster[2]) {
				ShowMonster(2);
			}
			
			if (showSpawnAnims) {
				ShowSpawnAnimations();			
			}
		
			if (showCanvasText[1]) {
				lerpTime += Time.deltaTime;
			
				if (lerpTime > 2) {
					monsters[0].GetComponent<Animator>().SetBool("isCombatIdle", true);
					if (lerpTime < 4) {
						textCanvas.SetActive(true);
						textCanvas.transform.GetChild(2).GetComponent<Text>().text = "And lose...";
					}
					else {
						textCanvas.SetActive(false);
						showCanvasText[1] = false;
						lerpTime = 0;
						showAttacking = true;
						showMonster[0] = true;
						players[0].SetActive(true);
						players[1].SetActive(true);
						players[2].SetActive(true);
						
						monsters[0].transform.position = slimePoints[0].position;
						monsters[1].transform.position = slimePoints[1].position;
						monsters[2].transform.position = slimePoints[2].position;
						
						splitCameras[0].SetActive(false);
						splitCameras[1].SetActive(false);
						splitCameras[2].SetActive(false);
						cam.gameObject.SetActive(true);
						cam.position = cameraPoints[3].position;
						cam.rotation = cameraPoints[3].rotation;
						
						UICanvas.SetActive(true);
						ChangeGUI(0, 2, 75, 45, 16, 4);
					}
				}
			}
		}
		else {
			if (showMonster[0]) {
				MonsterAttack(0);
			}	
			if (showMonster[1]) {
				MonsterAttack(1);
			}
			if (showMonster[2]) {
				MonsterAttack(2);
			}
		}
	}
	
	void ShowMonster(int index)
	{
		lerpTime += Time.deltaTime;

		if (lerpTime * 0.8f < 1f) {
			circles[index * 2].position = Vector3.Lerp (circlePoints[index * 3].position, circlePoints[index * 3 + 1].position, lerpTime * 0.8f);
			circles[index * 2 + 1].position = Vector3.Lerp (circlePoints[index * 3].position, circlePoints[index * 3 + 2].position, lerpTime * 0.8f);
			
			circles[index * 2].Rotate(Vector3.back, 180 * lerpTime * 0.95f);
			circles[index * 2].GetChild(0).Rotate(Vector3.back, -180 * lerpTime * 0.95f);
			circles[index * 2 + 1].Rotate(Vector3.back, 180 * lerpTime * 0.95f);
			circles[index * 2 + 1].GetChild(0).Rotate(Vector3.back, -180 * lerpTime * 0.95f);
		}
		else {
			circles[index * 2].gameObject.SetActive(false);
			circles[index * 2 + 1].gameObject.SetActive(false);
			showMonster[index] = false;
			lerpTime = 0;
			if (index < 2) {
				showMonster[index+1] = true;
				cam.position = cameraPoints[index+1].position;
				cam.rotation = cameraPoints[index+1].rotation;
			}
			else {
				showSpawnAnims = true;
				cam.gameObject.SetActive(false);
				splitCameras[0].SetActive(true);
				splitCameras[1].SetActive(true);
				splitCameras[2].SetActive(true);
			}
		}
	}
	
	void ShowSpawnAnimations()
	{
		if (changeAnimator) {
			monsters[0].GetComponent<Animator>().SetBool("isSpawn", false);
			monsters[1].GetComponent<Animator>().SetBool("isIdle", true);
			monsters[2].GetComponent<Animator>().SetBool("isIdle", true);
			
			showSpawnAnims = false;
			changeAnimator = false;
			showCanvasText[1] = true;
			//monsters[0].GetComponent<Animator>().SetBool("isCombatIdle", true);
		}
		else {
			monsters[0].GetComponent<Animator>().SetBool("isSpawn", true);
			monsters[1].GetComponent<Animator>().SetBool("isSpawn", true);
			monsters[2].GetComponent<Animator>().SetBool("isSpawn", true);
			
			changeAnimator = true;
		}
	}
	
	void ChangeGUI(int im, int pl, int fa, int go, int he, int ma)
	{
		UICanvas.transform.GetChild(0).GetComponent<Image>().sprite = playerFaces[im];
		UICanvas.transform.GetChild(1).GetComponent<Text>().text = "Player " + pl;
		UICanvas.transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = 0.05f * he;
		UICanvas.transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount = 0.05f * he;
		UICanvas.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = he + " / 20";
		UICanvas.transform.GetChild(3).GetChild(0).GetComponent<Image>().fillAmount = 0.2f * ma;
		UICanvas.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = ma + " / 5";
		UICanvas.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = " : " + fa;
		UICanvas.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = " : " + go;
	}
	
	void UpdateGUI(int index)
	{
		float heaFill = UICanvas.transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount/ 0.05f;
		heaFill -= Random.Range(3, 6);
		
		UICanvas.transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount = 0.05f * heaFill;
		UICanvas.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = heaFill + " / 20";	
		
		if (index == 2) {
			UICanvas.transform.GetChild(2).GetChild(1).GetComponent<Image>().color = new Color(255,164,0);
		}		
	}
	
	void ChangeFill(int index)
	{
		float origFill = 0;
		
		Debug.Log(index);
		
		if (index == 0) {
			origFill = 16;
		}
		else if (index == 1) {
			origFill = 18;
		}
		else {
			origFill = 12;
		}
		
		float currFill = UICanvas.transform.GetChild(2).GetChild(1).GetComponent<Image>().fillAmount / 0.05f;
		
		UICanvas.transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = 0.05f * LerpFloat(origFill, currFill, lerpTime / 1f);
	}
	
	void MonsterAttack(int index)
	{
		lerpTime += Time.deltaTime;
		
		if (!hasAttacked) {
			if (changeAnimator) {
				monsters[index].GetComponent<Animator>().SetBool("isAttacking", false);
				changeAnimator = false;
				hasAttacked = true;
			}
			else {
				monsters[index].GetComponent<Animator>().SetBool("isAttacking", true);
				changeAnimator = true;
			}
		}
		else {
			if (knockPlayerDown) {
				if (!knockedDown) {
					players[index].GetComponent<Animator>().SetBool("isKnockedDown", true);
					players[index].GetComponent<Character_Blink>().ChangeEyes(0);
					players[index].GetComponent<Character_Blink>().ChangeMouth(0);
					UpdateGUI(index);
					knockedDown = true;
					lerpTime = 0;
				}
				else {
					if (lerpTime > 1.1f) {
						showMonster[index] = false;
						lerpTime = 0;
						knockPlayerDown = false;
						
						if (index == 0) {
							ChangeGUI(1, 4, 110, 60, 18, 5);
						}
						if (index == 1) {
							ChangeGUI(2, 1, 30, 10, 12, 2);
						}
						if (index < 2) {
							showMonster[index + 1] = true;
							hasAttacked = false;
							knockedDown = false;
							cam.position = cameraPoints[index + 4].position;
							cam.rotation = cameraPoints[index + 4].rotation;
						}
					}
					else {
						ChangeFill(index);
					}
				}				
			}	
		}
	}
	
	bool knockPlayerDown = false;
	
	public void KnockDown()
	{
		knockPlayerDown = true;
	}
	
	#endregion
	
	#region Stage 4
	bool showAttackArmour = false;
	bool loseArmour = false;
	public Image armourDim;
	bool armourChanged = false;
	public ParticleSystem armourBreak;
	bool wait = false;
	bool multipleArmourBreaks = false;
	public GameObject[] depthCameras = new GameObject[2];
	public GameObject player4Boots;
	
	void Stage4Update()
	{
		if (Input.GetKeyDown(KeyCode.A)) {
			showAttacking = true;
			lerpTime = 0;
			changeAnimator = false;
		}
		
		if (Input.GetKeyDown(KeyCode.S)) {
			monsters[0].GetComponent<Animator>().SetBool("isIdle", true);
		}
		
		if (showAttacking) {
			MonsterAttack4();
		}
		
		if (loseArmour) {
			RemoveArmour();
		}
		
		if (wait) {
			lerpTime += Time.deltaTime;
			if (lerpTime > 2) {
				wait = false;
				multipleArmourBreaks = true;
				players[0].GetComponent<Animator>().enabled = false;
				monsters[0].GetComponent<Animator>().enabled = false;
				depthCameras[0].SetActive(false);
				depthCameras[1].SetActive(true);
				cam.GetComponent<Blur>().enabled = true;
				lerpTime = 0;
			}
		}
		
		if (multipleArmourBreaks) {
			MultipleArmourBreaks();
		}
	}
	
	void MonsterAttack4()
	{
		if (!showAttackArmour) {
			if (changeAnimator) {
				monsters[0].GetComponent<Animator>().SetBool("isAttacking", false);
				changeAnimator = true;
				showAttackArmour = true;
			}
			else {
				monsters[0].GetComponent<Animator>().SetBool("isAttacking", true);
				changeAnimator = true;
			}
		}
		else {
			if (knockPlayerDown) {
				players[0].GetComponent<Animator>().SetBool("isKnockedDown", true);
				players[0].GetComponent<Character_Blink>().ChangeEyes(0);
				players[0].GetComponent<Character_Blink>().ChangeMouth(0);
				showAttacking = false;
			}
		}
	}
	
	void RemoveArmour()
	{
		lerpTime += Time.deltaTime;
		players[0].GetComponent<Animator>().enabled = false;
		
		Color dim = Color.black;
		
		if (lerpTime < 1.5f) {
			dim.a = LerpFloat(0, 0.7f, lerpTime / 1.5f);
			armourDim.color = dim;
		}
		
		if (lerpTime > 2) {
			dim.a = 0;
			armourDim.color = dim;
			players[0].GetComponent<Animator>().enabled = true;
			players[0].GetComponent<Player>().ChangeLevel(-1);
			armourBreak.Emit(30);
			loseArmour = false;
			wait = true;
			lerpTime = 0;
			
		}
	}
	
	bool[] multipleBreaks = new bool[15];
	
	void MultipleArmourBreaks()
	{
		lerpTime += Time.deltaTime;
		
		if (!multipleBreaks[0] && lerpTime > 0.4f) {
			players[1].GetComponent<Player>().ChangeLevel(-1);
			multipleBreaks[0] = true;
		}
		if (!multipleBreaks[1] && lerpTime > 0.8f) {
			players[1].GetComponent<Player>().ChangeLevel(-1);
			multipleBreaks[1] = true;
		}
		if (!multipleBreaks[2] && lerpTime > 1.2f) {
			players[1].GetComponent<Player>().ChangeLevel(-1);
			players[1].GetComponent<Character_Blink>().StartBlinking();
			multipleBreaks[2] = true;
		}
		if (!multipleBreaks[3] && lerpTime > 1.6f) {
			players[1].GetComponent<Player>().ChangeLevel(-1);
			multipleBreaks[3] = true;
		}
		if (!multipleBreaks[4] && lerpTime > 2f) {
			players[1].SetActive(false);
			players[2].SetActive(true);
			multipleBreaks[4] = true;
		}
		
		if (!multipleBreaks[5] && lerpTime > 2.8f) {
			players[2].GetComponent<Player>().ChangeLevel(-1);
			multipleBreaks[5] = true;
		}
		if (!multipleBreaks[6] && lerpTime > 3.2f) {
			players[2].GetComponent<Player>().ChangeLevel(-1);
			players[2].GetComponent<Character_Blink>().StartBlinking();
			player4Boots.SetActive(true);
			multipleBreaks[6] = true;
		}
		if (!multipleBreaks[7] && lerpTime > 3.6f) {
			players[2].GetComponent<Player>().ChangeLevel(-1);
			player4Boots.SetActive(true);
			multipleBreaks[7] = true;
		}
		if (!multipleBreaks[8] && lerpTime > 4f) {
			players[2].GetComponent<Player>().ChangeLevel(-1);
			player4Boots.SetActive(true);
			multipleBreaks[8] = true;
		}
		if (!multipleBreaks[9] && lerpTime > 4.4f) {
			players[2].SetActive(false);
			players[3].SetActive(true);
			multipleBreaks[9] = true;
		}
		
		if (!multipleBreaks[10] && lerpTime > 5.2f) {
			players[3].GetComponent<Player>().ChangeLevel(-1);
			multipleBreaks[10] = true;
			players[3].GetComponent<Character_Blink>().StartBlinking();
		}
		if (!multipleBreaks[11] && lerpTime > 5.6f) {
			players[3].GetComponent<Player>().ChangeLevel(-1);
			multipleBreaks[11] = true;
		}
		if (!multipleBreaks[12] && lerpTime > 6f) {
			players[3].GetComponent<Player>().ChangeLevel(-1);
			multipleBreaks[12] = true;
		}
		if (!multipleBreaks[13] && lerpTime > 6.4f) {
			players[3].GetComponent<Player>().ChangeLevel(-1);
			multipleBreaks[13] = true;
		}
	}
	
	public void LoseArmour()
	{
		loseArmour = true;
	}
	
	#endregion	
	
	#region Stage 5
	bool revealSplits = false;
	bool showStage5Part1 = false;
	bool showStage5Part2 = false;
	bool changedSwords = false;
	public ParticleSystem[] cardParticles;
	
	void Stage5Update()
	{
		if (Input.GetKeyDown(KeyCode.A)) {
			revealSplits = true;
			showStage5Part1 = false;
			swords[0].SetActive(true);
			swords[1].SetActive(false);
			
			cam.gameObject.SetActive(false);
			splitCameras[0].transform.parent.gameObject.SetActive(true);
			
			lerpTime = 0;
			monsters[0].GetComponent<Animator>().SetBool("isDead", false);
			monsters[0].GetComponent<Animator>().SetBool("isCombatIdle", true);
			players[0].GetComponent<Animator>().SetBool("checkSword", false);
			players[1].GetComponent<Animator>().SetBool("isIdle", true);
			players[1].GetComponent<Animator>().SetBool("isLaughing", false);
			cam.position = cameraPoints[0].position;
			cam.rotation = cameraPoints[0].rotation;
			//cam.position = cameraPoints[4].position;
			//cam.rotation = cameraPoints[4].rotation;
			showText = false;
		}
		
		if (Input.GetKeyDown(KeyCode.S)) {
			cam.gameObject.SetActive(false);
			splitCameras[0].GetComponent<Camera>().rect = new Rect(0, 1, 0, 0);
			splitCameras[1].GetComponent<Camera>().rect = new Rect(1, 1, 0, 0);
			splitCameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 0, 0);
			splitCameras[3].GetComponent<Camera>().rect = new Rect(1, 0, 0, 0);
		}
		
		if (revealSplits) {
			RevealSplits ();
		}
		
		if (wait) {
			lerpTime += Time.deltaTime;
			
			if (lerpTime > 1.5f) {
				splitCameras[0].transform.parent.gameObject.SetActive(false);
				cam.gameObject.SetActive(true);
			}
			
			if (lerpTime > 2) {
				lerpTime = 0;
				wait = false;
				showStage5Part1 = true;
				players[0].GetComponent<Animator>().SetBool("isAttacking", true);
				players[0].GetComponent<Character_Blink>().StartBlinking();
				players[1].GetComponent<Animator>().SetBool("useCard", true);
				players[2].GetComponent<Animator>().SetBool("isWalking", true);
			}
		}
		
		if (showStage5Part1) {
			lerpTime += Time.deltaTime;
			
			if (waitToRun) {
				if (lerpTime > 2f) {
					players[0].GetComponent<Animator>().enabled = true;
				}
			}
			
			if (lerpTime > 2.4f && lerpTime < 2.7f) {
				players[0].GetComponent<Character_Blink>().ChangeEyes(4);
				players[0].GetComponent<Character_Blink>().ChangeMouth(1);
			}	
			
			if (lerpTime > 3f && lerpTime < 3.5f) {
				if (lerpTime > 3.1f && lerpTime < 3.3f) {
					players[0].GetComponent<Character_Blink>().ChangeMouth(0);
					players[0].GetComponent<Character_Blink>().ChangeEyes(0);
				}
				cam.position = cameraPoints[1].position;
				cam.rotation = cameraPoints[1].rotation;
				players[1].GetComponent<Animator>().SetBool("isLaughing", true);
				players[1].GetComponent<Character_Blink>().ChangeMouth(0);
			}
			
			if (lerpTime > 5) {
				cam.position = Vector3.Lerp(cameraPoints[1].position, cameraPoints[3].position, (lerpTime - 5) * 2.25f);
				cam.rotation = Quaternion.Lerp(cameraPoints[1].rotation, cameraPoints[3].rotation, (lerpTime - 5) * 2.25f);
			}
			
			if (lerpTime > 5 && lerpTime < 5.2f) {
				players[0].GetComponent<Character_Blink>().ChangeEyes(1);
			}
			
			if (lerpTime > 7f) {
				lerpTime = 0;
				showStage5Part1 = false;
				showStage5Part2 = true;
				players[2].GetComponent<Animator>().SetBool("isWalking", true);
				cam.position = cameraPoints[4].position;
				cam.rotation = cameraPoints[4].rotation;
			}			
		}
		
		if (showStage5Part2) {		
			lerpTime += Time.deltaTime;
			
			if (cam.position != cameraPoints[5].position) {
				cam.position = Vector3.Lerp(cameraPoints[4].position, cameraPoints[5].position, lerpTime / 1.75f);
				cam.rotation = Quaternion.Lerp(cameraPoints[4].rotation, cameraPoints[5].rotation, lerpTime / 1.75f);
			}
			
			if (lerpTime > 0.7f && lerpTime < 1f) {
				players[3].GetComponent<Animator>().SetBool("useCard", true);
			}
			
			if (lerpTime > 0.8f && lerpTime < 1) {
				players[2].GetComponent<Character_Blink>().StartBlinking();
			}
			
			if (players[2].transform.position != playerPoints[1].position) {
				players[2].GetComponent<Animator>().SetBool("isWalking", true);
				players[2].transform.position = Vector3.Lerp(playerPoints[0].position, playerPoints[1].position, lerpTime / 2.5f);
			}
			else {
				players[2].GetComponent<Animator>().SetBool("isShocked", true);
				players[2].GetComponent<Character_Blink>().ChangeEyes(4);
				players[2].GetComponent<Character_Blink>().ChangeMouth(0);
			}
			
			if (lerpTime > 2.5f && lerpTime < 6) {
				players[3].GetComponent<Animator>().SetBool("isWalking", true);
				players[3].transform.position = Vector3.Lerp(playerPoints[2].position, playerPoints[3].position, (lerpTime - 2.5f) / 1);
				players[3].transform.rotation = Quaternion.Lerp(playerPoints[2].rotation, playerPoints[3].rotation, (lerpTime - 2.5f) * 3);
			}
			
			if (lerpTime < 2.5f) {
				circles[0].localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.25f, lerpTime/1.5f);
				circles[1].localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.25f, lerpTime/1.5f);
			}
			
			if (!showText) {
				if ((lerpTime - 1.75f) * 0.8f > 0) {
					circles[0].position = Vector3.Lerp (circlePoints[0].position, circlePoints[1].position, (lerpTime - 1.75f) * 0.8f);
					circles[1].position = Vector3.Lerp (circlePoints[0].position, circlePoints[2].position, (lerpTime - 1.75f) * 0.8f);
					
					circles[0].Rotate(Vector3.back, 180 * lerpTime * 0.95f);
					circles[0].GetChild(0).Rotate(Vector3.back, -180 * lerpTime * 0.95f);
					circles[1].Rotate(Vector3.back, 180 * lerpTime * 0.95f);
					circles[1].GetChild(0).Rotate(Vector3.back, -180 * lerpTime * 0.95f);
					
					players[3].GetComponent<Animator>().SetBool("useCard", false);
					cardParticles[1].Stop();
				}
				if ((lerpTime - 1.75f) * 0.8f > 1) {
					circles[0].gameObject.SetActive(false);
					circles[1].gameObject.SetActive(false);
					monsters[1].GetComponent<Animator>().SetBool("isSpawn", true);
					showText = true;
					lerpTime = 0;
				}
			}
			if (showText) {
				monsters[1].GetComponent<Animator>().SetBool("isSpawn", false);
				monsters[1].GetComponent<Animator>().SetBool("isIdle", true);
			}
			
			if (lerpTime > 6) {
				showStage5Part2 = false;
			}
		}
	}
	
	void RevealSplits()
	{
		lerpTime += Time.deltaTime;
		lerpTime += Time.deltaTime;
		
		Rect start = new Rect(0, 1, 0, 0);
		Rect end = new Rect(0, 0.5f, 0.5f, 0.5f);
		
		splitCameras[0].GetComponent<Camera>().rect = LerpRect(start, end, lerpTime);
		
		start = new Rect(1, 1, 0, 0);
		end = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
		
		splitCameras[1].GetComponent<Camera>().rect = LerpRect(start, end, lerpTime);
		
		start = new Rect(0, 0, 0, 0);
		end = new Rect(0, 0, 0.5f, 0.5f);
		
		splitCameras[2].GetComponent<Camera>().rect = LerpRect(start, end, lerpTime);
		
		start = new Rect(1, 0, 0, 0);
		end = new Rect(0.5f, 0, 0.5f, 0.5f);
		
		splitCameras[3].GetComponent<Camera>().rect = LerpRect(start, end, lerpTime);
		
		if (lerpTime > 1) {
			revealSplits = false;
			wait = true;
			lerpTime = 0;
		}
	}
	
	void ShowStage5Part1()
	{
		
	}
	
	
	public void ChangeSwords()
	{
		swords[0].SetActive(false);
		swords[1].SetActive(true);
		
		armourBreak.Emit(40);
		players[0].GetComponent<Animator>().SetBool("checkSword", true);
		players[0].GetComponent<Animator>().SetBool("isAttacking", false);
		players[0].GetComponent<Animator>().enabled = false;
		cardParticles[0].Stop();
		waitToRun = true;
	}
	
	public void DealDamage()
	{
		monsters[0].GetComponent<Animator>().SetBool("isDead", true);
	}
	
	public void ShowCard()
	{
		if (showStage5Part1) {
			players[1].GetComponent<Animator>().SetBool("useCard", false);
			cardParticles[0].Play();
		}
		else {
			players[3].GetComponent<Animator>().SetBool("useCard", false);
			cardParticles[1].Play();
		}		
	}
	
	Rect LerpRect(Rect st, Rect en, float time)
	{
		Rect ret = new Rect();
		
		if (lerpTime >= 1) {
			return en;
		}
		
		ret.x = st.x - Mathf.Abs((en.x - st.x) * time);
		ret.y = st.y - Mathf.Abs((en.y - st.y) * time);
		ret.height = (en.height - st.height) * time;
		ret.width = (en.width - st.width) * time;
		
		return ret;
	}
	
	#endregion
	
	#region Stage 6
	
	int clip = 0;
	float clipTime = 1f;
	public Chest chest;
	public GameObject Player4Tome;
	public GameObject fireCircle;
	public GameObject fireball;
	public bool moveFireball = false;
	
	void Stage6Update()
	{
		if (Input.GetKeyDown(KeyCode.A)) {
			moveCamera = true;
			ChangePositionRotation(cam, 0, 0);
			ChangePositionRotation(players[0].transform, 0, 1);
			lerpTime = 0;
			slimeLerpTime = 0;
			clip = 0;
			Player4Tome.SetActive(false);
			players[0].GetComponent<Animator>().SetBool("isWalking", true);
			players[2].GetComponent<Animator>().SetBool("isWalking", true);
			monsters[0].GetComponent<Animator>().SetBool("isIdle", true);
			monsters[1].GetComponent<Animator>().SetBool("isIdle", true);
			fireball.SetActive(false);
			fireCircle.SetActive(false);
		}
		
		if (moveCamera) {
			lerpTime += Time.deltaTime;
		
			switch (clip) {
				case 0:
					Clip1();
					break;
				case 1:
					Clip2();
					break;
				
				case 2:
					Clip3();
					break;
				
				case 3:
					Clip4();
					break;
				
				case 4:
					Clip5();
					break;
				
				case 5:
					Clip6();
					break;
				
				case 6:
					Clip7();
					break;
				
				case 7:
					Clip8();
					break;
				
				case 8:
					Clip9();
					break;
				
				case 9:
					Clip10();
					break;
				
				case 10:
					Clip11();
					break;
			};
		}
		
	}
	
	void Clip1()
	{
		if (lerpTime >= clipTime) {
			++clip;
			lerpTime = 0;
			slimeLerpTime = 0;
			ChangePositionRotation(cam, 2, 0);
			ChangePositionRotation(players[1].transform, 2, 1);
			ChangePositionRotation(monsters[0].transform, 0, 2);
			return;
		}
		
		if (lerpTime >= clipTime * 0.75f) {
			players[1].GetComponent<Animator>().SetBool("isAttacking", true);
		}
		
		players[0].transform.position = Vector3.Lerp(playerPoints[0].position, playerPoints[1].position, lerpTime / 2);
		
		LerpCamera(cam, cameraPoints[0], cameraPoints[1], lerpTime / 1.9f, 2);
	}
	
	void Clip2()
	{
		if (lerpTime >= clipTime) {
			++clip;
			lerpTime = 0;
			slimeLerpTime = 0;
			changeAnimator = true;
			
			players[0].GetComponent<Animator>().SetBool("isWalking", false);
			ChangePositionRotation(cam, 3, 0);
			ChangePositionRotation(players[3].transform, 3, 1);
			Vector2[] changes = new Vector2[1] {new Vector2(1, 20) };
			GameObject.Find("Scroll Event").GetComponent<TESTScrollRoll>().CreateEvent(0, "Carriage Attack", "A passing cart driver hurls an insult at you so you attack his caravan. Turns out the caravan was transported kidnapped children.", changes, false, null);
			
			return;
		}
	}
	
	void Clip3()
	{
		if (lerpTime >= clipTime) {
			++clip;
			lerpTime = 0;
			slimeLerpTime = 0;
			changeAnimator = true;
			
			ChangePositionRotation(cam, 4, 0);
			ChangePositionRotation(players[2].transform, 4, 1);
			players[2].GetComponent<Animator>().SetBool("isWalking", true);
			players[3].GetComponent<Animator>().SetBool("isIdle", false);
			
			GameObject.Find("Scroll Event").SetActive(false);
			return;
		}
		
		if (lerpTime >= clipTime / 2) {
			players[3].GetComponent<Animator>().SetBool("isAttacking", true);
		}
	}
	
	void Clip4()
	{
		if (lerpTime >= clipTime) {
			++clip;
			lerpTime = 0;
			slimeLerpTime = 0;
			changeAnimator = true;
			
			ChangePositionRotation(cam, 6, 0);
			ChangePositionRotation(monsters[1].transform, 1, 2);
			players[2].GetComponent<Animator>().SetBool("isWalking", false);
			return;
		}
		
		if (lerpTime >= clipTime / 2) {
			monsters[1].GetComponent<Animator>().SetBool("isVictory", true);
			players[2].GetComponent<Character_Blink>().StartBlinking();
		}
		
		players[2].transform.position = Vector3.Lerp(playerPoints[4].position, playerPoints[5].position, lerpTime / 2);
		
		LerpCamera(cam, cameraPoints[4], cameraPoints[5], lerpTime / 1.9f, 2);
	}
	
	void Clip5()
	{
		if (lerpTime >= clipTime) {
			++clip;
			lerpTime = 0;
			slimeLerpTime = 0;
			
			ChangePositionRotation(cam, 7, 0);
			ChangePositionRotation(players[1].transform, 7, 1);
			ChangePositionRotation(players[3].transform, 6, 1);
			players[1].GetComponent<Animator>().SetBool("isCombatIdle", true);
			players[3].GetComponent<Animator>().SetBool("isAttacking", true);
			Player4Tome.SetActive(true);
			fireCircle.SetActive(true);
			return;
		}
	}
	
	void Clip6()
	{
		if (lerpTime >= clipTime) {
			++clip;
			lerpTime = 0;
			slimeLerpTime = 0;
			changeAnimator = true;
			
			ChangePositionRotation(cam, 8, 0);
			ChangePositionRotation(players[0].transform, 8, 1);
			ChangePositionRotation(players[2].transform, 9, 1);
			
			players[3].GetComponent<Animator>().SetBool("isAttacking", false);
			players[2].GetComponent<Animator>().SetBool("isCombatIdle", true);
			monsters[2].GetComponent<Animator>().SetBool("isVictory", true);
			fireball.SetActive(false);
			moveFireball = false;
			return;
		}
		
		fireCircle.transform.localScale = Vector3.Lerp(Vector3.one * 0.35f, Vector3.one * 0.5f, lerpTime * 3);
		
		if (moveFireball) {
			slimeLerpTime += Time.deltaTime;
			fireball.transform.position = Vector3.Lerp(fireCircle.transform.position, fireCircle.transform.GetChild(0).position, slimeLerpTime * 3);
		}
		
		if (lerpTime >= clipTime * 0.3f) {
			players[1].GetComponent<Animator>().SetBool("isKnockedDown", true);
			players[1].GetComponent<Character_Blink>().ChangeEyes(0);
			players[1].GetComponent<Character_Blink>().ChangeMouth(0);
		}
		
		if (lerpTime >= clipTime * 0.5f) {
			players[0].GetComponent<Animator>().SetBool("isAttacking", true);
		}
	}
	
	void Clip7()
	{
		if (lerpTime >= clipTime) {
			++clip;
			lerpTime = 0;
			slimeLerpTime = 0;
			changeAnimator = true;
			
			ChangePositionRotation(cam, 9, 0);
			players[0].GetComponent<Animator>().SetBool("isAttacking", false);
			players[2].GetComponent<Animator>().SetBool("isCombatIdle", false);
			players[0].GetComponent<Animator>().SetBool("isIdle", true);
			players[1].GetComponent<Animator>().SetBool("isWalking", true);
			ChangePositionRotation(monsters[2].transform, 2, 2);
			return;
		}
	}
	
	void Clip8()
	{
		if (lerpTime >= clipTime) {
			++clip;
			lerpTime = 0;
			slimeLerpTime = 0;
			changeAnimator = true;
			
			ChangePositionRotation(cam, 10, 0);
			chest.GetComponent<Animation>().Play("Open");
			ChangePositionRotation(players[0].transform, 10, 1);
			players[2].GetComponent<Animator>().SetBool("isKnockedDown", false);
			players[1].GetComponent<Animator>().SetBool("isKnockedDown", false);
			players[2].GetComponent<Character_Blink>().ChangeEyes(0);
			players[2].GetComponent<Character_Blink>().ChangeMouth(0);
			GameObject.Find("Rapier").SetActive(false);
			return;
		}
	}
	
	void Clip9()
	{
		if (lerpTime >= clipTime) {
			++clip;
			lerpTime = 0;
			slimeLerpTime = 0;
			changeAnimator = true;
			
			ChangePositionRotation(cam, 11, 0);
			ChangePositionRotation(players[1].transform, 11, 1);
			return;
		}
	}
	
	void Clip10()
	{
		if (lerpTime >= clipTime) {
			++clip;
			lerpTime = 0;
			slimeLerpTime = 0;
			changeAnimator = true;
			
			ChangePositionRotation(cam, 13, 0);
			ChangePositionRotation(players[2].transform, 13, 1);
			ChangePositionRotation(players[3].transform, 14, 1);
			players[2].GetComponent<Animator>().SetBool("isAttacking", true);
			players[3].GetComponent<Animator>().SetBool("isAttacking", true);
			fireCircle.SetActive(true);
			return;
		}
		
		if (lerpTime >= clipTime * 0.2f) {
			players[3].GetComponent<Animator>().SetBool("isAttacking", true);
		}
		
		players[1].transform.position = Vector3.Lerp(playerPoints[11].position, playerPoints[12].position, lerpTime / 2);
		
		LerpCamera(cam, cameraPoints[11], cameraPoints[12], lerpTime / 1.9f, 2);
	}
	
	void Clip11()
	{
		fireCircle.transform.localScale = Vector3.Lerp(Vector3.one * 0.35f, Vector3.one * 0.5f, lerpTime * 2);
		
		if (moveFireball) {
			slimeLerpTime += Time.deltaTime;
			fireball.transform.position = Vector3.Lerp(fireCircle.transform.position, fireCircle.transform.GetChild(0).position, slimeLerpTime * 3);
		}
		
		if (lerpTime >= 1.5f) {
			UICanvas.SetActive(true);
		}
	}
	
	public void AttackPlayer()
	{
		if (clip == 5) {
			players[1].GetComponent<Animator>().SetBool("isKnockedDown", true);
			players[1].GetComponent<Character_Blink>().ChangeEyes(0);
			players[1].GetComponent<Character_Blink>().ChangeMouth(0);
		}
		if (clip == 6) {
			players[2].GetComponent<Animator>().SetBool("isKnockedDown", true);
			players[2].GetComponent<Character_Blink>().ChangeEyes(4);
			players[2].GetComponent<Character_Blink>().ChangeMouth(1);
		}
		if (clip == 5 || clip == 10) {
			fireball.SetActive(true);
		}
	}
	
	void ChangePositionRotation(Transform trans, int index, int arr)
	{
		if (arr == 0) {
			trans.position = cameraPoints[index].position;
			trans.rotation = cameraPoints[index].rotation;
			return;
		}
		
		if (arr == 1) {
			trans.position = playerPoints[index].position;
			trans.rotation = playerPoints[index].rotation;
			return;
		}
		
		trans.position = slimePoints[index].position;
		trans.rotation = slimePoints[index].rotation;
	}
	
	void LerpCamera(Transform ca, Transform st, Transform ed, float ti, int ro)
	{
		if (ro == 0 || ro == 2) {
			ca.position = Vector3.Lerp(st.position, ed.position, ti);
		}
		if (ro == 1 || ro == 2) {
			ca.rotation = Quaternion.Lerp(st.rotation, ed.rotation, ti);
		}
	}
	
	#endregion
	
	
	#region Update
	// Update is called once per frame
	void Update ()
	{
		if (stage == 0) {
			Stage1Update();
		}	
		
		if (stage == 1) {
			Stage2Update();
		}
		
		if (stage == 2) {
			Stage3Update();
		}
		
		if (stage == 3) {
			Stage4Update();
		}
		
		if (stage == 4) {
			Stage5Update();
		}
		
		if (stage == 5) {
			Stage6Update();
		}	
	}
	#endregion
}
