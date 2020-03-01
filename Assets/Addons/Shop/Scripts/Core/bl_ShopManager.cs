using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MFPS.Shop;
using UnityEngine.Networking;
#if PSELECTOR
using MFPS.PlayerSelector;
#endif

public class bl_ShopManager : MonoBehaviour
{

    [Header("References")]
    public GameObject ContentUI;
    public GameObject BuyWindow;
    public GameObject BuyCoinsWindow;
    public GameObject ItemPrefab;
    public GameObject NoCoinsWindow;
    public GameObject LoadingUI;
    public GameObject PurchaseAnimationUI;
    public GameObject BuyPreviewButton;
    public Transform ListPanel;
    public Image[] PreviewIcons;
    public Image BuyIconImage;
    public Image ConfirmationIcon;
    public Text BuyNameText;
    public Text BuyPriceText;
    public Text PreviewNameText;
    public Text PreviewPriceText;
    public Image[] PreviewBars;
    public AnimationCurve ScaleCurve;

    public List<bl_ShopItemData> Items = new List<bl_ShopItemData>();
    List<bl_GunInfo> Weapons = new List<bl_GunInfo>();

    private bl_ShopItemData previewData = null;
    private bl_ShopItemData infoPreviewData = null;
    private int CurrentPrice = 0;
    private bool bussy = false;
    private RectTransform buyRect;
    private Vector3 defaultPosition;
    private List<GameObject> cacheUI = new List<GameObject>();
    [HideInInspector] public bl_ShopData.ShopVirtualCoins coinPack;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        buyRect = BuyWindow.GetComponent<RectTransform>();
        defaultPosition = buyRect.position;
        if (!bl_GameData.isDataCached)
        {
            while (!bl_GameData.isDataCached) { yield return null; }
        }
        if (Items == null || Items.Count <= 0) { BuildData(); }
        InstanceItems();
    }

    /// <summary>
    /// 
    /// </summary>
    void BuildData()
    {
        Items.Clear();
        Weapons = bl_GameData.Instance.AllWeapons;
        for (int i = 0; i < Weapons.Count; i++)
        {
            bl_ShopItemData data = new bl_ShopItemData();
            data.ID = i;
            data.Name = Weapons[i].Name;
            data.Type = ShopItemType.Weapon;
            data.GunInfo = Weapons[i];
#if SHOP
            data.Price = Weapons[i].Price;
#endif
            Items.Add(data);
        }

#if PSELECTOR
        for (int i = 0; i < bl_PlayerSelectorData.Instance.AllPlayers.Count; i++)
        {
            bl_PlayerSelectorInfo pinfo = bl_PlayerSelectorData.Instance.AllPlayers[i];
            bl_ShopItemData data = new bl_ShopItemData();
            data.ID = i;
            data.Name = pinfo.Name;
            data.Type = ShopItemType.PlayerSkin;
            data.PlayerSkinInfo = pinfo;
            data.Price = pinfo.Price;
            Items.Add(data);
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    public void InstanceItems()
    {
        CleanPanel();
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Price <= 0 && !bl_ShopData.Instance.ShowFreeItems) continue;

            GameObject g = Instantiate(ItemPrefab) as GameObject;
            g.GetComponent<bl_ShopItemUI>().Set(Items[i]);
            g.transform.SetParent(ListPanel, false);
            if (i == 0)
            {
                Preview(Items[i], false);
            }
            cacheUI.Add(g);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowInventory()
    {
        CleanPanel();
        for (int i = 0; i < Items.Count; i++)
        {
#if ULSP && SHOP
            if (bl_DataBase.Instance != null)
            {
                if (Items[i].Price > 0 && !bl_DataBase.Instance.LocalUser.ShopData.isItemPurchase(Items[i])) continue;
            }
#endif
            GameObject g = Instantiate(ItemPrefab) as GameObject;
            g.GetComponent<bl_ShopItemUI>().Set(Items[i]);
            g.transform.SetParent(ListPanel, false);
            if (i == 0)
            {
                Preview(Items[i], false);
            }
            cacheUI.Add(g);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void PreviewItem(bl_ShopItemData info, Vector3 origin)
    {
        if (info.Type == ShopItemType.Weapon)
        {
            BuyIconImage.sprite = info.GunInfo.GunIcon;
        }
        else if (info.Type == ShopItemType.PlayerSkin)
        {
#if PSELECTOR
            BuyIconImage.sprite = info.PlayerSkinInfo.Preview;
#endif
        }
        BuyNameText.text = info.Name.ToUpper();
        BuyPriceText.text = string.Format("${0}", info.Price);
        previewData = info;
        CurrentPrice = info.Price;
        BuyWindow.SetActive(false);
        BuyWindow.SetActive(true);
        StopCoroutine("OpenBuyWindow");
        StartCoroutine("OpenBuyWindow", origin);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowCurrentPreview()
    {
        if (infoPreviewData == null) return;
        PreviewItem(infoPreviewData, BuyPreviewButton.GetComponent<RectTransform>().position);
    }

    /// <summary>
    /// 
    /// </summary>
    public void BuyCurrent()
    {
        if (previewData == null || bussy) return;
#if ULSP
        if(previewData.Price > bl_DataBase.Instance.LocalUser.Coins) { NoCoinsWindow.SetActive(true); return; }
#endif
        StartCoroutine(ProcessBuy());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator ProcessBuy()
    {
#if ULSP && SHOP
        bussy = true;
        LoadingUI.SetActive(true);
        WWWForm wf = new WWWForm();
        string hash = bl_DataBase.Instance.GetUserTokenComplete();
        wf.AddField("hash", hash);
        wf.AddField("id", bl_DataBase.Instance.LocalUser.ID);
        wf.AddField("name", bl_DataBase.Instance.LocalUser.LoginName);
        wf.AddField("coins", CurrentPrice);
        //temp add the purchase
        List<bl_ShopPurchase> plist = bl_DataBase.Instance.LocalUser.ShopData.ShopPurchases;
        bl_ShopPurchase sp = new bl_ShopPurchase();
        sp.ID = previewData.ID;
        sp.TypeID = (int)previewData.Type;
        plist.Add(sp);
        wf.AddField("line", bl_ShopData.CompilePurchases(plist));

        using (UnityWebRequest w = UnityWebRequest.Post(bl_LoginProDataBase.Instance.GetUrl(bl_LoginProDataBase.URLType.Shop), wf))
        {
            yield return w.SendWebRequest();

            if (!w.isHttpError && !w.isNetworkError)
            {
                string result = w.downloadHandler.text;
                if (result.Contains("done"))
                {
                    bl_DataBase.Instance.LocalUser.ShopData.ShopPurchases = plist;
                    bl_DataBase.Instance.LocalUser.Coins -= CurrentPrice;
                    FindObjectOfType<bl_Lobby>().UpdateCoinsText();
                    //update all UI and Inventory
                    InstanceItems();
                    ConfirmationIcon.sprite = BuyIconImage.sprite;
                    PurchaseAnimationUI.SetActive(true);
                    Debug.Log("Purchase successfully");
                }
                else
                {
                    Debug.LogWarning(result);
                }
            }
            else
            {
                Debug.LogError(w.error);
            }
        }
        LoadingUI.SetActive(false);
        bussy = false;
#else
        yield break;
#endif
    }

    public void BuyCoinPack(int id)
    {
        coinPack = bl_ShopData.Instance.CoinsPacks[id];

        //add your payment system process here
        //use the 'coinPack' info like
        // coinPack.Price
        // coinPack.Name

        switch (bl_ShopData.Instance.ShopPayment)
        {
            case bl_ShopData.ShopPaymentTypes.UnityIAP:
                //Unity IAP integration here
                //check this: https://docs.unity3d.com/Manual/UnityIAP.html
                break;
            case bl_ShopData.ShopPaymentTypes.Paypal:
                //Paypal SDK integration here
                //this FREE asset will save you a lot of work: https://assetstore.unity.com/packages/tools/integration/simple-paypal-storefront-52468
                break;
            case bl_ShopData.ShopPaymentTypes.Steam:
                //Steam IAP integration here
                //check this: https://partner.steamgames.com/doc/features/microtransactions
                break;
            case bl_ShopData.ShopPaymentTypes.Other:
                //Your own payment API or wherever

                break;
            default:
                Debug.LogWarning("Payment not defined");
                break;
        }

        //once the payment is confirmed add the new coins to the player data using:
        //bl_DataBase.Instance.SaveNewCoins(coinPack.Amount);
        //that's

        BuyCoinsWindow.SetActive(false);
    }

    IEnumerator OpenBuyWindow(Vector3 origin)
    {
        buyRect.localScale = Vector3.zero;
        buyRect.position = origin;
        float d = 0;
        float t = 0;
        while (d < 1)
        {
            d += Time.deltaTime * 5;
            t = ScaleCurve.Evaluate(d);
            buyRect.localScale = Vector3.one * t;
            buyRect.position = Vector3.Lerp(origin, defaultPosition, d);
            yield return null;
        }
    }

    public void Preview(bl_ShopItemData info, bool isOwned)
    {
        foreach (Image i in PreviewIcons) { i.gameObject.SetActive(false); }
        infoPreviewData = info;
        PreviewNameText.text = info.Name.ToUpper();
        PreviewPriceText.text = string.Format("PRICE ${0}", info.Price);
        if (info.Type == ShopItemType.Weapon)
        {
            PreviewIcons[0].gameObject.SetActive(true);
            PreviewIcons[0].sprite = info.GunInfo.GunIcon;
            PreviewBars[0].transform.parent.parent.GetComponentInChildren<Text>().text = "DAMAGE:";
            PreviewBars[0].fillAmount = (float)info.GunInfo.Damage / 100f;
            PreviewBars[1].transform.parent.parent.GetComponentInChildren<Text>().text = "FIRE RATE:";
            PreviewBars[1].fillAmount = info.GunInfo.FireRate / 1f;
            PreviewBars[2].transform.parent.parent.GetComponentInChildren<Text>().text = "ACCURACY:";
            PreviewBars[2].fillAmount = (float)info.GunInfo.Accuracy / 5f;
            PreviewBars[3].transform.parent.parent.GetComponentInChildren<Text>().text = "WEIGHT:";
            PreviewBars[3].fillAmount = (float)info.GunInfo.Weight / 4f;
        }
        else if (info.Type == ShopItemType.PlayerSkin)
        {
#if PSELECTOR
            PreviewIcons[1].gameObject.SetActive(true);
            PreviewIcons[1].sprite = info.PlayerSkinInfo.Preview;
            bl_PlayerDamageManager pdm = info.PlayerSkinInfo.Prefab.GetComponent<bl_PlayerDamageManager>();
            bl_FirstPersonController fpc = info.PlayerSkinInfo.Prefab.GetComponent<bl_FirstPersonController>();
            PreviewBars[0].transform.parent.parent.GetComponentInChildren<Text>().text = "HEALTH:";
            PreviewBars[0].fillAmount = pdm.health / 125;
            PreviewBars[1].transform.parent.parent.GetComponentInChildren<Text>().text = "SPEED:";
            PreviewBars[1].fillAmount = fpc.WalkSpeed / 5;
            PreviewBars[2].transform.parent.parent.GetComponentInChildren<Text>().text = "REGENERATION:";
            PreviewBars[2].fillAmount = pdm.RegenerationSpeed / 5;
            PreviewBars[3].transform.parent.parent.GetComponentInChildren<Text>().text = "NOISE:";
            PreviewBars[3].fillAmount = fpc.FootStepVolume / 1;
#endif
        }
        BuyPreviewButton.SetActive(!isOwned);
    }

    public void CleanPanel()
    {
        for (int i = 0; i < cacheUI.Count; i++)
        {
            if (cacheUI[i] == null) continue;
            Destroy(cacheUI[i]);
        }
        cacheUI.Clear();
    }

    public void OpenBuyCoinsWindow()
    {
        BuyCoinsWindow.SetActive(true);
    }

    private static bl_ShopManager _instance = null;
    public static bl_ShopManager Instance
    {
        get
        {
            if (_instance == null) { _instance = FindObjectOfType<bl_ShopManager>(); }
            return _instance;
        }
    }
}