using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace MFPS.ClassCustomization
{
    public class bl_ClassInfoUI : MonoBehaviour
    {

        public Image Icon;
        public Text NameText;
        private int ID;
        [SerializeField] private GameObject LevelLock;
        [HideInInspector] public int ClassId = 0;
        [HideInInspector] public int ListID = 0;
        private CanvasGroup Alpha;

        /// <summary>
        /// 
        /// </summary>
        public void GetInfo(ClassInfo info, int slot, int lID)
        {
            Icon.sprite = info.Info.GunIcon;
            NameText.text = info.Info.Name.ToUpper();
            ID = info.ID;
            ClassId = slot;
            ListID = lID;
            int lockedStatus = 0;
#if SHOP && ULSP
            if (info.Info.Price > 0 && bl_DataBase.Instance != null)
            {
                int gunID = bl_GameData.Instance.GetWeaponID(info.Info.Name);
                if (bl_DataBase.Instance.LocalUser.ShopData.isItemPurchase(ShopItemType.Weapon, gunID))
                {
                    lockedStatus = 2;
                }
                else
                {
                    lockedStatus = 1;
                    LevelLock.SetActive(true);
                    LevelLock.GetComponentInChildren<Text>().text = "PRICE: $" + info.Info.Price;
                    GetComponent<Button>().interactable = false;
                }
            }
#else
            lockedStatus = 0;
#endif
#if LM
            if (bl_GameData.Instance.LockWeaponsByLevel && lockedStatus == 0)
            {
                int al = bl_LevelManager.Instance.GetLevelID();
                bool UnLock = (al >= info.Info.LevelUnlock);
                LevelLock.SetActive(!UnLock);
                if (!UnLock)
                {
                    LevelLock.GetComponentInChildren<Text>().text = "REQUIRE LEVEL: " + info.Info.LevelUnlock;
                    GetComponent<Button>().interactable = false;
                }

            }
#endif

            StartCoroutine(Fade(lID * 0.04f));
        }

        /// <summary>
        /// 
        /// </summary>
        public void ChangeSlot()
        {
            bl_ClassCustomize c = FindObjectOfType<bl_ClassCustomize>();
            c.ChangeSlotClass(ID, ClassId, ListID);
        }

        IEnumerator Fade(float wait)
        {
            Alpha = GetComponent<CanvasGroup>();
            Alpha.alpha = 0;
            yield return new WaitForSeconds(wait);
            float d = 0;
            while (d < 1)
            {
                d += Time.deltaTime * 2;
                Alpha.alpha = d;
                yield return null;
            }
        }
    }
}