﻿#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_Separator : EditorWindow
    {
        #region Properties
        #region GUI
        private Vector2 outerScroll;
        private Vector2 innerScroll;
        private GUIStyle headerGUIStyle;
        private GUIStyle contentGUIStyle;
        private GUIStyle messageGUIStyle;
        private GUIStyle outerBackgroundGUIStyle;
        private GUIStyle innerBackgroundGUIStyle;
        private GUIStyle contentBackgroundGUIStyle;
        #endregion
        #region Const
        private readonly int[] fontSizeOptions = new int[15];
        private const float moveSeparatorButtonWidth = 25;
        private const float separatorCreationLabelWidth = 140;
        private const float extraSeparatorLabelWidthOffset = 20;
        #endregion
        #region Temporary Values
        private Dictionary<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> tempSeparators;
        private List<string> separatorsOrder;
        private static bool hasModifiedChanges = false;
        #endregion
        #region Separator Creation Values
        private string newSeparatorName = "";
        private Color newTextColor = Color.white;
        private bool newIsGradient = false;
        private Color newBackgroundColor = Color.gray;
        private Gradient newBackgroundGradient = new Gradient();
        private int newFontSize = 12;
        private FontStyle newFontStyle = FontStyle.Normal;
        private TextAnchor newTextAnchor = TextAnchor.MiddleCenter;
        private HierarchyDesigner_Configurable_Separator.SeparatorImageType newSeparatorImageType = HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default;
        #endregion
        #region Separator Global Fields Values
        private Color tempGlobalTextColor = Color.white;
        private bool tempGlobalIsGradient = false;
        private Color tempGlobalBackgroundColor = Color.gray;
        private Gradient tempGlobalBackgroundGradient = new Gradient();
        private int tempGlobalFontSize = 12;
        private FontStyle tempGlobalFontStyle = FontStyle.Normal;
        private TextAnchor tempGlobalTextAnchor = TextAnchor.MiddleCenter;
        private HierarchyDesigner_Configurable_Separator.SeparatorImageType tempGlobalSeparatorImageType = HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default;
        #endregion
        #endregion

        #region Window
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separator + "/Separator Manager Window", false, HierarchyDesigner_Shared_MenuItems.LayerTwo)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_Separator window = GetWindow<HierarchyDesigner_Window_Separator>("Hierarchy Separator Manager");
            window.minSize = new Vector2(400, 200);
        }
        #endregion

        #region Initialization
        private void OnEnable()
        {
            InitFontSizeOptions();
            LoadTempValues();
        }

        private void InitFontSizeOptions()
        {
            for (int i = 0; i < fontSizeOptions.Length; i++)
            {
                fontSizeOptions[i] = 7 + i;
            }
        }

        private void LoadTempValues()
        {
            tempSeparators = HierarchyDesigner_Configurable_Separator.GetAllSeparatorsData(true);
            separatorsOrder = new List<string>(tempSeparators.Keys);
        }
        #endregion

        #region Main
        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out messageGUIStyle, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("Separators Manager", headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);

            #region Main
            #region Separator Creation
            #region Fields
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Separator Creation:", contentGUIStyle);
            GUILayout.Space(4);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", GUILayout.Width(separatorCreationLabelWidth));
            newSeparatorName = EditorGUILayout.TextField(newSeparatorName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Text Color", GUILayout.Width(separatorCreationLabelWidth));
            newTextColor = EditorGUILayout.ColorField(newTextColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Is Gradient Background", GUILayout.Width(separatorCreationLabelWidth));
            newIsGradient = EditorGUILayout.Toggle(newIsGradient);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (newIsGradient)
            {
                EditorGUILayout.LabelField("Background Gradient", GUILayout.Width(separatorCreationLabelWidth));
                newBackgroundGradient = EditorGUILayout.GradientField(newBackgroundGradient);
            }
            else
            {
                EditorGUILayout.LabelField("Background Color", GUILayout.Width(separatorCreationLabelWidth));
                newBackgroundColor = EditorGUILayout.ColorField(newBackgroundColor);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            string[] newFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int newFontSizeIndex = Array.IndexOf(fontSizeOptions, newFontSize);
            EditorGUILayout.LabelField("Font Size", GUILayout.Width(separatorCreationLabelWidth));
            newFontSize = fontSizeOptions[EditorGUILayout.Popup(newFontSizeIndex, newFontSizeOptionsStrings)];
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Font Style", GUILayout.Width(separatorCreationLabelWidth));
            newFontStyle = (FontStyle)EditorGUILayout.EnumPopup(newFontStyle);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Text Anchor", GUILayout.Width(separatorCreationLabelWidth));
            newTextAnchor = (TextAnchor)EditorGUILayout.EnumPopup(newTextAnchor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Background Type", GUILayout.Width(separatorCreationLabelWidth));
            if (GUILayout.Button(HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(newSeparatorImageType), EditorStyles.popup))
            {
                ShowSeparatorImageTypePopup();
            }
            EditorGUILayout.EndHorizontal();
            #endregion
            #region Button
            GUILayout.Space(4);
            if (GUILayout.Button("Create Separator", GUILayout.Height(25)))
            {
                if (IsSeparatorNameValid(newSeparatorName))
                {
                    CreateSeparator(newSeparatorName, newTextColor, newIsGradient, newBackgroundColor, newBackgroundGradient, newFontSize, newFontStyle, newTextAnchor, newSeparatorImageType);
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Separator Name", "Separator name is either duplicate or invalid.", "OK");
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion
            #endregion

            #region Separators's Global Fields and List
            if (tempSeparators.Count > 0)
            {
                #region Global Fields
                EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
                EditorGUILayout.LabelField("Separators' Global Fields", contentGUIStyle);
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                tempGlobalTextColor = EditorGUILayout.ColorField(tempGlobalTextColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorTextColor(tempGlobalTextColor); }
                EditorGUI.BeginChangeCheck();
                GUILayout.Space(2);
                tempGlobalIsGradient = EditorGUILayout.Toggle(tempGlobalIsGradient, GUILayout.Width(18));
                if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorIsGradientBackground(tempGlobalIsGradient); }
                EditorGUI.BeginChangeCheck();
                tempGlobalBackgroundColor = EditorGUILayout.ColorField(tempGlobalBackgroundColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorBackgroundColor(tempGlobalBackgroundColor); }
                EditorGUI.BeginChangeCheck();
                tempGlobalBackgroundGradient = EditorGUILayout.GradientField(tempGlobalBackgroundGradient, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorGradientBackground(tempGlobalBackgroundGradient); }
                EditorGUI.BeginChangeCheck();
                string[] tempFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
                int tempFontSizeIndex = Array.IndexOf(fontSizeOptions, tempGlobalFontSize);
                tempGlobalFontSize = fontSizeOptions[EditorGUILayout.Popup(tempFontSizeIndex, tempFontSizeOptionsStrings, GUILayout.Width(50))];
                if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorFontSize(tempGlobalFontSize); }
                EditorGUI.BeginChangeCheck();
                tempGlobalFontStyle = (FontStyle)EditorGUILayout.EnumPopup(tempGlobalFontStyle, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorFontStyle(tempGlobalFontStyle); }
                EditorGUI.BeginChangeCheck();
                tempGlobalTextAnchor = (TextAnchor)EditorGUILayout.EnumPopup(tempGlobalTextAnchor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorTextAnchor(tempGlobalTextAnchor); }
                if (GUILayout.Button(HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(tempGlobalSeparatorImageType), EditorStyles.popup, GUILayout.MinWidth(150), GUILayout.ExpandWidth(true))) { ShowSeparatorImageTypePopupGlobal(); }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                GUILayout.Space(4);
                #endregion

                #region Separator List
                EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
                EditorGUILayout.LabelField("Separators' List", contentGUIStyle);
                innerScroll = EditorGUILayout.BeginScrollView(innerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                GUILayout.Space(10);
                int index = 1;
                for (int i = 0; i < separatorsOrder.Count; i++)
                {
                    string key = separatorsOrder[i];
                    DrawSeparators(index, key, tempSeparators[key], i, separatorsOrder.Count);
                    index++;
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                GUILayout.Space(4);
                #endregion
            }
            else
            {
                EditorGUILayout.LabelField("No separators found. Please create a new separator.", messageGUIStyle);
            }
            EditorGUILayout.EndVertical();
            #endregion
            #endregion

            #region Footer
            if (GUILayout.Button("Update and Save Settings", GUILayout.Height(30)))
            {
                SaveSettings();
            }
            #endregion

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void OnDestroy()
        {
            if (hasModifiedChanges)
            {
                bool shouldSave = EditorUtility.DisplayDialog("Separator(s) Have Been Modified!",
                    "Do you want to save the changes you made to the separators?",
                    "Save", "Don't Save");

                if (shouldSave)
                {
                    SaveSettings();
                }
            }
            hasModifiedChanges = false;
        }

        private void SaveSettings()
        {
            HierarchyDesigner_Configurable_Separator.ApplyChangesToSeparators(tempSeparators, separatorsOrder);
            HierarchyDesigner_Configurable_Separator.SaveSettings();
            HierarchyDesigner_Manager_GameObject.ClearSeparatorCache();
            hasModifiedChanges = false;
        }
        #endregion

        #region GUI
        #region Folder Image Type
        private void ShowSeparatorImageTypePopup()
        {
            GenericMenu menu = new GenericMenu();
            Dictionary<string, List<string>> groupedTypes = HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypesGrouped();
            foreach (KeyValuePair<string, List<string>> group in groupedTypes)
            {
                foreach (string typeName in group.Value)
                {
                    menu.AddItem(new GUIContent($"{group.Key}/{typeName}"), typeName == HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(newSeparatorImageType), OnSeparatorImageTypeSelected, typeName);
                }
            }
            menu.ShowAsContext();
        }

        private void ShowSeparatorImageTypePopupGlobal()
        {
            GenericMenu menu = new GenericMenu();
            Dictionary<string, List<string>> groupedTypes = HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypesGrouped();
            foreach (KeyValuePair<string, List<string>> group in groupedTypes)
            {
                foreach (string typeName in group.Value)
                {
                    menu.AddItem(new GUIContent($"{group.Key}/{typeName}"), typeName == HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(tempGlobalSeparatorImageType), OnSeparatorImageTypeGlobalSelected, typeName);
                }
            }
            menu.ShowAsContext();
        }

        private void ShowSeparatorImageTypePopupForSeparator(HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData)
        {
            GenericMenu menu = new GenericMenu();
            Dictionary<string, List<string>> groupedTypes = HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypesGrouped();
            foreach (KeyValuePair<string, List<string>> group in groupedTypes)
            {
                foreach (string typeName in group.Value)
                {
                    menu.AddItem(new GUIContent($"{group.Key}/{typeName}"), typeName == HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(separatorData.ImageType), OnSeparatorImageTypeForSeparatorSelected, new KeyValuePair<HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData, string>(separatorData, typeName));
                }
            }
            menu.ShowAsContext();
        }

        private void OnSeparatorImageTypeSelected(object imageTypeObj)
        {
            string typeName = (string)imageTypeObj;
            newSeparatorImageType = HierarchyDesigner_Configurable_Separator.ParseSeparatorImageType(typeName);
        }

        private void OnSeparatorImageTypeGlobalSelected(object imageTypeObj)
        {
            string typeName = (string)imageTypeObj;
            tempGlobalSeparatorImageType = HierarchyDesigner_Configurable_Separator.ParseSeparatorImageType(typeName);
            UpdateGlobalSeparatorImageType(tempGlobalSeparatorImageType);
        }

        private void OnSeparatorImageTypeForSeparatorSelected(object separatorDataAndTypeObj)
        {
            KeyValuePair<HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData, string> separatorDataAndType = (KeyValuePair<HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData, string>)separatorDataAndTypeObj;
            separatorDataAndType.Key.ImageType = HierarchyDesigner_Configurable_Separator.ParseSeparatorImageType(separatorDataAndType.Value);
        }
        #endregion
        #endregion

        #region Operations
        private bool IsSeparatorNameValid(string separatorName)
        {
            return !string.IsNullOrEmpty(separatorName) && !tempSeparators.TryGetValue(separatorName, out _);
        }

        private void CreateSeparator(string separatorName, Color textColor, bool isGradient, Color backgroundColor, Gradient backgroundGradient, int fontSize, FontStyle fontStyle, TextAnchor textAnchor, HierarchyDesigner_Configurable_Separator.SeparatorImageType imageType)
        {
            HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData newSeparatorData = new HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData
            {
                Name = separatorName,
                TextColor = textColor,
                IsGradientBackground = isGradient,
                BackgroundColor = backgroundColor,
                BackgroundGradient = backgroundGradient,
                FontSize = fontSize,
                FontStyle = fontStyle,
                TextAnchor = textAnchor,
                ImageType = imageType,

            };
            tempSeparators[separatorName] = newSeparatorData;
            separatorsOrder.Add(separatorName);
            newSeparatorName = "";
            newTextColor = Color.white;
            newIsGradient = false;
            newBackgroundColor = Color.gray;
            newBackgroundGradient = new Gradient();
            newFontSize = 12;
            newFontStyle = FontStyle.Normal;
            newTextAnchor = TextAnchor.MiddleCenter;
            newSeparatorImageType = HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default;
            hasModifiedChanges = true;
            GUI.FocusControl(null);
        }

        private void DrawSeparators(int index, string key, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData, int position, int totalItems)
        {
            float separatorLabelWidth = HierarchyDesigner_Shared_GUI.CalculateMaxLabelWidth(tempSeparators.Keys);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField($"{index}) {separatorData.Name}", GUILayout.Width(separatorLabelWidth + extraSeparatorLabelWidthOffset));
            EditorGUI.BeginChangeCheck();
            separatorData.TextColor = EditorGUILayout.ColorField(separatorData.TextColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            GUILayout.Space(2);
            separatorData.IsGradientBackground = EditorGUILayout.Toggle(separatorData.IsGradientBackground, GUILayout.Width(18));
            if (separatorData.IsGradientBackground) { separatorData.BackgroundGradient = EditorGUILayout.GradientField(separatorData.BackgroundGradient, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true)); }
            else { separatorData.BackgroundColor = EditorGUILayout.ColorField(separatorData.BackgroundColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true)); }
            string[] fontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int fontSizeIndex = Array.IndexOf(fontSizeOptions, separatorData.FontSize);
            if (fontSizeIndex == -1) { fontSizeIndex = 5; }
            separatorData.FontSize = fontSizeOptions[EditorGUILayout.Popup(fontSizeIndex, fontSizeOptionsStrings, GUILayout.Width(50))];
            separatorData.FontStyle = (FontStyle)EditorGUILayout.EnumPopup(separatorData.FontStyle, GUILayout.Width(100));
            separatorData.TextAnchor = (TextAnchor)EditorGUILayout.EnumPopup(separatorData.TextAnchor, GUILayout.Width(125));
            if (GUILayout.Button(HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(separatorData.ImageType), EditorStyles.popup, GUILayout.Width(150))) { ShowSeparatorImageTypePopupForSeparator(separatorData); }
            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }

            if (GUILayout.Button("↑", GUILayout.Width(moveSeparatorButtonWidth)) && position > 0)
            {
                MoveSeparator(position, position - 1);
            }
            if (GUILayout.Button("↓", GUILayout.Width(moveSeparatorButtonWidth)) && position < totalItems - 1)
            {
                MoveSeparator(position, position + 1);
            }
            if (GUILayout.Button("Create", GUILayout.Width(60)))
            {
                CreateSeparatorGameObject(separatorData);
            }
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                RemoveSeparator(key);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void MoveSeparator(int indexA, int indexB)
        {
            string keyA = separatorsOrder[indexA];
            string keyB = separatorsOrder[indexB];

            separatorsOrder[indexA] = keyB;
            separatorsOrder[indexB] = keyA;
            hasModifiedChanges = true;
        }

        private void CreateSeparatorGameObject(HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData)
        {
            GameObject separator = new GameObject($"//{separatorData.Name}");
            separator.tag = "EditorOnly";
            HierarchyDesigner_Utility_Separator.SetSeparatorState(separator, false);
            separator.SetActive(false);
            Undo.RegisterCreatedObjectUndo(separator, $"Create {separatorData.Name}");

            Texture2D inspectorIcon = HierarchyDesigner_Shared_Resources.SeparatorInspectorIcon;
            if (inspectorIcon != null)
            {
                EditorGUIUtility.SetIconForObject(separator, inspectorIcon);
            }
        }

        private void RemoveSeparator(string separatorName)
        {
            if (tempSeparators.TryGetValue(separatorName, out _))
            {
                tempSeparators.Remove(separatorName);
                separatorsOrder.Remove(separatorName);
                hasModifiedChanges = true;
                GUIUtility.ExitGUI();
            }
        }

        #region Global Fields Methods
        private void UpdateGlobalSeparatorTextColor(Color color)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.TextColor = color;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorIsGradientBackground(bool isGradientBackground)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.IsGradientBackground = isGradientBackground;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorBackgroundColor(Color color)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.BackgroundColor = color;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorGradientBackground(Gradient gradientBackground)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.BackgroundGradient = gradientBackground;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorFontSize(int size)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.FontSize = size;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorFontStyle(FontStyle style)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.FontStyle = style;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorTextAnchor(TextAnchor anchor)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.TextAnchor = anchor;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorImageType(HierarchyDesigner_Configurable_Separator.SeparatorImageType imageType)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.ImageType = imageType;
            }
            hasModifiedChanges = true;
        }
        #endregion
        #endregion
    }
}
#endif