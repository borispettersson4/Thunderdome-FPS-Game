using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UCompassBarInitializer
{

    [MenuItem("MFPS/Addons/CompassBar/Integrate")]
    private static void Instegrate()
    {
        bool sucess = true;
        GameObject prefab = Resources.Load<GameObject>(bl_GameData.Instance.Player1.name);
        if (prefab != null)
        {
            GameObject c = prefab.GetComponentInChildren<bl_CameraShaker>().gameObject;
            if (c.GetComponent<bl_CompassMagnet>() == null)
            {
                c.AddComponent<bl_CompassMagnet>();
                EditorUtility.SetDirty(prefab);
            }
        }
        else
        {
            Debug.Log("Can't found player 1");
            sucess = false;
        }

        prefab = Resources.Load<GameObject>(bl_GameData.Instance.Player2.name);
        if (prefab != null)
        {
            GameObject c = prefab.GetComponentInChildren<bl_CameraShaker>().gameObject;
            if (c.GetComponent<bl_CompassMagnet>() == null)
            {
                c.AddComponent<bl_CompassMagnet>();
                EditorUtility.SetDirty(prefab);
            }
        }
        else
        {
            Debug.Log("Can't found player 2");
            sucess = false;
        }
        if (sucess)
        {
            Debug.Log("UCompass Bar successful integrate!");
        }
    }
}