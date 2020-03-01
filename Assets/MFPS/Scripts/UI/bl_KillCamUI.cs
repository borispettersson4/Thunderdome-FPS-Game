using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bl_KillCamUI : MonoBehaviour
{

    [SerializeField] private Text KillerNameText;
    [SerializeField] private Text KillerHealthText;
    [SerializeField] private Text GunNameText;
    [SerializeField] private Image GunImage;
    [SerializeField] private Sprite GenericDeathImage;
    [SerializeField] private Image Emblem;
    public Text KillCamSpectatingText;

    public void Show(string killer, int gunID)
    {
        if(gunID > 100)
        {
            GunImage.sprite = GenericDeathImage;
            GunNameText.text = bl_GameTexts.DeathByFall.ToUpper();
            KillerNameText.text = killer;
        }
        else
        {
            bl_GunInfo info = bl_GameData.Instance.GetWeapon(gunID);
            GunImage.sprite = info.GunIcon;
            GunNameText.text = info.Name.ToUpper();
            KillerNameText.text = killer;
        }

#if LOCALIZATION
        KillCamSpectatingText.text = string.Format("<size=8>{0}:</size>\n{1}", bl_Localization.Instance.GetText(26).ToUpper(), killer);
#else
        KillCamSpectatingText.text = string.Format("<size=8>{0}:</size>\n{1}", bl_GameTexts.Spectating.ToUpper(), killer);
#endif
        bl_GameManager.SceneActors actor = bl_GameManager.Instance.FindActor(killer);
        if(actor != null)
        {
            if (actor.isRealPlayer)
            {
                bl_PlayerDamageManager pdm = actor.Actor.GetComponent<bl_PlayerDamageManager>();
                int health = Mathf.FloorToInt(pdm.health);
                if (pdm != null) { KillerHealthText.text = string.Format("+{0}", health); }

                Player cachePlayer = null;
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    if (player.NickName == killer)
                    {
                        cachePlayer = player;
                    }
                }

                Emblem.sprite = bl_LevelManager.Instance.GetPlayerLevelInfo(cachePlayer).Icon;
            }
            else
            {
                bl_AIShooterHealth pdm = actor.Actor.GetComponent<bl_AIShooterHealth>();
                int health = Mathf.FloorToInt(pdm.Health);
                if (pdm != null) { KillerHealthText.text = string.Format("+{0}", health); }
                Emblem.sprite = bl_UIReferences.Instance.BotLevelIcon;
            }
        }
        else
        {
            KillerHealthText.text = string.Empty;
        }
    }
}