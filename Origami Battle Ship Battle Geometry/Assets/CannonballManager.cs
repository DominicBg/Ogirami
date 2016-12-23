using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballManager : MonoBehaviour {
    public Ship sip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Ennemy" && sip.cannonBall.useGravity == true)
        {
            Destroy(col.gameObject);
        }
    }
}
