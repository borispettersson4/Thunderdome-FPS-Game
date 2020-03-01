using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using MFPSEditor;
#endif
using MFPS.ClassCustomization;

public class bl_ClassManager : ScriptableObject {

    [Header("Player Class")]
    public PlayerClass m_Class = PlayerClass.Assault;
    //When if new player and not have data saved
    //take this weapons ID for default
    [Header("Assault")]
    [GunID] public int DefaulPrimaryAssault = 0;
    [GunID] public int DefaulSecondaryAssault = 1;
    [GunID] public int DefaulKnifeAssault = 2;
    [GunID] public int DefaulGrenadeAssault = 3;
    [Header("Engineer")]
    [GunID] public int DefaulPrimaryEnginner = 0;
    [GunID] public int DefaulSecondaryEnginner = 1;
    [GunID] public int DefaulKnifeEnginner = 2;
    [GunID] public int DefaulGrenadeEnginner = 3;
    [Header("Support")]
    [GunID] public int DefaulPrimarySupport = 0;
    [GunID] public int DefaulSecondarySupport = 1;
    [GunID] public int DefaulKnifeSupport = 2;
    [GunID] public int DefaulGrenadeSupport = 3;
    [Header("Recon")]
    [GunID] public int DefaulPrimaryRecon = 0;
    [GunID] public int DefaulSecondaryRecon = 1;
    [GunID] public int DefaulKnifeRecon = 2;
    [GunID] public int DefaulGrenadeRecon = 3;
    [Header("Grenadier")]
    [GunID] public int DefaulPrimaryGrenadier = 0;
    [GunID] public int DefaulSecondaryGrenadier = 1;
    [GunID] public int DefaulKnifeGrenadier = 2;
    [GunID] public int DefaulGrenadeGrenadier = 3;

#if UNITY_EDITOR
    [Space(10)]
    [InspectorButton("DeleteKeys",ButtonWidth = 150)] public bool DeleteSavedClasses;
#endif
    //
    public int PrimaryAssault { get; set; }
    public int SecondaryAssault { get; set; }
    public int KnifeAssault { get; set; }
    public int GrenadeAssault { get; set; }
    ///
    public int PrimaryEnginner { get; set; }
    public int SecondaryEnginner { get; set; }
    public int KnifeEnginner { get; set; }
    public int GrenadeEnginner { get; set; }
    //
    public int PrimarySupport { get; set; }
    public int SecondarySupport { get; set; }
    public int KnifeSupport { get; set; }
    public int GrenadeSupport { get; set; }
    //m_Class[3]
    public int PrimaryRecon { get; set; }
    public int SecondaryRecon { get; set; }
    public int KnifeRecon { get; set; }
    public int GrenadeRecon { get; set; }
    //m_Class[3]
    public int PrimaryGrenadier { get; set; }
    public int SecondaryGrenadier { get; set; }
    public int KnifeGrenadier { get; set; }
    public int GrenadeGrenadier { get; set; }
    [HideInInspector] public int ClassKit = 0;

    public void DeleteKeys()
    {
        PlayerPrefs.DeleteKey(ClassKey.PrimaryAssault);
        PlayerPrefs.DeleteKey(ClassKey.PrimaryEnginner);
        PlayerPrefs.DeleteKey(ClassKey.PrimaryRecon);
        PlayerPrefs.DeleteKey(ClassKey.PrimarySupport);
        PlayerPrefs.DeleteKey(ClassKey.PrimaryGrenadier);
        PlayerPrefs.DeleteKey(ClassKey.SecondaryAssault);
        PlayerPrefs.DeleteKey(ClassKey.SecondaryEnginner);
        PlayerPrefs.DeleteKey(ClassKey.SecondaryRecon);
        PlayerPrefs.DeleteKey(ClassKey.SecondarySupport);
        PlayerPrefs.DeleteKey(ClassKey.SecondaryGrenadier);
        PlayerPrefs.DeleteKey(ClassKey.KnifeAssault);
        PlayerPrefs.DeleteKey(ClassKey.KnifeEnginner);
        PlayerPrefs.DeleteKey(ClassKey.KnifeRecon);
        PlayerPrefs.DeleteKey(ClassKey.KnifeSupport);
        PlayerPrefs.DeleteKey(ClassKey.KnifeGrenadier);
        PlayerPrefs.DeleteKey(ClassKey.GrenadeAssault);
        PlayerPrefs.DeleteKey(ClassKey.GrenadeEnginner);
        PlayerPrefs.DeleteKey(ClassKey.GrenadeRecon);
        PlayerPrefs.DeleteKey(ClassKey.GrenadeSupport);
        PlayerPrefs.DeleteKey(ClassKey.GrenadeGrenadier);
        Debug.Log("Keys Cleaned!");
    }

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        Init();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Init()
    {
        GetID();
    }

    /// <summary>
    /// 
    /// </summary>
    void GetID()
    {
        int c = 0;
        if (PlayerPrefs.HasKey(ClassKey.ClassType)) { c = PlayerPrefs.GetInt(ClassKey.ClassType); }
        switch (c)
        {
            case 0 :
                m_Class = PlayerClass.Assault;
                break;
            case 1:
                m_Class = PlayerClass.Recon;
                break;
            case 2:
                m_Class = PlayerClass.Engineer;
                break;
            case 3:
                m_Class = PlayerClass.Support;
                break;
            case 4:
                m_Class = PlayerClass.Grenadier;
                break;
        }

        ClassKit = PlayerPrefs.GetInt(ClassKey.ClassKit, 0);
        if (PlayerPrefs.HasKey(ClassKey.PrimaryAssault)) { PrimaryAssault = PlayerPrefs.GetInt(ClassKey.PrimaryAssault); } else { PrimaryAssault = DefaulPrimaryAssault; }
        if (PlayerPrefs.HasKey(ClassKey.SecondaryAssault)) { SecondaryAssault = PlayerPrefs.GetInt(ClassKey.SecondaryAssault); } else { SecondaryAssault = DefaulSecondaryAssault; }
        if (PlayerPrefs.HasKey(ClassKey.KnifeAssault)) { KnifeAssault = PlayerPrefs.GetInt(ClassKey.KnifeAssault); } else { KnifeAssault = DefaulKnifeAssault; }
        if (PlayerPrefs.HasKey(ClassKey.GrenadeAssault)) { GrenadeAssault = PlayerPrefs.GetInt(ClassKey.GrenadeAssault); } else { GrenadeAssault = DefaulGrenadeAssault; }

        if (PlayerPrefs.HasKey(ClassKey.PrimaryEnginner)) { PrimaryEnginner = PlayerPrefs.GetInt(ClassKey.PrimaryEnginner); } else { PrimaryEnginner = DefaulPrimaryEnginner; }
        if (PlayerPrefs.HasKey(ClassKey.SecondaryEnginner)) { SecondaryEnginner = PlayerPrefs.GetInt(ClassKey.SecondaryEnginner); } else { SecondaryEnginner = DefaulSecondaryEnginner; }
        if (PlayerPrefs.HasKey(ClassKey.KnifeEnginner)) { KnifeEnginner = PlayerPrefs.GetInt(ClassKey.KnifeEnginner); } else { KnifeEnginner = DefaulKnifeEnginner; }
        if (PlayerPrefs.HasKey(ClassKey.GrenadeEnginner)) { GrenadeEnginner = PlayerPrefs.GetInt(ClassKey.GrenadeEnginner); } else { GrenadeEnginner = DefaulGrenadeEnginner; }

        if (PlayerPrefs.HasKey(ClassKey.PrimarySupport)) { PrimarySupport = PlayerPrefs.GetInt(ClassKey.PrimarySupport); } else { PrimarySupport = DefaulPrimarySupport; }
        if (PlayerPrefs.HasKey(ClassKey.SecondarySupport)) { SecondarySupport = PlayerPrefs.GetInt(ClassKey.SecondarySupport); } else { SecondarySupport = DefaulSecondarySupport; }
        if (PlayerPrefs.HasKey(ClassKey.KnifeSupport)) { KnifeSupport = PlayerPrefs.GetInt(ClassKey.KnifeSupport); } else { KnifeSupport = DefaulKnifeSupport; }
        if (PlayerPrefs.HasKey(ClassKey.GrenadeSupport)) { GrenadeSupport = PlayerPrefs.GetInt(ClassKey.GrenadeSupport); } else { GrenadeSupport = DefaulGrenadeSupport; }

        if (PlayerPrefs.HasKey(ClassKey.PrimaryRecon)) { PrimaryRecon = PlayerPrefs.GetInt(ClassKey.PrimaryRecon); } else { PrimaryRecon = DefaulPrimaryRecon; }
        if (PlayerPrefs.HasKey(ClassKey.SecondaryRecon)) { SecondaryRecon = PlayerPrefs.GetInt(ClassKey.SecondaryRecon); } else { SecondaryRecon = DefaulSecondaryRecon; }
        if (PlayerPrefs.HasKey(ClassKey.KnifeRecon)) { KnifeRecon = PlayerPrefs.GetInt(ClassKey.KnifeRecon); } else { KnifeRecon = DefaulKnifeRecon; }
        if (PlayerPrefs.HasKey(ClassKey.GrenadeRecon)) { GrenadeRecon = PlayerPrefs.GetInt(ClassKey.GrenadeRecon); } else { GrenadeRecon = DefaulGrenadeRecon; }

        if (PlayerPrefs.HasKey(ClassKey.PrimaryGrenadier)) { PrimaryGrenadier = PlayerPrefs.GetInt(ClassKey.PrimaryGrenadier); } else { PrimaryGrenadier = DefaulPrimaryGrenadier; }
        if (PlayerPrefs.HasKey(ClassKey.SecondaryGrenadier)) { SecondaryGrenadier = PlayerPrefs.GetInt(ClassKey.SecondaryGrenadier); } else { SecondaryGrenadier = DefaulSecondaryGrenadier; }
        if (PlayerPrefs.HasKey(ClassKey.KnifeGrenadier)) { KnifeGrenadier = PlayerPrefs.GetInt(ClassKey.KnifeGrenadier); } else { KnifeGrenadier = DefaulKnifeGrenadier; }
        if (PlayerPrefs.HasKey(ClassKey.GrenadeGrenadier)) { GrenadeGrenadier = PlayerPrefs.GetInt(ClassKey.GrenadeGrenadier); } else { GrenadeGrenadier = DefaulGrenadeGrenadier; }
    }

    public void SetUpClasses(bl_GunManager gm)
    {
        gm.m_Class[0].primary = PrimaryAssault;
        gm.m_Class[0].secondary = SecondaryAssault;
        gm.m_Class[0].Knife = KnifeAssault;
        gm.m_Class[0].Special = GrenadeAssault;

        gm.m_Class[2].primary = PrimaryEnginner;
        gm.m_Class[2].secondary = SecondaryEnginner;
        gm.m_Class[2].Knife = KnifeEnginner;
        gm.m_Class[2].Special = GrenadeEnginner;

        gm.m_Class[3].primary = PrimarySupport;
        gm.m_Class[3].secondary = SecondarySupport;
        gm.m_Class[3].Knife = KnifeSupport;
        gm.m_Class[3].Special = GrenadeSupport;

        gm.m_Class[1].primary = PrimaryRecon;
        gm.m_Class[1].secondary = SecondaryRecon;
        gm.m_Class[1].Knife = KnifeRecon;
        gm.m_Class[1].Special = GrenadeRecon;

        gm.m_Class[4].primary = PrimaryGrenadier;
        gm.m_Class[4].secondary = SecondaryGrenadier;
        gm.m_Class[4].Knife = KnifeGrenadier;
        gm.m_Class[4].Special = GrenadeGrenadier;

        switch (bl_RoomMenu.PlayerClass)
        {
            case PlayerClass.Assault:
                gm.PlayerEquip[0] = gm.GetGunOnListById(gm.m_Class[0].primary);
                gm.PlayerEquip[1] = gm.GetGunOnListById(gm.m_Class[0].secondary);
                gm.PlayerEquip[2] = gm.GetGunOnListById(gm.m_Class[0].Special);
                gm.PlayerEquip[3] = gm.GetGunOnListById(gm.m_Class[0].Knife);
                break;
            case PlayerClass.Recon:
                gm.PlayerEquip[0] = gm.GetGunOnListById(gm.m_Class[1].primary);
                gm.PlayerEquip[1] = gm.GetGunOnListById(gm.m_Class[1].secondary);
                gm.PlayerEquip[2] = gm.GetGunOnListById(gm.m_Class[1].Special);
                gm.PlayerEquip[3] = gm.GetGunOnListById(gm.m_Class[1].Knife);
                break;
            case PlayerClass.Engineer:
                gm.PlayerEquip[0] = gm.GetGunOnListById(gm.m_Class[2].primary);
                gm.PlayerEquip[1] = gm.GetGunOnListById(gm.m_Class[2].secondary);
                gm.PlayerEquip[2] = gm.GetGunOnListById(gm.m_Class[2].Special);
                gm.PlayerEquip[3] = gm.GetGunOnListById(gm.m_Class[2].Knife);
                break;
            case PlayerClass.Support:
                gm.PlayerEquip[0] = gm.GetGunOnListById(gm.m_Class[3].primary);
                gm.PlayerEquip[1] = gm.GetGunOnListById(gm.m_Class[3].secondary);
                gm.PlayerEquip[2] = gm.GetGunOnListById(gm.m_Class[3].Special);
                gm.PlayerEquip[3] = gm.GetGunOnListById(gm.m_Class[3].Knife);
                break;
            case PlayerClass.Grenadier:
                gm.PlayerEquip[0] = gm.GetGunOnListById(gm.m_Class[4].primary);
                gm.PlayerEquip[1] = gm.GetGunOnListById(gm.m_Class[4].secondary);
                gm.PlayerEquip[2] = gm.GetGunOnListById(gm.m_Class[4].Special);
                gm.PlayerEquip[3] = gm.GetGunOnListById(gm.m_Class[4].Knife);
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SaveClass()
    {
        PlayerPrefs.SetInt(ClassKey.PrimaryAssault, PrimaryAssault);
        PlayerPrefs.SetInt(ClassKey.SecondaryAssault, SecondaryAssault);
        PlayerPrefs.SetInt(ClassKey.KnifeAssault, KnifeAssault);
        PlayerPrefs.SetInt(ClassKey.GrenadeAssault, GrenadeAssault);

        PlayerPrefs.SetInt(ClassKey.PrimaryEnginner, PrimaryEnginner);
        PlayerPrefs.SetInt(ClassKey.SecondaryEnginner, SecondaryEnginner);
        PlayerPrefs.SetInt(ClassKey.KnifeEnginner, KnifeEnginner);
        PlayerPrefs.SetInt(ClassKey.GrenadeEnginner, GrenadeEnginner);

        PlayerPrefs.SetInt(ClassKey.PrimarySupport, PrimarySupport);
        PlayerPrefs.SetInt(ClassKey.SecondarySupport, SecondarySupport);
        PlayerPrefs.SetInt(ClassKey.KnifeSupport, KnifeSupport);
        PlayerPrefs.SetInt(ClassKey.GrenadeSupport, GrenadeSupport);

        PlayerPrefs.SetInt(ClassKey.PrimaryRecon, PrimaryRecon);
        PlayerPrefs.SetInt(ClassKey.SecondaryRecon, SecondaryRecon);
        PlayerPrefs.SetInt(ClassKey.KnifeRecon, KnifeRecon);
        PlayerPrefs.SetInt(ClassKey.GrenadeRecon, GrenadeRecon);

        PlayerPrefs.SetInt(ClassKey.PrimaryGrenadier, PrimaryGrenadier);
        PlayerPrefs.SetInt(ClassKey.SecondaryGrenadier, SecondaryGrenadier);
        PlayerPrefs.SetInt(ClassKey.KnifeGrenadier, KnifeGrenadier);
        PlayerPrefs.SetInt(ClassKey.GrenadeGrenadier, GrenadeGrenadier);

        Debug.Log("Class saved!");
    }

    private static bl_ClassManager _instance;
    public static bl_ClassManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<bl_ClassManager>("ClassManager") as bl_ClassManager;
            }
            return _instance;
        }
    }
}