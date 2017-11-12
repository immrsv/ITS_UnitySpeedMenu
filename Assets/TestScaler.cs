using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestScaler : MonoBehaviour {

    [Range(0,1.5f)]
    public float Scale;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.one * Scale;

	}
}
