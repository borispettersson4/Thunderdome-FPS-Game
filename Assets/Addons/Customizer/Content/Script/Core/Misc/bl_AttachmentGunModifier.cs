using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bl_AttachmentGunModifier : MonoBehaviour
{
    [Header("Sight")]
    public bool OverrideAimPosition = false;
    public Vector3 AimPosition;
    [Header("Barrel")]
    public bool OverrdieFireSound = false;
    public AudioClip FireSound;
    [Header("Magazine")]
    public bool AddBullets = false;
    public int ExtraBullets = 0;

    [SerializeField] private bl_Gun m_Gun;

    void OnEnable()
    {
        if (m_Gun == null)
            return;

        if(OverrideAimPosition)
        m_Gun.AimPosition = AimPosition;

        if (OverrdieFireSound)
            m_Gun.FireSound = FireSound;

        if (AddBullets)
            m_Gun.bulletsPerClip += ExtraBullets;
    }
}