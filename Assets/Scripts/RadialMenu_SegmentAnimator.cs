using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RadialMenu {
    public class RadialMenu_SegmentAnimator : MonoBehaviour {

        public bool AlignContentsToWorld;

        protected Image _BackgroundImage;
        protected Image BackgroundImage {
            get {
                if (_BackgroundImage == null ) _BackgroundImage = transform.GetChild(0).GetComponent<Image>();
                return _BackgroundImage;
            }
        }

        protected Image _BackgroundHighlightMask;
        protected Image BackgroundHighlightMask {
            get {
                if (_BackgroundHighlightMask == null) _BackgroundHighlightMask = transform.GetChild(1).GetComponent<Image>();
                return _BackgroundHighlightMask;
            }
        }

        protected Image _BackgroundHighlight;
        protected Image BackgroundHighlight {
            get {
                if (_BackgroundHighlight == null) _BackgroundHighlight = transform.GetChild(1).GetChild(0).GetComponent<Image>();
                return _BackgroundHighlight;
            }
        }

        protected Transform _Contents;
        protected Transform Contents {
            get {
                if ( _Contents == null ) _Contents = transform.GetChild(2).transform;
                return _Contents;
            }
        }

        public float SegmentFillAmount {
            get {
                return BackgroundImage.fillAmount;
            }
            set {
                var clampedValue = Mathf.Clamp01(value);

                if (SegmentFillAmount == clampedValue) return; // no change required

                BackgroundImage.fillAmount = clampedValue;
                BackgroundImage.transform.localRotation = Quaternion.Euler(0, 0, 180 * clampedValue);

                BackgroundHighlightMask.fillAmount = clampedValue;
                BackgroundHighlightMask.transform.localRotation = Quaternion.Euler(0, 0, 180 * clampedValue);

                BackgroundHighlight.transform.localRotation = Quaternion.Euler(0, 0, -180 * clampedValue);
            }
        }

        public float SegmentFillHalfangle { get { return BackgroundImage.transform.localRotation.z; } }

        public float SegmentRotation {
            get {
                return transform.rotation.z / 360.0f;
            }
            set {
                transform.rotation = Quaternion.Euler(0, 0, 360 * value);
            }
        }

        public Color BackgroundColor { get { return BackgroundImage.color; } set { BackgroundImage.color = value; } }
        public Color HighlightColor { get { return BackgroundImage.color; } set { BackgroundImage.color = value; } }
        public Color SelectedColor { get { return BackgroundImage.color; } set { BackgroundImage.color = value; } }
        public Color TextColor { get { return BackgroundImage.color; } set { BackgroundImage.color = value; } }
        
        
        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {

            if (AlignContentsToWorld)
                Contents.up = Camera.main.transform.up;
        }


        void RM_UpdateCursor(Vector3 localPosition) {
            //Debug.Log(gameObject.name + " - OptionAnimator::_UpdateCursor(): Local Position = " + localPosition + " [" + MaskImage.transform.up + "]");

            var clampedPosition = localPosition.sqrMagnitude <= 1.0 ? localPosition : localPosition.normalized;

            var dot = Mathf.Clamp01(1.05f * Vector3.Dot(clampedPosition, -transform.up));

            transform.localScale = Vector3.one * (1.0f + dot / 10.0f);

            BackgroundHighlight.transform.localScale = Vector3.one * dot;
            

        }
    }
}