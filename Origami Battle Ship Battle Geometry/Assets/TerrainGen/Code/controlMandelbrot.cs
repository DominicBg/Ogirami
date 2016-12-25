using UnityEngine;
using System.Collections;

public class controlMandelbrot : MonoBehaviour {
	
	public Material controlMat;
	
	public float maxStep = 1f, maxShape = 3f;
	private float curStep, curShape;	
	
	public float speed = 1f;

	public float real;
	public float imaginary;

	void Start() {
		curStep = controlMat.GetFloat("Creal");
		curShape = controlMat.GetFloat("Cimag");
	}
	
	// Update is called once per frame
	void Update () {
		//curStep = Mathf.Sin(Time.time * speed) * maxStep;
		//curShape = Mathf.Sin(Time.time * speed) * maxShape;

		curShape = Input.mousePosition.x / Screen.width;
		curStep = Input.mousePosition.y / Screen.height / 2;

		controlMat.SetFloat("Creal", curStep);
		controlMat.SetFloat("Cimag", curShape);
	}
}
