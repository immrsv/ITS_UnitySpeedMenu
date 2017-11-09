using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaHitTest : MonoBehaviour {

    public float AlphaThreshold = 0.5f;

	// Use this for initialization
	void Start () {
        
    }
	
    void Update() {
        GetComponent<UnityEngine.UI.Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
    }
}
