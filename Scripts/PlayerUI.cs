using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviourPunCallbacks
{
    [Header("Game Stats")]
    [SerializeField]
    AudioSource MatchEndSound;
    [SerializeField]
    AudioSource Size9ReachSound;

    [Header("Game Stats")]
    [SerializeField]
    RectTransform scrapAmount;
    [SerializeField]
    RectTransform abilityAmount;
    [SerializeField]
    RectTransform rushAmount;

    [SerializeField]
    Text stageTxt;
    [SerializeField]
    Text sizeTxt;
    [SerializeField]
    Text PasswordTxt;

    [SerializeField]
    GameObject DeathTxt;

    [Header("Scoreboard")]
    [SerializeField]
    Text YRank;
    [SerializeField]
    Text YSize;
    [SerializeField]
    Text YScrap;

    [SerializeField]
    Text GName1;
    [SerializeField]
    Text GSize1;
    [SerializeField]
    Text GScrap1;

    [SerializeField]
    Text GName2;
    [SerializeField]
    Text GSize2;
    [SerializeField]
    Text GScrap2;

    [SerializeField]
    Text GName3;
    [SerializeField]
    Text GSize3;
    [SerializeField]
    Text GScrap3;

    [SerializeField]
    Text GName4;
    [SerializeField]
    Text GSize4;
    [SerializeField]
    Text GScrap4;

    [SerializeField]
    Text GName5;
    [SerializeField]
    Text GSize5;
    [SerializeField]
    Text GScrap5;

    [Header("Panels")]
    [SerializeField]
    bool StartMenu;
    [SerializeField]
    bool DeathMenu;
    [SerializeField]
    bool FinishMenu;

    [SerializeField]
    public GameObject PlayerHUDPanel;
    [SerializeField]
    public GameObject DeathMenuPanel;
    [SerializeField]
    public GameObject StartMenuPanel;
    [SerializeField]
    public GameObject FinishPanel;

    [SerializeField] public Text WinnerText;
    [SerializeField] public GameObject StartShipPanel;
    [SerializeField] private InputField StartNameInputField = null;
    [SerializeField] public GameObject DeathShipPanel;

    [Header("Individual Data")]
    [SerializeField]
    public int WinCount = 0;
    [SerializeField]
    public string PlayName = "";

    [SerializeField] public float RedInput = 1.00f;
    [SerializeField] public float GreenInput = 1.00f;
    [SerializeField] public float BlueInput = 1.00f;

    [SerializeField]
    public GameObject DaRocket;
    public List<GameObject> RankSortedPlayerList;

    [SerializeField]
    private PlayerControls controller;

    [SerializeField]
    PhotonView view;

    private void Awake()
    {
        //PlayerUpdated(PhotonNetwork.LocalPlayer);
        //LocalPlayerAnnouncer.OnLocalPlayerUpdated += PlayerUpdated;

        controller = GetComponent<PlayerControls>(); 
    }
    public void Start()
    {
        AdInterMaker();
        StartMenu = true;
        DeathMenu = false;
        FinishMenu = false;
        //MatchEndSound.Stop();
        //Size9ReachSound.Stop();
        view = GetComponent<PhotonView>();
        GameObject[] playerCus = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in playerCus)  
        {
            /*if (!photonView.IsMine)
            {
                p.GetComponent<PlayerUI>().PlayerHUDPanel.SetActive(false);
                p.GetComponent<PlayerUI>().DeathMenuPanel.SetActive(false);
                p.GetComponent<PlayerUI>().StartMenuPanel.SetActive(false);
                p.GetComponent<PlayerUI>().FinishPanel.SetActive(false);
                return;
            }*/
            //p.GetComponent<PlayerUI>().RPC("RpcPaint", RpcTarget.AllViaServer, p, new Color(p.GetComponent<PlayerUI>().RedInput, p.GetComponent<PlayerUI>().GreenInput, p.GetComponent<PlayerUI>().BlueInput, 1.0f));
            p.GetComponent<PlayerUI>().CmdPaint( new Color(p.GetComponent<PlayerUI>().RedInput, p.GetComponent<PlayerUI>().GreenInput, p.GetComponent<PlayerUI>().BlueInput, 1.0f));
            //p.GetComponent<SpriteRenderer>().material.color = new Color(p.GetComponent<PlayerUI>().RedInput, p.GetComponent<PlayerUI>().GreenInput, p.GetComponent<PlayerUI>().BlueInput, 1.0f);

        }
    }
    public void Update()
    {
        if (!view.IsMine)
        {
            return;
        }
        SetAbilityType(GetComponent<ShipInteractions>().IsDead);
        DisplayRoomPassword();
        if (this.view.IsMine || !PhotonNetwork.InRoom)//
        {
            if (StartMenu)
            {
                
                PlayerHUDPanel.SetActive(false);
                DeathMenuPanel.SetActive(false);
                StartMenuPanel.SetActive(true);
                FinishPanel.SetActive(false);
                StartShipPanel.GetComponent<Image>().color = new Color(RedInput, GreenInput, BlueInput, 1.0f);
                return;
            }
            if (FinishMenu)
            {
                PlayerHUDPanel.SetActive(false);
                DeathMenuPanel.SetActive(false);
                StartMenuPanel.SetActive(false);
                FinishPanel.SetActive(true);
                return;
            }
            if (DeathMenu)
            {
                PlayerHUDPanel.SetActive(false);
                DeathMenuPanel.SetActive(true);
                StartMenuPanel.SetActive(false);
                FinishPanel.SetActive(false);
                DeathShipPanel.GetComponent<Image>().color = new Color(RedInput, GreenInput, BlueInput, 1.0f);
                return;
            }
            PlayerHUDPanel.SetActive(true);
            DeathMenuPanel.SetActive(false);
            StartMenuPanel.SetActive(false);
            FinishPanel.SetActive(false);
        }
        GameFinisher();
        Size9Flag();
        PlayerHUD();
        Scoreboard();

    }

    //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&         ADS DISABLED            &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
    //Ads
    public void AdInterMaker()
    {
        //this.GetComponent<AdMobScript>().RequestInterstitial();
        //this.GetComponent<AdMobScript>().ShowInterstitialAD();
    }

    //Textfeild Keyboard
    public void PoofAKeyboard()
    {
        //TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    //bigger Methods
    void GameFinisher()
    {
        if (10 <= WinCount)
        {
            //photonView.RPC("RpcSetWinnerName", RpcTarget.All, RankSortedPlayerList[0].GetComponent<PlayerUI>().GetName());
            controller.FinnishedWiping();
            MatchEndSound.Play();
            RpcSetWinnerName(RankSortedPlayerList[0].GetComponent<PlayerUI>().GetName());
            FinishMenu = true;
        }
    }
    void Size9Flag()
    {
        if (9 <= WinCount)
        {
            Size9ReachSound.Play();
        }
    }
    void PlayerHUD()
    {
        //Player HUD
        SetRushAmount(controller.GetRush());
        SetScrapAmount(controller.GetScrap());
    }
    void Scoreboard()
    {
        RankSortedPlayerList = GameObject.FindGameObjectWithTag("NetManager").GetComponent<RankingAndScoring>().GetSortedList();

        //Scoreboard Setter
        SetYourScoreTxt(GetCurrentPlayerRank(), controller.GetSize(), controller.GetScrap());

        photonView.RPC("RpcSetGPlayer1Txt", RpcTarget.All, RankSortedPlayerList[0].GetComponent<PlayerUI>().GetName(),
                RankSortedPlayerList[0].GetComponent<PlayerControls>().GetSize(),
                RankSortedPlayerList[0].GetComponent<PlayerControls>().GetScrap());
        GName1.color = RankSortedPlayerList[0].GetComponent<SpriteRenderer>().color;
        //RpcSetGPlayer1Txt(RankSortedPlayerList[0].GetComponent<PlayerUI>().GetName(),
        //    RankSortedPlayerList[0].GetComponent<PlayerControls>().GetSize(),
        //    RankSortedPlayerList[0].GetComponent<PlayerControls>().GetScrap());

        if (RankSortedPlayerList.Count < 2)
        {
            RpcSetGPlayer2Txt("---",0,0);
            GName2.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        }
        else 
        {
            photonView.RPC("RpcSetGPlayer2Txt", RpcTarget.All, RankSortedPlayerList[1].GetComponent<PlayerUI>().GetName(),
                RankSortedPlayerList[1].GetComponent<PlayerControls>().GetSize(),
                RankSortedPlayerList[1].GetComponent<PlayerControls>().GetScrap());
            GName2.color = RankSortedPlayerList[1].GetComponent<SpriteRenderer>().color;

            //SetGPlayer2Txt(RankSortedPlayerList[1].GetComponent<PlayerUI>().GetName(),
            //    RankSortedPlayerList[1].GetComponent<PlayerControls>().GetSize(),
            //    RankSortedPlayerList[1].GetComponent<PlayerControls>().GetScrap());
        }

        if (RankSortedPlayerList.Count < 3)
        {
            RpcSetGPlayer3Txt("---", 0, 0);
            GName3.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        }
        else
        {
            photonView.RPC("RpcSetGPlayer3Txt", RpcTarget.All, RankSortedPlayerList[2].GetComponent<PlayerUI>().GetName(),
                RankSortedPlayerList[2].GetComponent<PlayerControls>().GetSize(),
                RankSortedPlayerList[2].GetComponent<PlayerControls>().GetScrap());
            GName3.color = RankSortedPlayerList[2].GetComponent<SpriteRenderer>().color;

            //SetGPlayer3Txt(RankSortedPlayerList[2].GetComponent<PlayerUI>().GetName(),
            //    RankSortedPlayerList[2].GetComponent<PlayerControls>().GetSize(),
            //    RankSortedPlayerList[2].GetComponent<PlayerControls>().GetScrap());
        }

        if (RankSortedPlayerList.Count < 4)
        {
            RpcSetGPlayer4Txt("---", 0, 0);
            GName4.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else
        {
            photonView.RPC("RpcSetGPlayer4Txt", RpcTarget.All, RankSortedPlayerList[3].GetComponent<PlayerUI>().GetName(),
                RankSortedPlayerList[3].GetComponent<PlayerControls>().GetSize(),
                RankSortedPlayerList[3].GetComponent<PlayerControls>().GetScrap());
            GName4.color = RankSortedPlayerList[3].GetComponent<SpriteRenderer>().color;

            //SetGPlayer4Txt(RankSortedPlayerList[3].GetComponent<PlayerUI>().GetName(),
            //    RankSortedPlayerList[3].GetComponent<PlayerControls>().GetSize(),
            //    RankSortedPlayerList[3].GetComponent<PlayerControls>().GetScrap());
        }

        if (RankSortedPlayerList.Count < 5)
        {
            RpcSetGPlayer5Txt("---", 0, 0);
            GName5.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        }
        else
        {
            photonView.RPC("RpcSetGPlayer5Txt", RpcTarget.All, RankSortedPlayerList[4].GetComponent<PlayerUI>().GetName(), 
                RankSortedPlayerList[4].GetComponent<PlayerControls>().GetSize(),
                RankSortedPlayerList[4].GetComponent<PlayerControls>().GetScrap());
            GName5.color = RankSortedPlayerList[4].GetComponent<SpriteRenderer>().color;

            //SetGPlayer5Txt(RankSortedPlayerList[4].GetComponent<PlayerUI>().GetName(),
            //    RankSortedPlayerList[4].GetComponent<PlayerControls>().GetSize(),
            //    RankSortedPlayerList[4].GetComponent<PlayerControls>().GetScrap());
        }
        

        //photonView.RPC("RpcWinCounter", RpcTarget.All, RankSortedPlayerList[0].GetComponent<PlayerControls>().GetSize());
        WinCounter(RankSortedPlayerList[0].GetComponent<PlayerControls>().GetSize());
    }

    //Scoreboard Setters
    void SetYourScoreTxt(int argRank, int argSize, float argScrap)
    {
        YRank.text = "" + argRank;
        YSize.text = "" + argSize;
        YScrap.text = "" + argScrap;
    }
    [PunRPC]
    void RpcSetGPlayer1Txt(string argName, int argSize, float argScrap)
    {
        GName1.text = "" + argName;
        GSize1.text = "" + argSize;
        GScrap1.text = "" + argScrap;
    }
    [PunRPC]
    void RpcSetGPlayer2Txt(string argName, int argSize, float argScrap)
    {
        GName2.text = "" + argName;
        GSize2.text = "" + argSize;
        GScrap2.text = "" + argScrap;
    }
    [PunRPC]
    void RpcSetGPlayer3Txt(string argName, int argSize, float argScrap)
    {
        GName3.text = "" + argName;
        GSize3.text = "" + argSize;
        GScrap3.text = "" + argScrap;
    }
    [PunRPC]
    void RpcSetGPlayer4Txt(string argName, int argSize, float argScrap)
    {
        GName4.text = "" + argName;
        GSize4.text = "" + argSize;
        GScrap4.text = "" + argScrap;
    }
    [PunRPC]
    void RpcSetGPlayer5Txt(string argName, int argSize, float argScrap)
    {
        GName5.text = "" + argName;
        GSize5.text = "" + argSize;
        GScrap5.text = "" + argScrap;
    }
    void SetGPlayer1Txt(string argName, int argSize, float argScrap)
    {
        GName1.text = "" + argName;
        GSize1.text = "" + argSize;
        GScrap1.text = "" + argScrap;
    }
    void SetGPlayer2Txt(string argName, int argSize, float argScrap)
    {
        GName2.text = "" + argName;
        GSize2.text = "" + argSize;
        GScrap2.text = "" + argScrap;
    }
    void SetGPlayer3Txt(string argName, int argSize, float argScrap)
    {
        GName3.text = "" + argName;
        GSize3.text = "" + argSize;
        GScrap3.text = "" + argScrap;
    }
    void SetGPlayer4Txt(string argName, int argSize, float argScrap)
    {
        GName4.text = "" + argName;
        GSize4.text = "" + argSize;
        GScrap4.text = "" + argScrap;
    }
    void SetGPlayer5Txt(string argName, int argSize, float argScrap)
    {
        GName5.text = "" + argName;
        GSize5.text = "" + argSize;
        GScrap5.text = "" + argScrap;
    }

    //Scoreboard Value Getters
    public bool GetFinishMenuActive()
    {
        return FinishMenu;
    }
    string GetName()
    {
        return PlayName;
    }
    int GetCurrentPlayerRank()
    {
        int DaRanking = 1;
        for (int r = 0; r < RankSortedPlayerList.Count; r++)
        {
            if (GetName() == RankSortedPlayerList[r].GetComponent<PlayerUI>().GetName())
            {
                return DaRanking;
            }
            DaRanking++;
        }
        return DaRanking;
    }

    //Player HUD Setters
    [PunRPC]
    void RpcWinCounter(int argWinSize)
    {
        WinCount = argWinSize;
    }
    void WinCounter(int argWinSize)
    {
        WinCount = argWinSize;
    }
    [PunRPC]
    void RpcSetWinnerName(string argWinnerWinner)
    {
        WinnerText.text = argWinnerWinner;
    }
    void SetWinnerName(string argWinnerWinner)
    {
        WinnerText.text = argWinnerWinner;
    }
    void SetRushAmount(float argAmount)
    {
        rushAmount.anchorMin = new Vector2(1-argAmount/500f, .5f);
    }
    void SetAbilityAmount(float argAmount)
    {
        abilityAmount.anchorMin = new Vector2(1 - argAmount / 500f, .5f);
    }
    void SetScrapAmount(float argAmount)
    {
        scrapAmount.anchorMin = new Vector2(1 - argAmount/controller.NextScrapNeeded(), .5f);
    }
    void SetAbilityStageTXT(bool argStage2, bool argStage3)
    {
        if (argStage3)
        {
            stageTxt.text = "3";
        }
        else if (argStage2)
        {
            stageTxt.text = "2";
        }
        else
        {
            stageTxt.text = "1";
        }
    }
    void SetAbilityType(bool argIsDead)
    {
        if (argIsDead)
        {
            DeathTxt.SetActive(true);
        }
        else
        {
            DeathTxt.SetActive(false);
        }
    }
    void SetSizeTXT(int argsize, bool argFullOfScrap)
    {

        if (argFullOfScrap)
        {
            sizeTxt.text = "Upgrade to " + (argsize+1) + " at any planet!";
        }
        else
        {

            sizeTxt.text = "";//"Size: " + argsize;
        }
    }

    //customize and Death UI code (mostly for buttons/widgets on ui)
    public void RedAdjust(float argColor)
    {
        RedInput = argColor / 255;
    }
    public void GreenAdjust(float argColor)
    {
        GreenInput = argColor / 255;
    }
    public void BlueAdjust(float argColor)
    {
        BlueInput = argColor / 255;
    }
    public void StartPlayerConfirm()
    {
        //PlayName = StartNameInputField.text;
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(RedInput, GreenInput, BlueInput, 1.0f);
        CmdSNameChange(StartNameInputField.text);
        CmdPaint(new Color(RedInput, GreenInput, BlueInput, 1.0f));//gameObject, new Color(RedInput, GreenInput, BlueInput, 1.0f));
        StartMenu = false;
    }
    public void DeathPlayerConfirm()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(RedInput, GreenInput, BlueInput, 1.0f);
        CmdPaint(new Color(RedInput, GreenInput, BlueInput, 1.0f));//gameObject, new Color(RedInput, GreenInput, BlueInput, 1.0f));
        DeathMenu = false;
    }
    public void FinishContinue()
    {
        FinishMenu = false;
        StartMenu = true;
        AdInterMaker();
    }
    public void FinishQuit()
    {
        PhotonNetwork.Disconnect();
    }

    //Color Name Score methods [Command]
    public void CmdPaint(Color col)//GameObject obj, Color col)
    {
        // get the object's network ID
        Vector3 colTrans = new Vector3(col.r, col.g, col.b);
        photonView.RPC("RpcPaint", RpcTarget.AllViaServer, colTrans);//obj, col);
        //RpcPaint(obj, col);
    }

    [PunRPC]
    public void RpcPaint(Vector3 col)//GameObject obj, Color col)
    {
        Color culor = new Color(col.x, col.y, col.z);
        gameObject.GetComponent<SpriteRenderer>().color = culor;
        //obj.GetComponent<SpriteRenderer>().material.color = col;      // this is the line that actually makes the change in color happen
    }

    public void CmdSNameChange(string playername)
    {
        photonView.RPC("RpcSNameChange", RpcTarget.AllViaServer, playername);
    }
    [PunRPC]
    public void RpcSNameChange(string playername)
    {
        PlayName = playername;
    }

    //bool Getters
    public bool GetStartMenu()
    {
        return StartMenu;
    }
    public bool GetDeathMenu()
    {
        return DeathMenu;
    }

    //bool Setters
    public void SetDeathMenu(bool argDed)
    {
        DeathMenu = argDed;
    }
    public void SetFinishMenu(bool argFin)
    {
        FinishMenu = argFin;
        controller.FinnishedWiping();
    }
    public void DisplayRoomPassword()
    {
        PasswordTxt.text = PhotonNetwork.CurrentRoom.Name;//.singleton.networkAddress;//GameObject.Find("RocketsNetManager").GetComponent<NetworkManager>().singleton.networkAddress;
    }

}
