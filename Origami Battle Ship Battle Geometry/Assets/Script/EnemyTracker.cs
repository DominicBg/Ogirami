using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTracker : MonoBehaviour {

	[SerializeField]Transform normalContainer;
	[SerializeField]Transform fractalContainer;
	public Transform normalUI;
	public Transform fractalUI;
	[SerializeField]Color maxColorNormal;
	[SerializeField]Color maxColorFractal;

	// Update is called once per frame

	float miniTimer = 0;
	void Update () 
	{
		if (!Game_Manager.inMenu)
		{
			if (World_Manager.canChange) 
			{
				miniTimer += Time.deltaTime * 5;
				if (miniTimer > 1) 
				{
					ScanEnemyPos ();
					miniTimer = 0;
				}
			} 
			else
			{
				normalUI.gameObject.SetActive (false);
				fractalUI.gameObject.SetActive (false);
			}
		}


	}
	void ScanEnemyPos()
	{
		//render other world
		if (World_Manager.currentWorld == World_Manager.EnumWorld.Fractal)
		{
			normalUI.gameObject.SetActive (true);
			fractalUI.gameObject.SetActive (false);

			foreach (Transform enemy in normalContainer) 
			{
				UpdateTracking (enemy.gameObject,maxColorNormal);
			}
		}
		else if (World_Manager.currentWorld == World_Manager.EnumWorld.Normal) 
		{
			normalUI.gameObject.SetActive (false);
			fractalUI.gameObject.SetActive (true);

			foreach (Transform enemy in fractalContainer) 
			{
				UpdateTracking (enemy.gameObject,maxColorFractal);
			}
		}
	}
	void UpdateTracking(GameObject enemy, Color col)
	{
		EnemyScript enemyScript = enemy.GetComponent<EnemyScript> ();
		Vector3 enemyPos = enemy.transform.position;
		Transform radarPos = enemyScript.radarTriangle.transform;

		float distance = enemyScript.CalculateDistance ();

		float size = 1 - (distance / enemyScript.initiatlDistance) ; ///GameMath.Map (distance, enemyScript.initiatlDistance,2, 0, 1);
		if (size < 0)
			size = 0;
		else if (size > 1)
			size = 1;
		radarPos.localScale = new Vector2 (size*1.5f, size*1.5f);

		//float xPos =  GameMath.Map ( (enemyPos.x - enemyScript.destination.transform.position.x) * 2 , enemyScript.initiatlDistance, 0,0, Screen.width/2);
		float xPos = (enemyScript.destination.transform.position.x - enemyPos.x) * 20 + (Screen.width/2);// * (Screen.width / 10);
		radarPos.position = new Vector2 (xPos ,0);

		radarPos.GetComponent<Image>().color =
			new Color
			(
				Mathf.Lerp(1,col.r,size),
				Mathf.Lerp(1,col.g,size),
				Mathf.Lerp(1,col.b,size),
				.6f
			);	
	}
}
