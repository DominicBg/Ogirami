using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour {

	public int maxLife = 5;
	public int currentLife;

	public int score;
	public Text scoreText;
	public Text highScoreText;

	[SerializeField]Image audioIcon;
	[SerializeField]Image musicIcon;
	[SerializeField]GameObject pressStart;

	public static bool ResetSpawnTimer = false;
	public static bool inMenu = true;
	bool preIngame = false;
	public GameObject menuGameObject;
	public GameObject ingameUI;
	public GameObject creditUI;
	public GameObject scoreUI;


	[SerializeField]GameObject[] introUI_Texts_icons;
	[SerializeField]Transform Arc;
	[SerializeField]Color lifeOrbColor;
	[SerializeField]Color lifeOrbColorDestroyed;

	public Ship ship;
	[SerializeField]AudioClip sfxDamageFromNormal;
	[SerializeField]AudioClip sfxDamageFromFractal;

	[SerializeField]AudioClip sfxUIclick1;
	[SerializeField]AudioClip sfxUIclick2;
	[SerializeField]AudioClip sfxUIstartGame;
	[SerializeField]AudioClip sfxUIDeath;
	[SerializeField]AudioClip sfxUIShutDown;

	[SerializeField]AudioClip musicNormal;
	[SerializeField]AudioClip musicFractal;
	// Use this for initialization




	void Start ()
	{
		GameSound.EnableAudio ();

		GameSound.SetMusicChannel (2);
		GameSound.SetMusicIntoChannel (musicNormal,0,1,true);
		GameSound.SetMusicIntoChannel (musicFractal,1,0,true);
		GameSound.EnableToggleMusicVolume (3);
		GameSound.EnableToggleSoundVolume (3);

		LoadToggleVolume ();
		StarMenu ();
	}
	void Update()
	{
		if (inMenu) 
		{
			Color color =  HSBColor.ToColor
			(
				new HSBColor
				( 
					Mathf.Lerp (.6f, 1f, (Mathf.Sin(Time.timeSinceLevelLoad * 0.2f) + 1) / 2),
					.4f,
					1f
				)
			);
			Camera.main.backgroundColor = color;
			ship.light.color = color;
		}
	}
	public void ToggleSoundVolume()
	{
		GameSound.ToggleSoundVolume();
		GameSound.PlaySound (sfxDamageFromNormal, true);
		SetBtnColor ();
	//	SaveToggleVolume ();
	}
	public void ToggleMusicVolume()
	{
		GameSound.ToggleMusicVolume();
		SetBtnColor ();
	//	SaveToggleVolume ();
	}
	void SetBtnColor()
	{
		float t_sound = GameSound.GetToggleValueSound();
		float t_music = GameSound.GetToggleValueMusic();

		audioIcon.color = new Color (t_sound, t_sound, t_sound, 1);
		musicIcon.color = new Color (t_music, t_music, t_music, 1);

	}
	void LoadToggleVolume()
	{
		if (PlayerPrefs.GetInt ("new game") == 0)
		{
			PlayerPrefs.SetInt ("new game", 1);
			PlayerPrefs.SetFloat ("GameSound_ToggleSoundVolume", 1);
			PlayerPrefs.SetFloat ("GameSound_ToggleMusicVolume", 1);
			PlayerPrefs.Save ();
		}

		SetBtnColor ();
	}

	public void OpenCreditMenu()
	{
		creditUI.SetActive (true);
		GameSound.PlaySound (sfxUIclick1, true);

	}
	public void CloseCreditMenu()
	{
		creditUI.SetActive (false);
		GameSound.PlaySound (sfxUIclick2, true);

	}
	// Update is called once per frame
	public void StarMenu()
	{
		menuGameObject.SetActive (true);
		ingameUI.SetActive (false);
		inMenu = true;
		highScoreText.text = PlayerPrefs.GetInt ("Highscore").ToString ();
		scoreUI.SetActive (false);
		pressStart.SetActive (true);

		preIngame = false;
	}
	public void StartGame()
	{	
		if(!preIngame)
			StartCoroutine (delayStartGame ());
	}

	public void GiveScore(int points)
	{
		score += points;
		scoreText.text = score.ToString();
	}
	public void GetHit(bool fromNormalEnemy)
	{
		if (fromNormalEnemy)
			GameSound.PlaySound (sfxDamageFromNormal,true);
		else
			GameSound.PlaySound (sfxDamageFromFractal,true);

		GameEffect.FlashCamera (new Color(1,0,0,0.5f), .3f);
		GameEffect.Shake (Camera.main.gameObject,.2f);
	}
	public void ChangeLife(int damage)
	{
		currentLife += damage;


		if (currentLife <= 0) 
		{
			Debug.Log ("game over");
			CompareHighScore ();
			StartCoroutine (delayEndGame ());
			//StarMenu ();
		}
			
		for (int i = 0; i < Arc.childCount; i++)
		{
			if (currentLife-1 >= i)//got the life
				Arc.GetChild(i).GetComponent<MeshRenderer> ().material.SetColor ("_Color",lifeOrbColor);
				//Arc.GetChild(i).gameObject.SetActive (true);
			else
				Arc.GetChild(i).GetComponent<MeshRenderer> ().material.SetColor ("_Color", lifeOrbColorDestroyed);
				//Arc.GetChild(i).gameObject.SetActive (false);

		}

	}
	void CompareHighScore()
	{
		if (score > PlayerPrefs.GetInt ("Highscore")) 
		{
			PlayerPrefs.SetInt ("Highscore", score);
			PlayerPrefs.Save ();
		}
	}

	IEnumerator delayStartGame()
	{
		preIngame = true;
		ship.ShutDown (false);

		pressStart.SetActive (false);
		GameSound.PlaySound (sfxUIstartGame, true);
		GameEffect.Shake ();
		GetComponent<Animator> ().Play ("CameraAnimation");

		foreach (GameObject text in introUI_Texts_icons)
			text.GetComponent<Animator> ().Play ("fadeOut");

		yield return new WaitForSeconds (.2f);

		GameSound.PlaySound (GetComponent<World_Manager>().sfxSwitchWorld, true,100,1);

		score = 0;
		currentLife = maxLife;
		GiveScore (0);
		ChangeLife (0);


		yield return new WaitForSeconds (1);
		GameSound.PlaySound (GetComponent<World_Manager>().sfxSwitchWorld, true,.5f,1.2f);
		yield return new WaitForSeconds (1);
		inMenu = false;

		menuGameObject.SetActive (false);
		ingameUI.SetActive (true);
	
		ship.StartGameShip ();
		GetComponent<Animator> ().enabled = false;
		ResetSpawnTimer = true;
	}
	IEnumerator delayEndGame()
	{
		
		ingameUI.SetActive (false);
		inMenu = true;
		ship.EndGameShip ();
		GameSound.PlaySound (sfxUIDeath, true);

		yield return new WaitForSeconds (1);
	
		GameSound.PlaySound (sfxUIShutDown, true);
		ship.ShutDown (true);
		yield return new WaitForSeconds (2);
		GameEffect.FlashCamera (Color.black,1);

		yield return new WaitForSeconds (.5f);
		GetComponent<World_Manager>().returnToNormal ();
		GetComponent<Wave>().ResetWave ();
		Camera.main.GetComponent<Animator> ().enabled = true;
		Camera.main.GetComponent<Animator> ().Play ("cameraIdle");
		yield return new WaitForSeconds (1);
		ShowCurrentAndBestScore ();
		yield return new WaitForSeconds (3);
		foreach (Transform text in scoreUI.transform)
			text.GetComponent<Animator> ().Play ("fadeOut");
		yield return new WaitForSeconds (1);
		StarMenu ();
	}
	void ShowCurrentAndBestScore()
	{
		scoreUI.SetActive (true);
		scoreUI.transform.GetChild(0).GetComponent<Text>().text = "Highest Score \n" + PlayerPrefs.GetInt ("Highscore").ToString ();
		scoreUI.transform.GetChild(1).GetComponent<Text>().text =  "Score \n" + score.ToString();
	}
}
