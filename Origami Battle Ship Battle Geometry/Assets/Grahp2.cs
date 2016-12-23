using UnityEngine;
using System.Collections;

public class Grahp2 : MonoBehaviour {

	public enum FunctionOption
	{
		Linear,
		Exponential,
		Parabola,
		Sine,
		Parabola2,
		Sine2,
		Ripple,
		Ripple2,
		Cicle
	}

	public FunctionOption option;

	private delegate Vector3 FunctionDelegate (Vector3 p,float t);
	private FunctionDelegate[] functionDelegate = 
	{
		Linear,
		Exponential,
		Parabola,
		Sine,
		Parabola2,
		Sine2,
		Ripple,
		Ripple2,
		Circle
	};


	[Range(10,100)]
	[SerializeField]int Resolution;

	[Range(1,10)]
	[SerializeField]float Size;

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
		points = new ParticleSystem.Particle[Resolution * Resolution];
//		Debug.Log (points.Length + " points");
	//	Debug.Log (Resolution);
		int i = 0;
		float increment = 1f / (Resolution-1);
		for (int x = 0; x < Resolution; x++) 
		{
			for (int z = 0; z < Resolution; z++) 
			{
				points [i].position = new Vector3 (x*increment * Size, x*increment * Size, z*increment* Size);
				points [i].color = new Color (x*increment,0,  z*increment);
				points [i].size = 0.1f;
				i++;
			}
		}


	}

	void Update()
	{
		ChangeResolution ();


		ripple2_1x = X;
		ripple2_1z = Z;

		ripple2_2 = NbRipple;
		ripple2_3 = Height;

		harmonicstatic = harmonic;

		FunctionDelegate f = functionDelegate [(int)option];
		float t = Time.timeSinceLevelLoad;


		for(int i = 0; i < points.Length; i++)
		{
			Vector3 p;
			p.x = points[i].position.x;
			p.y = points[i].position.y;
			p.z = points[i].position.z;



			p = f(p,t);


			points[i].position = p;
			Color c = points[i].color;
			c.g = p.y;
			points[i].color = c;
		}

		particleSystem.SetParticles(points, points.Length);
	}




	private static Vector3 Linear (Vector3 p,float t)
	{
		return new Vector3 (p.x, p.y, p.z);
	}
	private static Vector3 Exponential (Vector3 p,float t)
	{
		return new Vector3 (p.x * p.x,  p.y , p.z);
	}

	private static Vector3 Parabola (Vector3 p,float t)
	{
		p.y = 2f * p.x - 1f ;
		return new Vector3 (p.x * p.x, p.y * Mathf.Cos(t), p.z * Mathf.Sin(t));
	}

	private static Vector3 Sine (Vector3 p,float t)
	{

		Vector3 vector = new Vector3 (p.x, p.y, p.z);

		for (int i = 1; i <= harmonicstatic; i++)
		{
			vector.y += Mathf.Sin (2 * Mathf.PI * p.y + t * i);
		}
		return vector;
	}
	private static Vector3 Parabola2 (Vector3 p, float t){
		Vector3 vector = new Vector3 (p.x, p.y, p.z);

		p.x += p.x - 1f;
		p.z += p.z - 1f;


		vector.x = (1f - p.x * (p.x + Mathf.Cos (t) / 2) * p.z * (p.z + Mathf.Sin (t) / 2));


		return vector;
	}

	private static Vector3 Sine2 (Vector3 p, float t){
		Vector3 vector = new Vector3 (p.x, p.y, p.z);

		vector.x = 0.50f +
			0.25f * Mathf.Sin(4f * Mathf.PI * p.x + 4f * t) * Mathf.Sin(2f * Mathf.PI * p.z + t) +
			0.10f * Mathf.Cos(3f * Mathf.PI * p.x + 5f * t) * Mathf.Cos(5f * Mathf.PI * p.z + 3f * t) +
			0.15f * Mathf.Sin(Mathf.PI * p.x + 0.6f * t);
		return vector;
	}
	private static Vector3 Ripple (Vector3 p, float t){
		Vector3 vector = new Vector3 (p.x, p.y, p.z);


		p.x -= 0.5f;
		p.z -= 0.5f;
		float squareRadius = p.x * p.x + p.z * p.z;
		vector.x =  0.5f + Mathf.Sin(15f * Mathf.PI * squareRadius - 2f * t) / (2f + 100f * squareRadius);
		return vector;

	}
	private static Vector3 Ripple2 (Vector3 p, float t)
	{
		Vector3 vector = new Vector3 (p.x, p.y, p.z);


		p.x -= ripple2_1x;
		p.z -= ripple2_1z;
		float squareRadius = p.x * p.x + p.z * p.z;
		vector.x = 0.5f + Mathf.Sin(ripple2_2 * Mathf.PI * squareRadius - ripple2_3 * t) / (ripple2_3 + 100f * squareRadius);
		return vector;
	}

	private static Vector3 Circle(Vector3 p, float t)
	{
		Vector3 vector = new Vector3 (p.x, p.y, p.z);
		vector.x = p.x * p.x;
		vector.z = p.z * p.z;
		return vector;
	}

	[Range(0,1)]
	[SerializeField]float X = 0.5f;

	[Range(0,1)]
	[SerializeField]float Z = 0.5f;

	[Range(0,50)]
	[SerializeField]float NbRipple = 0.15f;
	[SerializeField]float Height;



	[Range(0,50)]
	[SerializeField]float harmonic;



	static float ripple2_1x = 0.5f;
	static float ripple2_1z = 0.5f;

	static float ripple2_2 = 15;
	static float ripple2_3 = 2;

	static float harmonicstatic;



}
