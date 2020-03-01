using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MFPS.ClassCustomization
{
    [System.Serializable]
    public class HudClassContent
    {
        public Image Icon;
        public Text WeaponNameText;
        public Text DamageValue;
        public Text AccuracyValue;
        public Text RateValue;
        public Text WeightValue;
        public Text RangeValue;
    }
}