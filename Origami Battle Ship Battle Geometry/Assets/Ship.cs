﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class Ship : MonoBehaviour {


	private Vector3 _startPosition; 
	[SerializeField]GameObject Cannon;

	[SerializeField] float minY;
	[SerializeField] float maxY;
	[SerializeField] float minX;
	[SerializeField] float maxX;


	[SerializeField] float posMinY;
	[SerializeField] float posMaxY;
	[SerializeField] float posMinX;
	[SerializeField] float posMaxX;


	public Rigidbody cannonBall;
	public Transform target;
	public Vector3 targetAim;


	public float h = 5;
	const float MAX_H = .1f;
	const float MIN_H = 6;
	float t_h = 0;
	public float gravity = -18;
	public bool drawingPath;

	bool readyToLaunch;

	//Path and such
	public int pathResolution;
	[SerializeField]ParticleSystem.Particle[] pathPoints;
	ParticleSystem pathParticleSystem;

	[SerializeField]Light light;
	Color ColorGradient;
	float currentH;


	void Start()
	{
		_startPosition = transform.position;

		//Cannon
		cannonBall.useGravity = false;
		pathParticleSystem = GetComponent<ParticleSystem> ();
		readyToLaunch = true;

		ColorGradient = HSBColor.ToColor(new HSBColor(currentH, 1, 1));

	}
	// Update is called once per frame
	void Update ()
	{
		floatingWater ();
		//FingerControl ();
		MoveCannon();

		if (Input.GetMouseButtonDown (0)) 
			t_h = 0;


		if (Input.GetMouseButtonUp (0) && readyToLaunch)
			Launch ();
		else if (Input.GetMouseButton (0))
			DrawPath ();

		CannonBallTest ();
	}
	void floatingWater()
	{
				transform.position = _startPosition + new Vector3 (0, 0.2f * Mathf.Sin ( Time.time), 0);
	}
	void FingerControl()
	{
		
		if (Input.GetMouseButton (0))
		{

			Vector2 mouse = Input.mousePosition;

			float x = mouse.x / Screen.width;
			float y = mouse.y / Screen.height;


			Vector3 rotation = new Vector3 
				(
					Mathf.Lerp(minX,maxX,y),
					Mathf.Lerp(minY,maxY,x),
					0
				);
			Cannon.transform.localEulerAngles = rotation;

		}
	}
	void MoveCannon()
	{

		if (Input.GetMouseButton (0))
		{

			Vector2 mouse = Input.mousePosition;

			float x = mouse.x / Screen.width;
			float y = mouse.y / Screen.height;


			Vector3 position = new Vector3 
				(
					Mathf.Lerp(posMinX,posMaxX,x),
					0,
					Mathf.Lerp(posMinY,posMaxY,y)
				);

			target.transform.position = position;

			//Cannon.transform.LookAt (target);
			Cannon.transform.LookAt (targetAim);


		}
	}

	//CANNON
	void CannonBallTest()
	{
		if (readyToLaunch)
			return;

		if (cannonBall.transform.position.y < transform.position.y - 5)
		{
			cannonBall.useGravity = false;
			cannonBall.transform.position = transform.position;
			readyToLaunch = true;
			cannonBall.velocity = new Vector3 (0, 0, 0);
		}
	}
	void Launch()
	{
		Physics.gravity = Vector3.up * gravity;
		cannonBall.useGravity = true;
		cannonBall.velocity = CalculateLaunchData ().initialVelocity;

		readyToLaunch = false;

		//remove aim
		pathPoints = new ParticleSystem.Particle[0];
		pathParticleSystem.SetParticles(pathPoints, pathPoints.Length);
		currentH = 0;
		LightColorConvertion ();
	}
	void LightColorUpdate()
	{
		t_h += Time.deltaTime * 0.5f;
		h = Mathf.Lerp (MIN_H, MAX_H, t_h);


		currentH += .005f;


		if (currentH > .8f)
			currentH = .8f;

		LightColorConvertion ();
	}
	void LightColorConvertion()
	{
		ColorGradient = HSBColor.ToColor(new HSBColor(currentH, 1, 1));
		light.color = ColorGradient;
	}
	void DrawPath()
	{

		LightColorUpdate ();

		LaunchData launchData = CalculateLaunchData ();
		//Vector3 previousDrawPoint = cannonBall.position;

		pathPoints = new ParticleSystem.Particle[pathResolution];
		int aimTargetIndex = pathResolution / 4;
		for (int i = 3; i < pathResolution; i++) 
		{


			float simulationTime = i / (float)pathResolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = transform.position + displacement;

			//	Debug.DrawLine (previousDrawPoint, drawPoint, Color.red);

			if (i == aimTargetIndex)
				targetAim = drawPoint;

			pathPoints [i].position = new Vector3(drawPoint.x,drawPoint.y,drawPoint.z + 5);

			pathPoints [i].size = 0.3f;
			if (readyToLaunch) 
			{
				Color col = new Color (Mathf.Sin (Time.time) / 2 + 0.5f, Mathf.Cos (Time.time) / 2 + 0.5f, Mathf.Sin (Time.time * 2) / 2 + 0.5f);
				pathPoints [i].color = col;
			} 
			else 
			{
				pathPoints [i].color = new Color (0, 0, 0, 0);

			}
			//	Debug.Log (pathPoints [i].position);
			//previousDrawPoint = drawPoint;
		}
		pathParticleSystem.SetParticles(pathPoints, pathPoints.Length);

	}
	LaunchData CalculateLaunchData()
	{

		float displacementY = target.position.y - transform.position.y;
		Vector3 displacementXZ = new Vector3(
			target.position.x - transform.position.x, 
			0,
			target.position.z - transform.position.z
		);
		float time =  (Mathf.Sqrt (-2 * h / gravity ) + Mathf.Sqrt(2*(displacementY - h)/gravity));
		Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * gravity * h);
		Vector3 velocityXZ = displacementXZ / time;

		return new LaunchData (velocityXZ + velocityY * -Mathf.Sign (gravity), time);
	}
	struct LaunchData{
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public LaunchData(Vector3 initialVelocity, float timeToTarget)
		{
			this.initialVelocity = initialVelocity;
			this.timeToTarget = timeToTarget;
		}
	}
}