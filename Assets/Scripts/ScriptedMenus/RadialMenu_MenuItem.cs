using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RadialMenu
{
    [CreateAssetMenu(fileName = "MenuItem", menuName = "Radial Menu/Items/Action")]
    public class RadialMenu_MenuItem : ScriptableObject
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
        public List<RadialMenu_MenuItem> Children;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if has submenu</returns>
        public bool Selected() {

            if (ActionOverride!= null) {
                ActionOverride();
                return false;
            }

            if (!(string.IsNullOrEmpty(ObjectName) || string.IsNullOrEmpty(ActionName))) {
                var go = GameObject.Find(ObjectName);

                if (go != null)
                    go.BroadcastMessage(ActionName, Parameter, SendMessageOptions.DontRequireReceiver);
            }

            return Children.Count > 0;

        }


    }
}