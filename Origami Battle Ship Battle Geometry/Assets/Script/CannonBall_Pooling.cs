using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall_Pooling : MonoBehaviour {

	[SerializeField]int howManyCannonBall;
	[SerializeField]GameObject prefabCannonBall;
	GameObject[] cannonBallList;
	int indexCpt = 0;
	[SerializeField]Transform parent;
	[HideInInspector]public Ship shipRef;
	[SerializeField]Transform UI_CannonBall_null;
	[SerializeField]GameObject prefab_UI_CannonBall;
	[SerializeField]float distanceBetweenUI_Cannonball;
	// Use this for initialization

	void Start()
	{
		shipRef = GetComponent<Ship> ();
	}
	public void Pooling () 
	{
		GameEffect.DestroyChilds (parent);
		GameEffect.DestroyChilds (UI_CannonBall_null);

		cannonBallList = new GameObject[howManyCannonBall+BonusManager.bonusCannonBall];
		for (int i = 0; i < howManyCannonBall+BonusManager.bonusCannonBall; i++)
		{
			GameObject Cannon = Instantiate (prefabCannonBall, transform.position, Quaternion.identity) as GameObject;
			Cannon.transform.SetParent (parent, true);
			Cannon.GetComponent<Cannonball> ().SetPooling (this);
			cannonBallList[i] = Cannon;
			cannonBallList [i].SetActive (false);

			GameObject CannonUI = Instantiate (prefab_UI_CannonBall, UI_CannonBall_null.position, Quaternion.identity) as GameObject;
			CannonUI.transform.SetParent (UI_CannonBall_null, false);
			CannonUI.name = "CannonUI_" + i;

			float x = GameMath.CenterAlign (howManyCannonBall + BonusManager.bonusCannonBall, distanceBetweenUI_Cannonball, i);
			CannonUI.transform.localPosition = new Vector3 (x, 0, 0);
			
		}
		AjustUI_Cannonball ();
	}
	/// <summary>
	/// Returns the cannon ball. Return null if its on cooldown
	/// </summary>
	/// <returns>The cannon ball. Or null</returns>
	public GameObject returnCannonBall()
	{
		indexCpt++;
		if (indexCpt >= cannonBallList.Length)
			indexCpt = 0;

		if (cannonBallList [indexCpt].GetComponent<Cannonball> ().isUsed)
			return null;

		cannonBallList [indexCpt].GetComponent<Cannonball> ().isUsed = true;
		AjustUI_Cannonball ();

		return cannonBallList [indexCpt];
	}
	public void AjustUI_Cannonball()
	{
		int cptActive = 0;
		foreach (GameObject go in cannonBallList) 
		{
			if (go.GetComponent<Cannonball> ().isUsed == false)
				cptActive++;
		}
		for (int i = 0; i < UI_CannonBall_null.childCount; i++)
		{
			if (cptActive-1 >= i)
				UI_CannonBall_null.GetChild(i).gameObject.SetActive (true);
			else
				UI_CannonBall_null.GetChild(i).gameObject.SetActive (false);
			
		}
	}

}
