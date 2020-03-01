using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;
using MFPSEditor;

public class bl_LocalizationAddonInitializer 
{

    private const string DEFINE_KEY = "LOCALIZATION";

    static bl_LocalizationAddonInitializer()
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


    [MenuItem("MFPS/Addons/Localization/Enable")]
    private static void Enable()
    {
        EditorUtils.SetEnabled(DEFINE_KEY, true);
    }


    [MenuItem("MFPS/Addons/Localization/Enable", true)]
    private static bool EnableValidate()
    {
        return !EditorUtils.CompilerIsDefine(DEFINE_KEY);
    }


    [MenuItem("MFPS/Addons/Localization/Disable")]
    private static void Disable()
    {
        EditorUtils.SetEnabled(DEFINE_KEY, false);
    }


    [MenuItem("MFPS/Addons/Localization/Disable", true)]
    private static bool DisableValidate()
    {
        return EditorUtils.CompilerIsDefine(DEFINE_KEY);
    }

    [MenuItem("MFPS/Addons/Localization/Integrate")]
    private static void Instegrate()
    {
        if (AssetDatabase.IsValidFolder("Assets/MFPS/Scenes"))
        {
            GameObject selector = AssetDatabase.LoadAssetAtPath("Assets/Addons/Localization/Content/Prefabs/UI/LanguageSelector.prefab", typeof(GameObject)) as GameObject;
            if (selector != null)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                string path = "Assets/MFPS/Scenes/MainMenu.unity";
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                bl_Lobby lb = GameObject.FindObjectOfType<bl_Lobby>();
                if (lb != null)
                {
                    GameObject ccb = lb.AddonsButtons[6];
                    if (ccb != null)
                    {
                        GameObject inst = PrefabUtility.InstantiatePrefab(selector, EditorSceneManager.GetActiveScene()) as GameObject;
                        inst.transform.SetParent(ccb.transform, false);
                        EditorUtility.SetDirty(inst);
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        Debug.Log("Localization integrate!");
                    }
                }
                else
                {
                    Debug.Log("Can't found Menu scene.");
                }
            }
            else { Debug.Log("Can't found selector prefab."); }
        }
    }
}