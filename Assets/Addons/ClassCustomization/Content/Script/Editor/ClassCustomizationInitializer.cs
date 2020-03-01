using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using MFPSEditor;

public class ClassCustomizationInitializer : MonoBehaviour
{

    private const string DEFINE_KEY = "CLASS_CUSTOMIZER";

    static ClassCustomizationInitializer()
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


    [MenuItem("MFPS/Addons/ClassCustomizer/Enable")]
    private static void Enable()
    {
        EditorUtils.SetEnabled(DEFINE_KEY, true);
    }


    [MenuItem("MFPS/Addons/ClassCustomizer/Enable", true)]
    private static bool EnableValidate()
    {
        return !EditorUtils.CompilerIsDefine(DEFINE_KEY);
    }


    [MenuItem("MFPS/Addons/ClassCustomizer/Disable")]
    private static void Disable()
    {
        EditorUtils.SetEnabled(DEFINE_KEY, false);
    }


    [MenuItem("MFPS/Addons/ClassCustomizer/Disable", true)]
    private static bool DisableValidate()
    {
        return EditorUtils.CompilerIsDefine(DEFINE_KEY);
    }

    [MenuItem("MFPS/Addons/ClassCustomizer/Integrate")]
    private static void Instegrate()
    {
        if (AssetDatabase.IsValidFolder("Assets/MFPS/Scenes"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            string path = "Assets/MFPS/Scenes/MainMenu.unity";
            EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            bl_Lobby lb = FindObjectOfType<bl_Lobby>();
            if (lb != null)
            {
                GameObject ccb = lb.AddonsButtons[0];
                if (ccb != null) { ccb.SetActive(true); }
                EditorUtility.SetDirty(lb);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                Debug.Log("Class Customization integrate!");
            }
            else
            {
                Debug.Log("Can't found Menu scene.");
            }
        }
        else
        {
            Debug.LogWarning("Can't integrate the addons because MFPS folder structure has been change, please do the manual integration.");
        }
    }
}