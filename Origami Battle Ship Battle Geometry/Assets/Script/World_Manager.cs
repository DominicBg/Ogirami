using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class World_Manager : MonoBehaviour {
	public enum EnumWorld 
	{
		Normal,
		Fractal
	};

	public static EnumWorld currentWorld;
	static public bool canChange = true;

	[SerializeField]Transform Null_World;
	[SerializeField]GameObject FractalWall;
	[SerializeField]GameObject NormalCloud;


	[SerializeField]Transform enemyContainerNormal;
	[SerializeField]Transform enemyContainerFractal;

	[SerializeField]Color COLORNORMAL;
	[SerializeField]Color COLORFRACTAL;


	float t_lerp_rotation;
	float z_startRotation;
	float z_endRotation;

	public AudioClip sfxSwitchWorld;


	public void SwitchWorld()
	{
		if (canChange) 
		{
			canChange = false;
			StartCoroutine (delayChangeWorld ());
			t_lerp_rotation = 0;

			z_startRotation = Null_World.eulerAngles.z;
			z_endRotation = Null_World.eulerAngles.z + 180; 


			if (currentWorld == EnumWorld.Fractal)
				currentWorld = EnumWorld.Normal;
			else
				currentWorld = EnumWorld.Fractal;
			
		}
	}

	IEnumerator delayChangeWorld()
	{
		GameSound.PlaySound (sfxSwitchWorld,true);
		GetComponent<MotionBlur> ().enabled = true;

		if(currentWorld == EnumWorld.Normal)
			GameSound.SetTransition (0, 1, 1);
		else
			GameSound.SetTransition (1, 0, 1);


		yield return new WaitForSeconds (.2f);

		if(currentWorld == EnumWorld.Fractal)
			GameEffect.FlashCamera (new Color(0,0,0,0.8f), .8f);
		else
			GameEffect.FlashCamera (new Color(1,1,1,0.8f), .8f);
		
		yield return new WaitForSeconds (.3f);

		ChangeCameraBackGround ();

		yield return new WaitForSeconds (.5f);

		canChange = true;
		GetComponent<MotionBlur> ().enabled = false;

	}
	void Start()
	{
		ChangeCameraBackGround ();
	}
	// Update is called once per frame
	void Update () 
	{
		RotationWorld ();
	}
	void ChangeCameraBackGround()
	{

        if (currentWorld == EnumWorld.Normal)
        {
            Camera.main.GetComponent<Camera>().backgroundColor = COLORNORMAL;
        //    Camera.main.GetComponent<NoiseAndScratches>().enabled = false;
       //     Camera.main.GetComponent<ScreenOverlay>().enabled = false;
			FractalWall.SetActive(false);
			NormalCloud.SetActive (true);

			foreach (Transform enemy in enemyContainerNormal)
				enemy.GetComponent<EnemyScript> ().ShowEnemy (true);
			foreach (Transform enemy in enemyContainerFractal) 
				enemy.GetComponent<EnemyScript> ().ShowEnemy (false);
			
        }
        else
        {
            Camera.main.GetComponent<Camera>().backgroundColor = COLORFRACTAL;
         //   Camera.main.GetComponent<NoiseAndScratches>().enabled = true;
        //    Camera.main.GetComponent<ScreenOverlay>().enabled = true;
			FractalWall.SetActive(true);
			NormalCloud.SetActive (false);


			foreach (Transform enemy in enemyContainerNormal)
				enemy.GetComponent<EnemyScript> ().ShowEnemy (false);
			foreach (Transform enemy in enemyContainerFractal) 
				enemy.GetComponent<EnemyScript> ().ShowEnemy (true);
        }
           
		

	}
	void RotationWorld()
	{
		if (!canChange)
		{
			t_lerp_rotation += Time.deltaTime;

			Null_World.eulerAngles = new Vector3
				(
					Null_World.eulerAngles.x,
					Null_World.eulerAngles.y,
					Mathf.Lerp (z_startRotation, z_endRotation, Mathf.Tan(t_lerp_rotation))
				);


		}
	}
	public void returnToNormal()
	{
		if (currentWorld == EnumWorld.Fractal) 
		{
			
			Null_World.eulerAngles = new Vector3 (
				Null_World.eulerAngles.x,
				Null_World.eulerAngles.y,
				Null_World.eulerAngles.z + 180
			);
			currentWorld = EnumWorld.Normal;
			ChangeCameraBackGround ();
			GameSound.SetTransition (0, 1, 1);
			GameSound.SetTransition (1, 0, 1);

		}
	}

}
