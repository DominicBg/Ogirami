using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour {

	public int maxLife = 5;
	public int currentLife;
	public Text currentLifeText;

	public int score;
	public Text scoreText;
	public Text highScoreText;

	[SerializeField]Image audioIcon;
	[SerializeField]Image musicIcon;

	public static bool inMenu = true;
	public GameObject menuGameObject;
	public GameObject ingameUI;
	public GameObject creditUI;

	float toggleSoundRatio;
	float toggleMusicRatio;


	[SerializeField]Ship ship;
	[SerializeField]AudioClip sfxDamageFromNormal;
	[SerializeField]AudioClip sfxDamageFromFractal;

	[SerializeField]AudioClip sfxUIclick1;
	[SerializeField]AudioClip sfxUIclick2;
	[SerializeField]AudioClip sfxUIDeath;

	[SerializeField]AudioClip musicNormal;
	[SerializeField]AudioClip musicFractal;
	// Use this for initialization
	void Start ()
	{
		GameSound.EnableAudio ();
		GameSound.EnableToggleSoundVolume (3);
		GameSound.EnableToggleMusicVolume (3);

		GameSound.SetMusicChannel (2);
		GameSound.SetMusicIntoChannel (musicNormal,0,1,true);
		GameSound.SetMusicIntoChannel (musicFractal,1,0,true);

		LoadToggleVolume ();
		StarMenu ();
	}
	public void ToggleSoundVolume()
	{
		toggleSoundRatio = GameSound.ToggleSoundVolume();
		GameSound.PlaySound (sfxDamageFromNormal, true);
		SetBtnColor ();
		SaveToggleVolume ();
	}
	public void ToggleMusicVolume()
	{
		toggleMusicRatio = GameSound.ToggleMusicVolume();
		SetBtnColor ();
		SaveToggleVolume ();
	}
	void SetBtnColor()
	{
		audioIcon.color = new Color (1, 1, 1, toggleSoundRatio);
		musicIcon.color = new Color (1, 1, 1, toggleMusicRatio);

	}
	void SaveToggleVolume()
	{
		PlayerPrefs.SetFloat ("GameSound_ToggleSoundVolume", toggleSoundRatio);
		PlayerPrefs.SetFloat ("GameSound_ToggleMusicVolume", toggleMusicRatio);
		PlayerPrefs.Save ();
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

		toggleSoundRatio = PlayerPrefs.GetFloat ("GameSound_ToggleSoundVolume");
		toggleMusicRatio = PlayerPrefs.GetFloat ("GameSound_ToggleMusicVolume");

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
	}
	public void StartGame()
	{	
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
		currentLifeText.text = currentLife.ToString();


		if (currentLife <= 0) 
		{
			Debug.Log ("game over");
			CompareHighScore ();
			StartCoroutine (delayEndGame ());
			//StarMenu ();
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
		GameSound.PlaySound (sfxUIclick1, true);

		menuGameObject.SetActive (false);
		yield return new WaitForSeconds (1);
		ingameUI.SetActive (true);
		inMenu = false;
		score = 0;
		currentLife = maxLife;
		GiveScore (0);
		ChangeLife (0);
		ship.StartGameShip ();
	}
	IEnumerator delayEndGame()
	{
		ingameUI.SetActive (false);
		inMenu = true;
		GameEffect.FlashCamera (Color.black, 1);
		ship.EndGameShip ();
		GameSound.PlaySound (sfxUIDeath, true);
		yield return new WaitForSeconds (3);
		StarMenu ();
	}
}
