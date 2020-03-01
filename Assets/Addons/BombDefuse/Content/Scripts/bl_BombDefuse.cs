using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class bl_BombDefuse : bl_MonoBehaviour
{
    [HideInInspector]public int ID;
    [Header("Settings")]
    public Team BombTeam = Team.All;
    public float TimeToExplote = 20f;
    public float TimeToActive = 7;
    public float TimeForNextRound = 5;
    public int XPForExplote = 200;
    [HideInInspector]
    public string ActorPlant = "";
    [Header("Effects")]
    public GameObject BombObject;
    public GameObject ExplosionEffect;
    public AudioClip BipSound;
    public AudioClip ExplosionSound;
    public AudioSource Source;
    public Light RedLight;
    public AnimationCurve LightCurve;


    [Header("UI")]
    public GUISkin Skin;
    public Texture2D BackBar;
    public Texture2D BarTexture;
    [SerializeField] private Renderer ColorRender;
    public GameObject BombRoot;
#if BDGM
    private bl_RoomSettings SettingsPropierties;
    private bl_GameManager Manager;
#endif

    private bool isBussy = false;
    private float TimeRemaing;
    private float TimeRemaingDesactive;
    private float countdown;
    private bool CanPlant = false;
    private bool CanDesactive = false;
    private float BarProgress;
    private float BarProgressDesactive;
    private bool Active = false;
    private bool BombDone = false;
    private bool RoundFinish = false;
    string lastWinner = "";
    private string LastPlayerIn;
    private bool CanUseThis = true;
    private bl_BombDefuse[] AllBombs;
    private bool NetStart = false;
    public int MaxBDRounds;

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        if (!PhotonNetwork.IsConnected || PhotonNetwork.CurrentRoom == null)
            return;
#if BDGM
        if (GetGameMode != GameMode.SND)
            return;

        SettingsPropierties = FindObjectOfType<bl_RoomSettings>();
        Manager = SettingsPropierties.GetComponent<bl_GameManager>();
        MaxBDRounds = (int)PhotonNetwork.CurrentRoom.CustomProperties[PropertiesKeys.BombDefuseRounds];
#endif
        GetBombs();
        TimeRemaing = TimeToActive;
        BarProgress = TimeToActive;
        BarProgressDesactive = TimeToActive;
        countdown = TimeToExplote;
        Source.clip = BipSound;
        if(ColorRender != null)
        {
            ColorRender.material.SetColor("_Color", (BombTeam == Team.Delta) ? bl_GameData.Instance.Team1Color : bl_GameData.Instance.Team2Color);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        bl_EventHandler.OnLocalPlayerDeath += OnLocalDeath;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        bl_EventHandler.OnLocalPlayerDeath -= OnLocalDeath;
    }

    void OnLocalDeath()
    {
        if(ActorPlant == PhotonNetwork.NickName)
        {
            CanPlant = false;
            CanDesactive = false;
            CancelInvoke();
            photonView.RPC("StartPlantBomb", RpcTarget.AllBuffered, false);
        }
    }

    void GetBombs()
    {
        AllBombs = FindObjectsOfType<bl_BombDefuse>();
        for(int i = 0; i < AllBombs.Length; i++)
        {
            AllBombs[i].ID = i;
        }
    }

#region Triggers
    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    void OnTriggerEnter(Collider c)
    {
        if (isBussy)
            return;
        if (!CanUseThis)
            return;

        if (c.transform.tag == bl_PlayerSettings.LocalTag)
        {
            PhotonView v = c.gameObject.GetPhotonView();
            if (v == null)
            {
                Debug.LogWarning("This object doesn't have a photonview");
            }
            if (!Active)
            {
                if (v.Owner.GetPlayerTeam() != BombTeam)
                {
                    CanPlant = true;
                }
            }
            else
            {
                if (v.Owner.GetPlayerTeam() == BombTeam)
                {
                    CanDesactive = true;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    void OnTriggerExit(Collider c)
    {
        if (c.transform.tag == bl_PlayerSettings.LocalTag)
        {
            if (c.gameObject.name == ActorPlant)
            {
                photonView.RPC("StartPlantBomb", RpcTarget.AllBuffered, false);

            }
            CanPlant = false;
            CanDesactive = false;
        }
    }
#endregion

    /// <summary>
    /// 
    /// </summary>
    void CountDown()
    {
        TimeRemaing -= 1;
        if (TimeRemaing <= 0)
        {
            CancelInvoke("CountDown");
            TimeRemaing = 0;
            photonView.RPC("BombActive", RpcTarget.AllBuffered, true,ID);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    void CountDownDesactive()
    {
        TimeRemaingDesactive -= 1;
        if (TimeRemaingDesactive <= 0)
        {
            CancelInvoke("CountDownDesactive");
            TimeRemaingDesactive = 0;
            photonView.RPC("DesactiveBomb", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnUpdate()
    {
        if (BombDone)
            return;
        if (!CanUseThis)
        {
            CanPlant = false;
            Active = false;
            return;
        }

        if (CanPlant)
        {
            PlantLogic();
        }
        if (CanDesactive)
        {
            DesactiveLogic();
        }
        if (Active)
        {
            ActiveCountDown();
        }
        LightControll();
    }

    float lightAlpha;
    float curveTime;
    void LightControll()
    {
        if (Active)
        {
            curveTime += Time.deltaTime * 1.7f;
            lightAlpha = LightCurve.Evaluate(curveTime);
        }
        else
        {
            lightAlpha = Mathf.Lerp(lightAlpha, 0, Time.deltaTime * 2);
        }
        RedLight.intensity = lightAlpha;
    }

    /// <summary>
    /// 
    /// </summary>
    private bool StartDesactive;
    void DesactiveLogic()
    {
        if (Input.GetKey(KeyCode.E) && CanDesactive)
        {
            if (!StartDesactive)
            {
                StartDesactive = true;
                photonView.RPC("StartDesactiveBomb", RpcTarget.AllBuffered, true);
            }
        }
        else
        {
            if (StartDesactive)
            {
                StartDesactive = false;
                photonView.RPC("StartDesactiveBomb", RpcTarget.AllBuffered, false);
            }
            TimeRemaingDesactive = TimeToActive;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void ActiveCountDown()
    {
        countdown -= Time.deltaTime;
        if (countdown > 0)
        {
            if (!Source.isPlaying)
            {
                Source.PlayDelayed(0.8f);
            }
        }
        if (countdown <= 0)
        {
            countdown = 0;
            BombDone = true;
            StartCoroutine(ExplosionSecuence());
            if (ActorPlant == PhotonNetwork.NickName)
            {
                PhotonNetwork.LocalPlayer.PostScore(XPForExplote);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void WonDone()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        int dscore = 0;
        Hashtable prod = new Hashtable();
        if (this.BombTeam == Team.Recon)
        {
            dscore = (int)PhotonNetwork.CurrentRoom.CustomProperties[PropertiesKeys.Team1Score];
            dscore++;
            prod.Add(PropertiesKeys.Team1Score, dscore);
            lastWinner = bl_GameData.Instance.Team1Name;
        }
        else if (this.BombTeam == Team.Delta)
        {
            dscore = (int)PhotonNetwork.CurrentRoom.CustomProperties[PropertiesKeys.Team2Score];
            dscore++;
            prod.Add(PropertiesKeys.Team2Score, dscore);
            lastWinner = bl_GameData.Instance.Team2Name;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(prod);
    }

    /// <summary>
    /// 
    /// </summary>
    void WonDoneByDefuse()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        int dscore = 0;
        Hashtable prod = new Hashtable();
        if (this.BombTeam == Team.Recon)
        {
            dscore = (int)PhotonNetwork.CurrentRoom.CustomProperties[PropertiesKeys.Team2Score];
            dscore++;
            prod.Add(PropertiesKeys.Team2Score, dscore);
            lastWinner = bl_GameData.Instance.Team2Name;
        }
        else if (this.BombTeam == Team.Delta)
        {
            dscore = (int)PhotonNetwork.CurrentRoom.CustomProperties[PropertiesKeys.Team1Score];
            dscore++;
            prod.Add(PropertiesKeys.Team1Score, dscore);
            lastWinner = bl_GameData.Instance.Team1Name;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(prod);
    }

    /// <summary>
    /// 
    /// </summary>
    void GetWinner(bool byDefuse = false)
    {
        if (this.BombTeam == Team.Recon)
        {
            lastWinner = (byDefuse) ? bl_GameData.Instance.Team2Name : bl_GameData.Instance.Team1Name;
        }
        else if (this.BombTeam == Team.Delta)
        {
            lastWinner = (byDefuse) ? bl_GameData.Instance.Team1Name : bl_GameData.Instance.Team2Name;
        }
    }

    private bool isStart = false;
    /// <summary>
    /// 
    /// </summary>
    void PlantLogic()
    {
        if (Input.GetKey(KeyCode.E) && CanPlant)
        {
            if (!isStart)
            {
                isStart = true;
                photonView.RPC("StartPlantBomb", RpcTarget.AllBuffered, true);
            }
        }
        else
        {
            if (isStart)
            {
                isStart = false;
                photonView.RPC("StartPlantBomb", RpcTarget.AllBuffered, false);
            }
            TimeRemaing = TimeToActive;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        countdown = TimeToExplote;
        TimeRemaing = TimeToActive;
        TimeRemaingDesactive = TimeToActive;
        BombDone = false;
        CanPlant = false;
        CanDesactive = false;
        isStart = false;
        isBussy = false;
        lightAlpha = 0;
        curveTime = 0;
        BombObject.SetActive(true);
        StartDesactive = false;
        RedLight.intensity = 0;
        Active = false;
        RoundFinish = false;
        CanUseThis = true;
        Debug.Log("Reset");
    }


    public void PlayerReset()
    {
        CanPlant = false;
        CanDesactive = false;
        StartCoroutine(checkAgain());
    }

    /// <summary>
    /// 
    /// </summary>
    void OnGUI()
    {
        GUI.skin = Skin;
        if (!Active)
        {
            if (CanPlant && !NetStart)
            {
                GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 50, 250, 50), "Press [E] to plant bomb!.");
            }
            if (isStart && CanPlant)
            {
                GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 50, 350, 70), "Activating bomb in: " + TimeRemaing.ToString("0.0"));
                BarProgress = Mathf.Lerp(BarProgress, (TimeRemaing - 1), Time.deltaTime / 2f);
                GUI.DrawTexture(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 110, TimeToActive * 30, 15), BackBar);
                GUI.DrawTexture(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 110, BarProgress * 30, 15), BarTexture);
            }
        }
        else
        {
            if (CanDesactive && !StartDesactive)
            {
                GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 50, 250, 50), "Press [E] to desactive the bomb!.");
            }
            if (CanDesactive && StartDesactive)
            {
                GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 50, 350, 70), "Desactive bomb in: " + TimeRemaingDesactive.ToString("0.0"));
                BarProgressDesactive = Mathf.Lerp(BarProgressDesactive, (TimeRemaingDesactive - 1), Time.deltaTime / 2f);
                GUI.DrawTexture(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 110, TimeToActive * 30, 15), BackBar);
                GUI.DrawTexture(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 110, BarProgressDesactive * 30, 15), BarTexture);
            }
        }

        if (Active && !RoundFinish)
        {
            GUI.color = (countdown <= 5) ? Color.red : Color.white;
            GUI.Label(new Rect(10, Screen.height - 175, 350, 50), BombTeam.ToString() + " Bomb will explode in: " + countdown.ToString("00.0"));
            GUI.color = Color.white;
        }
        GUI.depth = -600;
        if (RoundFinish)
        {
            string t = string.Format("TEAM {0} WON THIS ROUND\n<size=10>WAIT...</size>", lastWinner.ToUpper());
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(t));
            GUI.Label(new Rect(Screen.width / 2 - (size.x * 0.5f), Screen.height / 2 + 10, 300, 70), t);
        }
    }

#region RPCs

    /// <summary>
    /// 
    /// </summary>
    [PunRPC]
    void StartPlantBomb(bool enter, PhotonMessageInfo sender)
    {
        ActorPlant = sender.Sender.NickName;
        NetStart = enter;
        if (enter)
        {
            isBussy = true;
            if (sender.Sender == PhotonNetwork.LocalPlayer)
            {
                InvokeRepeating("CountDown", 1, 1);
            }
        }
        else
        {
            isBussy = false;
            if (sender.Sender == PhotonNetwork.LocalPlayer)
            {
                CancelInvoke("CountDown");
                TimeRemaing = TimeToActive;
                BarProgress = TimeRemaing;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enter"></param>
    /// <param name="sender"></param>
    [PunRPC]
    void StartDesactiveBomb(bool enter, PhotonMessageInfo sender)
    {
        if (enter)
        {
            isBussy = true;
            if (sender.Sender == PhotonNetwork.LocalPlayer)
            {
                InvokeRepeating("CountDownDesactive", 1, 1);
            }
        }
        else
        {
            isBussy = false;
            if (sender.Sender == PhotonNetwork.LocalPlayer)
            {
                CancelInvoke("CountDownDesactive");
                TimeRemaingDesactive = TimeToActive;
                BarProgressDesactive = TimeRemaingDesactive;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="act"></param>
    /// <param name="sender"></param>
    [PunRPC]
    void BombActive(bool act,int _id, PhotonMessageInfo sender)
    {
        NetStart = false;
        if (act)
        {
            Active = true;
            isBussy = false;
            foreach(bl_BombDefuse bd in AllBombs)
            {
                bd.DesactiveOther(_id);
            }
        }
        else
        {
            Active = false;
            countdown = TimeToExplote;
            isBussy = false;
        }
    }

    [PunRPC]
    void DesactiveBomb(PhotonMessageInfo sender)
    {
        Reset();
        StartCoroutine(BombDefuseSecuence());
        NetStart = false;
        if (sender.Sender.NickName == PhotonNetwork.NickName)
        {
            PhotonNetwork.LocalPlayer.PostScore(XPForExplote);
        }
    }
  
    public void DesactiveOther(int theActive)
    {
        if (theActive == this.ID)
            return;

        CanUseThis = false;
        BombObject.SetActive(false);
    }
#endregion

#region Corrutines

    IEnumerator checkAgain()
    {
        yield return new WaitForSeconds(1);
        CanPlant = false;
        CanDesactive = false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator ExplosionSecuence()
    {
        AudioSource.PlayClipAtPoint(ExplosionSound, transform.position);
        GameObject e = Instantiate(ExplosionEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(e, 7);
        BombObject.SetActive(false);
        yield return new WaitForSeconds(1.2f);


        AudioSource.PlayClipAtPoint(ExplosionSound, transform.position);
        GameObject fe = Instantiate(ExplosionEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(fe, 7);
        StartCoroutine(bl_UIReferences.Instance.FinalFade(true,false));


        yield return new WaitForSeconds(0.85f);

        if (PhotonNetwork.IsMasterClient) { WonDone(); } else { GetWinner(); }

        RoundFinish = true;
        AudioSource.PlayClipAtPoint(ExplosionSound, transform.position);
        GameObject f = Instantiate(ExplosionEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(f, 7);

        //This is a good place for save info in your DataBase
        yield return new WaitForSeconds(TimeForNextRound);
        //If still missing rounds
#if BDGM
        if (CheckForBDRound)
        {
            foreach (bl_BombDefuse bd in AllBombs) { bd.Reset(); }
            Manager.SpawnPlayer(PhotonNetwork.LocalPlayer.GetPlayerTeam());
        }
        else//if rached max rounds
        {
            PhotonNetwork.LeaveRoom();
            bl_UtilityHelper.LockCursor(false);
        }
#endif
    }


    public bool CheckForBDRound
    {
        get
        {
            int deltaScore = (int)PhotonNetwork.CurrentRoom.CustomProperties[PropertiesKeys.Team1Score];
            int reconScore = (int)PhotonNetwork.CurrentRoom.CustomProperties[PropertiesKeys.Team2Score];

            if (deltaScore >= MaxBDRounds)
            {
                return false;
            }
            if (reconScore >= MaxBDRounds)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator BombDefuseSecuence()
    {
        if (PhotonNetwork.IsMasterClient) { WonDoneByDefuse(); } else { GetWinner(true); }
        RoundFinish = true;
        StartCoroutine(bl_UIReferences.Instance.FinalFade(true));
        yield return new WaitForSeconds(TimeForNextRound);
        //If still missing rounds
#if BDGM
        if (CheckForBDRound)
        {
            foreach (bl_BombDefuse bd in AllBombs) { bd.Reset(); }
           Manager.SpawnPlayer(PhotonNetwork.LocalPlayer.GetPlayerTeam());
        }
        else//if rached max rounds
        {
            PhotonNetwork.LeaveRoom();
            bl_UtilityHelper.LockCursor(false);
        }
#endif
    }
#endregion
}