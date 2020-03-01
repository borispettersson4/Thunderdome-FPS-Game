using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bl_ClassUIButton : MonoBehaviour
{
    public int classNumber;

    public Image PrimaryWeapon;
    public Text PrimaryName;
    public Image SecondaryWeapon;
    public Text SecondaryName;
    public Image Grenade;
    public Text GrenadeName;
    public Image MeleeWeapon;
    public Text MeleeName;

    public void Awake()
    {
        switch (classNumber)
        {
            case 0:
                PrimaryWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.PrimaryAssault).GunIcon;
                PrimaryName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.PrimaryAssault).Name;
                SecondaryWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.SecondaryAssault).GunIcon;
                SecondaryName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.SecondaryAssault).Name;
                Grenade.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.GrenadeAssault).GunIcon;
                GrenadeName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.GrenadeAssault).Name;
                MeleeWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.KnifeAssault).GunIcon;
                MeleeName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.KnifeAssault).Name;
                break;
            case 1:
                PrimaryWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.PrimaryRecon).GunIcon;
                PrimaryName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.PrimaryRecon).Name;
                SecondaryWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.SecondaryRecon).GunIcon;
                SecondaryName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.SecondaryRecon).Name;
                Grenade.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.GrenadeRecon).GunIcon;
                GrenadeName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.GrenadeRecon).Name;
                MeleeWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.KnifeRecon).GunIcon;
                MeleeName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.KnifeRecon).Name;
                break;
            case 2:
                PrimaryWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.PrimaryEnginner).GunIcon;
                PrimaryName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.PrimaryEnginner).Name;
                SecondaryWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.SecondaryEnginner).GunIcon;
                SecondaryName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.SecondaryEnginner).Name;
                Grenade.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.GrenadeEnginner).GunIcon;
                GrenadeName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.GrenadeEnginner).Name;
                MeleeWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.KnifeEnginner).GunIcon;
                MeleeName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.KnifeEnginner).Name;
                break;
            case 3:
                PrimaryWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.PrimarySupport).GunIcon;
                PrimaryName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.PrimarySupport).Name;
                SecondaryWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.SecondarySupport).GunIcon;
                SecondaryName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.SecondarySupport).Name;
                Grenade.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.GrenadeSupport).GunIcon;
                GrenadeName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.GrenadeSupport).Name;
                MeleeWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.KnifeSupport).GunIcon;
                MeleeName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.KnifeSupport).Name;
                break;
            case 4:
                PrimaryWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.PrimaryGrenadier).GunIcon;
                PrimaryName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.PrimaryGrenadier).Name;
                SecondaryWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.SecondaryGrenadier).GunIcon;
                SecondaryName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.SecondaryGrenadier).Name;
                Grenade.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.GrenadeGrenadier).GunIcon;
                GrenadeName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.GrenadeGrenadier).Name;
                MeleeWeapon.sprite = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.KnifeGrenadier).GunIcon;
                MeleeName.text = bl_GameData.Instance.GetWeapon(bl_ClassManager.Instance.KnifeGrenadier).Name;
                break;
        }

    }

    public void Update()
    {
       // if(bl_ClassManager.Instance.ClassKit == classNumber)
         //   GetComponent<Image>().color = GetComponent<Button>().colors.disabledColor;
        //else
          //  GetComponent<Image>().color = GetComponent<Button>().colors.normalColor;
    }

}
