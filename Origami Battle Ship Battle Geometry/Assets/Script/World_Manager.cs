using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class World_Manager : MonoBehaviour {
	enum EnumWorld 
	{
		Normal,
		Fractal
	};

	[SerializeField]EnumWorld currentWorld;
	static public bool canChange = true;

	[SerializeField]Transform Null_World;


	[SerializeField]Color COLORNORMAL;
	[SerializeField]Color COLORFRACTAL;


	float t_lerp_rotation;
	float z_startRotation;
	float z_endRotation;

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
		GetComponent<MotionBlur> ().enabled = true;
		yield return new WaitForSeconds (.5f);
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
			Camera.main.GetComponent<Camera> ().backgroundColor = COLORNORMAL;
		else
			Camera.main.GetComponent<Camera> ().backgroundColor = COLORFRACTAL;
		

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
					Mathf.Lerp (z_startRotation, z_endRotation, t_lerp_rotation)
				);


		}
	}
}
