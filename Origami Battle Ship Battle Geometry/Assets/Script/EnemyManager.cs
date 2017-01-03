using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour {

    public Ship ship;
	delegate Vector3 MovingType(); 
	MovingType movingType;

    public GameObject playerP;
	float speedSin = 4;
	float speedMov = 1;
	float amplitudeSin = .5f;
	public GameObject destination;
    // Use this for initialization
    void Start() {
		movingType = MoveNormal;
        ship = transform.parent.GetComponent<EnemySpawn>().shipP;
    }

    // Update is called once per frame
    void Update() {
		Mouvement();
		TestCollision ();
    }

    void Mouvement()
    {
		/*
		Vector3 dir = new Vector3
		(
			destination.x - transform.position.x, 
			0,
			destination.z - transform.position.z
		);
		dir.Normalize();
		*/
		//transform.localPosition += ((dir * speedMov) + movingType()) * Time.deltaTime;
	//	transform.position += dir * Time.deltaTime;
		transform.LookAt (new Vector3(destination.transform.position.x,0,destination.transform.position.z));
		transform.position = Vector3.MoveTowards(transform.position,destination.transform.position,speedMov * Time.deltaTime);
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
		if 
		(
			Mathf.Abs(transform.position.x - destination.transform.position.x) < 2 	&&
			Mathf.Abs(transform.position.z - destination.transform.position.z) < 2
		) 
		{
			Debug.Log ("collision");
           

            Destroy (gameObject);
            ship.healthRemain--;
			ship.healthCurrent.text = "hP : " + ship.healthRemain.ToString();
            Debug.Log(ship.healthRemain);
            if(ship.healthRemain <= 0)
            {
                Debug.Log("ur lose");
                ship.healthRemain = 0;
                ship.healthCurrent.GetComponent<Text>().text = ship.healthRemain.ToString();
            }
            
            

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
    
}
