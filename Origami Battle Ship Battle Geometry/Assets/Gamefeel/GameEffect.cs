using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	/// Shake the camera with base settings.
	/// </summary>
	public static void Shake()
	{
		ShakeEffect (Camera.main.gameObject, 1, .2f);

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
	public static Color ColorLerp(Color color1, Color32 color2, float t)
	{
		return new Color 
			(
			Mathf.Lerp (color1.r, color2.r, t),
			Mathf.Lerp (color1.g, color2.g, t),
			Mathf.Lerp (color1.b, color2.b, t),
			Mathf.Lerp (color1.a, color2.a, t)
			);
	}
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

	public static void FlashCamera(Color color, float time)
	{
		if(Camera.main.gameObject.GetComponent<FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<FlashCameraClass> ().Flash (color,null, time);
	}
	public static void FlashCamera(Sprite image, float time)
	{
		if(Camera.main.gameObject.GetComponent<FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<FlashCameraClass> ();

		Camera.main.gameObject.GetComponent<FlashCameraClass> ().Flash (Color.white,image, time);

	}
	public static void DestroyChilds(Transform parent)
	{
		if (parent.childCount != null) 
		{
			int childs = parent.childCount;
			for (int i = 0; i <= childs - 1; i++)
				MonoBehaviour.Destroy (parent.GetChild (i).gameObject);
		}

	}
	public static void DestroyChilds(GameObject parent)
	{
		GameEffect.DestroyChilds (parent.transform);
	}
}
public static class GameSound
{
	static bool isSetAudio = false;
	static AudioSource source;
	static MusicClass musicSource;
	static float soundVolume = 1;
	static float musicVolume = 1;

	//toggle
	static float toggleSoundState = 0;
	static float toggleSoundRatio = 0;

	//toggle
	static float toggleMusicState = 0;
	static float toggleMusicRatio = 0;

	public static void EnableAudio()
	{
		isSetAudio = true;
		Camera.main.gameObject.AddComponent<AudioSource> ();
		source = Camera.main.gameObject.GetComponent<AudioSource> ();
	}
	public static void EnableMusicChannel()
	{
		if (Camera.main.gameObject.GetComponent<MusicClass> () == null) 
		{
			Camera.main.gameObject.AddComponent<MusicClass> ();
			musicSource = Camera.main.gameObject.GetComponent<MusicClass> ();
		}
	}

	public static void SetGlobalMusicVolume(int volume)
	{
		musicVolume = (float)volume / 100;
		musicSource.globalMusicVolume = musicVolume;
		musicSource.UpdateAllVolume ();
	}
	/// <summary>
	/// Sets the sound volume. Value fom 0 to 1
	/// </summary>
	/// <param name="volume">Volume.</param>
	public static void SetGlobalMusicVolume(float volume)
	{
		musicVolume = volume;
		musicSource.globalMusicVolume = musicVolume;
		musicSource.UpdateAllVolume ();
	}
	public static void EnableToggleMusicVolume(int howManyState)
	{
		if (PlayerPrefs.GetFloat ("GameSound_ToggleMusicVolume_init") == 0) {
			PlayerPrefs.SetFloat ("GameSound_ToggleMusicVolume_init", 1);
			toggleMusicState = 1;

		} 
		else 
		{
			toggleMusicState =PlayerPrefs.GetFloat ("GameSound_ToggleMusicVolume") ;
		}
		toggleMusicRatio = 1 / (float)howManyState;
		musicVolume = toggleMusicState;
		SetGlobalMusicVolume (musicVolume);

	}
	public static void ToggleMusicVolume()
	{
		toggleMusicState -= toggleMusicRatio;
		if (toggleMusicState < -.1f)
			toggleMusicState = 1;

		musicVolume = toggleMusicState;

		SetGlobalMusicVolume (musicVolume);
		PlayerPrefs.SetFloat ("GameSound_ToggleMusicVolume",toggleMusicState);
		PlayerPrefs.Save ();
	}
	public static float GetToggleValueMusic()
	{
		return toggleMusicState;

	}
	public static float GetToggleValueSound()
	{
		return toggleSoundState;
	}
	public static void SetMusicChannel(int howManychannel)
	{
		EnableMusicChannel ();
		musicSource.SetChannel (howManychannel);
	}
	public static void SetVolumeMusicChannel(int whichChannel, float volume)
	{
		musicSource.SetVolume (whichChannel,volume,false);
	}
	public static void SetVolumeMusicChannel(int whichChannel, float volume,bool andPlay)
	{
		musicSource.SetVolume (whichChannel,volume,andPlay);
	}
	public static void SetMusicIntoChannel(AudioClip[] musics)
	{
		musicSource.SetMusicIntoChannel(musics);
	}
	public static void SetMusicIntoChannel(AudioClip music, int whichChannel)
	{
		musicSource.SetMusicIntoChannel(music,whichChannel);
	}
	public static void SetMusicIntoChannel(AudioClip music, int whichChannel,float volume, bool andPlay)
	{
		musicSource.SetMusicIntoChannel(music,whichChannel,volume,andPlay);
	}
	public static void SetTransition(int channelFrom,int channelTo, int time)
	{
		musicSource.SetTransition(channelFrom,channelTo,time);
	}
	/// <summary>
	/// Sets the sound volume. Value fom 0 to 100
	/// </summary>
	/// <param name="volume">Volume.</param>
	public static void SetGlobalSoundVolume(int volume)
	{
		soundVolume = (float)volume / 100;
	}
	/// <summary>
	/// Sets the sound volume. Value fom 0 to 1
	/// </summary>
	/// <param name="volume">Volume.</param>
	public static void SetGlobalSoundVolume(float volume)
	{
		soundVolume = volume;
	}

	public static void EnableToggleSoundVolume(int howManyState)
	{
		if (PlayerPrefs.GetFloat ("GameSound_ToggleSound_init") == 0) {
			PlayerPrefs.SetFloat ("GameSound_ToggleSound_init", 1);
			toggleSoundState = 1;
		} 
		else 
		{
			toggleSoundState =PlayerPrefs.GetFloat ("GameSound_ToggleSoundVolume") ;
		}

		toggleSoundRatio = 1 / (float)howManyState;
		soundVolume = toggleSoundState;
	}
	public static void ToggleSoundVolume()
	{
		toggleSoundState -= toggleSoundRatio;
		if (toggleSoundState < -.1f)
			toggleSoundState = 1;

		soundVolume = toggleSoundState;
		PlayerPrefs.SetFloat ("GameSound_ToggleSoundVolume",toggleSoundState);
		PlayerPrefs.Save ();
	}
	public static void PlaySound(AudioClip clip)
	{
		source.volume = Random.Range (0.9f, 1) * soundVolume;
		source.pitch = Random.Range (0.9f, 1.1f);
		source.PlayOneShot (clip);
	}
	public static void PlaySound(AudioClip clip, float varianceVolume, float variancePitch)
	{
		source.volume = Random.Range (1 - varianceVolume, 1) * soundVolume;
		source.pitch = Random.Range (1-variancePitch,1+variancePitch);
		source.PlayOneShot (clip);
	}
	public static void PlaySound(AudioClip[] clip)
	{
		source.volume = Random.Range (0.9f, 1) * soundVolume;
		source.pitch = Random.Range (0.9f, 1.1f);
		source.PlayOneShot (clip[Random.Range(0,clip.Length)]);
	}
	public static void PlaySound(AudioClip[] clip, float varianceVolume, float variancePitch)
	{
		source.volume = Random.Range (1-varianceVolume, 1) * soundVolume;
		source.pitch = Random.Range (1-variancePitch,1+variancePitch);
		source.PlayOneShot (clip[Random.Range(0,clip.Length)]);
	}
	public static void PlaySound(AudioClip clip,bool isStatic)
	{
		
		if (isStatic)
		{
			source.volume = soundVolume;
			source.pitch = 1;
			source.PlayOneShot (clip);
		}
		else
			GameSound.PlaySound (clip);
	}
	public static void PlaySound(AudioClip clip, bool isStatic,int volume)
	{
		if (isStatic)
		{
			source.volume = ((float)volume/100) * soundVolume;
			source.PlayOneShot (clip);
		}
		else
			GameSound.PlaySound (clip);
	}
	public static void PlaySound(AudioClip clip, bool isStatic,float volume)
	{
		if (isStatic)
		{
			source.volume = volume * soundVolume;
			source.PlayOneShot (clip);
		}
		else
			GameSound.PlaySound (clip);
	}
	public static void PlaySound(AudioClip clip, bool isStatic,int volume,float pitch)
	{
		if (isStatic)
		{
			source.volume = ((float)volume/100) * soundVolume;
			source.pitch = pitch;
			source.PlayOneShot (clip);
		}
		else
			GameSound.PlaySound (clip);
	}
	public static void PlaySound(AudioClip clip, bool isStatic,float volume,float pitch)
	{
		if (isStatic)
		{
			source.volume = volume * soundVolume;
			source.pitch = pitch;
			source.PlayOneShot (clip);
		}
		else
			GameSound.PlaySound (clip);
	}

	public static void PlaySound(AudioClip clip, int volume)
	{
		source.volume = Random.Range (0.9f, 1) * soundVolume * ((float)volume / 100);
		source.pitch = Random.Range (0.9f, 1.1f);
		source.PlayOneShot (clip);
	}
	public static void PlaySound(AudioClip clip, float volume)
	{
		source.volume = Random.Range (0.9f, 1) * soundVolume * volume;
		source.pitch = Random.Range (0.9f, 1.1f);
		source.PlayOneShot (clip);
	}
	public static void PlaySound(AudioClip[] clip, int volume)
	{
		source.volume = Random.Range (0.9f, 1) * soundVolume * ((float)volume / 100);
		source.pitch = Random.Range (0.9f, 1.1f);
		source.PlayOneShot (clip[Random.Range(0,clip.Length)]);
	}
	public static void PlaySound(AudioClip[] clip, float volume)
	{
		source.volume = Random.Range (0.9f, 1) * soundVolume * volume;
		source.pitch = Random.Range (0.9f, 1.1f);
				source.PlayOneShot (clip[Random.Range(0,clip.Length)]);
	}
	//specific audio source
	public static void PlaySound(AudioSource _source, AudioClip clip)
	{
		_source.volume = Random.Range (0.9f, 1) * soundVolume;
		_source.pitch = Random.Range (0.9f, 1.1f);
		_source.PlayOneShot (clip);
	}
	public static void PlaySound(AudioSource _source, AudioClip[] clip)
	{
		_source.volume = Random.Range (0.8f, 1) * soundVolume;
		_source.pitch = Random.Range (0.8f, 1.2f);
		_source.PlayOneShot (clip[Random.Range(0,clip.Length)]);
	}

}
public static class GameMath
{
	///////***************** Math ********************////////
	public static float CenterAlign(int NumberOfObject, float distance, int i)
	{
		return ((i - (((NumberOfObject) - 1) / 2)) * distance) - (((NumberOfObject + 1) % 2) * (distance / 2));
	}
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

	public static double Map(double x, double in_min, double in_max, double out_min, double out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}
	public static int Map(int x, int in_min, int in_max, int out_min, int out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}
	public static uint Map(uint x, uint in_min, uint in_max, uint out_min, uint out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}
	public static float Map(float x, float in_min, float in_max, float out_min, float out_max)
	{
		float y = (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	/*	if (y > out_max)
			return out_max;
		else if(y < out_min)
			return out_min;
*/
		return y;
	}
	public static long Map(long x, long in_min, long in_max, long out_min, long out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}
	public static ulong Map(ulong x, ulong in_min, ulong in_max, ulong out_min, ulong out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}

	public static float Distance3D(GameObject object1,GameObject object2)
	{
			return Mathf.Sqrt
				( 
				Mathf.Pow((object1.transform.position.x - object2.transform.position.x), 2) +
				Mathf.Pow((object1.transform.position.y - object2.transform.position.y), 2) +
				Mathf.Pow((object1.transform.position.z - object2.transform.position.z), 2)
				);
	}
	public static float Distance3D(Transform transform1,Transform transform2)
	{
		return Mathf.Sqrt
			( 
				Mathf.Pow((transform1.position.x - transform2.position.x), 2) +
				Mathf.Pow((transform1.position.y - transform2.position.y), 2) +
				Mathf.Pow((transform1.position.z - transform2.position.z), 2)
			);
	}
	public static float DistanceXY(GameObject object1,GameObject object2)
	{
		return Mathf.Sqrt
			( 
				Mathf.Pow((object1.transform.position.x - object2.transform.position.x), 2) +
				Mathf.Pow((object1.transform.position.y - object2.transform.position.y), 2)
			);
	}
	public static float DistanceXY(Transform transform1,Transform transform2)
	{
		return Mathf.Sqrt
			( 
				Mathf.Pow((transform1.position.x - transform2.position.x), 2) +
				Mathf.Pow((transform1.position.y - transform2.position.y), 2)
			);
	}
	public static float DistanceXZ(GameObject object1,GameObject object2)
	{
		return Mathf.Sqrt
			( 
				Mathf.Pow((object1.transform.position.x - object2.transform.position.x), 2) +
				Mathf.Pow((object1.transform.position.z - object2.transform.position.z), 2)
			);
	}
	public static float DistanceXZ(Transform transform1,Transform transform2)
	{
		return Mathf.Sqrt
			( 
				Mathf.Pow((transform1.position.x - transform2.position.x), 2) +
				Mathf.Pow((transform1.position.z - transform2.position.z), 2)
			);
	}
	public static float DistanceYZ(GameObject object1,GameObject object2)
	{
		return Mathf.Sqrt
			( 
				Mathf.Pow((object1.transform.position.y - object2.transform.position.y), 2) +
				Mathf.Pow((object1.transform.position.z - object2.transform.position.z), 2)
			);
	}
	public static float DistanceYZ(Transform transform1,Transform transform2)
	{
		return Mathf.Sqrt
			( 
				Mathf.Pow((transform1.position.y - transform2.position.y), 2) +
				Mathf.Pow((transform1.position.z - transform2.position.z), 2)
			);
	}
}

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

public class FlashSpriteClass : MonoBehaviour
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
public class FlashCameraClass : MonoBehaviour
{
	bool isUsed = false;
	float t = 0;
	float speed;
	GameObject screen;
	Color color;
	Image image;

	bool isFlashing = false;
	bool isIncreasing = true;

	void Awake()
	{
		screen = new GameObject ();
		screen.AddComponent<Image> ();
		screen.GetComponent<Image> ().raycastTarget = false;
		screen.name = "Flashing Screen";


		screen.transform.SetParent (GameObject.Find ("Canvas").transform, true);
		screen.GetComponent<Image> ().rectTransform.sizeDelta = new Vector2 (Screen.width, Screen.height);
		screen.GetComponent<Image> ().rectTransform.localPosition = Vector2.zero;

	//	screen.GetComponent<Image> ().rectTransform.x
	}

	public void Flash(Color _color, Sprite sprite, float time)
	{
		speed = 1 / time;
		t = 0;
		color = _color;
		screen.GetComponent<Image> ().sprite = sprite;
		isIncreasing = true;
		isFlashing = true;
	}
	void Update()
	{
		if (!isFlashing)
			return;
		
		t += Time.deltaTime * speed * 2;


		if (isIncreasing)
		{
			screen.GetComponent<Image> ().color  = new Color (color.r, color.g, color.b, Mathf.Lerp (0,  color.a, t));
			if (t > 1)
			{
				isIncreasing = false;
				t = 0;
			}
		}
		else
		{
			screen.GetComponent<Image> ().color  = new Color (color.r, color.g, color.b, Mathf.Lerp (color.a, 0, t));
			if (t > 1)
				isFlashing = false;
		}
	}
}
public class MusicClass : MonoBehaviour
{
	GameObject channelGameObject;
	AudioSource[] channels;
	float[] volumes;

	float t;
	bool inTransition;
	float speed;
	int channelFrom, channelTo;

	public float globalMusicVolume = 1;

	void Awake()
	{
		GameObject _channel = Instantiate (new GameObject (), Camera.main.gameObject.transform.position, Quaternion.identity) as GameObject;
		_channel.name = "Music Manager";
		_channel.transform.SetParent (Camera.main.gameObject.transform, true);
		channelGameObject = _channel;
	}
	public void SetChannel(int howManyChannel)
	{
		channels = new AudioSource[howManyChannel];
		volumes = new float[howManyChannel];

		for (int i = 0; i < howManyChannel; i++)
		{
			channelGameObject.AddComponent<AudioSource> ();
		}
		channels = channelGameObject.GetComponents<AudioSource> ();

		foreach (AudioSource s in channels) 
			s.loop = true;

	}
	public void UpdateAllVolume()
	{
		for (int i = 0; i < channels.Length; i++)
			channels[i].volume = volumes[i] * globalMusicVolume;
	}
	public void SetMusicIntoChannel(AudioClip music, int channel)
	{
		channels [channel].clip = music;
	}
	public void SetMusicIntoChannel(AudioClip music, int channel,float volume,bool andPlay)
	{
		channels [channel].clip = music;
		volumes [channel] = volume;
		channels [channel].volume = volumes [channel] * globalMusicVolume ;
		if (andPlay)
			channels [channel].Play ();
	}
	public void SetMusicIntoChannel(AudioClip[] music)
	{
		for (int i = 0; i < music.Length; i++)
		{
			channels [i].clip = music[i];
		}
	}
	public void SetVolume(int ch, float volume,bool andPlay)
	{
		volumes [ch] = volume;
		channels [ch].volume = volumes [ch] * globalMusicVolume ;

		if (andPlay)
			channels [ch].Play ();
	}
	public void SetTransition(int chFrom, int chTo, float time)
	{
		channelFrom = chFrom;
		channelTo = chTo;
		speed = 1 / time;
		inTransition = true;
		t = 0;
	}
	void Update()
	{
		if (inTransition) 
		{
			t += Time.deltaTime * speed;

			volumes [channelFrom] = Mathf.Lerp (1*globalMusicVolume, 0, t);
			volumes [channelTo] = Mathf.Lerp (0,1*globalMusicVolume, t);

			channels [channelFrom].volume = volumes [channelFrom];
			channels [channelTo].volume = volumes [channelTo];

			if (t > 1)
			{
				inTransition = false;
			}
		}
	}
}
