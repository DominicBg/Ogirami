using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_System : MonoBehaviour {
	string startLetter = "F";
	[SerializeField]string changeBy = "FF+[+F+F-F]-[-F-F+F]";
	string current = "";
	[SerializeField]GameObject branch;
	Transform CurrentPosition;
	GameObject LastBranch;
	Stack<GameObject> stack = new Stack<GameObject>();
	float len = 2;
	float startSize = 1;
	float nextAngle = 0;
	[SerializeField]float angle = 25;
	[SerializeField]int iteration = 4;

	// Use this for initialization
	void Start()
	{
		current = startLetter;
	
		for (int i = 0; i < iteration; i++) 
		{
			GenerateText ();
			StartCoroutine(CreateTree ());
			startSize *= 0.5f;
		}
		//CreateTree ();
	}
	void GenerateText()
	{
		string newCurrent = "";
		for (int j = 0; j < current.Length; j++)
		{
			if (current [j] == 'F') 
				newCurrent += changeBy;
			else 
				newCurrent += current [j];
		}
		current = newCurrent;
	}
	IEnumerator CreateTree ()
	{
		CurrentPosition = transform;
		LastBranch = transform.gameObject;

		for (int i = 0; i < current.Length; i++) 
		{
			if (current [i] == 'F')
			{
				GameObject br = Instantiate (branch, CurrentPosition.position, Quaternion.identity) as GameObject;
				if (i == 0) 
				{
					br.transform.localScale = new Vector3 (startSize, startSize, startSize);
					Debug.Log (LastBranch + " " + startSize);
				}
				br.transform.SetParent (LastBranch.transform, true);
				br.transform.localPosition = new Vector3 (0, 2, 0);
				br.transform.localEulerAngles = new Vector3 (0, 0, nextAngle);
				nextAngle = 0;
				CurrentPosition = br.transform;
				LastBranch = br;
				yield return new WaitForSeconds (0.1f);
			} 
			else if (current [i] == '+')
			{
				nextAngle += angle;
			} 
			else if (current [i] == '-') 
			{
				nextAngle -= angle;
			} 
			else if (current [i] == '[')
			{
				stack.Push (LastBranch);
			}
			else if (current [i] == ']')
			{
				LastBranch = stack.Pop ();
			}
		}
	}

}
