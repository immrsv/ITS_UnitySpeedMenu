using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieMenu : MonoBehaviour, UnityEngine.EventSystems.I
{

    public GameObject OptionBack;
    public List<GameObject> Options;

    public void OnMove(AxisEventData eventData) {
        Debug.Log("OnMove(): " + eventData.moveVector);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("PointerEnter(): " + eventData.position);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
