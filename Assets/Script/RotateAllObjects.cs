﻿using UnityEngine;
using System.Collections;

public class RotateAllObjects : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0.0f,1.0f,0.0f, Space.Self);
	}
}