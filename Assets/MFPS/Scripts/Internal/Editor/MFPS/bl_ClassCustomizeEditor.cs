using MFPS.ClassCustomization;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(bl_ClassCustomize))]
public class bl_ClassCustomizeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        bl_ClassCustomize myScript = (bl_ClassCustomize)target;
        if (GUILayout.Button("Copy All Classes From First"))
        {
            myScript.copyAllClassesFromFirst();
        }
    }
}