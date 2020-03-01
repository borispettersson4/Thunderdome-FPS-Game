using UnityEngine;
using System;

namespace MFPS.ClassCustomization
{
    public static class ClassKey
    {
        public const string PrimaryAssault = "PrimaryIDAssault";
        public const string SecondaryAssault = "SecondaryIDAssault";
        public const string KnifeAssault = "KnifeIDAssault";
        public const string GrenadeAssault = "GrenadeIDAssault";

        public const string PrimaryEnginner = "PrimaryIDEnginner";
        public const string SecondaryEnginner = "SecondaryIDEnginner";
        public const string KnifeEnginner = "KnifeIDEnginner";
        public const string GrenadeEnginner = "GrenadeIDEnginner";

        public const string PrimarySupport = "PrimaryIDSupport ";
        public const string SecondarySupport = "SecondaryIDSupport ";
        public const string KnifeSupport = "KnifeIDSupport ";
        public const string GrenadeSupport = "GrenadeIDSupport ";

        public const string PrimaryRecon = "PrimaryIDRecon";
        public const string SecondaryRecon = "SecondaryIDRecon";
        public const string KnifeRecon = "KnifeIDRecon";
        public const string GrenadeRecon = "GrenadeIDRecon";

        public const string PrimaryGrenadier = "PrimaryIDGrenadier";
        public const string SecondaryGrenadier = "SecondaryIDGrenadier";
        public const string KnifeGrenadier = "KnifeIDGrenadier";
        public const string GrenadeGrenadier = "GrenadeIDGrenadier";

        public const string ClassType = "ClassID";
        public static readonly string ClassKit = "class.kit.id";
    }

    [Serializable]
    public class ClassAllowedWeaponsType
    {
        public bool AllowMachineGuns = true;
        public bool AllowPistols = true;
        public bool AllowShotguns = true;
        public bool AllowSnipers = true;
        public bool AllowKnifes = true;
        public bool AllowGrenades = true;
    }
}