using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool isAiming = false;


        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;


        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }


        public void LookRotation(Transform character, Transform camera)
        {

            float yRot;
            float xRot;

            var m_sensitive = PlayerPrefs.GetFloat(PropertiesKeys.SensitivityAim, bl_GameData.Instance.DefaultSettings.DefaultSensitivityAim);
            var AimSensitivity = PlayerPrefs.GetFloat(PropertiesKeys.Sensitivity, bl_GameData.Instance.DefaultSettings.DefaultSensitivity);

            if (!bl_Input.Instance.isGamePad)
            {
                if (isAiming)
                {
                    yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity * m_sensitive;
                    xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity * m_sensitive;
                }
                else
                {
                    yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity * AimSensitivity;
                    xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity * AimSensitivity;
                }
            }
            else
            {
                if (isAiming)
                {
                    yRot = CrossPlatformInputManager.GetAxis("Controller X") * XSensitivity * m_sensitive * Time.deltaTime * 10;
                    xRot = CrossPlatformInputManager.GetAxis("Controller Y") * YSensitivity * m_sensitive * Time.deltaTime * 10;
                }
                else
                {
                    yRot = CrossPlatformInputManager.GetAxis("Controller X") * XSensitivity * AimSensitivity * Time.deltaTime * 100;
                    xRot = CrossPlatformInputManager.GetAxis("Controller Y") * YSensitivity * AimSensitivity * Time.deltaTime * 100;
                }
            }

            m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

            if(clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

            if(smooth)
            {
                character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
        }


        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }
}
