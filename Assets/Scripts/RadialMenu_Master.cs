using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RadialMenu {
    public class RadialMenu_Master : MonoBehaviour {
        [System.Serializable]
        public class MenuColors {
            public Color BackColor = Color.black;
            public Color HoverColor = Color.gray;
        }

        public float RevealDelay = 0.3f;

        public MenuColors Colors;

        [Header("Segments")]
        public GameObject Centre;
        public GameObject SegmentsMaster;
        public RadialMenu_SegmentAnimator[] Segments;

        public bool IsVisible { get { return Centre.activeSelf; } }

        [Header("Selection")]
        [Range(0,1)]
        public float SelectionRadius;
        [Range(0,1)]
        public float ConfirmationRadius;

        protected int SelectedIndex;
        protected float SelectionStart;

        // Use this for initialization
        void Start() {
            ConfirmationRadius = Mathf.Min(SelectionRadius, ConfirmationRadius);
        }

        // Update is called once per frame
        void Update() {

        }


        public void Show(Vector3 position, Quaternion rotation) {

            Centre.SetActive(true);
            for (var i = 0; i < Segments.Length; i++) {
                if (Segments[i] == null) continue;
                Segments[i].gameObject.SetActive(true);
            }

            transform.position = position;
            transform.rotation = rotation;


            LeanTween.value(SegmentsMaster, (value) => { SegmentsMaster.GetComponent<Image>().fillAmount = value; }, 0, 1, RevealDelay)
                .setOnStart(() => {
                    SegmentsMaster.GetComponent<Image>().enabled = true;
                    SegmentsMaster.GetComponent<Image>().fillClockwise = true;
                    SegmentsMaster.GetComponent<Image>().fillAmount = 0;

                    SegmentsMaster.GetComponent<Mask>().enabled = true;
                })
                .setOnComplete(() => {
                    SegmentsMaster.GetComponent<Image>().enabled = false;
                    SegmentsMaster.GetComponent<Mask>().enabled = false;
                });
        }


        public void Hide() {

            LeanTween.value(SegmentsMaster, (value) => { SegmentsMaster.GetComponent<Image>().fillAmount = value; }, 1, 0, RevealDelay)
                .setOnStart(() => {
                    SegmentsMaster.GetComponent<Image>().enabled = true;
                    SegmentsMaster.GetComponent<Image>().fillClockwise = false;
                    SegmentsMaster.GetComponent<Image>().fillAmount = 1;

                    SegmentsMaster.GetComponent<Mask>().enabled = true;
                })
                .setOnComplete(() => {
                    Centre.SetActive(false);
                    for (var i = 0; i < Segments.Length; i++) {
                        if (Segments[i] == null) continue;
                        Segments[i].gameObject.SetActive(false);
                    }
                });

        }


        void ShowSegments(Action onComplete) { }
        void HideSegments(Action onComplete) { }

        public void UpdateCursor(Vector3 position) {
            BroadcastMessage("RM_UpdateCursor", position, SendMessageOptions.DontRequireReceiver);

            foreach ( var segment in Segments) {
                if (SegmentContains(segment, position))
                    segment.BackgroundColor = Color.green;
                else
                    segment.BackgroundColor = Color.black;
            }
        }


        protected bool SegmentContains(RadialMenu_SegmentAnimator segment, Vector3 point) {

            if (point.magnitude < SelectionRadius)
                return false;

            if (Mathf.Acos(Vector3.Dot(point.normalized, -segment.transform.up)) < segment.SegmentFillHalfangle)
                return true;

            return false;
        }
    }
}