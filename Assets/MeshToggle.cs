using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshToggle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Toggle() {
        var renderer = GetComponent<MeshRenderer>();
        renderer.enabled = !renderer.enabled;
    }
}
