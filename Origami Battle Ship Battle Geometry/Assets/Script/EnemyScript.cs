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
	[HideInInspector]public bool isNormalEnemy;

	[SerializeField] GameObject birthParticleAnimation;
	[SerializeField] GameObject deathParticleAnimation;
	[SerializeField] AudioClip sfxDeath;
    // Use this for initialization
    void Start() {
		movingType = MoveNormal;
		ship = Camera.main.GetComponent<Game_Manager> ().ship;

		initiatlDistance = CalculateDistance ();

		if (isNormalEnemy) 
		{
			transform.GetChild (0).GetComponent<MeshRenderer> ().enabled = false;
		//	transform.GetChild (0).GetComponent<MeshRenderer> ().material.SetColor ("_TintColor",HSBColor.ToColor(new HSBColor(Random.Range(.3f, .9f), .4f, 1)));
		}
		else
		{
			foreach (Transform g in gameObject.transform)
				g.gameObject.SetActive (false);
		}
		//Spawn particles
		GameObject particle = Instantiate(birthParticleAnimation,transform.position,Quaternion.identity) as GameObject;
		particle.transform.SetParent (transform, true);
		Destroy (particle, 2);
		StartCoroutine (delayAppear ());
    }
	IEnumerator delayAppear()
	{
		yield return new WaitForSeconds (0.5f);
		if (isNormalEnemy)
			transform.GetChild(0).GetComponent<MeshRenderer> ().enabled = true;
		else
		{
			foreach (Transform g in gameObject.transform)
				g.gameObject.SetActive (true);
		}	}
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
		//	GameObject particle = Instantiate(particle, gameObjec.transform.position,Quaternion.identity) as gameObject;
		GameObject particle = Instantiate(deathParticleAnimation,transform.position,Quaternion.identity) as GameObject;
		particle.transform.SetParent (transform.parent.parent, true);
		Destroy (particle, 2);
		Destroy (radarTriangle);
		Destroy (gameObject);

	}
	public float CalculateDistance()
	{
		return Mathf.Sqrt
		( 
			Mathf.Pow((transform.position.x - destination.transform.position.x), 2) +
			Mathf.Pow((transform.position.z - destination.transform.position.z), 2)
		);
	}
}
