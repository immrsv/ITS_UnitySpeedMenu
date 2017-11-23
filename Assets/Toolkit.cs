using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolkit : MonoBehaviour {

    public List<GameObject> Objects;

    public void Toggle(string name) {
        foreach ( var go in Objects) {
            if ( go != null &&  go.name == name) {
                go.SetActive(!go.activeSelf);
                break;
            }
        }
    }
}
