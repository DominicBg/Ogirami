using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball: MonoBehaviour {
	CannonBall_Pooling pooling;
	Rigidbody rigidbody;

	public bool isUsed = false;
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

		if (transform.position.y < transform.parent.position.y - 5)
		{
			rigidbody.useGravity = false;
			rigidbody.transform.position = transform.parent.position;
			rigidbody.velocity = new Vector3 (0, 0, 0);
			isUsed = false;
			gameObject.SetActive (false);
		}

	}
    void OnTriggerEnter(Collider col)
    {
		if(col.gameObject.tag == "Ennemy" && rigidbody.useGravity == true)
        {
            Destroy(col.gameObject);
        }
    }
}
