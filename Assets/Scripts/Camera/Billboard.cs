﻿using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	    transform.rotation = Camera.main.transform.rotation;
	}
}