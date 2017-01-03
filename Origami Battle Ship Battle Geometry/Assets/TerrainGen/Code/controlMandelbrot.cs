using UnityEngine;
using System.Collections;

public class controlMandelbrot : MonoBehaviour {
	
	public Material controlMat;
	
	public float maxStep = 1f, maxShape = 3f;
	private float curStep, curShape;	
	
	public float speed = 1f;
	bool lerpMousePosition;
	float t = 0;
	float pcurStep,pcurShape;
	void Start() {
		curStep = controlMat.GetFloat("Creal");
		curShape = controlMat.GetFloat("Cimag");
	}
	
	// Update is called once per frame
	void Update () {
		//curStep = Mathf.Sin(Time.time * speed) * maxStep;
		//curShape = Mathf.Sin(Time.time * speed) * maxShape;

		if (Input.GetMouseButtonUp (0))
		{
			lerpMousePosition = true;
			pcurStep = curStep;
			pcurShape = curStep;
			t = 0;
		} 
		else if (Input.GetMouseButton (0)) 
		{
			if (!lerpMousePosition)
			{
				curShape = Input.mousePosition.x / Screen.width;
				curStep = Input.mousePosition.y / Screen.height / 2;
			} 
			else
			{
				lerping ();
				curShape = Mathf.Lerp (pcurShape, Input.mousePosition.x / Screen.width, t);
				curStep = Mathf.Lerp (curStep, Input.mousePosition.y / Screen.height / 2, t);
			}
		}
		controlMat.SetFloat("Creal", curStep);
		controlMat.SetFloat("Cimag", curShape);
	}

	void lerping()
	{
		t += Time.deltaTime * 3;
		if (t > 1)
			lerpMousePosition = false;
	}
}
