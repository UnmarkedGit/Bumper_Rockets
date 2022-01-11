using System.Collections;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManagerScript : MonoBehaviourPunCallbacks
{
    public static GameManagerScript Instance = null;

    //public Text InfoText;

    public GameObject[] AsteroidPrefabs;

    #region UNITY

    public void Awake()
    {
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        StartGame();
    }

    public void Start()
    {
        Hashtable props = new Hashtable
            {
                {RocketGameData.PLAYER_LOADED_LEVEL, true}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    #endregion

    #region COROUTINES
    //+=+=+=+=+=+=+=++++++++++++++++++++++++++++++++++++++====      Put Terrain/spawnables here
    private IEnumerator SpawnResources()
    {
        GameObject spawner = GameObject.FindGameObjectWithTag("ResourceSpawning");
        while (true)
        {
            yield return new WaitForSeconds(1);
            spawner.GetComponent<ResourceSpawn>().SpawnSatellite();
            spawner.GetComponent<ResourceSpawn>().SpawnScrap0();
            yield return new WaitForSeconds(1);
            spawner.GetComponent<ResourceSpawn>().SpawnScrap0();
            yield return new WaitForSeconds(1);
            spawner.GetComponent<ResourceSpawn>().SpawnScrap0();
            yield return new WaitForSeconds(1);
            spawner.GetComponent<ResourceSpawn>().SpawnScrap0();
            yield return new WaitForSeconds(1);
            spawner.GetComponent<ResourceSpawn>().SpawnScrap0();
        }
    }
    #endregion

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected by:  " + cause);
        UnityEngine.SceneManagement.SceneManager.LoadScene("main menu");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartCoroutine(SpawnResources());
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " has left");
        PhotonNetwork.DestroyPlayerObjects(otherPlayer);
        base.OnPlayerLeftRoom(otherPlayer);
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
    }
    #endregion


    // called by OnCountdownTimerIsExpired() when the timer ended
    private void StartGame()
    {
        Debug.Log("StartGame!");
        string Spawntxt;
        int num = 0;
        Vector3 position;
        Quaternion rotation;
        GameObject SpawnGaOb;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            num++;
            if(p == PhotonNetwork.LocalPlayer)
            {
            Spawntxt = "SpawnLocation" + num;
            SpawnGaOb = GameObject.Find(Spawntxt);
            position = new Vector2(SpawnGaOb.transform.position.x, SpawnGaOb.transform.position.y);
            rotation = SpawnGaOb.transform.rotation;
            PhotonNetwork.Instantiate("PlayerRocket", position, rotation, 0);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnResources());
        }
    }
}
