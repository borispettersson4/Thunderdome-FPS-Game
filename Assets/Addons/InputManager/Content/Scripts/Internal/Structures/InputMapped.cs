namespace Lovatto.Asset.InputManager
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class InputMapped : ScriptableObject
    {
        public Mapped m_Mapped;

        public List<bl_KeyInfo> AllKeys
        {
            get
            {
                return m_Mapped.AllKeys;
            }
        }

        [System.Serializable]
        public class Mapped
        {
            public List<bl_KeyInfo> AllKeys = new List<bl_KeyInfo>();
        }
    }
}