using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(bl_CustomizerWeapon))]
public class bl_CustomizerWeaponEditor : Editor
{

    bl_CustomizerWeapon script;
    SerializedProperty attac;
    private string weaponName = "";
    SerializedProperty camor;

    private void OnEnable()
    {
        script = (bl_CustomizerWeapon)target;
        attac = serializedObject.FindProperty("Attachments");
        camor = serializedObject.FindProperty("CamoRender");
        weaponName = script.WeaponName;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("box");
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal("box");
        script.WeaponID = EditorGUILayout.Popup("Customizer ID", script.WeaponID, bl_CustomizerData.Instance.GetWeaponStringArray(), EditorStyles.toolbarDropDown);
        GUILayout.Space(5);
        if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(100)))
        {
            script.RefreshAttachments();
        }
        GUILayout.EndHorizontal();
        script.isFPWeapon = EditorGUILayout.ToggleLeft("is First Person Weapon", script.isFPWeapon, EditorStyles.toolbarButton);
        script.ApplyOnStart = EditorGUILayout.ToggleLeft("Apply On Start", script.ApplyOnStart, EditorStyles.toolbarButton);
        if (GUI.changed)
        {
            script.WeaponName = bl_CustomizerData.Instance.Weapons[script.WeaponID].WeaponName;
            if (script.WeaponName != weaponName)
            {
                script.BuildAttachments();
                weaponName = script.WeaponName;
            }
        }

        serializedObject.Update();
        GUILayout.BeginHorizontal("box");
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(camor, true);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal("box");
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(attac, true);
        GUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndVertical();
        EditorGUI.EndChangeCheck();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}