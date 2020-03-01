using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MFPS.ClassCustomization
{
    [System.Serializable]
    public class ClassInfo
    {

        [GunID] public int ID;

        public bl_GunInfo Info
        {
            get
            {
                return bl_GameData.Instance.GetWeapon(ID);
            }
        }
    }
}