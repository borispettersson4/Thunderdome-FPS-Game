using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MFPSEditor;

public class CustomizerInitializer : MonoBehaviour
{
    private const string DEFINE_KEY = "CUSTOMIZER";

    static CustomizerInitializer()
    {
        int start = PlayerPrefs.GetInt("mfps.addons.define." + DEFINE_KEY, 0);
        if (start == 0)
        {
            bool defines = EditorUtils.CompilerIsDefine(DEFINE_KEY);
            if (!defines)
            {
                EditorUtils.SetEnabled(DEFINE_KEY, true);
            }
            PlayerPrefs.SetInt("mfps.addons.define." + DEFINE_KEY, 1);
        }
    }


    [MenuItem("MFPS/Addons/Customizer/Enable")]
    private static void Enable()
    {
        EditorUtils.SetEnabled(DEFINE_KEY, true);
    }


    [MenuItem("MFPS/Addons/Customizer/Enable", true)]
    private static bool EnableValidate()
    {
        return !EditorUtils.CompilerIsDefine(DEFINE_KEY);
    }


    [MenuItem("MFPS/Addons/Customizer/Disable")]
    private static void Disable()
    {
        EditorUtils.SetEnabled(DEFINE_KEY, false);
    }


    [MenuItem("MFPS/Addons/Customizer/Disable", true)]
    private static bool DisableValidate()
    {
        return EditorUtils.CompilerIsDefine(DEFINE_KEY);
    }
}