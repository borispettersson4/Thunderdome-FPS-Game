using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MFPS.ClassCustomization
{
    public class bl_ClassCustomizationUI : MonoBehaviour
    {
        [Header("HUD")]
        public HudClassContent PrimaryHUD;
        public HudClassContent SecondaryHUD;
        public HudClassContent KnifeHUD;
        public HudClassContent GrenadeHUD;
        public HudClassContent WeaponPreviewHUD;
        [Space(7)]
        public RectTransform AssaultButton = null;
        public RectTransform EnginnerButton = null;
        public RectTransform SupportButton = null;
        public RectTransform ReconButton = null;
        public RectTransform GrenadierButton = null;
        public Text ClassText = null;
        [Space(7)]
        public GameObject GunSelectPrefabs = null;
        public Transform PanelWeaponList = null;
    }
}