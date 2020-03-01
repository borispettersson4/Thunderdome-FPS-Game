using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bl_KillFeedUI : MonoBehaviour
{
    [SerializeField]private Text KillerText;
    [SerializeField]private Text KilledText;
    [SerializeField]private Text WeaponText;
    [SerializeField]private Image KillTypeImage;
    public Color WeaponColor = Color.red;
    public Color GrenadeColor = Color.yellow;
    private CanvasGroup Alpha;

    public void Init(KillFeed feed)
    {
        //if (bl_GameData.Instance.GetWeaponType(feed.HowKill) == GunType.Grenade)
            //WeaponText.color = GrenadeColor;

        KillerText.text = feed.Killer;
        KillerText.color = feed.KillerColor;
        KilledText.color = feed.KilledColor;
        KilledText.text = feed.Killed;
        if (bl_GameData.Instance.isWeapon(feed.HowKill))
        {
            WeaponText.text = string.Format("[{0}]", feed.HowKill);
        }
        else
        {
            WeaponText.text = string.Format("{0}", feed.HowKill);
        }
        KillTypeImage.gameObject.SetActive(feed.HeadShot);
        Alpha = GetComponent<CanvasGroup>();
        StartCoroutine(Hide(10));
    }

    IEnumerator Hide(float time)
    {
        yield return new WaitForSeconds(time);
        while(Alpha.alpha > 0)
        {
            Alpha.alpha -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

}