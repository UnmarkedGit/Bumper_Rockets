using UnityEngine;
using Photon.Pun;

public class Scrap0Code : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float Speed = 4f;

    public void Update()
    {
        GetComponent<Rigidbody2D>().transform.position = Vector2.MoveTowards(GetComponent<Rigidbody2D>().transform.position, GameObject.FindGameObjectWithTag("Sun").transform.position, Speed * Time.deltaTime);

    }
    public void OnTriggerEnter2D(Collider2D col)
    {

        if (!photonView.IsMine)
        {
            return;
        }

        if (col.gameObject.CompareTag("Sun") || (col.gameObject.CompareTag("Player") && !col.gameObject.GetComponentInParent<ShipInteractions>().IsDead))
        {
            //RPCDestroy(gameObject, 3);
            //photonView.RPC("RPCDestroy", RpcTarget.AllViaServer, gameObject, 3);
            //photonView.RPC("RPCDestroy", RpcTarget.AllViaServer);
            PhotonNetwork.Destroy(gameObject);
            //photonView.RPC("RPCDone", RpcTarget.AllViaServer, true);
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }/*
    [PunRPC]
    public void RPCDone(bool argDone)
    {
        Done = argDone;
    }

    [PunRPC]
    public void RPCDestroy(bool argDone)
    {
        //Destroy(gameObject);
        Done = argDone;
    }
    public void CMDDestroySelf()
    {
        //Destroy(gameObject);
        photonView.RPC("RPCDestroy", RpcTarget.AllViaServer,true);
    }*/
}
