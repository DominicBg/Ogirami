using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VOID_MANAGER_OF_SUPREME_GALAXY : MonoBehaviour {
	public float curveRadius, pipeRadius;
	public int curveSegmentCount, pipeSegmentCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	Vector3 GetPointOnTorus (float u, float v, float z)
	{
		Vector3 p;
		float r = curveRadius + pipeRadius * Mathf.Cos(v);
		p.x = r * Mathf.Cos (u);
		p.y = r * Mathf.Sin (u);
	//p.z = pipeRadius * Mathf.Sin (v);
		p.z = z;
		return p;
	}
	private void OnDrawGizmos ()
	{
		float vSteps = (2f * Mathf.PI) / pipeSegmentCount;
		float uStep = (2f * Mathf.PI) / curveSegmentCount;

		for (int u = 0; u < curveSegmentCount; u++) 
		{
			for (int v = 0; v < curveSegmentCount; v++) 
			{
				Vector3 pointPos = GetPointOnTorus(u* uStep,v * vSteps,v);
				Gizmos.DrawSphere(pointPos, 0.1f);
			}
		}
	}
}
