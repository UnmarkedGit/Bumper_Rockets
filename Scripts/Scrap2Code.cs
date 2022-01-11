using UnityEngine;
using Photon.Pun;

public class Scrap2Code : MonoBehaviourPunCallbacks
{

    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;

    private bool latestDestroySync;

    [SerializeField]
    private bool DestroySync = false;
    [SerializeField]
    private float Speed = 4f;

    public void Update()
    {
        if (DestroySync)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            Vector2 direction = transform.position - GameObject.FindGameObjectWithTag("Sun").transform.position;
            Vector2 newvector = direction.normalized * -Speed;
            GetComponent<Rigidbody2D>().velocity = newvector;
            //GetComponent<Rigidbody2D>().transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Sun").transform.position, Speed * Time.deltaTime);
        }
    }
        public void OnTriggerEnter2D(Collider2D col)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (col.gameObject.CompareTag("Sun")|| col.gameObject.CompareTag("Player"))
        {
            //RPCDestroy(gameObject, 3);
            //photonView.RPC("RPCDestroy", RpcTarget.AllViaServer, gameObject, 3);
            //photonView.RPC("RPCDestroy", RpcTarget.AllViaServer);
            //if (photonView.IsMine)
            //{
                DestroySync = true;
            //}
            //photonView.RPC("RPCDone", RpcTarget.AllViaServer, true);
        }
        gravity(col, 1, 3);
    }
    void gravity(Collider2D col, float sunPullGain, float planetPull)
    {
        if (col.gameObject.CompareTag("SunAC"))
        {
            GetComponent<Rigidbody2D>().transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Sun").transform.position, (sunPullGain * 3) * Time.deltaTime);
        }
        else if (col.gameObject.CompareTag("SunAM"))
        {
            GetComponent<Rigidbody2D>().transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Sun").transform.position, (sunPullGain * 2) * Time.deltaTime);
        }
        else if (col.gameObject.CompareTag("SunAF"))
        {
            GetComponent<Rigidbody2D>().transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Sun").transform.position, sunPullGain * Time.deltaTime);
        }
        if (col.gameObject.CompareTag("GGrav"))
        {
            GetComponent<Rigidbody2D>().transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("GPlanet").transform.position, planetPull * Time.deltaTime);
        }
        if (col.gameObject.CompareTag("YGrav"))
        {
            GetComponent<Rigidbody2D>().transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("YPlanet").transform.position, planetPull * Time.deltaTime);
        }
        if (col.gameObject.CompareTag("BGrav"))
        {
            GetComponent<Rigidbody2D>().transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("BPlanet").transform.position, planetPull * Time.deltaTime);
        }
        if (col.gameObject.CompareTag("RGrav"))
        {
            GetComponent<Rigidbody2D>().transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("RPlanet").transform.position, planetPull * Time.deltaTime);
        }
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(DestroySync);
            return;
        }
        else
        {
            latestDestroySync = (bool)stream.ReceiveNext();
        }
    }
    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            //Lag compensation
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            DestroySync = latestDestroySync;
        }
    }*/
}
