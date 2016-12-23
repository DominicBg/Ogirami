﻿using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {
	public MeshFilter Water;
	public MeshFilter WaterCopy;

	Vector3[] vertices;
	Vector3[] normals;

	delegate float WaveFunctionDelegate(int i);
	WaveFunctionDelegate WaveFunction;

	// Use this for initialization
	void Start () 
	{
		vertices = Water.mesh.vertices;
		normals = Water.mesh.normals;
		WaveFunction = SinWave;
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < vertices.Length; i++) 
		{
			/*
			vertices [i] += normals [i] *
			(0.01f * Mathf.Sin (2 * i + 3 * Time.time)) *
				(Mathf.Cos (3 * i + 7 * Time.time)) *
				(Mathf.Cos (100 * i + 1 * Time.time));
				*/
			vertices [i] += normals [i] * WaveFunction(i);
		}
		Water.mesh.vertices = vertices;
		WaterCopy.mesh.vertices = vertices;
	}
	float SinWave(int i)
	{
		return (0.01f * Mathf.Sin (19 * i+3 * Time.time) * Mathf.Sin (11 * i+5 * Time.time));
	}
	float Ripple(int i)
	{
		return 0.01f * (Mathf.Sin (10 * (i * i) ) / 10);
	}
}