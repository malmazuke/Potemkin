using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
    
//	private Camera mainCamera;
    
	void Start () {
	    transform.rotation = Camera.main.transform.rotation;
//        mainCamera = Camera.main;
	}
    
    void Update () {
//        transform.LookAt(mainCamera.transform.position, transform.up);
    }
}
