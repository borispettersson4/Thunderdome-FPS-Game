using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditorInternal;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(bl_LocalizationUI))]
public class bl_LocalizationUIEditor : Editor
{

    private bl_LocalizationUI script;
    private string[] StringIds;
    private bool addTextOpen = false;
    private string idToAdd = "";
    private string textToAdd = "";
    private string[] StringCases = new string[] { "AS IS", "UPPERCASE", "LOWERCASE", "CAPITAL", };
    private ReorderableList OptionsList;
    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        script = (bl_LocalizationUI)target;
        StringIds = bl_Localization.Instance.GetIdsList().ToArray();
        OptionsList = new ReorderableList(serializedObject, serializedObject.FindProperty("StringIDs"), true, true, true, true);
        OptionsList.drawElementCallback += DrawElement();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.BeginVertical("box");
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();
        script.m_UIType = (bl_LocalizationUI.UIType)EditorGUILayout.EnumPopup("UI Type", script.m_UIType, EditorStyles.toolbarDropDown);
        GUILayout.EndHorizontal();
        if (script.m_UIType == bl_LocalizationUI.UIType.Text)
        {
            GUILayout.Space(4);
            GUILayout.BeginHorizontal();
            if (script.ManuallyAssignId)
            {
                script.StringID = EditorGUILayout.TextField("Text ID", script.StringID);
                if (GUILayout.Button("List", EditorStyles.toolbarButton, GUILayout.Width(75)))
                {
                    script.ManuallyAssignId = false;
                }
            }
            else
            {
                if (StringIds.Length > 0)
                {
                    script._arrayID = EditorGUILayout.Popup("KEY", script._arrayID, StringIds, EditorStyles.toolbarDropDown);
                    script.StringID = StringIds[script._arrayID];
                    GUILayout.Space(2);
                    if (GUILayout.Button("Manual", EditorStyles.toolbarButton, GUILayout.Width(75)))
                    {
                        script.ManuallyAssignId = true;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        if (script.m_UIType == bl_LocalizationUI.UIType.Text)
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            script.Plural = EditorGUILayout.ToggleLeft("Plural", script.Plural, EditorStyles.toolbarButton);
            script.StringCase = EditorGUILayout.Popup(script.StringCase, StringCases, EditorStyles.toolbarDropDown);
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            script.Extra = EditorGUILayout.ToggleLeft("Extra String", script.Extra, EditorStyles.toolbarButton);
            GUI.enabled = script.Extra;
            script.ExtraString = EditorGUILayout.TextField(script.ExtraString);
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal("box");

            if (!addTextOpen)
            {
                if (GUILayout.Button("Add New Text", EditorStyles.toolbarButton))
                {
                    addTextOpen = true;
                    if (string.IsNullOrEmpty(textToAdd) && script.GetComponent<Text>() != null)
                    {
                        textToAdd = script.GetComponent<Text>().text;
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Cancel", EditorStyles.toolbarButton))
                {
                    addTextOpen = false;
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Open Editor", EditorStyles.toolbarButton, GUILayout.Width(110)))
            {
                EditorGUIUtility.PingObject(bl_Localization.Instance);
                Selection.activeObject = bl_Localization.Instance;
            }
            GUILayout.EndHorizontal();

            if (addTextOpen)
            {
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Key", GUILayout.Width(110));
                EditorGUILayout.LabelField("Default Text");
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                idToAdd = EditorGUILayout.TextField(idToAdd, GUILayout.Width(110));
                textToAdd = EditorGUILayout.TextField(textToAdd);
                GUILayout.EndHorizontal();
                GUI.enabled = (!string.IsNullOrEmpty(idToAdd) && !string.IsNullOrEmpty(textToAdd));
                if (GUILayout.Button("ADD TEXT", EditorStyles.toolbarButton))
                {
                    if (bl_Localization.Instance.AddText(idToAdd, textToAdd))
                    {
                        addTextOpen = false;
                        StringIds = bl_Localization.Instance.GetIdsList().ToArray();
                        script._arrayID = bl_Localization.Instance.DefaultLanguage.Text.Data.Length - 1;
                        script.StringID = idToAdd;
                        idToAdd = string.Empty;
                        textToAdd = string.Empty;
                    }
                }
                GUI.enabled = true;
                GUILayout.EndVertical();
            }
        }
        else if (script.m_UIType == bl_LocalizationUI.UIType.DropDown)
        {
            OptionsList.DoLayoutList();
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            script.Plural = EditorGUILayout.ToggleLeft("Plural", script.Plural, EditorStyles.toolbarButton);
            script.StringCase = EditorGUILayout.Popup(script.StringCase, StringCases, EditorStyles.toolbarDropDown);
            GUILayout.EndHorizontal();
            GUILayout.Space(2);
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
        GUILayout.EndVertical();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
            EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
        }
    }

    private ReorderableList.ElementCallbackDelegate DrawElement()
    {
        return (rect, index, isActive, isFocused) =>
        {
            rect.height = EditorGUIUtility.singleLineHeight;
            var property = OptionsList.serializedProperty.GetArrayElementAtIndex(index);
            Rect r = rect;
            r.width = 30;
            GUI.Label(r, "KEY");
            r = rect;
            r.x += 33;
            r.width -= 40;
            property.stringValue = EditorGUI.TextField(r, property.stringValue);
        };
    }
}