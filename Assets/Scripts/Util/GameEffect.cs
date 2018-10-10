//By Dominic Brodeur-Gendron & Patrice Le Nouveau
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameEffect {
	
	#region SHAKE
	public static void Shake(GameObject obj)
	{
		ShakeEffect (obj, 1, .2f, Vector3.zero, false);
	}

	public static void Shake(GameObject obj,float intensity)
	{
		ShakeEffect (obj, intensity, .2f, Vector3.zero, false);
		
	}

	public static void Shake(GameObject obj,float intensity,float time)
	{
		ShakeEffect (obj, intensity, time, Vector3.zero, false);
		
	}

	public static void ShakeDynamic(GameObject obj)
	{
		ShakeEffect (obj, 1, .2f, Vector3.zero, true);
	}

	public static void ShakeDynamic(GameObject obj,float intensity)
	{
		ShakeEffect (obj, intensity, .2f, Vector3.zero, true);
		
	}

	public static void ShakeDynamic(GameObject obj,float intensity,float time)
	{
		ShakeEffect (obj, intensity, time, Vector3.zero, true);
	}

	public static void ShakeRotation(GameObject obj, float intensity, Vector3 rotation, float time)
	{
		ShakeEffect (obj, intensity, time, rotation, true);
	}



	static void ShakeEffect(GameObject obj, float intensity, float time, Vector3 rotation, bool isDynamic)
	{
        if (obj.GetComponent<_GEffect.ShakeClass>() == null)
        {
            obj.AddComponent<_GEffect.ShakeClass>();
            _GEffect.ShakeClass shake = obj.GetComponent<_GEffect.ShakeClass>();
            shake.Init(intensity, time, rotation, isDynamic);
        }
	}



    #endregion

    #region Freeze Frame
    /// <summary>
    /// Freeze frame, useful when dealing with multicamera
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="sec"></param>
    public static void FreezeFrame(GameObject gameObject, float sec)
    {
        if (gameObject.GetComponent<_GEffect.FreezeFrameClass>() == null)
            gameObject.AddComponent<_GEffect.FreezeFrameClass>().freezeSec = sec;
    }
    /// <summary>
    /// Freezes frame.
    /// </summary>
    /// <param name="sec">Sec.</param>
    public static void FreezeFrame(float sec)
	{
		if (Camera.main.gameObject.GetComponent<_GEffect.FreezeFrameClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FreezeFrameClass> ().freezeSec = sec;
	}
	
	/// <summary>
	/// Freezes the frame with default value of 0.1.
	/// </summary>
	public static void FreezeFrame()
	{
		if (Camera.main.gameObject.GetComponent<_GEffect.FreezeFrameClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FreezeFrameClass> ().freezeSec = .1f;
	}
	#endregion
	
	#region Sprite & Color
	
	/// <summary>
	/// Sins the gradient.
	/// </summary>
	/// <returns>The gradient.</returns>
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
		Color color = Color.Lerp(color1, color2,t);
		return color;
	}
	public static Color SinGradient(Color color1, Color color2, float time, float speed)
	{
		float t = (Mathf.Sin(time * speed)+1) / 2;
		Color color = Color.Lerp(color1, color2,t);
		return color;
	}

	public static void FlashSprite(GameObject obj, Color color,float duration)
	{
		if (obj.GetComponent<_GEffect.FlashSpriteClass> () == null)
		{
			obj.AddComponent<_GEffect.FlashSpriteClass> ();
			_GEffect.FlashSpriteClass flashSprite = obj.GetComponent<_GEffect.FlashSpriteClass> ();
			
			flashSprite.flashColor = color;
			flashSprite.duration = duration;
			flashSprite.flashSpriteEnum = _GEffect.FlashSpriteClass.FlashSpriteType.Simple;
		}
		
	}
	
	public static void FlashSprite(GameObject obj, Color color,float duration, int flashCount)
	{
		if (obj.GetComponent<_GEffect.FlashSpriteClass> () == null)
		{
			obj.AddComponent<_GEffect.FlashSpriteClass> ();
			_GEffect.FlashSpriteClass flashSprite = obj.GetComponent<_GEffect.FlashSpriteClass> ();
			
			flashSprite.flashColor = color;
			flashSprite.duration = duration;
			flashSprite.flashCount = flashCount;
			flashSprite.flashSpriteEnum = _GEffect.FlashSpriteClass.FlashSpriteType.Multiple;
		}
		
	}
	
	public static void FlashSpriteLerp(GameObject obj, Color color,float duration)
	{
		if (obj.GetComponent<_GEffect.FlashSpriteClass> () == null)
		{
			obj.AddComponent<_GEffect.FlashSpriteClass> ();
			_GEffect.FlashSpriteClass flashSprite = obj.GetComponent<_GEffect.FlashSpriteClass> ();
			flashSprite.flashColor = color;
			flashSprite.speed = duration;
			flashSprite.flashSpriteEnum = _GEffect.FlashSpriteClass.FlashSpriteType.Lerp;
		}
		
	}
	
	public static void FlashCamera(Color color, float time)
	{
		if(Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> ().Flash (color,null, time,null);
	}
	public static void FlashCamera(Sprite image, float time)
	{
		if(Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> ().Flash (Color.white,image, time,null);
		
	}
	public static void FlashCamera(Color color, float time,Transform canvas)
	{
		if(Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> ().Flash (color,null, time,canvas);
	}
	public static void FlashCamera(Sprite image, float time,Transform canvas)
	{
		if(Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> ().Flash (Color.white,image, time,canvas);
		
	}
	public static void FlashCamera(Color color,Sprite image, float time,Transform canvas)
	{
		if(Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> () == null)
			Camera.main.gameObject.AddComponent<_GEffect.FlashCameraClass> ();
		
		Camera.main.gameObject.GetComponent<_GEffect.FlashCameraClass> ().Flash (color,image, time,canvas);
		
	}
	#endregion
	
	public static void DestroyChilds(Transform parent)
	{
		if (parent.childCount != 0) 
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
public static class GamePhysics
{
	public static Vector3 BallisticVel(Transform origin,Transform target)
	{
		Vector3 dir = target.position - origin.position;
		dir.y = 0;
		
		float dist = dir.magnitude;
		
		float vel = Mathf.Sqrt (dist * Physics.gravity.magnitude);
		
		return vel * dir.normalized;
	}
	
	public static Vector3 BallisticVel(Transform origin,Transform target, float angle)
	{
		Vector3 dir = target.position - origin.position;
		float h = dir.y;
		dir.y = 0;
		
		float dist = dir.magnitude;
		float a = angle * Mathf.Deg2Rad;
		
		dir.y = dist * Mathf.Tan (a);
		dist += h / Mathf.Tan (a);
		
		float vel = Mathf.Sqrt (dist * Physics.gravity.magnitude / Mathf.Sin(2.5f * a));
		return vel * dir.normalized ;
	}
}
public static class GameMath
{
	///// <summary>
	/// Calculate the horizontal position of a given index if you have n objects.
	/// Everything will be align by center
	/// Ideally, you call this function in a for loop to position every objects
	/// 
	/// for(i ...)
	/// 	objects[i].position = new Vector2(CenterAlign(objects.length, dist, i), yPos);
	/// </summary>
	/// <returns>The horizontal position</returns>
	/// <param name="NumberOfObject">Number of objects.</param>
	/// <param name="distance">Distance between each objects.</param>
	/// <param name="i">The index of the object.</param>
	public static float CenterAlign(int NumberOfObject, float distance, int i)
	{
		return ((i - (((NumberOfObject) - 1) / 2)) * distance) - (((NumberOfObject + 1) % 2) * (distance / 2));
	}

	public static float PerlinNoiseNegOneToOne(float x, float y)
	{
		return (Mathf.PerlinNoise(x,y) * 2) - 1;
	}

    public static Vector3 SphericalRotation(float radius, float theta, float phi)
    {
        float x = radius * Mathf.Sin(theta) * Mathf.Cos(phi);
        float y = radius * Mathf.Sin(theta) * Mathf.Sin(phi);
        float z = radius * Mathf.Cos(theta);

        return new Vector3(x, y, z);
    }

    #region Curves

    public static float Stretch(float A, float stretchAmount)
	{
		if (A > 1)
			A = 1;
		
		float C = Mathf.Abs (A - .5f);
		
		return .5f +  stretchAmount + ((C + .5f) * stretchAmount);
	}

	/// <summary>
	/// Return a log function between 0 and 1
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	public static float Log01(float t)
	{
		return Mathf.Log10 ((t + .11f) * 9f);
	}
	/// <summary>
	/// Return a smooth Sigmoid between 0 and 1
	/// </summary>
	/// <param name="x">The t coordinate between 0 and 1.</param>
	public static float Sigmoid01(float t)
	{
		return Mathf.Clamp01 (1 / (1 + Mathf.Exp (-(10 * t - 5.1f))));
	}
	/// <summary>
	/// Return a steep sigmoid between 0 and 1
	/// </summary>
	/// <param name="t">The t coordinate between 0 and 1.</param>
	public static float SteepSigmoid01(float t)
	{
		return Mathf.Clamp01 (1 / (1 + Mathf.Exp (-(30 * t - 15))));
	}
	/// <summary>
	/// Return a zigzag from 0 to 1
	/// </summary>
	/// <returns>The zag01.</returns>
	/// <param name="t">T.</param>
	public static float ZigZag01(float t)
	{
		return Mathf.Clamp01((.9f * Mathf.Sin (t * 1.37f) + .1f * Mathf.Sin (t * 1.37f * 15)) * 1.02f);
	}
	public static float ExtremeExp01(float t)
	{
		return Mathf.Clamp01 (5.9f * Mathf.Exp(t * 10 - 11.77f));
	}
	/// <summary>
	/// Bounce between 0 and 1
	/// </summary>
	public static float Bounce01(float t) //from Mathfx
	{
		return Mathf.Abs (Mathf.Sin (6.28f * (t + 1) * (t + 1)) * (1 - t));
	}
	#endregion

	#region Distance
	public static bool IsMinimumDistance(GameObject object1,GameObject object2,float minimumDistance)
	{
		if (Mathf.Abs(object1.transform.position.x - object2.transform.position.x) < minimumDistance &&
		    Mathf.Abs(object1.transform.position.y - object2.transform.position.y) < minimumDistance &&
		    Mathf.Abs(object1.transform.position.z - object2.transform.position.z) < minimumDistance)
			return true;
		
		return false;
	}
	public static bool IsMinimumDistance(Transform object1,Transform object2,float minimumDistance)
	{
		if (Mathf.Abs(object1.position.x - object2.position.x) < minimumDistance &&
		    Mathf.Abs(object1.position.y - object2.position.y) < minimumDistance &&
		    Mathf.Abs(object1.position.z - object2.position.z) < minimumDistance)
			return true;
		
		return false;
	}
	public static bool IsMinimumDistance(Vector3 object1,Vector3 object2,float minimumDistance)
	{
		if (Mathf.Abs(object1.x - object2.x) < minimumDistance &&
		    Mathf.Abs(object1.y - object2.y) < minimumDistance &&
		    Mathf.Abs(object1.z - object2.z) < minimumDistance)
			return true;
		
		return false;
	}
	public static bool IsMinimumDistance(Vector2 object1,Vector2 object2,float minimumDistance)
	{
		if (Mathf.Abs(object1.x - object2.x) < minimumDistance &&
		    Mathf.Abs(object1.y - object2.y) < minimumDistance)
			return true;
		
		return false;
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
    #endregion

    #region Vectors

    public static Vector3 GetNormal(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return Vector3.Cross(p2 - p1, p3 - p1).normalized;
    }

    public static Vector2 RotateVector(float angle, Vector2 point)
	{
		float a = angle * Mathf.PI / 180;
		float cosA = Mathf.Cos (a);
		float sinA = Mathf.Sin (a);
		Vector2 newPoint = 
			new Vector2 (
				(point.x * cosA - point.y * sinA),
				(point.x * sinA + point.y * cosA)
				);
		return newPoint;
	}
	
    public static Vector2 PolarToCartesian(float radius, float theta)
    {
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);
        return new Vector2(x, y);
    }

    /// <summary>
    /// </summary>
    /// <param name="radiusAndTheta">A Vector2 containing x = radius, y = theta</param>
    /// <returns></returns>
    public static Vector2 PolarToCartesian(Vector2 radiusAndTheta)
    {
        return PolarToCartesian(radiusAndTheta.x, radiusAndTheta.y);
    }

    /// <summary>
    /// Start with a x,y and receive a Vector2(Radius, Theta)
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector2 CartesianToPolar(Vector2 position)
    {
        float r = Mathf.Sqrt(position.x * position.x + position.y * position.y);
        float theta = Mathf.Atan2(position.y, position.x);
        return new Vector2(r, theta);
    }

    public static Vector2 CartesianToPolar(float x, float y)
    {
        return CartesianToPolar(new Vector2(x,y));
    }

    public static Vector3 RotateVectorY(float angle, Vector3 point)
	{
		Vector2 vec = RotateVector(angle,  new Vector2 (point.x, point.z));
		return new Vector3 (vec.x, point.y, vec.y);
	}
	/// <summary>
	/// Return the angle of two vectors from -180 to 180 degree
	/// </summary>
	public static float Angle(Vector3 from, Vector3 to)
	{
		return (Vector3.Angle (from,to))* (-Mathf.Sign (Vector3.Cross (from, to).y));
	}
	
	/// <summary>
	/// Return the angle of two vectors from -180 to 180 degree (to test lol)
	/// </summary>
	public static float Angle(Vector2 from, Vector2 to)
	{
		///to test
		return (Vector2.Angle (from,to))* (-Mathf.Sign (Vector3.Cross (from, to).y));
	}
	#endregion
}
#region hidden functions
namespace _GEffect
{
	public class FreezeFrameClass : MonoBehaviour {
		
		public float freezeSec;
		
		void Start()
		{
			StartCoroutine (FreezeFrameEffect());
		}
		
		IEnumerator FreezeFrameEffect()
		{
			Time.timeScale = 0f;
			float pauseEndTime = Time.realtimeSinceStartup + freezeSec;
			while (Time.realtimeSinceStartup < pauseEndTime)
				yield return 0;
			
			Time.timeScale = 1;
			Destroy (this);
		}
	}
	
	public class ShakeClass : MonoBehaviour
	{
		float intensity;
		float time;
		Vector3 rotation;
		bool isDynamic;
		bool isPerlinMode = false;
		Vector3 originalPos;
		Vector3 originalRotation;
		float speedPerlin = 25f;

		public void Init(float intensity, float time, Vector3 rotation, bool isDynamic)
		{
			this.intensity = intensity;
			this.time = time;
			this.rotation = rotation;
			this.isDynamic = isDynamic;
		}
		
		void OnEnable()
		{
			originalPos = transform.localPosition;
			originalRotation = transform.localEulerAngles;
		}
		
		void Update()
		{
			if (isDynamic)
			{	
				originalPos = transform.localPosition;
			}
		}

		void LateUpdate()
		{
			if (time > 0)
			{
				if(isPerlinMode)
				{
					transform.localEulerAngles = originalRotation + RandomRotationPerlin();
					transform.localPosition = originalPos + RandomPerlinPosition() * intensity;
				}
				else
				{
					transform.localEulerAngles = originalRotation + RandomRotation();
					transform.localPosition = originalPos + Random.insideUnitSphere * intensity;
				}

				time -= Time.deltaTime;
			}
			else
			{
				time = 0f;
				transform.localPosition = originalPos;
				transform.localEulerAngles = originalRotation;
                intensity *= 0.95f;

                Destroy(this);
			}
		}

		Vector3 RandomPerlinPosition()
		{
			float seed = speedPerlin * Time.time;
			return new Vector3(
				GameMath.PerlinNoiseNegOneToOne(seed, seed + 1),
				GameMath.PerlinNoiseNegOneToOne(seed + 2, seed + 3),
				GameMath.PerlinNoiseNegOneToOne(seed + 4, seed + 5)
				);
		}

		Vector3 RandomRotation()
		{
			if(rotation == Vector3.zero)
			{	
				return Vector3.zero;
			}
			else
			{
				return new Vector3(
					rotation.x * Random.Range(-1.0f,1.0f),
					rotation.y * Random.Range(-1.0f,1.0f),
					rotation.z * Random.Range(-1.0f,1.0f)
					);
			}
		}
		
		Vector3 RandomRotationPerlin()
		{
			if(rotation == Vector3.zero)
			{	
				return Vector3.zero;
			}
			else
			{
				float seed = speedPerlin * Time.time;
				return new Vector3(
					rotation.x = GameMath.PerlinNoiseNegOneToOne(seed, 0),
					rotation.y = GameMath.PerlinNoiseNegOneToOne(seed + 15, 0),
					rotation.z = GameMath.PerlinNoiseNegOneToOne(0, seed)
					);
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
		float t = 0;
		float speed;
		GameObject screen;
		Color color;
		Image image;
		
		bool isFlashing = false;
		bool isIncreasing = true;
		Transform canvas;

		void Awake()
		{
			screen = new GameObject ();
			screen.AddComponent<Image> ();
			screen.GetComponent<Image> ().raycastTarget = false;
			screen.name = "Flashing Screen";
			screen.GetComponent<Image> ().color = new Color (0, 0, 0, 0);
		}

		void SetCanvas()
		{
			if(canvas == null)
				screen.transform.SetParent (GameObject.Find ("Canvas").transform, true);
			else
				screen.transform.SetParent (canvas, true);
			
			screen.GetComponent<Image> ().rectTransform.sizeDelta = new Vector2 (Screen.width, Screen.height);
			screen.GetComponent<Image> ().rectTransform.localPosition = Vector2.zero;
		}
		
		public void Flash(Color _color, Sprite sprite, float time, Transform _canvas)
		{
			if (_canvas != null)
			{
				if(canvas != _canvas)
					canvas = _canvas;		
				
				SetCanvas ();
			}
			
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
}
#endregion
