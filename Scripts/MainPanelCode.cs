using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainPanelCode : MonoBehaviourPunCallbacks
{

    //[Header("Login Panel")]
    public GameObject StartPanel;

    //public InputField PlayerNameInput;
    public string PlayerName;

    [Header("Selection Panel")]
    public GameObject OptionPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;
    public Text MapSizeDescription;
    public InputField RoomNameInputField;
    public bool BigMapIsSelected;
    public bool MediumMapIsSelected;
    private bool isPrivate;

    [Header("Direct Join Room Panel")]
    public GameObject JoinRandomRoomPanel;
    public InputField PasswordInputField;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;
    private string RoomPassword;
    public Button StartGameButton;
    public GameObject PlayerListEntryPrefab;
    public GameObject PlayerListContent;
    public Text RoomCodeText;
    public InputField PlayerNameInputField;

    [Header("HTP and More Panel")]
    public GameObject HTPNMorePanel;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    #region UNITY

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();

        PlayerName = "Player " + Random.Range(1000, 10000);
    }

    #endregion

    #region PUN CALLBACKS
    public override void OnConnectedToMaster()
    {
        this.SetActivePanel(OptionPanel.name);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }
    public override void OnJoinedLobby()
    {
        // whenever this joins a new lobby, clear any previous room lists
        cachedRoomList.Clear();
        ClearRoomListView();
    }
    // note: when a client joins / creates a room, OnLeftLobby does not get called, even if the client was in a lobby before
    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(OptionPanel.name);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(OptionPanel.name);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = 8 };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }
    public override void OnJoinedRoom()
    {
        // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
        cachedRoomList.Clear();


        SetActivePanel(InsideRoomPanel.name);

        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }


        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            //entry.transform.SetParent(InsideRoomPanel.transform);
            entry.transform.SetParent(PlayerListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerListEntry>().Initialize(p.ActorNumber, p.NickName);

            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(RocketGameData.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
            }

            playerListEntries.Add(p.ActorNumber, entry);
        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());

        Hashtable props = new Hashtable
            {
                {RocketGameData.PLAYER_LOADED_LEVEL, false}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        RoomCodeText.text = "Room Code: " + PhotonNetwork.CurrentRoom.Name;
    }
    public override void OnLeftRoom()
    {
        SetActivePanel(OptionPanel.name);

        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        playerListEntries.Clear();
        playerListEntries = null;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(PlayerListContent.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        RoomCodeText.text = "Room Code: " + PhotonNetwork.CurrentRoom.Name;
        playerListEntries.Add(newPlayer.ActorNumber, entry);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(RocketGameData.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
            }
        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    #endregion

    #region UI CALLBACKS
    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        SetActivePanel(OptionPanel.name);
    }
    public void SetPlayerName()
    {
        PlayerName = PlayerNameInputField.text;
        PhotonNetwork.LocalPlayer.NickName = PlayerName;
    }
    public void OnCreateRoomButtonClicked()
    {
        //string roomName = RoomNameInputField.text;
        //roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

        if (!isPrivate && RoomNameInputField.text == "")
        {
            MapSizeDescription.text = "Please make a room name or set to private!";
        }
        else
        {
            byte maxPlayers;
            if (BigMapIsSelected)
            {
                byte.TryParse("20", out maxPlayers);
            }
            else if (MediumMapIsSelected)
            {
                byte.TryParse("10", out maxPlayers);
            }
            else
            {
                byte.TryParse("5", out maxPlayers);
            }
            maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, maxPlayers);
            RoomOptions options = new RoomOptions { IsVisible = !isPrivate, MaxPlayers = maxPlayers, PlayerTtl = 10000 };
            string RoomCode = Random.Range(0, 999999).ToString("000000");
            //RoomPassword = RoomCode;
            if (!isPrivate)
            {
                PhotonNetwork.CreateRoom(RoomNameInputField.text, options, TypedLobby.Default);
                return;
            }
            PhotonNetwork.CreateRoom(RoomCode, options, TypedLobby.Default);
        }
    }
    public void OnDirectJoinRoomButtonClicked()
    {
        //SetActivePanel(JoinRandomRoomPanel.name);
        //PhotonNetwork.JoinRandomRoom();


        PhotonNetwork.JoinRoom(PasswordInputField.text);
    }
    public void doExitGame()
    {
        Application.Quit();
    }
    public void doFullscreen(bool fullscreen)
    {
        if (fullscreen) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else Screen.fullScreenMode = FullScreenMode.Windowed;
    }
    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void OnBigMapButtonClicked()
    {
        MediumMapIsSelected = false;
        BigMapIsSelected = true;
        MapSizeDescription.text = "Size: Big | Max Players: 20 | Win At Size: 10";
    }
    public void OnMediumMapButtonClicked()
    {
        MediumMapIsSelected = true;
        BigMapIsSelected = false;
        MapSizeDescription.text = "Size: Medium | Max Players: 10 | Win At Size: 10";
    }
    public void OnSmallMapButtonClicked()
    {
        MediumMapIsSelected = false;
        BigMapIsSelected = false;
        MapSizeDescription.text = "Size: Small | Max Players: 5 | Win At Size: 10";
    }
    public void OnLoginButtonClicked()
    {
        string playerName = "Player " + Random.Range(1000, 10000);
        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }
        AdBannerMaker();
    }
    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        SetActivePanel(RoomListPanel.name);
    }
    public void OnHTPButtonClicked()
    {
        SetActivePanel(HTPNMorePanel.name);
    }
    public void OnStartGameButtonClicked()
    {
        //PhotonNetwork.CurrentRoom.IsOpen = false;
        //PhotonNetwork.CurrentRoom.IsVisible = false;
        if (BigMapIsSelected)
        {
            //Big Map
            PhotonNetwork.LoadLevel("GameSceneBig");
        }
        else if (MediumMapIsSelected)
        {
            //Medium Map
            PhotonNetwork.LoadLevel("GameSceneMedium");
        }
        else
        {
            //Small Map
            PhotonNetwork.LoadLevel("GameSceneSmall");
        }
    }
    public void IsPrivateGame(bool argPrivate)
    {
        if (argPrivate)
        {
            isPrivate = true;
            //PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        else
        {
            isPrivate = false;
            //PhotonNetwork.CurrentRoom.IsVisible = true;
        }
    }

    #endregion


    //Ads
    public void AdBannerMaker()
    {
        this.GetComponent<AdMobScript>().RequestBanner();
        this.GetComponent<AdMobScript>().ShowBannerAD();
    }

    //Textfield Keyboard
    public void PoofAKeyboard()
    {
        //TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(RocketGameData.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    public void LocalPlayerPropertiesUpdated()
    {
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public void SetActivePanel(string activePanel)
    {
        StartPanel.SetActive(activePanel.Equals(StartPanel.name));
        OptionPanel.SetActive(activePanel.Equals(OptionPanel.name));
        CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
        JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
        RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
        InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
        HTPNMorePanel.SetActive(activePanel.Equals(HTPNMorePanel.name));
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }
}

