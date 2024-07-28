#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Utility_Separator
    {
        #region Menu Items
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separator + "/Create All Separators", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateAllSeparators()
        {
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> separator in HierarchyDesigner_Configurable_Separator.GetAllSeparatorsData(false))
            {
                CreateSeparator(separator.Key);
            }
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separator + "/Create Default Separator", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateDefaultSeparator()
        {
            CreateSeparator("Separator");
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separator + "/Create Missing Separators", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateMissingSeparators()
        {
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> separator in HierarchyDesigner_Configurable_Separator.GetAllSeparatorsData(false))
            {
                if (!SeparatorExists(separator.Key))
                {
                    CreateSeparator(separator.Key);
                }
            }
        }
        #endregion

        #region Context menu
        [MenuItem(HierarchyDesigner_Shared_MenuItems.ContextMenu_Separators + "/Create All Separators", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void ContextMenu_Separator_CreateAllSeparators() => CreateAllSeparators();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.ContextMenu_Separators + "/Create Default Separator", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void ContextMenu_Separator_CreateDefaultSeparator() => CreateDefaultSeparator();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.ContextMenu_Separators + "/Create Missing Separators", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void ContextMenu_Separator_CreateMissingSeparators() => CreateMissingSeparators();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.ContextMenu_Separators + "/Transform GameObject into a Separator", false, HierarchyDesigner_Shared_MenuItems.LayerOne)]
        public static void ContextMenu_Separator_TransformGameObjectIntoASeparator() => TransformGameObjectIntoASeparator();
        #endregion

        #region Methods
        private static void CreateSeparator(string separatorName)
        {
            GameObject separator = new GameObject($"//{separatorName}");
            separator.tag = "EditorOnly";
            SetSeparatorState(separator, false);
            separator.SetActive(false);
            EditorGUIUtility.SetIconForObject(separator, HierarchyDesigner_Shared_Resources.SeparatorInspectorIcon);
            Undo.RegisterCreatedObjectUndo(separator, $"Create {separatorName}");
        }

        public static void SetSeparatorState(GameObject gameObject, bool editable)
        {
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component) 
                {
                    component.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable; 
                }
            }
            gameObject.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable;
            gameObject.transform.hideFlags = HideFlags.HideInInspector;
            EditorUtility.SetDirty(gameObject);
        }

        private static bool SeparatorExists(string separatorName)
        {
            string fullSeparatorName = "//" + separatorName;
            #if UNITY_6000_0_OR_NEWER
            Transform[] allTransforms = GameObject.FindObjectsByType<Transform>(FindObjectsSortMode.None);
            #else
            Transform[] allTransforms = Object.FindObjectsOfType<Transform>(true);
            #endif
            foreach (Transform t in allTransforms)
            {
                if (t.gameObject.CompareTag("EditorOnly") && t.gameObject.name.Equals(fullSeparatorName))
                {
                    return true;
                }
            }
            return false;
        }

        private static void TransformGameObjectIntoASeparator()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogWarning("No GameObject selected.");
                return;
            }
            if (selectedObject.GetComponents<Component>().Length > 1)
            {
                Debug.LogWarning("Separators cannot have components because separators are marked as editorOnly, meaning they will not be present in your game's build.");
                return;
            }

            string separatorName = HierarchyDesigner_Configurable_Separator.StripPrefix(selectedObject.name);
            HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData = HierarchyDesigner_Configurable_Separator.GetSeparatorData(separatorName);
            if (separatorData == null)
            {
                HierarchyDesigner_Configurable_Separator.SetSeparatorData(
                    separatorName,
                    Color.white,
                    false,
                    Color.gray,
                    new Gradient(),
                    12,
                    FontStyle.Normal,
                    TextAnchor.MiddleCenter,
                    HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default
                );
                if (!selectedObject.name.StartsWith("//"))
                {
                    selectedObject.name = $"//{selectedObject.name}";
                }
                selectedObject.tag = "EditorOnly";
                selectedObject.SetActive(false);
                EditorGUIUtility.SetIconForObject(selectedObject, HierarchyDesigner_Shared_Resources.SeparatorInspectorIcon);
                Debug.Log($"GameObject <color=#73FF7A>'{separatorName}'</color> was transformed into a Separator and added to the Separators dictionary.");
            }
            else
            {
                Debug.LogWarning($"GameObject <color=#FF7674>'{separatorName}'</color> already exists in the Separators dictionary.");
                return;
            }
            SetSeparatorState(selectedObject, false);
        }
        #endregion
    }
}
#endif