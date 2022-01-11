using UnityEngine;
using Photon.Pun;

public class SatelliteCode : MonoBehaviourPunCallbacks
{
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;

    private bool latestDestroySync;

    [SerializeField]
    private bool DestroySync = false;
    [SerializeField]
    private float Speed = 5f;

    public void Update()
    {
        if (DestroySync)
        {
            Destruction();
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
        if (col.gameObject.CompareTag("Sun"))
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        if ((col.gameObject.CompareTag("Player") && col.GetComponent<PlayerControls>().GetIsRushing()) || col.gameObject.CompareTag("AbiR1") 
            || col.gameObject.CompareTag("AbiR2") || col.gameObject.CompareTag("AbiR3") || col.gameObject.CompareTag("RPlanet") || col.gameObject.CompareTag("BPlanet")
            || col.gameObject.CompareTag("YPlanet") || col.gameObject.CompareTag("GPlanet"))
        {
            //if (photonView.IsMine)
            //{
                DestroySync = true;
                //Destruction();
            //}
        }
        gravity(col, 1, 3);
    }
    public void Destruction()
    {
        GameObject.FindGameObjectWithTag("ResourceSpawning").GetComponent<ResourceSpawn>().SpawnScrap1(gameObject.transform.position);
        Vector3 force = -gameObject.transform.position.normalized * 1000.0f;
        Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
        object[] instantiationData = { force, torque, true };
        PhotonNetwork.InstantiateRoomObject("BlowUpFlash", gameObject.transform.position, Quaternion.identity, 0, instantiationData); 
        PhotonNetwork.Destroy(gameObject);
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
