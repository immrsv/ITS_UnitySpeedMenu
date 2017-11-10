using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieMenu : MonoBehaviour
{

    public GameObject Centre;
    public GameObject[] Options;

    public string ToggleButton = "Fire1";

    public bool Visible = true;



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
    void Update() {

        if (Input.GetButtonDown(ToggleButton))
            Show();

        if (Input.GetButtonUp(ToggleButton))
            Hide();
    }


    void Show() {



        Centre.SetActive(true);
        for (var i = 0; i < Options.Length; i++) {
            Options[i].SetActive(true);
        }


        transform.position = Input.mousePosition;
    }


    void Hide() {
        Centre.SetActive(false);
        for ( var i = 0; i < Options.Length; i++) {
            Options[i].SetActive(false);
        }
    }
}
