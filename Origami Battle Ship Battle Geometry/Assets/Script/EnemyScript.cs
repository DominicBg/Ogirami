using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour {

	[HideInInspector]public Ship ship;
	delegate Vector3 MovingType(); 
	MovingType movingType;

	float speedSin = 2;
	float speedMov = 1;
	float amplitudeSin = .01f;
	[HideInInspector]public GameObject destination;

	[HideInInspector]public GameObject radarTriangle;
	[HideInInspector]public float initiatlDistance;
	public bool isNormalEnemy;

	[SerializeField] GameObject birthParticleAnimation;
	[SerializeField] GameObject deathParticleAnimation;
	[SerializeField] AudioClip sfxDeath;
    // Use this for initialization
    void Start() {
		movingType = MoveNormal;
		ship = Camera.main.GetComponent<Game_Manager> ().ship;

		initiatlDistance = CalculateDistance ();

		ShowEnemy (false);
		SpawnParticle (birthParticleAnimation,true);



		StartCoroutine (delayAppear ());
    }
	void SpawnParticle(GameObject whichParticle,bool setParent)
	{
		if (isEnemyFromCurrentWorld()) 
		{
			GameObject particle = Instantiate(whichParticle,transform.position,Quaternion.identity) as GameObject;
			if(setParent)
				particle.transform.SetParent (transform, true);
			Destroy (particle, 2);
		}
	}
	bool isEnemyFromCurrentWorld()
	{
		if((World_Manager.currentWorld == World_Manager.EnumWorld.Normal && isNormalEnemy) ||
			(World_Manager.currentWorld == World_Manager.EnumWorld.Fractal && !isNormalEnemy))
			return true;

		return false;
	}
	IEnumerator delayAppear()
	{
		yield return new WaitForSeconds (0.5f);
		ShowEnemy (isEnemyFromCurrentWorld());

	}
    // Update is called once per frame
    void Update() 
	{
		if (isNormalEnemy)
			transform.LookAt (new Vector3 (destination.transform.position.x, 0, destination.transform.position.z));
		else
			transform.eulerAngles += new Vector3(1,1,0) * Time.deltaTime * 300;

		if (!World_Manager.canChange)
			return;
		
		Mouvement();
		TestCollision ();
		if (Game_Manager.inMenu)
			Death ();
    }

    void Mouvement()
    {
		transform.position = Vector3.MoveTowards(transform.position,destination.transform.position,speedMov * Time.deltaTime);
		transform.position += MoveSin ();
    }
	/*
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            

			Destroy (gameObject);

        }
    }
	*/


	public void ShowEnemy(bool show)
	{
		if (isNormalEnemy)
			transform.GetChild(0).GetComponent<MeshRenderer> ().enabled = show;
		else
		{
			transform.GetComponent<MeshRenderer> ().enabled = show;

			foreach (Transform g in gameObject.transform)
				g.gameObject.SetActive (show);
		}	
	}

	void TestCollision()
	{
		if (Mathf.Abs(transform.position.x - destination.transform.position.x) < 2 	&& Mathf.Abs(transform.position.z - destination.transform.position.z) < 2) 
		{
			Debug.Log ("collision");
			Death ();
			ship.gameManager.ChangeLife (-1);
			ship.gameManager.GetHit (isNormalEnemy);

        }
	}

	Vector3 MoveSin()
	{
		return new Vector3 (amplitudeSin * Mathf.Sin(Time.timeSinceLevelLoad * speedSin),0,0);
	}

	Vector3 MoveNormal()
	{
		return Vector3.zero;
	}
	public void Death()
	{
		GameSound.PlaySound (sfxDeath,.2f,.2f);
		SpawnParticle(deathParticleAnimation,false);
		Destroy (radarTriangle);
		Destroy (gameObject);
	}
	public float CalculateDistance()
	{
		return GameMath.DistanceXZ (gameObject, destination);
	}
}
