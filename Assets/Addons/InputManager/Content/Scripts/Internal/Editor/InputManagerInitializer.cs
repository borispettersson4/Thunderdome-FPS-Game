using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using MFPSEditor;

public class InputManagerInitializer : MonoBehaviour
{

    private const string DEFINE_KEY = "INPUT_MANAGER";

    static InputManagerInitializer()
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

    [MenuItem("MFPS/Addons/InputManager/Enable")]
    private static void Enable()
    {
        EditorUtils.SetEnabled(DEFINE_KEY, true);
    }


    [MenuItem("MFPS/Addons/InputManager/Enable", true)]
    private static bool EnableValidate()
    {
        return !EditorUtils.CompilerIsDefine(DEFINE_KEY);
    }


    [MenuItem("MFPS/Addons/InputManager/Disable")]
    private static void Disable()
    {
        EditorUtils.SetEnabled(DEFINE_KEY, false);
    }


    [MenuItem("MFPS/Addons/InputManager/Disable", true)]
    private static bool DisableValidate()
    {
        return EditorUtils.CompilerIsDefine(DEFINE_KEY);
    }

    [MenuItem("MFPS/Addons/InputManager/Integrate")]
    private static void Instegrate()
    {
        GameObject inp = AssetDatabase.LoadAssetAtPath("Assets/Addons/InputManager/Content/Prefabs/InputManager.prefab", typeof(GameObject)) as GameObject;
        GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Addons/InputManager/Content/Prefabs/KeyOptionsIntegrate.prefab", typeof(GameObject)) as GameObject;
        if (prefab != null)
        {
            if (SceneManager.sceneCountInBuildSettings > 0)
            {
                EditorSceneManager.OpenScene("Assets/MFPS/Scenes/MainMenu.unity", OpenSceneMode.Single);
                GameObject inputWindow = PrefabUtility.InstantiatePrefab(inp, EditorSceneManager.GetActiveScene()) as GameObject;
                GameObject panel = PrefabUtility.InstantiatePrefab(prefab, EditorSceneManager.GetActiveScene()) as GameObject;
                bl_Lobby lb = FindObjectOfType<bl_Lobby>();
                if (lb != null)
                {
                    GameObject ccb = lb.AddonsButtons[1];
                    if (ccb != null)
                    {
                        panel.transform.SetParent(ccb.transform, false);
                        if (lb.AddonsButtons[8] != null)
                        {
                            lb.AddonsButtons[8].SetActive(false);
                            EditorUtility.SetDirty(lb.AddonsButtons[8]);
                        }
                        EditorUtility.SetDirty(ccb);
                        EditorUtility.SetDirty(inputWindow);                  
                        EditorUtility.SetDirty(panel);
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        Debug.Log("<color=green>InputManager integrate!</color>");
                    }
                }
                else
                {
                    //use U login
                }
            }
            else
            {
                Debug.LogWarning("Scenes has not been added in Build Settings, Can't integrate CC Add-on.");
            }
        }
        else
        {
            Debug.Log("Can't found prefab!");
        }
    }

    [MenuItem("MFPS/Addons/InputManager/Integrate", true)]
    private static bool InstegrateValidate()
    {
        bl_KeyOptionsUI km = GameObject.FindObjectOfType<bl_KeyOptionsUI>();
        return (km == null);
    }
}