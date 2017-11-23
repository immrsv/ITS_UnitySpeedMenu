using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RadialMenu.ScriptedMenus
{
    [CreateAssetMenu(fileName = "MenuItem", menuName = "Radial Menu/Menu Item")]
    public class RadialMenu_MenuItem : ScriptableObject, IRadialMenuContainer, IRadialMenuAction
    {

        public RadialMenu_MenuItem() {

        }

        public RadialMenu_MenuItem(System.Action actionOverride) {
            ActionOverride = actionOverride;
        }

        protected System.Action ActionOverride;

        [Header("Interface Options")]
        public UnityEngine.UI.Image Icon;

        [Header("Actions")]
        public string ObjectName;
        public string ActionName;
        public string Parameter;

        [Header("Submenu")]
        public List<RadialMenu_MenuItem> _Children = new List<RadialMenu_MenuItem>();
        public List<RadialMenu_MenuItem> Children { get { return _Children; } }

        public bool HasChildren {  get { return _Children != null && _Children.Count > 0; } }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if has submenu</returns>
        public void PerformAction() {

            if (ActionOverride!= null) {
                ActionOverride();
                return;
            }

            if (!(string.IsNullOrEmpty(ObjectName) || string.IsNullOrEmpty(ActionName))) {
                var go = GameObject.Find(ObjectName);

                if (go != null)
                    go.BroadcastMessage(ActionName, Parameter, SendMessageOptions.DontRequireReceiver);
            }
        }


    }
}