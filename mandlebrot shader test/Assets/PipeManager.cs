using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour {
	public float curveRadius, pipeRadius;
	public int curveSegmentCount, pipeSegmentCount;

	Mesh mesh;
	Vector3[] vertices;
	private int[] triangles;

	// Use this for initialization
	void Start () {
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "grosse pipe";
		SetVerticies ();
		SetTriangles ();

		mesh.RecalculateNormals ();
	}

	private void SetVerticies()
	{
		vertices = new Vector3[pipeSegmentCount * curveSegmentCount * 4];
		float uStep = (2f * Mathf.PI) / curveSegmentCount;
		CreateFirstQuadRing (uStep);
		int iDelta = pipeSegmentCount * 4;
		for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta) 
		{
			CreateQuadRing (u * uStep, i);
		}
		mesh.vertices = vertices;
	}
	private void SetTriangles()
	{
		triangles = new int[pipeSegmentCount * curveSegmentCount * 6];
		for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4) 
		{
			triangles[t] = i;
			triangles [t + 1] = triangles [t + 4] = 1 + i;
			triangles [t + 2] = triangles [t + 3] = 2 + i;
			triangles [t + 5] = 3 + i;
		}
		mesh.triangles = triangles;
	}
	private void CreateFirstQuadRing (float u)
	{
		float vStep = (2f * Mathf.PI) / pipeSegmentCount;
		Vector3 vertexA = GetPointOnTorus (0f, 0f);
		Vector3 vertexB = GetPointOnTorus (u, 0f);
		for(int v = 1, i = 0; v <= pipeSegmentCount; v++, i+=4)
		{
			vertices [i] = vertexA;
			vertices [i + 1] = vertexA = GetPointOnTorus (0f, v * vStep);
			vertices [i + 2] = vertexB;
			vertices [i + 3] = vertexB = GetPointOnTorus(u,v*vStep);
		}

	}
	private void CreateQuadRing (float u,int i)
	{
		float vStep = (2f * Mathf.PI) / pipeSegmentCount;
		int ringOffset = pipeSegmentCount * 4;

		Vector3 vertex = GetPointOnTorus (u, 0f);
		for(int v = 1; v <= pipeSegmentCount; v++, i+=4)
		{
			vertices [i] = vertices[i - ringOffset + 2];
			vertices [i + 1] =  vertices[i - ringOffset + 3];
			vertices [i + 2] = vertex;
			vertices [i + 3] = vertex = GetPointOnTorus(u,v*vStep);
		}

	}

	// Update is called once per frame
	void Update () {
		
	}
	Vector3 GetPointOnTorus (float u, float v)
	{
		Vector3 p;
		float r = curveRadius + pipeRadius * Mathf.Cos(v);
		p.x = r * Mathf.Cos (u);
		p.y = r * Mathf.Sin (u);
		p.z = pipeRadius * Mathf.Sin (v);
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
				Vector3 pointPos = GetPointOnTorus(u* uStep,v * vSteps);
				Gizmos.DrawSphere(pointPos, 0.1f);
			}
		}
	}
}
