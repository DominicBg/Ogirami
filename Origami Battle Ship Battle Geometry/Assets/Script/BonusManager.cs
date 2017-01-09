using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour {

	static int achivementSteps = 0;
	public static int bonusCannonBall = 0;
	public static float bonusAimSpeed = 1;
	public static bool bonusLifeRegen = false;
	public static float bonusBiggerCannonBall = 1;
	public static int bonusLife = 0;


	// Use this for initialization
	public static void LoadAchivement()
	{
		achivementSteps = PlayerPrefs.GetInt ("AchivementSteps");
	}
	// Update is called once per frame
	public static void TestGainAchivement(int score)
	{
		if (score / 50 > achivementSteps) {
			achivementSteps++;
			Debug.Log ("new achivement " + achivementSteps);
		}
		PlayerPrefs.SetInt ("AchivementSteps",achivementSteps);
		PlayerPrefs.Save ();
	}
	public static void AjustBonus()
	{
		if (achivementSteps >= 1)//50
			bonusCannonBall = 1;
		
		if (achivementSteps >= 2)//100
			bonusAimSpeed = 1.25f;
		
		if (achivementSteps >= 3)//150
			bonusLifeRegen = true;
		
		if (achivementSteps >= 4)//200
			bonusCannonBall = 2;
		
		if (achivementSteps >= 5)//250
			bonusAimSpeed = 1.50f;
		
		if (achivementSteps >= 6)//300
			bonusLife=1;
		
		if (achivementSteps >= 7)//350
			bonusBiggerCannonBall=1.25f;
		
		if (achivementSteps >= 8)//400
			bonusAimSpeed = 1.75f;
		
		if (achivementSteps >= 9)//450
			bonusLife=2;
		
		if (achivementSteps >= 10)//500
			bonusBiggerCannonBall=1.5f;
		
		if (achivementSteps >= 11)//550
			bonusAimSpeed = 2.0f;

		if (achivementSteps >= 12)//600
			bonusLife=3;
		
		if (achivementSteps >= 13)//650
			bonusCannonBall = 3;

		if (achivementSteps >= 14)//700
			bonusAimSpeed = 2.25f;

		if (achivementSteps >= 15)//750
			bonusLife=4;
		
		if (achivementSteps >= 16)//800
			bonusCannonBall = 4;

		if (achivementSteps >= 17)//850
			bonusAimSpeed = 2.50f;

		if (achivementSteps >= 18)//900
			bonusLife=5;
		
		if (achivementSteps >= 19)//950
			bonusCannonBall = 5;

		if (achivementSteps >= 20)//1000
			bonusBiggerCannonBall=2f;
	}
}
