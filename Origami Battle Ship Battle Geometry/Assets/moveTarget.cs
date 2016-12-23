using UnityEngine;
using System.Collections;

public class moveTarget : MonoBehaviour {
	[SerializeField]float speed = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");
		Vector3 newpos = new Vector3 (horizontal, 0, vertical);
		//newpos.Normalize ();

		transform.position += speed * newpos * Time.deltaTime;

	}
}
