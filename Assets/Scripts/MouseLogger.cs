using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseLogger : MonoBehaviour, 
    IPointerClickHandler   
    
{
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Clicked on: " + gameObject.name + " >> " + eventData.position);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
