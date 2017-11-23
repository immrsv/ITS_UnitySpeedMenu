using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using RadialMenu.ScriptedMenus;

namespace RadialMenu {
    public class RadialMenu_SegmentAnimator : MonoBehaviour {

        public bool AlignContentsToWorld = true;

        protected Image _BackgroundImage;
        public Image BackgroundImage {
            get {
                if (_BackgroundImage == null ) _BackgroundImage = transform.GetChild(0).GetComponent<Image>();
                return _BackgroundImage;
            }
        }

        protected Image _BackgroundHighlightMask;
        public Image BackgroundHighlightMask {
            get {
                if (_BackgroundHighlightMask == null) _BackgroundHighlightMask = transform.GetChild(1).GetComponent<Image>();
                return _BackgroundHighlightMask;
            }
        }

        protected Image _BackgroundHighlight;
        public Image BackgroundHighlight {
            get {
                if (_BackgroundHighlight == null) _BackgroundHighlight = transform.GetChild(1).GetChild(0).GetComponent<Image>();
                return _BackgroundHighlight;
            }
        }

        protected Transform _Contents;
        public Transform Contents {
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

        public float SegmentFillHalfangle { get { return SegmentFillAmount * 180; } }

        public float SegmentRotation {
            get {
                return transform.localRotation.z / 360.0f;
            }
            set {
                transform.localRotation = Quaternion.Euler(0, 0, 360 * value);
            }
        }

        public Color BackgroundColor { get { return BackgroundImage.color; } set { BackgroundImage.color = value; } }
        public Color HighlightColor { get { return BackgroundImage.color; } set { BackgroundImage.color = value; } }
        public Color SelectingColor { get { return BackgroundImage.color; } set { BackgroundImage.color = value; } }
        public Color TextColor { get { return BackgroundImage.color; } set { BackgroundImage.color = value; } }
        
        public bool IsSelectable { get { return Item != null; } }

        protected RadialMenu_MenuItem _Item;
        public RadialMenu_MenuItem Item {
            get {
                return _Item;
            }
            set {
                //if (_Item == value) return;
                _Item = value;

                if (_Item != null) {
                    Contents.GetComponent<TextMeshProUGUI>().text = _Item.name;
                }
                else {
                    Contents.GetComponent<TextMeshProUGUI>().text = "-";
                }
            }
        }

        private void Start() {
            Contents.GetComponent<TextMeshProUGUI>().text = "--";
        }

        private void Update() {
        }

        void RM_UpdateCursor(Vector3 localPosition) {
            //Debug.Log(gameObject.name + " - OptionAnimator::_UpdateCursor(): Local Position = " + localPosition + " [" + MaskImage.transform.up + "]");

            var localUp = transform.localRotation * Vector3.up;

            var clampedPosition = localPosition.sqrMagnitude <= 1.0 ? localPosition : localPosition.normalized;

            var dot = Mathf.Clamp01(1.05f * Vector3.Dot(clampedPosition, -localUp));

            transform.localScale = Vector3.one * (1.0f + dot / 10.0f);

            BackgroundHighlight.transform.localScale = Vector3.one * dot;
            

        }
        
        public bool Contains(Vector3 localPosition)
        {
            if (!IsSelectable) return false;

            var localUp = (transform.localRotation * Vector3.up);

            //Debug.Log("Half Angle: " + (Mathf.Acos(Vector3.Dot(localPosition.normalized, -localUp)) * Mathf.Rad2Deg).ToString("N3") + " < " + SegmentFillHalfangle.ToString("N3"));

            return (Mathf.Acos(Vector3.Dot(localPosition.normalized, -localUp)) * Mathf.Rad2Deg) < SegmentFillHalfangle;
        }
    }
}