using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall_Pooling : MonoBehaviour {

	[SerializeField]int howManyCannonBall;
	[SerializeField]GameObject prefabCannonBall;
	GameObject[] cannonBallList;
	public Ship shipRef;
	int indexCpt = 0;
	[SerializeField]Transform parent;
	// Use this for initialization
	void Start () 
	{
		cannonBallList = new GameObject[howManyCannonBall];
		for (int i = 0; i < howManyCannonBall; i++)
		{
			GameObject Cannon = Instantiate (prefabCannonBall, transform.position, Quaternion.identity) as GameObject;
			Cannon.transform.SetParent (parent, true);
			Cannon.GetComponent<Cannonball> ().SetPooling (this);
			cannonBallList[i] = Cannon;
			cannonBallList [i].SetActive (false);

		}
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
		return cannonBallList [indexCpt];
	}

}
