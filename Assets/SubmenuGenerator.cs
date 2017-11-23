using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RadialMenu;
using RadialMenu.ScriptedMenus;

public class SubmenuGenerator : MonoBehaviour {

    public RadialMenu_Master Menu;

    protected RadialMenu_MenuItem ContextItem;

	// Use this for initialization
	void Start () {

        ContextItem = new RadialMenu_MenuItem();
        ContextItem.name = "Submenu";

        var temp = new RadialMenu_MenuItem(() => { SubgenSays("Hello"); });
        temp.name = "Say Hello!";

        ContextItem.Children.Add(temp);

        Menu.RootItems.Add(ContextItem);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SubgenSays(string val) {

        Debug.Log("SubmenuGenerator::SubgenSays(): " + val);
    }
}
