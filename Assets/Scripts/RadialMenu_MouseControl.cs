using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadialMenu {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RadialMenu_Master))]
    public class RadialMenu_MouseControl : MonoBehaviour {

        [UnityEngine.Serialization.FormerlySerializedAs("ButtonName")]
        public string TriggerName = "Fire1";

        public bool AlignToViewsphere;
        public float Distance = 20;

        protected RadialMenu_Master Menu;

        protected CursorLockMode PreviousCursorLockState;

        // Use this for initialization
        void Start() {
            Menu = GetComponent<RadialMenu_Master>();
        }

        // Update is called once per frame
        void Update() {
            var position = Input.mousePosition;
            position.z = Distance;

            var worldPosition = Camera.main.ScreenToWorldPoint(position);

            if (Input.GetButtonDown(TriggerName)) {
                var rotation = Camera.main.transform.rotation;
                if (AlignToViewsphere) {
                    Debug.LogWarning(gameObject.name + " - PieMenu_MouseControl::Update(): AlignToViewsphere Not Implemented.");
                }

                Menu.Show(worldPosition, rotation);
            }

            if (Input.GetButtonUp(TriggerName)) {
                Menu.Hide();
            }

            if (Menu.IsVisible) {
                Menu.UpdateCursor(worldPosition - Menu.transform.position);
            }
        }
    }
}