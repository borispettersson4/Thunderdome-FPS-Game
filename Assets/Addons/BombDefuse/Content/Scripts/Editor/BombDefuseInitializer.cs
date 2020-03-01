﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class BombDefuseInitializer : MonoBehaviour
{

    private const string DEFINE_KEY = "BDGM";

    static BombDefuseInitializer()
    {
        int start = PlayerPrefs.GetInt("mfps.addons.define." + DEFINE_KEY, 0);
        if (start == 0)
        {
            var defines = GetDefinesList(buildTargetGroups[0]);
            if (!defines.Contains(DEFINE_KEY))
            {
                SetEnabled(DEFINE_KEY, true);
            }
            PlayerPrefs.SetInt("mfps.addons.define." + DEFINE_KEY, 1);
        }
    }


    [MenuItem("MFPS/Addons/BombDefuse/Enable")]
    private static void Enable()
    {
        SetEnabled(DEFINE_KEY, true);
    }


    [MenuItem("MFPS/Addons/BombDefuse/Enable", true)]
    private static bool EnableValidate()
    {
        var defines = GetDefinesList(buildTargetGroups[0]);
        return !defines.Contains(DEFINE_KEY);
    }


    [MenuItem("MFPS/Addons/BombDefuse/Disable")]
    private static void Disable()
    {
        SetEnabled(DEFINE_KEY, false);
    }


    [MenuItem("MFPS/Addons/BombDefuse/Disable", true)]
    private static bool DisableValidate()
    {
        var defines = GetDefinesList(buildTargetGroups[0]);
        return defines.Contains(DEFINE_KEY);
    }

    [MenuItem("MFPS/Addons/BombDefuse/Integrate")]
    private static void Instegrate()
    {
        bl_RoomSettings lb = FindObjectOfType<bl_RoomSettings>();
        if (lb != null)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Addons/BombDefuse/Content/Prefabs/SNDObjects.prefab", typeof(GameObject)) as GameObject;
            if (prefab != null)
            {
#if BDGM
                PrefabUtility.InstantiatePrefab(prefab, UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
#endif
                EditorSceneManager.SaveOpenScenes();
                Debug.Log("Bomb defuse integrate in this map!");
            }
            else
            {
                Debug.Log("Can't found prefab!");
            }
        }
    }

    private static BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[]
        {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.Android,
                BuildTargetGroup.iOS,
                BuildTargetGroup.WSA,
                BuildTargetGroup.WebGL,
                BuildTargetGroup.Facebook,
        };


    private static void SetEnabled(string defineName, bool enable)
    {
        foreach (var group in buildTargetGroups)
        {
            var defines = GetDefinesList(group);
            if (enable)
            {
                if (defines.Contains(defineName))
                {
                    return;
                }
                defines.Add(defineName);
            }
            else
            {
                if (!defines.Contains(defineName))
                {
                    return;
                }
                while (defines.Contains(defineName))
                {
                    defines.Remove(defineName);
                }
            }
            string definesString = string.Join(";", defines.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
        }
    }


    private static List<string> GetDefinesList(BuildTargetGroup group)
    {
        return new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
    }
}