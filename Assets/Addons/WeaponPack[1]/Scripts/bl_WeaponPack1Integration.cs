using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace MFPS.Others
{
    public class bl_WeaponPack1Integration : MonoBehaviour
    {
        public int defaultListStart = 7;
        public List<bl_GunInfo> GunData = new List<bl_GunInfo>();
        public GameObject PlayerPrefab;

        [ContextMenu("Get Info")]
        void GetInfo()
        {
            GunData.Clear();
            for (int i = 0; i < bl_GameData.Instance.AllWeapons.Count; i++)
            {
                if (i < 7) continue;
                GunData.Add(bl_GameData.Instance.AllWeapons[i]);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(bl_WeaponPack1Integration))]
    public class bl_WeaponPack1IntegrationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            bl_WeaponPack1Integration script = (bl_WeaponPack1Integration)target;
            GUILayout.BeginVertical("box");
            var property = serializedObject.FindProperty("GunData");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            script.defaultListStart = EditorGUILayout.IntField("Default Start Index:", script.defaultListStart);
            script.PlayerPrefab = EditorGUILayout.ObjectField("Player", script.PlayerPrefab, typeof(GameObject), false) as GameObject;
            serializedObject.ApplyModifiedProperties();
            GUILayout.Space(5);
            if (GUILayout.Button("Integrate", EditorStyles.toolbarButton))
            {
                Integrate(script);
            }
            GUILayout.EndVertical();
        }

        void Integrate(bl_WeaponPack1Integration script)
        {
            bl_GunManager gm = script.PlayerPrefab.GetComponentInChildren<bl_GunManager>();
            for (int i = 0; i < script.GunData.Count; i++)
            {
                int index = 0;
                if(bl_GameData.Instance.AllWeapons.Exists(x => x.Name == script.GunData[i].Name))
                {
                    index = bl_GameData.Instance.AllWeapons.FindIndex(x => x.Name == script.GunData[i].Name);
                }
                else
                {
                    bl_GameData.Instance.AllWeapons.Add(script.GunData[i]);
                    index = bl_GameData.Instance.AllWeapons.Count - 1;
                }
                for (int e = 0; e < gm.AllGuns.Count; e++)
                {
                    if(gm.AllGuns[e].GunID == (script.defaultListStart + i))
                    {
                        gm.AllGuns[e].GunID = index;
                        break;
                    }
                }
            }
            EditorUtility.SetDirty(gm);
            EditorUtility.SetDirty(script.PlayerPrefab);
            EditorUtility.SetDirty(bl_GameData.Instance);
        }
    }
#endif
}