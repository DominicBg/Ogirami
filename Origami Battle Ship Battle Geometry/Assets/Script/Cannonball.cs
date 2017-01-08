using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Cannonball: MonoBehaviour {
	CannonBall_Pooling pooling;
	Rigidbody rigidbody;
	public bool isUsed = false;
	public int cannonBall_index = 0;
	///	///	///	///	///	///	///	///	///

	void Start()
	{
		rigidbody = GetComponent<Rigidbody> ();
	}
	public void SetPooling(CannonBall_Pooling _pooling)
	{
		pooling = _pooling;
	}

	///	///	///	///	///	///	///	///	///
	void Update ()
	{
		testReturnPooling ();

	}
	void testReturnPooling()
	{

		if (transform.position.y < transform.parent.position.y - 5 || !World_Manager.canChange)
		{
			rigidbody.useGravity = false;
			rigidbody.transform.position = transform.parent.position;
			rigidbody.velocity = new Vector3 (0, 0, 0);
			isUsed = false;
			gameObject.SetActive (false);
			pooling.AjustUI_Cannonball ();

		}

	}

    void OnTriggerEnter(Collider col)
    {
		if(col.gameObject.tag == "Ennemy" && rigidbody.useGravity == true)
        {
			pooling.AjustUI_Cannonball ();
			//add particle, give points
			pooling.shipRef.gameManager.GiveScore(1);
			col.GetComponent<EnemyScript> ().Death ();
        }
    }
}
