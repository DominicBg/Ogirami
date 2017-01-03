using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEffect {

	///////***************** Shake ********************////////

	/// <summary>
	/// Shake the specified obj with default intensity of 1 and time of 0.2.
	/// </summary>
	/// <param name="obj">Object.</param>
	public static void Shake(GameObject obj)
	{
		ShakeEffect (obj, 1, .2f);
	}

	/// <summary>
	/// Shake the specified obj and intensity with default time value of 0.2.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="intensity">Intensity.</param>
	public static void Shake(GameObject obj,float intensity)
	{
		ShakeEffect (obj, intensity, .2f);

	}

	/// <summary>
	/// Shake the specified obj, intensity and time.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="intensity">Intensity.</param>
	/// <param name="time">Time.</param>
	public static void Shake(GameObject obj,float intensity,float time)
	{
		ShakeEffect (obj, intensity, time);

	}
	static void ShakeEffect(GameObject obj,float intensity,float time)
	{
		if (obj.GetComponent<ShakeClass> () == null)
			obj.AddComponent<ShakeClass> ();
		ShakeClass shake = obj.GetComponent<ShakeClass> ();
		shake.shake = time;
		shake.shakeAmount = intensity;
	}

	///////***************** Freeze Frame ********************////////

	/// <summary>
	/// Freezes the frame.
	/// </summary>
	/// <param name="sec">Sec.</param>
	public static void FreezeFrame(float sec)
	{
		if (Camera.main.gameObject.GetComponent<FreezeFrameClass> () == null)
			Camera.main.gameObject.AddComponent<FreezeFrameClass> ().freezeSec = sec;
	}

	/// <summary>
	/// Freezes the frame with default value of 0.1.
	/// </summary>
	public static void FreezeFrame()
	{
		if (Camera.main.gameObject.GetComponent<FreezeFrameClass> () == null)
			Camera.main.gameObject.AddComponent<FreezeFrameClass> ().freezeSec = .1f;
	}


	///////***************** Sprite And Color ********************////////


	/// <summary>
	/// Sins the gradient.
	/// </summary>
	/// <returns>The gradient.</returns>
	/// <param name="color1">Color1.</param>
	/// <param name="color2">Color2.</param>
	/// <param name="speed">Speed.</param>

	public static Color SinGradient(Color color1, Color color2, float speed)
	{
		float t = (Mathf.Sin(Time.timeSinceLevelLoad * speed)+1) / 2;
		Color color = new Color
		(
				Mathf.Lerp(color1.r, color2.r,t),
				Mathf.Lerp(color1.g, color2.g,t),
				Mathf.Lerp(color1.b, color2.b,t),
				Mathf.Lerp(color1.a, color2.a,t)		
		);

		return color;
	}

	public static void FlashSprite(GameObject obj, Color color,float duration)
	{
		if (obj.GetComponent<FlashSpriteClass> () == null)
		{
			obj.AddComponent<FlashSpriteClass> ();
			FlashSpriteClass flashSprite = obj.GetComponent<FlashSpriteClass> ();

			flashSprite.flashColor = color;
			flashSprite.duration = duration;
			flashSprite.flashSpriteEnum = FlashSpriteClass.FlashSpriteType.Simple;
		}
			
	}

	public static void FlashSprite(GameObject obj, Color color,float duration, int flashCount)
	{
		if (obj.GetComponent<FlashSpriteClass> () == null)
		{
			obj.AddComponent<FlashSpriteClass> ();
			FlashSpriteClass flashSprite = obj.GetComponent<FlashSpriteClass> ();

			flashSprite.flashColor = color;
			flashSprite.duration = duration;
			flashSprite.flashCount = flashCount;
			flashSprite.flashSpriteEnum = FlashSpriteClass.FlashSpriteType.Multiple;
		}

	}

	public static void FlashSpriteLerp(GameObject obj, Color color,float duration)
	{
		if (obj.GetComponent<FlashSpriteClass> () == null)
		{
			obj.AddComponent<FlashSpriteClass> ();
			FlashSpriteClass flashSprite = obj.GetComponent<FlashSpriteClass> ();
			flashSprite.flashColor = color;
			flashSprite.speed = duration;
			flashSprite.flashSpriteEnum = FlashSpriteClass.FlashSpriteType.Lerp;
		}

	}


}
public static class GameMath
{
	///////***************** Math ********************////////

	public static float sinerp(float t)
	{
		return Mathf.Sin (t * Mathf.PI * 0.5f);
	}
	public static float smoothstep(float t)
	{
		return t * t * (3f - 2f * t);
	}
	public static float smootherstep(float t)
	{
		return t * t * t * (t * (6f * t - 15f) + 10);
	}
	public static float sigmoidErf(float t)
	{
		return 1 / ( 1 + Mathf.Exp(-t));
		//return Mathf.Tan (t);
	}

	public static float easeInOut(float A, float speed)
	{
		//return % of speed
		return 1 - ((Mathf.Abs (A - .5f)) * speed);

	}
	public static float stretch(float A, float stretchAmount)
	{
		if (A > 1)
			A = 1;

		float C = Mathf.Abs (A - .5f);

		return .5f +  stretchAmount + ((C + .5f) * stretchAmount);
	}

}
/*
public static class GameController
{
	public static Vector2 Control(GameObject obj, string horizontal, string vertical, float speed)
	{
		Vector2 vec = new Vector2 (Input.GetAxis (horizontal), Input.GetAxis (vertical));
		vec.Normalize ();
		vec *= speed;
		return vec;
	}
	public static void Move(GameObject obj, Vector2 vector)
	{
		//obj.transform.position += vector;
	}
}
*/
public class FreezeFrameClass : MonoBehaviour {

	public float freezeSec;

	void Start()
	{
		StartCoroutine (FreezeFrameEffect());
	}

	IEnumerator FreezeFrameEffect()
	{
		Time.timeScale = 0.01f;
		float pauseEndTime = Time.realtimeSinceStartup + freezeSec;
		while (Time.realtimeSinceStartup < pauseEndTime)
			yield return 0;

		Time.timeScale = 1;
		Destroy (this);
	}
}

public class ShakeClass : MonoBehaviour {

	public Transform shakeTransform;

	// How long the object should shake for.
	public float shake = 0.1f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.10f;
	public float decreaseFactor = 0.7f;

	Vector3 originalPos;


	void Awake()
	{
		if (shakeTransform == null)
		{
			shakeTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = shakeTransform.localPosition;
	}

	void Update()
	{
		if (shake > 0)
		{
			shakeTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shake = 0f;
			shakeTransform.localPosition = originalPos;
			Destroy(this);
		}
	}
}

public class FlashSpriteClass:MonoBehaviour
{
	public enum FlashSpriteType
	{
		Multiple, Simple, Lerp
	};

	public FlashSpriteType flashSpriteEnum;

	Color originalColor;
	public Color flashColor;

	public float speed, duration;
	float t;
	public int flashCount;

	SpriteRenderer spriteRender;

	void Start()
	{
		spriteRender = gameObject.GetComponent<SpriteRenderer> ();
		originalColor = spriteRender.color;

		if (flashSpriteEnum == FlashSpriteType.Simple) 
		{
			StartCoroutine(simpleFlash ());
		}
		else if (flashSpriteEnum == FlashSpriteType.Multiple)
		{
			StartCoroutine(multipleFlash ());
		}

	}

	void Update()
	{
		
		if (flashSpriteEnum == FlashSpriteType.Lerp)
		{
			lerpFlash ();
		}
	}

	IEnumerator simpleFlash()
	{
		
		spriteRender.color = flashColor;
		yield return new WaitForSeconds (duration);
		spriteRender.color = originalColor;
		Destroy (this);
	}

	IEnumerator multipleFlash()
	{
		float splitTime = (duration / flashCount) / 2;
		
		for(int i = 0; i < flashCount; i++)
		{
			spriteRender.color = flashColor;
			yield return new WaitForSeconds (splitTime);
			spriteRender.color = originalColor;
			yield return new WaitForSeconds (splitTime);
		}
		Destroy (this);
	}

	void lerpFlash()
	{
		t += Time.deltaTime / speed;
		float t2 = (Mathf.Sin(t)+1) / 2;

		spriteRender.color = new Color
			(
				Mathf.Lerp(originalColor.r, flashColor.r,t),
				Mathf.Lerp(originalColor.g, flashColor.g,t),
				Mathf.Lerp(originalColor.b, flashColor.b,t),
				Mathf.Lerp(originalColor.a, flashColor.a,t)		
			);
		if (t > 1) 
		{
			spriteRender.color = originalColor;
			Destroy (this);
		}
	}

}
