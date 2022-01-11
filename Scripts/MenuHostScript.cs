using UnityEngine;
using Photon.Pun;
public class MenuHostScript : MonoBehaviourPunCallbacks
{
    GameObject manager;
    public void BigMapSelect()
    {
        //NetworkManager.singleton.ServerChangeScene("GameScene");
        //NetworkManager.singleton.networkAddress = RandomizePassword();
        //NetworkManager.singleton.StartHost();
        //NetworkManager.singleton;
        //NetworkManager.singleton.Join
        //manager = GameObject.Find("NetManager");
        //manager.GetComponent<NetworkManager>().StartHost();
    }
    public string RandomizePassword()
    {
        string Password;
        Password = "" + Random.Range(0, 10);
        Password = Password + Random.Range(0, 10);
        Password = Password + Random.Range(0, 10);
        Password = Password + Random.Range(0, 10);
        Password = Password + Random.Range(0, 10);
        Password = Password + Random.Range(0, 10);
        return Password;
    }

}
