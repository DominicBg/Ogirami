using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class Cannon : MonoBehaviour {

	public Rigidbody cannonBall;
	public Transform target;
	public Vector3 targetAim;


	public float h = 25;
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
	// Use this for initialization

	void Start () {
		cannonBall.useGravity = false;
		pathParticleSystem = GetComponent<ParticleSystem> ();
		readyToLaunch = true;

		ColorGradient = HSBColor.ToColor(new HSBColor(currentH, 1, 1));

	}
	void Update()
	{
		if (Input.GetMouseButtonUp (0) && readyToLaunch)
			Launch ();
		else if (Input.GetMouseButton (0)  && readyToLaunch)
			DrawPath ();

		CannonBallTest ();
	}
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
		for (int i = 0; i < pathResolution; i++) 
		{


			float simulationTime = i / (float)pathResolution * launchData.timeToTarget;
			Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
			Vector3 drawPoint = transform.position + displacement;

		//	Debug.DrawLine (previousDrawPoint, drawPoint, Color.red);

			if (i == aimTargetIndex)
				targetAim = drawPoint;

			pathPoints [i].position = new Vector3(drawPoint.x,drawPoint.y,drawPoint.z + 5);

			pathPoints [i].size = 0.3f;
			Color col = new Color (Mathf.Sin (Time.time) / 2 + 0.5f, Mathf.Cos (Time.time) / 2 + 0.5f, Mathf.Sin (Time.time * 2) / 2 + 0.5f);
			pathPoints [i].color = col;

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
