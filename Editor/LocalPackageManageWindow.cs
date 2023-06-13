using UnityEditor;
using UnityEngine;

namespace LocalPackageManager
{
    /// <summary>
    /// Local Package Manager Window.
    /// WIP
    /// </summary>
    internal sealed class LocalPackageManageWindow : EditorWindow
    {
        private Rect _dropDownButtonRect;
        private Rect _popupButtonRect;


        // TODO: ウィンドウの中身作る
        private void OnGUI()
        {
            GUILayout.Label("Example Editor Window");

            if (GUILayout.Button("Close"))
                Close();

            // DropDown
            if (GUILayout.Button("Show As Dropdown"))
            {
                var popupPosition = GUIUtility.GUIToScreenPoint(_dropDownButtonRect.center);
                var window = CreateInstance<LocalPackageManageWindow>();
                var windowSize = new Vector2(200, 100);
                window.ShowAsDropDown(new Rect(popupPosition, Vector2.zero), windowSize);
            }

            if (Event.current.type == EventType.Repaint) _dropDownButtonRect = GUILayoutUtility.GetLastRect();

            // Popup
            if (GUILayout.Button("Show Popup"))
            {
                // Stylingだけ
                var popupPosition = GUIUtility.GUIToScreenPoint(_popupButtonRect.center);
                var window = CreateInstance<LocalPackageManageWindow>();
                window.position = new Rect(popupPosition, new Vector2(200, 100));
                window.ShowPopup();
            }

            if (Event.current.type == EventType.Repaint) _popupButtonRect = GUILayoutUtility.GetLastRect();
        }

        /// <summary>
        /// Open the window.
        /// </summary>
        public static void Open(){
            var window = CreateInstance<LocalPackageManageWindow>();
            window.Show();
        }

        /// <summary>
        /// Open the window as modal.
        /// </summary>
        public static void OpenModal()
        {
            var window = CreateInstance<LocalPackageManageWindow>();
            window.ShowModal();
        }
    }
}