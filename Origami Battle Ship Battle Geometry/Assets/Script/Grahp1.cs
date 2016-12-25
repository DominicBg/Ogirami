using UnityEngine;
using System.Collections;

public class Grahp1 : MonoBehaviour {

	public enum FunctionOption
	{
		Linear,
		Exponential,
		Parabola,
		Sine
	}
		
	public FunctionOption option;

	private delegate float FunctionDelegate (float x);
	private FunctionDelegate[] functionDelegate = 
	{
		Linear,
		Exponential,
		Parabola,
		Sine
	};


	[Range(10,100)]
	[SerializeField]int Resolution;
	[SerializeField]ParticleSystem.Particle[] points;
	ParticleSystem particleSystem;
	int currentResolution;
	// Use this for initialization
	void Start()
	{
		ChangeResolution ();
	}


	void ChangeResolution () {




		particleSystem = GetComponent<ParticleSystem> ();

		if(Resolution > 100 || Resolution < 10)
			Resolution = 10;





		currentResolution = Resolution;
		points = new ParticleSystem.Particle[Resolution];
		Debug.Log (points.Length + " points");
		Debug.Log (Resolution);

		float increment = 1f / (Resolution-1);
		for (int i = 0; i < Resolution; i++) 
		{
			float x = i * increment;
			points [i].position = new Vector3 (x,0, 0);
			points [i].color = new Color (x, 0, 0);
			points [i].size = 0.1f;

		}


	}

	void Update()
	{
		if (currentResolution != Resolution || points == null)
			ChangeResolution ();


		FunctionDelegate f = functionDelegate [(int)option];

		for(int i = 0; i < Resolution; i++)
		{
			Vector3 p = points[i].position;
			p.y = f(p.x);
			points[i].position = p;
			Color c = points[i].color;
			c.g = p.y;
			points[i].color = c;
		}

		particleSystem.SetParticles(points, points.Length);
	}




	private static float Linear (float x)
	{
		return x;
	}
	private static float Exponential (float x)
	{
		return x * x;
	}

	private static float Parabola (float x)
	{
		x = 2f * x - 1f;
		return x * x;
	}

	private static float Sine (float x)
	{
		return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);
	}



}
