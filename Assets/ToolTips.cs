using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTips : MonoBehaviour {
    public GameObject MeasuringTool;
    public GameObject ToolTipsHolder;
    public GameObject GripTT;
    public bool tutorial;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(tutorial)
        {
            ToolTipsHolder.SetActive(true);
        }
        if(tutorial && MeasuringTool)
        {
            GripTT.SetActive(true);
        }
	}
}
