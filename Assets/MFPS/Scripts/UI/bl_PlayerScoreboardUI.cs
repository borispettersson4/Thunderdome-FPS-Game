using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class bl_PlayerScoreboardUI : MonoBehaviour
{
    [SerializeField]private Text NameText;
    [SerializeField]private Text KillsText;
    [SerializeField]private Text DeathsText;
    [SerializeField]private Text ScoreText;
    [SerializeField] private GameObject KickButton;
    [SerializeField] private Image LevelIcon;

    private Player cachePlayer = null;
    private bl_UIReferences UIReference;
    private bool isInitializated = false;
    private Image BackgroundImage;
    private Team InitTeam = Team.None;
    private bl_AIMananger.BotsStats Bot;
    public ColorBlock localPlayerColor;

    public void Init(Player player, bl_UIReferences uir, bl_AIMananger.BotsStats bot = null)
    {
        Bot = bot;
        UIReference = uir;
        BackgroundImage = GetComponent<Image>();

        if (player != null && player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            gameObject.GetComponent<Button>().colors = localPlayerColor;
        }
        else if (uir.GetGameMode != GameMode.FFA)
        {
            var teamColorBlock = DuplicateNewColorBlock(localPlayerColor);
            var teamColor = bot == null ? PlayerProperties.GetTeamColor(player.GetPlayerTeam()) : PlayerProperties.GetTeamColor(bot.Team);
            gameObject.GetComponent<Button>().colors = GetOverwritenBaseColorBlock(teamColorBlock, teamColor);
        }

        if (Bot != null)
        {
            InitBot();
            return;
        }
        
        cachePlayer = player;
        gameObject.name = player.NickName + player.ActorNumber;
        if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            gameObject.GetComponent<Button>().colors = localPlayerColor;
        }

        InitTeam = player.GetPlayerTeam();
        NameText.text = player.NickNameAndRole();
        KillsText.text = player.CustomProperties[PropertiesKeys.KillsKey].ToString();
        DeathsText.text = player.CustomProperties[PropertiesKeys.DeathsKey].ToString();
        ScoreText.text = player.CustomProperties[PropertiesKeys.ScoreKey].ToString();
        KickButton.SetActive(PhotonNetwork.IsMasterClient && player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber && bl_GameData.Instance.MasterCanKickPlayers);
#if LM
         LevelIcon.gameObject.SetActive(true);
         LevelIcon.sprite = bl_LevelManager.Instance.GetPlayerLevelInfo(cachePlayer).Icon;
#else
        LevelIcon.gameObject.SetActive(false);
#endif
    }

    public void Refresh()
    {
        if(Bot != null) { InitBot(); return; }

        if(cachePlayer == null || cachePlayer.GetPlayerTeam() != InitTeam)
        {
            UIReference.RemoveUIPlayer(this);
            Destroy(gameObject);
        }

        NameText.text = cachePlayer.NickNameAndRole();
        KillsText.text = cachePlayer.CustomProperties[PropertiesKeys.KillsKey].ToString();
        DeathsText.text = cachePlayer.CustomProperties[PropertiesKeys.DeathsKey].ToString();
        ScoreText.text = cachePlayer.CustomProperties[PropertiesKeys.ScoreKey].ToString();

        if (cachePlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            gameObject.GetComponent<Button>().colors.normalColor.Equals(localPlayerColor);
        }

#if LM
        LevelIcon.sprite = bl_LevelManager.Instance.GetPlayerLevelInfo(cachePlayer).Icon;
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    public void InitBot()
    {
        gameObject.name = Bot.Name;
        NameText.text = Bot.Name;
        KillsText.text = Bot.Kills.ToString();
        DeathsText.text = Bot.Deaths.ToString();
        ScoreText.text = Bot.Score.ToString();
        InitTeam = Bot.Team;
        KickButton.SetActive(false);
        LevelIcon.gameObject.SetActive(true);
        LevelIcon.sprite = bl_UIReferences.Instance.BotLevelIcon;
    }

    public void Kick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            bl_PhotonGame.Instance.KickPlayer(cachePlayer);
        }
    }

    public void OnClick()
    {
        if (cachePlayer == null)
            return;
        if (cachePlayer.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber && Bot == null)
        {
            bl_UIReferences.Instance.OpenScoreboardPopUp(true, cachePlayer);
        }
    }

    void OnEnable()
    {
        if (cachePlayer == null && isInitializated)
        {
            Destroy(gameObject);
            isInitializated = true;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public int GetScore()
    {
        if(Bot == null) { return cachePlayer.GetPlayerScore(); }
        else { return Bot.Score; }
    }

    public int GetKills()
    {
        if (Bot == null) { return cachePlayer.GetKills(); }
        else { return Bot.Kills; }
    }

    public string GetName()
    {
        if (Bot == null) { return cachePlayer.NickName; }
        else { return Bot.Name; }
    }

    public Team GetTeam()
    {
        return InitTeam;
    }

    private ColorBlock DuplicateNewColorBlock(ColorBlock b)
    {
        return new ColorBlock
        {
            normalColor = b.normalColor,
            colorMultiplier = b.colorMultiplier,
            disabledColor = b.disabledColor,
            fadeDuration = b.fadeDuration,
            highlightedColor = b.highlightedColor,
            pressedColor = b.pressedColor,
            selectedColor = b.selectedColor
        };
    }

    private ColorBlock GetOverwritenBaseColorBlock(ColorBlock b,Color c)
    {
        return new ColorBlock
        {
            normalColor = new Color { a = b.normalColor.a, r = c.r, b = c.b, g = c.g },
            disabledColor = new Color { a = b.disabledColor.a, r = c.r, b = c.b, g = c.g },
            highlightedColor = new Color { a = b.highlightedColor.a, r = c.r, b = c.b, g = c.g },
            pressedColor = new Color { a = b.pressedColor.a, r = c.r, b = c.b, g = c.g },
            selectedColor = new Color { a = b.selectedColor.a, r = c.r, b = c.b, g = c.g },
            colorMultiplier = b.colorMultiplier,
            fadeDuration = b.fadeDuration
            
        };

    }
}