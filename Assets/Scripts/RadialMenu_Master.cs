using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using RadialMenu.ScriptedMenus;

/// <summary>
/// TODO:
/// Preselect segment
/// Confirm Select Segment
/// Activate Segment
/// "Paging"
/// </summary>
namespace RadialMenu {
    public class RadialMenu_Master : MonoBehaviour {

        #region " Segment Layout Presets "

        private enum SegmentSystemButton {
            None,
            Back,
            PagePrev,
            PageNext,
        }

        private class SegmentPreset {
            public SegmentSystemButton SystemButton;
            public float Rotation;
            public float Size;
        }

        private Dictionary<int, List<SegmentPreset>> SegmentPresets = new Dictionary<int, List<SegmentPreset>>();

        public RadialMenu_Master() {
            // 8 Elements:
            var segmentCount = 8;
            SegmentPresets.Add(segmentCount, new List<SegmentPreset>());
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 0, Size = 40, SystemButton = SegmentSystemButton.Back });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 045, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 090, Size = 40, SystemButton = SegmentSystemButton.PageNext });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 135, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 180, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 225, Size = 40 });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 270, Size = 40, SystemButton = SegmentSystemButton.PagePrev });
            SegmentPresets[segmentCount].Add(new SegmentPreset { Rotation = 315, Size = 40 });
        }

        #endregion
        #region " Events "
        public event Action<object> MenuItemsChanged;
        public void RaiseMenuItemsChanged() { if (MenuItemsChanged != null) MenuItemsChanged(this); }

        public event Action<object, int> MenuPageChanged;
        public void RaiseMenuPageChanged(int oldValue) { if (MenuPageChanged != null) MenuPageChanged(this, oldValue); }

        public event Action<object, int> SelectionChanged;

        #endregion

        [System.Serializable]
        public class MenuColors {
            public Color BackColor = Color.black;
            public Color HoverColor = Color.gray;
        }

        public float RevealDelay = 0.3f;

        public MenuColors Colors;

        [Header("Segments")]
        public GameObject Cursor;
        public GameObject Centre;
        public GameObject SegmentsMaster;
        public RadialMenu_SegmentAnimator[] Segments;

        public bool Visible { get { return Centre.activeSelf; } }

        [Header("Selection")]
        [Range(0, 1)]
        public float SelectionThreshold;

        protected int _SelectedIndex;
        protected int SelectedIndex {
            get { return _SelectedIndex; }
            set {
                if (_SelectedIndex == value) return;
                if (_SelectedIndex >= 0 && _SelectedIndex < Segments.Length) {
                    //Segments[_SelectedIndex].SetSelecting(false);
                    DeselectSegment(_SelectedIndex);
                }

                var old = _SelectedIndex;
                _SelectedIndex = value;

                if (_SelectedIndex >= 0 && _SelectedIndex < Segments.Length) {
                    //Segments[_SelectedIndex].SetSelecting(true);
                    SelectSegment(_SelectedIndex);
                }

                // TODO: Raise Selecting Index Changed

            }
        }

        [Header("Items")]
        public List<RadialMenu_MenuItem> RootItems;

        protected Stack<IRadialMenuContainer> MenuStack = new Stack<IRadialMenuContainer>();
        
        protected RadialMenu_MenuItem[] CurrentItems {
            get {
                return MenuStack.Count == 0 ? RootItems.ToArray() : MenuStack.Peek().Children.ToArray();
            }
        }

        protected int _ItemsPage;
        protected int ItemsPage {
            get {
                return _ItemsPage;
            }
            set {
                if (_ItemsPage == value) return;

            }
        }

        protected int TweenId;



        void Start() {
            Hide();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="forward"></param>
        public void Show(Vector3 position, Vector3 forward) {

            SelectedIndex = -1;
            MenuStack.Clear();

            if (Cursor != null) Cursor.SetActive(true);

            Centre.SetActive(true);
            for (var i = 0; i < Segments.Length; i++) {
                if (Segments[i] == null) continue;
                Segments[i].gameObject.SetActive(true);
            }

            transform.position = position;
            transform.forward = forward;

            
            BuildSegments();
            RevealSegments();
        }


        /// <summary>
        /// 
        /// </summary>
        public void Hide() {
            if (!Visible) return;

            SelectedIndex = -1;
            UnrevealSegments();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public void UpdateCursor(Vector3 position) {
            if (!Visible) return;

            var localPosition = transform.InverseTransformPoint(position);

            BroadcastMessage("RM_UpdateCursor", localPosition, SendMessageOptions.DontRequireReceiver);

            if (Cursor != null) Cursor.transform.localPosition = localPosition;

            TestSegments(localPosition);
        }


        #region " Segment Management "


        /// <summary>
        /// 
        /// </summary>
        void BuildSegments() {

            //var activeSegments = MenuStack.Count > 0 ? MenuStack.Peek().Children : RootItems;
            var ShowPaging = false && CurrentItems.Length > 7;

            var SystemButtons = (ShowPaging ? 3 : 1);

            // Count = 1 (Back) + Item Count + (optional: paging buttons)
            var count = CurrentItems.Length + SystemButtons;
            var layout = SegmentPresets.ContainsKey(count) ? SegmentPresets[count] : SegmentPresets[8];

            var skipped = 0;
            for (int segmentIdx = 0; segmentIdx < layout.Count; segmentIdx++) {
                var state = Segments[segmentIdx].gameObject.activeSelf;

                Segments[segmentIdx].gameObject.SetActive(true);
                Segments[segmentIdx].transform.localRotation = Quaternion.Euler(0, 0, layout[segmentIdx].Rotation);
                Segments[segmentIdx].Contents.transform.rotation = transform.rotation;
                Segments[segmentIdx].SegmentFillAmount = layout[segmentIdx].Size / 360.0f;

                if (layout[segmentIdx].SystemButton == SegmentSystemButton.Back) {
                    Segments[segmentIdx].Item = CreateSystemButton(SegmentSystemButton.Back);
                    skipped++;

                }
                else if (ShowPaging && layout[segmentIdx].SystemButton == SegmentSystemButton.PageNext) {
                    Segments[segmentIdx].Item = CreateSystemButton(SegmentSystemButton.PageNext);
                    skipped++;
                }
                else if (ShowPaging && layout[segmentIdx].SystemButton == SegmentSystemButton.PagePrev) {
                    Segments[segmentIdx].Item = CreateSystemButton(SegmentSystemButton.PagePrev);
                    skipped++;
                }
                else {

                    var itemIdx = segmentIdx - skipped;

                    if (itemIdx < CurrentItems.Length) {
                        Segments[segmentIdx].Item = CurrentItems[itemIdx];
                        //state = true;
                    }
                    else {
                        Segments[segmentIdx].Item = null;
                        //state = false;
                    }
                }

                Segments[segmentIdx].gameObject.SetActive(state);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected void RevealSegments() {

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ignoreBack"></param>
        protected void UnrevealSegments(bool ignoreBack = true) {

            LeanTween.value(SegmentsMaster, (value) => { SegmentsMaster.GetComponent<Image>().fillAmount = value; }, 1, 0, RevealDelay)
            .setOnStart(() => {
                SegmentsMaster.GetComponent<Image>().enabled = true;
                SegmentsMaster.GetComponent<Image>().fillClockwise = false;
                SegmentsMaster.GetComponent<Image>().fillAmount = 1;

                SegmentsMaster.GetComponent<Mask>().enabled = true;
            })
            .setOnComplete(() => {
                if (Cursor != null) Cursor.SetActive(false);

                Centre.SetActive(false);
                for (var i = 0; i < Segments.Length; i++) {
                    if (Segments[i] == null) continue;
                    Segments[i].gameObject.SetActive(false);
                }
            });
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localPosition"></param>
        protected void TestSegments(Vector3 localPosition) {
            if (localPosition.magnitude < SelectionThreshold) {
                if (SelectedIndex >= 0 && SelectedIndex < Segments.Length) {
                    // Confirm Selection
                    ConfirmSegment();
                }
            }
            else {
                var hit = false;
                for (var i = 0; i < Segments.Length; i++) {
                    hit = Segments[i].Contains(localPosition);
                    if (hit) {
                        SelectedIndex = i;
                        break;
                    }
                }

                if (!hit) SelectedIndex = -1;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        protected void SelectSegment(int index) {

            Segments[index].BackgroundColor = Color.green;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        protected void DeselectSegment(int index) {

            Segments[index].BackgroundColor = Color.black;
        }


        /// <summary>
        /// 
        /// </summary>
        protected void ConfirmSegment() {

            if (Segments[SelectedIndex].IsSelectable) {

                Debug.Log("Selected: " + Segments[SelectedIndex].Item.name);

                var actionable = Segments[SelectedIndex].Item as IRadialMenuAction;
                if (actionable != null) actionable.PerformAction();

                var submenu = Segments[SelectedIndex].Item as IRadialMenuContainer;
                if (submenu != null && submenu.HasChildren) { 

                    MenuStack.Push(submenu);
                    BuildSegments();
                }

                SelectedIndex = -1;
            }

            
        }

        #endregion
        #region " System Buttons "

        RadialMenu_MenuItem CreateSystemButton( SegmentSystemButton button) {
            RadialMenu_MenuItem result = null;
            switch (button) {
                case SegmentSystemButton.Back:
                    result = new RadialMenu_MenuItem(_SystemBack);
                    result.name = "Back";
                    break;
                case SegmentSystemButton.PageNext:
                    result = new RadialMenu_MenuItem(_SystemPageNext);
                    result.name = "Next";
                    break;
                case SegmentSystemButton.PagePrev:
                    result = new RadialMenu_MenuItem(_SystemPagePrev);
                    result.name = "Prev";
                    break;

            }
            return result;
        }
        
        void _SystemBack() {
            Debug.Log("RM System Back");
            if ( MenuStack.Count > 0)
                MenuStack.Pop();

            BuildSegments();
        }
        void _SystemPagePrev() { Debug.Log("RM System PagePrev"); }
        void _SystemPageNext() { Debug.Log("RM System PageNext"); }

        #endregion
    }
}