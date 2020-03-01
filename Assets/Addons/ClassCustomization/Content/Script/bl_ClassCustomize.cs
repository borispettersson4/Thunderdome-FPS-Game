using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using MFPS.ClassCustomization;

namespace MFPS.ClassCustomization
{
    public class bl_ClassCustomize : MonoBehaviour
    {

        [Header("Info")]
        public PlayerClass m_Class = PlayerClass.Assault;
        public string SceneToReturn = "";
        public bool CloseListOnChangeWeapon = true;

        [Header("Weapons Info")]
        public List<ClassInfo> AssaultClass = new List<ClassInfo>();
        public List<ClassInfo> EngineerClass = new List<ClassInfo>();
        public List<ClassInfo> SupportClass = new List<ClassInfo>();
        public List<ClassInfo> ReconClass = new List<ClassInfo>();
        public List<ClassInfo> GrenadierClass = new List<ClassInfo>();

        [Header("Slots Rules")]
        public ClassAllowedWeaponsType PrimaryAllowedWeapons;
        public ClassAllowedWeaponsType SecondaryAllowedWeapons;
        public ClassAllowedWeaponsType KnifeAllowedWeapons;
        public ClassAllowedWeaponsType GrenadesAllowedWeapons;

        private bl_ClassManager ClassManager;
        private int CurrentSlot = 0;
        private bl_ClassCustomizationUI UI;

        public List<GameObject> WeaponPreview;

        private ClassInfo currentGun;

        void Awake()
        {
            //Fix issue
            if (PhotonNetwork.IsConnected)
            {
                //PhotonNetwork.Disconnect();
            }
            UI = FindObjectOfType<bl_ClassCustomizationUI>();
            ClassManager = bl_ClassManager.Instance;
            ClassManager.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            TakeCurrentClass(bl_ClassManager.Instance.m_Class);
            currentGun = new ClassInfo { ID = 0 };
            PreviewCurrentGun(ClassManager.PrimaryAssault);
        }

        public void copyAllClassesFromFirst()
        {
            EngineerClass.Clear();
            ReconClass.Clear();
            SupportClass.Clear();
            GrenadierClass.Clear();

            foreach (ClassInfo weapon in AssaultClass)
            {
                EngineerClass.Add(weapon);
                ReconClass.Add(weapon);
                SupportClass.Add(weapon);
                GrenadierClass.Add(weapon);
            }
            SaveClass();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void ChangeSlotClass(int id, int slot, int listid)
        {
            switch (CurrentSlot)
            {
                case 0:
                    switch (bl_ClassManager.Instance.m_Class)
                    {
                        case PlayerClass.Assault:
                            ClassManager.PrimaryAssault = id;
                            PlayerPrefs.SetInt(ClassKey.PrimaryAssault, id);
                            break;
                        case PlayerClass.Engineer:
                            ClassManager.PrimaryEnginner = id;
                            PlayerPrefs.SetInt(ClassKey.PrimaryEnginner, id);
                            break;
                        case PlayerClass.Support:
                            ClassManager.PrimarySupport = id;
                            PlayerPrefs.SetInt(ClassKey.PrimarySupport, id);
                            break;
                        case PlayerClass.Recon:
                            ClassManager.PrimaryRecon = id;
                            PlayerPrefs.SetInt(ClassKey.PrimaryRecon, id);
                            break;
                        case PlayerClass.Grenadier:
                            ClassManager.PrimaryGrenadier = id;
                            PlayerPrefs.SetInt(ClassKey.PrimaryGrenadier, id);
                            break;

                    }
                    CleanList();
                    break;
                case 1:
                    switch (bl_ClassManager.Instance.m_Class)
                    {
                        case PlayerClass.Assault:
                            ClassManager.SecondaryAssault = id;
                            PlayerPrefs.SetInt(ClassKey.SecondaryAssault, id);
                            break;
                        case PlayerClass.Engineer:
                            ClassManager.SecondaryEnginner = id;
                            PlayerPrefs.SetInt(ClassKey.SecondaryEnginner, id);
                            break;
                        case PlayerClass.Support:
                            ClassManager.SecondarySupport = id;
                            PlayerPrefs.SetInt(ClassKey.SecondarySupport, id);
                            break;
                        case PlayerClass.Recon:
                            ClassManager.SecondaryRecon = id;
                            PlayerPrefs.SetInt(ClassKey.SecondaryRecon, id);
                            break;
                        case PlayerClass.Grenadier:
                            ClassManager.SecondaryGrenadier = id;
                            PlayerPrefs.SetInt(ClassKey.SecondaryGrenadier, id);
                            break;
                    }
                    CleanList();
                    break;
                case 2:
                    switch (bl_ClassManager.Instance.m_Class)
                    {
                        case PlayerClass.Assault:
                            ClassManager.KnifeAssault = id;
                            PlayerPrefs.SetInt(ClassKey.KnifeAssault, id);
                            break;
                        case PlayerClass.Engineer:
                            ClassManager.KnifeEnginner = id;
                            PlayerPrefs.SetInt(ClassKey.KnifeEnginner, id);
                            break;
                        case PlayerClass.Support:
                            ClassManager.KnifeSupport = id;
                            PlayerPrefs.SetInt(ClassKey.KnifeSupport, id);
                            break;
                        case PlayerClass.Recon:
                            ClassManager.KnifeRecon = id;
                            PlayerPrefs.SetInt(ClassKey.KnifeRecon, id);
                            break;
                        case PlayerClass.Grenadier:
                            ClassManager.KnifeGrenadier = id;
                            PlayerPrefs.SetInt(ClassKey.KnifeGrenadier, id);
                            break;
                    }
                    CleanList();
                    break;
                case 3:
                    switch (bl_ClassManager.Instance.m_Class)
                    {
                        case PlayerClass.Assault:
                            ClassManager.GrenadeAssault = id;
                            PlayerPrefs.SetInt(ClassKey.GrenadeAssault, id);
                            break;
                        case PlayerClass.Engineer:
                            ClassManager.GrenadeEnginner = id;
                            PlayerPrefs.SetInt(ClassKey.GrenadeEnginner, id);
                            break;
                        case PlayerClass.Support:
                            ClassManager.GrenadeSupport = id;
                            PlayerPrefs.SetInt(ClassKey.GrenadeSupport, id);
                            break;
                        case PlayerClass.Recon:
                            ClassManager.GrenadeRecon = id;
                            PlayerPrefs.SetInt(ClassKey.GrenadeRecon, id);
                            break;
                        case PlayerClass.Grenadier:
                            ClassManager.GrenadeGrenadier = id;
                            PlayerPrefs.SetInt(ClassKey.GrenadeGrenadier, id);
                            break;
                    }
                    CleanList();
                    break;

            }

            UpdateClassUI(listid, slot);
            PreviewCurrentGun(id);

            if (CloseListOnChangeWeapon)
            {
                CleanList();
            }
        }

        void UpdateClassUI(int id, int slot)
        {
            switch (bl_ClassManager.Instance.m_Class)
            {
                case PlayerClass.Assault:
                    switch (slot)
                    {
                        case 0:
                            UI.PrimaryHUD.Icon.sprite = AssaultClass[id].Info.GunIcon;
                            UI.PrimaryHUD.WeaponNameText.text = AssaultClass[id].Info.Name.ToUpper();
                            UI.PrimaryHUD.AccuracyValue.text = AssaultClass[id].Info.Accuracy + "";
                            UI.PrimaryHUD.DamageValue.text = AssaultClass[id].Info.Damage + "";
                            UI.PrimaryHUD.RateValue.text = AssaultClass[id].Info.FireRate + "";
                            UI.PrimaryHUD.RangeValue.text = AssaultClass[id].Info.Range + "";
                            UI.PrimaryHUD.WeightValue.text = AssaultClass[id].Info.Weight + "";
                            break;
                        case 1:
                            UI.SecondaryHUD.Icon.sprite = AssaultClass[id].Info.GunIcon;
                            UI.SecondaryHUD.WeaponNameText.text = AssaultClass[id].Info.Name.ToUpper();
                            UI.SecondaryHUD.AccuracyValue.text = AssaultClass[id].Info.Accuracy + "";
                            UI.SecondaryHUD.DamageValue.text = AssaultClass[id].Info.Damage + "";
                            UI.SecondaryHUD.RateValue.text = AssaultClass[id].Info.FireRate + "";
                            UI.SecondaryHUD.RangeValue.text = AssaultClass[id].Info.Range + "";
                            UI.SecondaryHUD.WeightValue.text = AssaultClass[id].Info.Weight + "";
                            break;
                        case 2:
                            UI.KnifeHUD.Icon.sprite = AssaultClass[id].Info.GunIcon;
                            UI.KnifeHUD.WeaponNameText.text = AssaultClass[id].Info.Name.ToUpper();
                            UI.KnifeHUD.AccuracyValue.text = AssaultClass[id].Info.Accuracy + "";
                            UI.KnifeHUD.DamageValue.text = AssaultClass[id].Info.Damage + "";
                            UI.KnifeHUD.RateValue.text = AssaultClass[id].Info.FireRate + "";
                            UI.KnifeHUD.RangeValue.text = AssaultClass[id].Info.Range + "";
                            UI.KnifeHUD.WeightValue.text = AssaultClass[id].Info.Weight + "";
                            break;
                        case 3:
                            UI.GrenadeHUD.Icon.sprite = AssaultClass[id].Info.GunIcon;
                            UI.GrenadeHUD.WeaponNameText.text = AssaultClass[id].Info.Name.ToUpper();
                            UI.GrenadeHUD.AccuracyValue.text = AssaultClass[id].Info.Accuracy + "";
                            UI.GrenadeHUD.DamageValue.text = AssaultClass[id].Info.Damage + "";
                            UI.GrenadeHUD.RateValue.text = AssaultClass[id].Info.FireRate + "";
                            UI.GrenadeHUD.RangeValue.text = AssaultClass[id].Info.Range + "";
                            UI.GrenadeHUD.WeightValue.text = AssaultClass[id].Info.Weight + "";
                            break;
                    }
                    break;
                //------------------------------------------------------------------
                case PlayerClass.Engineer:
                    switch (slot)
                    {
                        case 0:
                            UI.PrimaryHUD.Icon.sprite = EngineerClass[id].Info.GunIcon;
                            UI.PrimaryHUD.WeaponNameText.text = EngineerClass[id].Info.Name.ToUpper();
                            UI.PrimaryHUD.AccuracyValue.text = EngineerClass[id].Info.Accuracy + "";
                            UI.PrimaryHUD.DamageValue.text = EngineerClass[id].Info.Damage + "";
                            UI.PrimaryHUD.RateValue.text = EngineerClass[id].Info.FireRate + "";
                            UI.PrimaryHUD.RangeValue.text = EngineerClass[id].Info.Range + "";
                            UI.PrimaryHUD.WeightValue.text = EngineerClass[id].Info.Weight + "";
                            break;
                        case 1:
                            UI.SecondaryHUD.Icon.sprite = EngineerClass[id].Info.GunIcon;
                            UI.SecondaryHUD.WeaponNameText.text = EngineerClass[id].Info.Name.ToUpper();
                            UI.SecondaryHUD.AccuracyValue.text = EngineerClass[id].Info.Accuracy + "";
                            UI.SecondaryHUD.DamageValue.text = EngineerClass[id].Info.Damage + "";
                            UI.SecondaryHUD.RateValue.text = EngineerClass[id].Info.FireRate + "";
                            UI.SecondaryHUD.RangeValue.text = EngineerClass[id].Info.Range + "";
                            UI.SecondaryHUD.WeightValue.text = EngineerClass[id].Info.Weight + "";
                            break;
                        case 2:
                            UI.KnifeHUD.Icon.sprite = EngineerClass[id].Info.GunIcon;
                            UI.KnifeHUD.WeaponNameText.text = EngineerClass[id].Info.Name.ToUpper();
                            UI.KnifeHUD.AccuracyValue.text = EngineerClass[id].Info.Accuracy + "";
                            UI.KnifeHUD.DamageValue.text = EngineerClass[id].Info.Damage + "";
                            UI.KnifeHUD.RateValue.text = EngineerClass[id].Info.FireRate + "";
                            UI.KnifeHUD.RangeValue.text = EngineerClass[id].Info.Range + "";
                            UI.KnifeHUD.WeightValue.text = EngineerClass[id].Info.Weight + "";
                            break;
                        case 3:
                            UI.GrenadeHUD.Icon.sprite = EngineerClass[id].Info.GunIcon;
                            UI.GrenadeHUD.WeaponNameText.text = EngineerClass[id].Info.Name.ToUpper();
                            UI.GrenadeHUD.AccuracyValue.text = EngineerClass[id].Info.Accuracy + "";
                            UI.GrenadeHUD.DamageValue.text = EngineerClass[id].Info.Damage + "";
                            UI.GrenadeHUD.RateValue.text = EngineerClass[id].Info.FireRate + "";
                            UI.GrenadeHUD.RangeValue.text = EngineerClass[id].Info.Range + "";
                            UI.GrenadeHUD.WeightValue.text = EngineerClass[id].Info.Weight + "";
                            break;
                    }
                    break;
                case PlayerClass.Support:
                    switch (slot)
                    {
                        case 0:
                            UI.PrimaryHUD.Icon.sprite = SupportClass[id].Info.GunIcon;
                            UI.PrimaryHUD.WeaponNameText.text = SupportClass[id].Info.Name.ToUpper();
                            UI.PrimaryHUD.AccuracyValue.text = SupportClass[id].Info.Accuracy + "";
                            UI.PrimaryHUD.DamageValue.text = SupportClass[id].Info.Damage + "";
                            UI.PrimaryHUD.RateValue.text = SupportClass[id].Info.FireRate + "";
                            UI.PrimaryHUD.RangeValue.text = SupportClass[id].Info.Range + "";
                            UI.PrimaryHUD.WeightValue.text = SupportClass[id].Info.Weight + "";

                            break;
                        case 1:
                            UI.SecondaryHUD.Icon.sprite = SupportClass[id].Info.GunIcon;
                            UI.SecondaryHUD.WeaponNameText.text = SupportClass[id].Info.Name.ToUpper();
                            UI.SecondaryHUD.AccuracyValue.text = SupportClass[id].Info.Accuracy + "";
                            UI.SecondaryHUD.DamageValue.text = SupportClass[id].Info.Damage + "";
                            UI.SecondaryHUD.RateValue.text = SupportClass[id].Info.FireRate + "";
                            UI.SecondaryHUD.RangeValue.text = SupportClass[id].Info.Range + "";
                            UI.SecondaryHUD.WeightValue.text = SupportClass[id].Info.Weight + "";
                            break;
                        case 2:
                            UI.KnifeHUD.Icon.sprite = SupportClass[id].Info.GunIcon;
                            UI.KnifeHUD.WeaponNameText.text = SupportClass[id].Info.Name.ToUpper();
                            UI.KnifeHUD.AccuracyValue.text = SupportClass[id].Info.Accuracy + "";
                            UI.KnifeHUD.DamageValue.text = SupportClass[id].Info.Damage + "";
                            UI.KnifeHUD.RateValue.text = SupportClass[id].Info.FireRate + "";
                            UI.KnifeHUD.RangeValue.text = SupportClass[id].Info.Range + "";
                            UI.KnifeHUD.WeightValue.text = SupportClass[id].Info.Weight + "";
                            break;
                        case 3:
                            UI.GrenadeHUD.Icon.sprite = SupportClass[id].Info.GunIcon;
                            UI.GrenadeHUD.WeaponNameText.text = SupportClass[id].Info.Name.ToUpper();
                            UI.GrenadeHUD.AccuracyValue.text = SupportClass[id].Info.Accuracy + "";
                            UI.GrenadeHUD.DamageValue.text = SupportClass[id].Info.Damage + "";
                            UI.GrenadeHUD.RateValue.text = SupportClass[id].Info.FireRate + "";
                            UI.GrenadeHUD.RangeValue.text = SupportClass[id].Info.Range + "";
                            UI.GrenadeHUD.WeightValue.text = SupportClass[id].Info.Weight + "";
                            break;
                    }
                    break;
                case PlayerClass.Recon:
                    switch (slot)
                    {
                        case 0:
                            UI.PrimaryHUD.Icon.sprite = ReconClass[id].Info.GunIcon;
                            UI.PrimaryHUD.WeaponNameText.text = ReconClass[id].Info.Name.ToUpper();
                            UI.PrimaryHUD.AccuracyValue.text = ReconClass[id].Info.Accuracy + "";
                            UI.PrimaryHUD.DamageValue.text = ReconClass[id].Info.Damage + "";
                            UI.PrimaryHUD.RateValue.text = ReconClass[id].Info.FireRate + "";
                            UI.PrimaryHUD.RangeValue.text = ReconClass[id].Info.Range + "";
                            UI.PrimaryHUD.WeightValue.text = ReconClass[id].Info.Weight + "";
                            break;
                        case 1:
                            UI.SecondaryHUD.Icon.sprite = ReconClass[id].Info.GunIcon;
                            UI.SecondaryHUD.WeaponNameText.text = ReconClass[id].Info.Name.ToUpper();
                            UI.SecondaryHUD.AccuracyValue.text = ReconClass[id].Info.Accuracy + "";
                            UI.SecondaryHUD.DamageValue.text = ReconClass[id].Info.Damage + "";
                            UI.SecondaryHUD.RateValue.text = ReconClass[id].Info.FireRate + "";
                            UI.SecondaryHUD.RangeValue.text = ReconClass[id].Info.Range + "";
                            UI.SecondaryHUD.WeightValue.text = ReconClass[id].Info.Weight + "";
                            break;
                        case 2:
                            UI.KnifeHUD.Icon.sprite = ReconClass[id].Info.GunIcon;
                            UI.KnifeHUD.WeaponNameText.text = ReconClass[id].Info.Name.ToUpper();
                            UI.KnifeHUD.AccuracyValue.text = ReconClass[id].Info.Accuracy + "";
                            UI.KnifeHUD.DamageValue.text = ReconClass[id].Info.Damage + "";
                            UI.KnifeHUD.RateValue.text = ReconClass[id].Info.FireRate + "";
                            UI.KnifeHUD.RangeValue.text = ReconClass[id].Info.Range + "";
                            UI.KnifeHUD.WeightValue.text = ReconClass[id].Info.Weight + "";
                            break;
                        case 3:
                            UI.GrenadeHUD.Icon.sprite = ReconClass[id].Info.GunIcon;
                            UI.GrenadeHUD.WeaponNameText.text = ReconClass[id].Info.Name.ToUpper();
                            UI.GrenadeHUD.AccuracyValue.text = ReconClass[id].Info.Accuracy + "";
                            UI.GrenadeHUD.DamageValue.text = ReconClass[id].Info.Damage + "";
                            UI.GrenadeHUD.RateValue.text = ReconClass[id].Info.FireRate + "";
                            UI.GrenadeHUD.RangeValue.text = ReconClass[id].Info.Range + "";
                            UI.GrenadeHUD.WeightValue.text = ReconClass[id].Info.Weight + "";
                            break;
                    }
                    break;
                case PlayerClass.Grenadier:
                    switch (slot)
                    {
                        case 0:
                            UI.PrimaryHUD.Icon.sprite = GrenadierClass[id].Info.GunIcon;
                            UI.PrimaryHUD.WeaponNameText.text = GrenadierClass[id].Info.Name.ToUpper();
                            UI.PrimaryHUD.AccuracyValue.text = GrenadierClass[id].Info.Accuracy + "";
                            UI.PrimaryHUD.DamageValue.text = GrenadierClass[id].Info.Damage + "";
                            UI.PrimaryHUD.RateValue.text = GrenadierClass[id].Info.FireRate + "";
                            UI.PrimaryHUD.RangeValue.text = GrenadierClass[id].Info.Range + "";
                            UI.PrimaryHUD.WeightValue.text = GrenadierClass[id].Info.Weight + "";
                            break;
                        case 1:
                            UI.SecondaryHUD.Icon.sprite = GrenadierClass[id].Info.GunIcon;
                            UI.SecondaryHUD.WeaponNameText.text = GrenadierClass[id].Info.Name.ToUpper();
                            UI.SecondaryHUD.AccuracyValue.text = GrenadierClass[id].Info.Accuracy + "";
                            UI.SecondaryHUD.DamageValue.text = GrenadierClass[id].Info.Damage + "";
                            UI.SecondaryHUD.RateValue.text = GrenadierClass[id].Info.FireRate + "";
                            UI.SecondaryHUD.RangeValue.text = GrenadierClass[id].Info.Range + "";
                            UI.SecondaryHUD.WeightValue.text = GrenadierClass[id].Info.Weight + "";
                            break;
                        case 2:
                            UI.KnifeHUD.Icon.sprite = GrenadierClass[id].Info.GunIcon;
                            UI.KnifeHUD.WeaponNameText.text = GrenadierClass[id].Info.Name.ToUpper();
                            UI.KnifeHUD.AccuracyValue.text = GrenadierClass[id].Info.Accuracy + "";
                            UI.KnifeHUD.DamageValue.text = GrenadierClass[id].Info.Damage + "";
                            UI.KnifeHUD.RateValue.text = GrenadierClass[id].Info.FireRate + "";
                            UI.KnifeHUD.RangeValue.text = GrenadierClass[id].Info.Range + "";
                            UI.KnifeHUD.WeightValue.text = GrenadierClass[id].Info.Weight + "";
                            break;
                        case 3:
                            UI.GrenadeHUD.Icon.sprite = GrenadierClass[id].Info.GunIcon;
                            UI.GrenadeHUD.WeaponNameText.text = GrenadierClass[id].Info.Name.ToUpper();
                            UI.GrenadeHUD.AccuracyValue.text = GrenadierClass[id].Info.Accuracy + "";
                            UI.GrenadeHUD.DamageValue.text = GrenadierClass[id].Info.Damage + "";
                            UI.GrenadeHUD.RateValue.text = GrenadierClass[id].Info.FireRate + "";
                            UI.GrenadeHUD.RangeValue.text = GrenadierClass[id].Info.Range + "";
                            UI.GrenadeHUD.WeightValue.text = GrenadierClass[id].Info.Weight + "";
                            break;
                    }
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void SaveClass() { bl_ClassManager.Instance.SaveClass(); }
        public void ReturnToScene() { bl_UtilityHelper.LoadLevel(SceneToReturn); }

        void CleanList()
        {
            foreach (Transform t in UI.PanelWeaponList.GetComponentsInChildren<Transform>())
            {
                if (t.GetComponent<bl_ClassInfoUI>() != null)
                {
                    Destroy(t.gameObject);
                }
            }
        }

        private bool isAllowedWeapon(bl_GunInfo info, int slot)
        {
            ClassAllowedWeaponsType rules = PrimaryAllowedWeapons;
            if (slot == 1) { rules = SecondaryAllowedWeapons; }
            else if (slot == 2) { rules = KnifeAllowedWeapons; }
            else if (slot == 3) { rules = GrenadesAllowedWeapons; }

            if ((rules.AllowMachineGuns && info.Type == GunType.Machinegun) || (rules.AllowPistols && info.Type == GunType.Pistol) || (rules.AllowShotguns && info.Type == GunType.Shotgun)
                || (rules.AllowKnifes && info.Type == GunType.Knife) || (rules.AllowGrenades && info.Type == GunType.Grenade) || (rules.AllowSnipers && info.Type == GunType.Sniper))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowList(int slot)
        {
            CurrentSlot = slot;
            CleanList();

            switch (bl_ClassManager.Instance.m_Class)
            {
                case PlayerClass.Assault:
                    for (int i = 0; i < AssaultClass.Count; i++)
                    {
                        if (isAllowedWeapon(AssaultClass[i].Info, slot))
                        {
                            GameObject b = Instantiate(UI.GunSelectPrefabs) as GameObject;
                            bl_ClassInfoUI iui = b.GetComponent<bl_ClassInfoUI>();
                            iui.GetInfo(AssaultClass[i], slot, i);
                            b.transform.SetParent(UI.PanelWeaponList, false);
                        }
                    }
                    break;
                case PlayerClass.Engineer://----------------------------------------------------------------------------------------------------
                    for (int i = 0; i < EngineerClass.Count; i++)
                    {
                        if (isAllowedWeapon(EngineerClass[i].Info, slot))
                        {
                            GameObject b = Instantiate(UI.GunSelectPrefabs) as GameObject;
                            bl_ClassInfoUI iui = b.GetComponent<bl_ClassInfoUI>();
                            iui.GetInfo(EngineerClass[i], slot, i);
                            b.transform.SetParent(UI.PanelWeaponList, false);
                        }
                    }
                    break;
                case PlayerClass.Support://-----------------------------------------------------------------------------------------------------
                    for (int i = 0; i < SupportClass.Count; i++)
                    {
                        if (isAllowedWeapon(SupportClass[i].Info, slot))
                        {
                            GameObject b = Instantiate(UI.GunSelectPrefabs) as GameObject;
                            bl_ClassInfoUI iui = b.GetComponent<bl_ClassInfoUI>();
                            iui.GetInfo(SupportClass[i], slot, i);
                            b.transform.SetParent(UI.PanelWeaponList, false);
                        }
                    }
                    break;
                case PlayerClass.Recon://-----------------------------------------------------------------------------
                    for (int i = 0; i < ReconClass.Count; i++)
                    {
                        if (isAllowedWeapon(ReconClass[i].Info, slot))
                        {
                            GameObject b = Instantiate(UI.GunSelectPrefabs) as GameObject;
                            bl_ClassInfoUI iui = b.GetComponent<bl_ClassInfoUI>();
                            iui.GetInfo(ReconClass[i], slot, i);
                            b.transform.SetParent(UI.PanelWeaponList, false);
                        }
                    }
                    break;
                case PlayerClass.Grenadier://-----------------------------------------------------------------------------
                    for (int i = 0; i < GrenadierClass.Count; i++)
                    {
                        if (isAllowedWeapon(GrenadierClass[i].Info, slot))
                        {
                            GameObject b = Instantiate(UI.GunSelectPrefabs) as GameObject;
                            bl_ClassInfoUI iui = b.GetComponent<bl_ClassInfoUI>();
                            iui.GetInfo(GrenadierClass[i], slot, i);
                            b.transform.SetParent(UI.PanelWeaponList, false);
                        }
                    }
                    break;
            }
        }

        public void PreviewCurrentGun(int id)
        {
            currentGun.ID = id;
            WeaponPreview.Find(x => x.gameObject.name == currentGun.Info.Name).SetActive(true);
            foreach(GameObject weapon in WeaponPreview)
            {
                if (weapon.name != currentGun.Info.Name)
                    weapon.SetActive(false);
            }

            UI.WeaponPreviewHUD.WeaponNameText.text = currentGun.Info.Name;
            UI.WeaponPreviewHUD.Icon.sprite = currentGun.Info.GunIcon;
            UI.WeaponPreviewHUD.AccuracyValue.text = Mathf.Clamp((int)(currentGun.Info.Accuracy), 0, 100) + "";
            UI.WeaponPreviewHUD.DamageValue.text = currentGun.Info.Damage + "";
            UI.WeaponPreviewHUD.RateValue.text = Mathf.Clamp((int)(1/currentGun.Info.FireRate),0,100) + "";
            UI.WeaponPreviewHUD.RangeValue.text = currentGun.Info.Range/10 + "";
            UI.WeaponPreviewHUD.WeightValue.text = currentGun.Info.Weight + "";
        }

        public void ChangeKit(int kit)
        {
            bl_ClassManager.Instance.ClassKit = kit;
            PlayerPrefs.SetInt(ClassKey.ClassKit, kit);
        }

        /// <summary>
        /// 
        /// </summary>
        private float DSDC = 0;
        public void ChangeClass(PlayerClass newclass)
        {
            if (m_Class == newclass && bl_ClassManager.Instance.m_Class == newclass)
                return;

            m_Class = newclass;
            bl_ClassManager.Instance.m_Class = newclass;
            newclass.SavePlayerClass();
            UI.ClassText.text = (newclass.ToString() + " Class").ToUpper();
            ResetClassHUD();
            int i = 0;
            switch (newclass)
            {
                case PlayerClass.Assault:
                    i = 0;
                    Vector2 v = UI.AssaultButton.sizeDelta;
                    DSDC = v.y;
                    UI.EnginnerButton.sizeDelta = v;
                    UI.SupportButton.sizeDelta = v;
                    UI.ReconButton.sizeDelta = v;
                    UI.GrenadierButton.sizeDelta = v;
                    v.y = DSDC + 10;
                    UI.AssaultButton.sizeDelta = v;
                    UpdateClassUI(GetListId(PlayerClass.Assault, ClassManager.PrimaryAssault), 0);
                    UpdateClassUI(GetListId(PlayerClass.Assault, ClassManager.SecondaryAssault), 1);
                    UpdateClassUI(GetListId(PlayerClass.Assault, ClassManager.KnifeAssault), 2);
                    UpdateClassUI(GetListId(PlayerClass.Assault, ClassManager.GrenadeAssault), 3);
                    PreviewCurrentGun(ClassManager.PrimaryAssault);
                    break;
                case PlayerClass.Engineer:
                    i = 1;
                    Vector2 va = UI.EnginnerButton.sizeDelta;
                    DSDC = va.y;
                    UI.AssaultButton.sizeDelta = va;
                    UI.SupportButton.sizeDelta = va;
                    UI.ReconButton.sizeDelta = va;
                    UI.GrenadierButton.sizeDelta = va;
                    va.y = DSDC + 10;
                    UI.EnginnerButton.sizeDelta = va;
                    UpdateClassUI(GetListId(PlayerClass.Engineer, ClassManager.PrimaryEnginner), 0);
                    UpdateClassUI(GetListId(PlayerClass.Engineer, ClassManager.SecondaryEnginner), 1);
                    UpdateClassUI(GetListId(PlayerClass.Engineer, ClassManager.KnifeEnginner), 2);
                    UpdateClassUI(GetListId(PlayerClass.Engineer, ClassManager.GrenadeEnginner), 3);
                    PreviewCurrentGun(ClassManager.PrimaryEnginner);
                    break;
                case PlayerClass.Support:
                    i = 2;
                    Vector2 ve = UI.SupportButton.sizeDelta;
                    DSDC = ve.y;
                    UI.AssaultButton.sizeDelta = ve;
                    UI.EnginnerButton.sizeDelta = ve;
                    UI.ReconButton.sizeDelta = ve;
                    UI.GrenadierButton.sizeDelta = ve;
                    ve.y = DSDC + 10;
                    UI.SupportButton.sizeDelta = ve;
                    UpdateClassUI(GetListId(PlayerClass.Support, ClassManager.PrimarySupport), 0);
                    UpdateClassUI(GetListId(PlayerClass.Support, ClassManager.SecondarySupport), 1);
                    UpdateClassUI(GetListId(PlayerClass.Support, ClassManager.KnifeSupport), 2);
                    UpdateClassUI(GetListId(PlayerClass.Support, ClassManager.GrenadeSupport), 3);
                    PreviewCurrentGun(ClassManager.PrimarySupport);
                    break;
                case PlayerClass.Recon:
                    i = 3;
                    Vector2 vr = UI.ReconButton.sizeDelta;
                    DSDC = vr.y;
                    UI.AssaultButton.sizeDelta = vr;
                    UI.EnginnerButton.sizeDelta = vr;
                    UI.SupportButton.sizeDelta = vr;
                    UI.GrenadierButton.sizeDelta = vr;
                    vr.y = DSDC + 10;
                    UI.ReconButton.sizeDelta = vr;
                    UpdateClassUI(GetListId(PlayerClass.Recon, ClassManager.PrimaryRecon), 0);
                    UpdateClassUI(GetListId(PlayerClass.Recon, ClassManager.SecondaryRecon), 1);
                    UpdateClassUI(GetListId(PlayerClass.Recon, ClassManager.KnifeRecon), 2);
                    UpdateClassUI(GetListId(PlayerClass.Recon, ClassManager.GrenadeRecon), 3);
                    PreviewCurrentGun(ClassManager.PrimaryRecon);
                    break;
                case PlayerClass.Grenadier:
                    i = 3;
                    Vector2 vg = UI.GrenadierButton.sizeDelta;
                    DSDC = vg.y;
                    UI.AssaultButton.sizeDelta = vg;
                    UI.EnginnerButton.sizeDelta = vg;
                    UI.SupportButton.sizeDelta = vg;
                    UI.ReconButton.sizeDelta = vg;
                    vg.y = DSDC + 10;
                    UI.GrenadierButton.sizeDelta = vg;
                    UpdateClassUI(GetListId(PlayerClass.Grenadier, ClassManager.PrimaryGrenadier), 0);
                    UpdateClassUI(GetListId(PlayerClass.Grenadier, ClassManager.SecondaryGrenadier), 1);
                    UpdateClassUI(GetListId(PlayerClass.Grenadier, ClassManager.KnifeGrenadier), 2);
                    UpdateClassUI(GetListId(PlayerClass.Grenadier, ClassManager.GrenadeGrenadier), 3);
                    PreviewCurrentGun(ClassManager.PrimaryGrenadier);
                    break;
            }

            PlayerPrefs.SetInt(ClassKey.ClassType, i);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clas"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private int GetListId(PlayerClass clas, int id)
        {
            switch (clas)
            {
                case PlayerClass.Assault:
                    for (int i = 0; i < AssaultClass.Count; i++)
                    {
                        if (AssaultClass[i].ID == id)
                        {
                            return i;
                        }
                    }
                    break;
                case PlayerClass.Engineer:
                    for (int i = 0; i < EngineerClass.Count; i++)
                    {
                        if (EngineerClass[i].ID == id)
                        {
                            return i;
                        }
                    }
                    break;
                case PlayerClass.Support:
                    for (int i = 0; i < SupportClass.Count; i++)
                    {
                        if (SupportClass[i].ID == id)
                        {
                            return i;
                        }
                    }
                    break;
                case PlayerClass.Recon:
                    for (int i = 0; i < ReconClass.Count; i++)
                    {
                        if (ReconClass[i].ID == id)
                        {
                            return i;
                        }
                    }
                    break;
                case PlayerClass.Grenadier:
                    for (int i = 0; i < GrenadierClass.Count; i++)
                    {
                        if (GrenadierClass[i].ID == id)
                        {
                            return i;
                        }
                    }
                    break;
            }

            return 0;
        }
        void ResetClassHUD()
        {
            UI.PrimaryHUD.WeaponNameText.text = "None";
            UI.PrimaryHUD.Icon.sprite = null;
            UI.PrimaryHUD.DamageValue.text = 0 + "";
            UI.PrimaryHUD.AccuracyValue.text = 0 + "";
            UI.PrimaryHUD.RateValue.text = 0 + "";
            UI.PrimaryHUD.RangeValue.text = 0 + "";
            UI.PrimaryHUD.WeightValue.text = 0 + "";
            //
            UI.SecondaryHUD.WeaponNameText.text = "None";
            UI.SecondaryHUD.Icon.sprite = null;
            UI.SecondaryHUD.DamageValue.text = 0 + "";
            UI.SecondaryHUD.AccuracyValue.text = 0 + "";
            UI.SecondaryHUD.RateValue.text = 0 + "";
            UI.SecondaryHUD.RangeValue.text = 0 + "";
            UI.SecondaryHUD.WeightValue.text = 0 + "";
            //
            UI.KnifeHUD.WeaponNameText.text = "None";
            UI.KnifeHUD.Icon.sprite = null;
            UI.KnifeHUD.DamageValue.text = 0 + "";
            UI.KnifeHUD.AccuracyValue.text = 0 + "";
            UI.KnifeHUD.RateValue.text = 0 + "";
            UI.KnifeHUD.RangeValue.text = 0 + "";
            UI.KnifeHUD.WeightValue.text = 0 + "";
            //
            UI.GrenadeHUD.WeaponNameText.text = "None";
            UI.GrenadeHUD.Icon.sprite = null;
            UI.GrenadeHUD.DamageValue.text = 0 + "";
            UI.GrenadeHUD.AccuracyValue.text = 0 + "";
            UI.GrenadeHUD.RateValue.text = 0 + "";
            UI.GrenadeHUD.RangeValue.text = 0 + "";
            UI.GrenadeHUD.WeightValue.text = 0 + "";
        }
        /// <summary>
        /// Take the current class
        /// </summary>
        void TakeCurrentClass(PlayerClass mclass)
        {
            switch (mclass)
            {
                case PlayerClass.Assault:
                    foreach (ClassInfo ci in AssaultClass)
                    {
                        if (ci.ID == ClassManager.PrimaryAssault)
                        {
                            UI.PrimaryHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.PrimaryHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.PrimaryHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.PrimaryHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.PrimaryHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.PrimaryHUD.RangeValue.text = ci.Info.Range + "";
                            UI.PrimaryHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in AssaultClass)
                    {
                        if (ci.ID == ClassManager.SecondaryAssault)
                        {
                            UI.SecondaryHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.SecondaryHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.SecondaryHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.SecondaryHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.SecondaryHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.SecondaryHUD.RangeValue.text = ci.Info.Range + "";
                            UI.SecondaryHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in AssaultClass)
                    {
                        if (ci.ID == ClassManager.KnifeAssault)
                        {
                            UI.KnifeHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.KnifeHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.KnifeHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.KnifeHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.KnifeHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.KnifeHUD.RangeValue.text = ci.Info.Range + "";
                            UI.KnifeHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in EngineerClass)
                    {
                        if (ci.ID == ClassManager.GrenadeAssault)
                        {
                            UI.GrenadeHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.GrenadeHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.GrenadeHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.GrenadeHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.GrenadeHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.GrenadeHUD.RangeValue.text = ci.Info.Range + "";
                            UI.GrenadeHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    break;
                case PlayerClass.Engineer://---------------------------------------------------------------------------------
                    foreach (ClassInfo ci in EngineerClass)
                    {
                        if (ci.ID == ClassManager.PrimaryEnginner)
                        {
                            UI.PrimaryHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.PrimaryHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.PrimaryHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.PrimaryHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.PrimaryHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.PrimaryHUD.RangeValue.text = ci.Info.Range + "";
                            UI.PrimaryHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in EngineerClass)
                    {
                        if (ci.ID == ClassManager.SecondaryEnginner)
                        {
                            UI.SecondaryHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.SecondaryHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.SecondaryHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.SecondaryHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.SecondaryHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.SecondaryHUD.RangeValue.text = ci.Info.Range + "";
                            UI.SecondaryHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in EngineerClass)
                    {
                        if (ci.ID == ClassManager.KnifeEnginner)
                        {
                            UI.KnifeHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.KnifeHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.KnifeHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.KnifeHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.KnifeHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.KnifeHUD.RangeValue.text = ci.Info.Range + "";
                            UI.KnifeHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in EngineerClass)
                    {
                        if (ci.ID == ClassManager.GrenadeEnginner)
                        {
                            UI.GrenadeHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.GrenadeHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.GrenadeHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.GrenadeHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.GrenadeHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.GrenadeHUD.RangeValue.text = ci.Info.Range + "";
                            UI.GrenadeHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    break;
                case PlayerClass.Recon://-------------------------------------------------------------------------------------
                    foreach (ClassInfo ci in ReconClass)
                    {
                        if (ci.ID == ClassManager.PrimaryRecon)
                        {
                            UI.PrimaryHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.PrimaryHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.PrimaryHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.PrimaryHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.PrimaryHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.PrimaryHUD.RangeValue.text = ci.Info.Range + "";
                            UI.PrimaryHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in ReconClass)
                    {
                        if (ci.ID == ClassManager.SecondaryRecon)
                        {
                            UI.SecondaryHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.SecondaryHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.SecondaryHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.SecondaryHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.SecondaryHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.SecondaryHUD.RangeValue.text = ci.Info.Range + "";
                            UI.SecondaryHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in ReconClass)
                    {
                        if (ci.ID == ClassManager.KnifeRecon)
                        {
                            UI.KnifeHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.KnifeHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.KnifeHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.KnifeHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.KnifeHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.KnifeHUD.RangeValue.text = ci.Info.Range + "";
                            UI.KnifeHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in ReconClass)
                    {
                        if (ci.ID == ClassManager.GrenadeRecon)
                        {
                            UI.GrenadeHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.GrenadeHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.GrenadeHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.GrenadeHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.GrenadeHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.GrenadeHUD.RangeValue.text = ci.Info.Range + "";
                            UI.GrenadeHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    break;
                case PlayerClass.Support://--------------------------------------------------------------------------------------
                    foreach (ClassInfo ci in SupportClass)
                    {
                        if (ci.ID == ClassManager.PrimarySupport)
                        {
                            UI.PrimaryHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.PrimaryHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.PrimaryHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.PrimaryHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.PrimaryHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.PrimaryHUD.RangeValue.text = ci.Info.Range + "";
                            UI.PrimaryHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in SupportClass)
                    {
                        if (ci.ID == ClassManager.SecondarySupport)
                        {
                            UI.SecondaryHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.SecondaryHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.SecondaryHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.SecondaryHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.SecondaryHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.SecondaryHUD.RangeValue.text = ci.Info.Range + "";
                            UI.SecondaryHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in SupportClass)
                    {
                        if (ci.ID == ClassManager.KnifeSupport)
                        {
                            UI.KnifeHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.KnifeHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.KnifeHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.KnifeHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.KnifeHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.KnifeHUD.RangeValue.text = ci.Info.Range + "";
                            UI.KnifeHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in SupportClass)
                    {
                        if (ci.ID == ClassManager.GrenadeSupport)
                        {
                            UI.GrenadeHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.GrenadeHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.GrenadeHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.GrenadeHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.GrenadeHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.GrenadeHUD.RangeValue.text = ci.Info.Range + "";
                            UI.GrenadeHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    break;
                case PlayerClass.Grenadier://-------------------------------------------------------------------------------------
                    foreach (ClassInfo ci in GrenadierClass)
                    {
                        if (ci.ID == ClassManager.PrimaryGrenadier)
                        {
                            UI.PrimaryHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.PrimaryHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.PrimaryHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.PrimaryHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.PrimaryHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.PrimaryHUD.RangeValue.text = ci.Info.Range + "";
                            UI.PrimaryHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in GrenadierClass)
                    {
                        if (ci.ID == ClassManager.SecondaryGrenadier)
                        {
                            UI.SecondaryHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.SecondaryHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.SecondaryHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.SecondaryHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.SecondaryHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.SecondaryHUD.RangeValue.text = ci.Info.Range + "";
                            UI.SecondaryHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in GrenadierClass)
                    {
                        if (ci.ID == ClassManager.KnifeGrenadier)
                        {
                            UI.KnifeHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.KnifeHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.KnifeHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.KnifeHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.KnifeHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.KnifeHUD.RangeValue.text = ci.Info.Range + "";
                            UI.KnifeHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    foreach (ClassInfo ci in GrenadierClass)
                    {
                        if (ci.ID == ClassManager.GrenadeGrenadier)
                        {
                            UI.GrenadeHUD.WeaponNameText.text = ci.Info.Name.ToUpper();
                            UI.GrenadeHUD.Icon.sprite = ci.Info.GunIcon;
                            UI.GrenadeHUD.DamageValue.text = ci.Info.Damage + "";
                            UI.GrenadeHUD.AccuracyValue.text = ci.Info.Accuracy + "";
                            UI.GrenadeHUD.RateValue.text = ci.Info.FireRate + "";
                            UI.GrenadeHUD.RangeValue.text = ci.Info.Range + "";
                            UI.GrenadeHUD.WeightValue.text = ci.Info.Weight + "";
                        }
                    }
                    break;
            }
        }

    }
}

