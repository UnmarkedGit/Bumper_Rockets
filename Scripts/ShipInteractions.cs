using UnityEngine;
using Photon.Pun;

public class ShipInteractions : MonoBehaviourPunCallbacks
{
    [Header("GameObjects")]
    private GameObject RoBoom;
    [SerializeField]
    private GameObject FrontBox;
    [SerializeField]
    private GameObject BackBox;

    [Header("Sounds")]
    [SerializeField]
    AudioSource DeathState;
    [SerializeField]
    AudioSource KillSound;
    [SerializeField]
    AudioSource Collection;

    [Header("Simple Variables")]
    [SerializeField]
    public bool RPLA;
    [SerializeField]
    public bool GPLA;
    [SerializeField]
    public bool BPLA;
    [SerializeField]
    public bool YPLA;
    [SerializeField]
    public bool IsDead;
    [SerializeField]
    public bool SunAreaF;
    [SerializeField]
    public bool SunAreaM;
    [SerializeField]
    public bool SunAreaC;
    [SerializeField]
    public bool RPlaG;
    [SerializeField]
    public bool BPlaG;
    [SerializeField]
    public bool YPlaG;
    [SerializeField]
    public bool GPlaG;

    [SerializeField]
    private Collider2D detection;

    public void Update()
    {

        if (!photonView.IsMine)
        {
            return;
        }
        if (IsDead)
        {
            if (GetComponent<PlayerControls>().ScrapAmount == 0.0f || GetComponent<PlayerControls>().ScrapAmount <= 0.0f)
            {
                GetComponent<PlayerControls>().ScrapAmount = 0;
            }
            else
            {
                GetComponent<PlayerControls>().ScrapAmount -= 0.20f;
            }
            GetComponent<PlayerControls>().AbilityTimer = 0;
            Activity(false);
            DeathState.mute = false;
        }
        else
        {
            Activity(true);
            DeathState.mute = true;

            //Vector2 dirction = transform.position - GameObject.FindGameObjectWithTag("Sun").transform.position;
            //dirction = -dirction.normalized;
            if (SunAreaF)
            {
                //Vector2 direction = transform.position - GameObject.FindGameObjectWithTag("Sun").transform.position;
                //Vector2 newvector = direction.normalized * -5000000 * Time.deltaTime;
                //GetComponent<Rigidbody2D>().velocity = newvector;
                //GetComponent<Rigidbody2D>().transform.position = Vector2.MoveTowards(GetComponent<Rigidbody2D>().transform.position, GameObject.FindGameObjectWithTag("Sun").transform.position, 2 * Time.deltaTime);
                //GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + (dirction * 200 * Time.deltaTime));

                //Vector2 direction = transform.position - GameObject.FindGameObjectWithTag("Sun").transform.position;
                //Vector2 newvector = direction.normalized * -2;
                //GetComponent<Rigidbody2D>().velocity += newvector;
            }
            if (SunAreaM)
            {
                //Vector2 direction = transform.position - GameObject.FindGameObjectWithTag("Sun").transform.position;
                //Vector2 newvector = direction.normalized * -5000000 * Time.deltaTime;
                //GetComponent<Rigidbody2D>().AddForce(newvector);//.velocity = newvector;
                //transform.position = Vector2.MoveTowards(GetComponent<Rigidbody2D>().transform.position, GameObject.FindGameObjectWithTag("Sun").transform.position, 2 * Time.deltaTime);
                //GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + (dirction * 200 * Time.deltaTime));
            }
            if (SunAreaC)
            {
                //Vector2 direction = transform.position - GameObject.FindGameObjectWithTag("Sun").transform.position;
                //Vector2 newvector = direction.normalized * -5000000 * Time.deltaTime;
                //GetComponent<Rigidbody2D>().AddForce(newvector);//.velocity = newvector;
                //transform.position = Vector2.MoveTowards(GetComponent<Rigidbody2D>().transform.position, GameObject.FindGameObjectWithTag("Sun").transform.position, 2 * Time.deltaTime);
                //GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + (dirction * 200 * Time.deltaTime));
            }
            if (RPlaG)
            {
                //transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("RPlanet").transform.position, 3 * Time.deltaTime);
                //Vector2 planetDir = transform.position - GameObject.FindGameObjectWithTag("RPlanet").transform.position;
                //planetDir = -planetDir.normalized;
                //GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + (planetDir * 30 * Time.deltaTime));
            }
            if (BPlaG)
            {
                //transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("BPlanet").transform.position, 3 * Time.deltaTime);
                //Vector2 planetDir = transform.position - GameObject.FindGameObjectWithTag("BPlanet").transform.position;
                //planetDir = -planetDir.normalized;
                //GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + (planetDir * 30 * Time.deltaTime));
            }
            if (YPlaG)
            {
                //transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("YPlanet").transform.position, 3 * Time.deltaTime);
                //Vector2 planetDir = transform.position - GameObject.FindGameObjectWithTag("YPlanet").transform.position;
                //planetDir = -planetDir.normalized;
                //GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + (planetDir * 3 * Time.deltaTime));
            }
            if (GPlaG)
            {
                //transform.position = Vector2.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("GPlanet").transform.position, 3 * Time.deltaTime);
                //Vector2 planetDir = transform.position - GameObject.FindGameObjectWithTag("GPlanet").transform.position;
                //planetDir = -planetDir.normalized;
                //GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + (planetDir * 3 * Time.deltaTime));
            }
        }
    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (col.gameObject.CompareTag("Wall"))
        {
            //Destroy(gameObject);
            //Scraped();
            if (IsDead)
            {
                //photonView.RPC("RpcIsDead", RpcTarget.AllViaServer, false);
                IsDead = false;
            }
            gameObject.transform.rotation = col.transform.rotation;
        }

        if (!IsDead)
        {
            //Player to Player
            ifPVP(col);
            //Abilities
            ifAbilities(col);

            //Map Hazards
            ifInHazards(col);
            //Areas with effects
            ifInAreasWEffects(col);
            //Scrap
            ifCollectingScrap(col);


            
            gravity(col);
        }
    }

    void ifInAreasWEffects(Collider2D col)
    {
        if (col.gameObject.CompareTag("YPL"))
        {
            YPLA = true;
        }
        if (col.gameObject.CompareTag("GPL"))
        {
            GPLA = true;
        }
        if (col.gameObject.CompareTag("BPL"))
        {
            BPLA = true;
        }
        if (col.gameObject.CompareTag("RPL"))
        {
            RPLA = true;
        }
    }
    void ifInHazards(Collider2D col)
    {
        if (col.gameObject.CompareTag("Satellite") && !GetComponent<PlayerControls>().GetIsRushing())
        {
            Scraped();
            GameObject.FindGameObjectWithTag("ResourceSpawning").GetComponent<ResourceSpawn>().SpawnScrap2(GetComponent<PlayerControls>().GetSize(), gameObject.transform.position);
        }
        if (col.gameObject.CompareTag("Sun") && transform.Find("PhysicalArea").CompareTag("PhysArea"))
        {
            Scraped();
        }
        if ((col.gameObject.CompareTag("RPlanet") || col.gameObject.CompareTag("BPlanet")
            || col.gameObject.CompareTag("YPlanet") || col.gameObject.CompareTag("GPlanet")) && transform.Find("PhysicalArea").CompareTag("PhysArea"))
        {
            Scraped();
            GameObject.FindGameObjectWithTag("ResourceSpawning").GetComponent<ResourceSpawn>().SpawnScrap2(GetComponent<PlayerControls>().GetSize(), gameObject.transform.position);
        }
    }
    void ifCollectingScrap(Collider2D col)
    {
        if (!GetComponent<PlayerControls>().ScrapFull())
        {
            if (col.gameObject.CompareTag("Sc0"))
            {
                Collection.Play();
                GetComponent<PlayerControls>().ScrapAmount += 10;
            }
            if (col.gameObject.CompareTag("Sc1"))
            {
                Collection.Play();
                GetComponent<PlayerControls>().ScrapAmount += 20;
            }
            if (col.gameObject.CompareTag("Sc2"))
            {
                Collection.Play();
                GetComponent<PlayerControls>().ScrapAmount += 30;
            }
        }
    }
    void ifAbilities(Collider2D col)
    {
        if (col.gameObject.CompareTag("AbiR1"))
        {
            Scraped();
            GameObject.FindGameObjectWithTag("ResourceSpawning").GetComponent<ResourceSpawn>().SpawnScrap2(GetComponent<PlayerControls>().GetSize(), gameObject.transform.position);
        }
        if (col.gameObject.CompareTag("AbiR2"))
        {
                //Destroy(gameObject);
                Scraped();
                GameObject.FindGameObjectWithTag("ResourceSpawning").GetComponent<ResourceSpawn>().SpawnScrap2(GetComponent<PlayerControls>().GetSize(), gameObject.transform.position);
        }
        if (col.gameObject.CompareTag("AbiR3"))
        {
            Scraped();
            GameObject.FindGameObjectWithTag("ResourceSpawning").GetComponent<ResourceSpawn>().SpawnScrap2(GetComponent<PlayerControls>().GetSize(), gameObject.transform.position);
        }
        if (col.gameObject.CompareTag("AbiG2"))//green 2
        {
            int sideSelect = Random.Range(1, 5);
            string Spawntxt;
            GameObject SpawnGaOb;
            Spawntxt = "SpawnLocation" + sideSelect;
            SpawnGaOb = GameObject.Find(Spawntxt);
            gameObject.transform.position = new Vector2(SpawnGaOb.transform.position.x, SpawnGaOb.transform.position.y);
            gameObject.transform.rotation = SpawnGaOb.transform.rotation;
        }
        if (col.gameObject.CompareTag("AbiY2"))//yellow 2
        {
                gameObject.GetComponent<PlayerControls>().AddAbilityTime(-150);
                //col.GetComponent<PlayerControls>().AbilityUse(col.GetComponent<PlayerControls>().GetAbility());
        }
        if (col.gameObject.CompareTag("AbiY3"))//yellow 3
        {
                gameObject.transform.rotation = col.transform.rotation;
                //gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, col.transform.rotation.z));
        }
    }
    void ifPVP(Collider2D col)
    {
        if (col.gameObject.CompareTag("PlayerFront") && transform.Find("FrontHitBox").CompareTag("PlayerFront"))//both ships hit front&& gameObject.tag == "PlayerFront"
        {
            int ColId = col.GetComponentInParent<PhotonView>().ViewID;
            if (gameObject.transform.lossyScale.x >= col.transform.lossyScale.x)//other ship is bigger
            {
                col.GetComponentInParent<ShipInteractions>().photonView.RPC("RpcScraped", RpcTarget.AllViaServer, ColId);
                GetComponent<PlayerControls>().ScrapAmount += 50 * col.GetComponent<PlayerControls>().size;
            }
        }
        if (transform.Find("FrontHitBox").CompareTag("PlayerFront") && col.gameObject.CompareTag("PlayerBack"))//&& gameObject.tag == "PlayerBack"
        {
            //Scraped();
            int ColId = col.GetComponentInParent<PhotonView>().ViewID;
            //col.GetComponentInParent<ShipInteractions>().photonView.RPC("RpcReceiveScraped", RpcTarget.AllViaServer, ColId, 50 * GetComponent<PlayerControls>().size);
            col.GetComponentInParent<ShipInteractions>().photonView.RPC("RpcScraped", RpcTarget.AllViaServer, ColId);
            GetComponent<PlayerControls>().ScrapAmount += 50 * col.GetComponent<PlayerControls>().size;
        }
    }
    void gravity(Collider2D col)
    {
        if (col.gameObject.CompareTag("SunAC"))
        {
            SunAreaC = true;
        }
        if (col.gameObject.CompareTag("SunAM"))
        {
            SunAreaM = true;
        }
        if (col.gameObject.CompareTag("SunAF"))
        {
            SunAreaF = true;
        }
        if (col.gameObject.CompareTag("GGrav"))
        {
            GPlaG = true;
        }
        if (col.gameObject.CompareTag("YGrav"))
        {
            YPlaG = true;
        }
        if (col.gameObject.CompareTag("BGrav"))
        {
            BPlaG = true;
        }
        if (col.gameObject.CompareTag("RGrav"))
        {
            RPlaG = true;
        }
    }

    //Leaving the area effects
    public void OnTriggerExit2D(Collider2D col)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (col.gameObject.CompareTag("YPL"))
        {
            YPLA = false;
        }
        if (col.gameObject.CompareTag("GPL"))
        {
            GPLA = false;
        }
        if (col.gameObject.CompareTag("BPL"))
        {
            BPLA = false;
        }
        if (col.gameObject.CompareTag("RPL"))
        {
            RPLA = false;
        }

        if (col.gameObject.CompareTag("SunAC"))
        {
            SunAreaC = false;
        }
        if (col.gameObject.CompareTag("SunAM"))
        {
            SunAreaM = false;
        }
        if (col.gameObject.CompareTag("SunAF"))
        {
            SunAreaF = false;
        }
        if (col.gameObject.CompareTag("GGrav"))
        {
            GPlaG = false;
        }
        if (col.gameObject.CompareTag("YGrav"))
        {
            YPlaG = false;
        }
        if (col.gameObject.CompareTag("BGrav"))
        {
            BPlaG = false;
        }
        if (col.gameObject.CompareTag("RGrav"))
        {
            RPlaG = false;
        }
    }
    void CmdScraped()
    {
        photonView.RPC("RpcScraped", RpcTarget.All);
    }
    [PunRPC]
    void RpcReceiveScraped(int argViewID, float argScrap)
    {
        if (this.gameObject.GetComponent<PhotonView>().ViewID == argViewID)
        {
            GetComponent<PlayerControls>().ScrapAmount += argScrap;
        }
    }
    [PunRPC]
    void RpcScraped(int argViewID)
    {
        if (this.gameObject.GetComponent<PhotonView>().ViewID == argViewID)
        {
            //Scraped();
            //photonView.RPC("RpcIsDead", RpcTarget.All, true);
            IsDead = true;
            //KillSound.Play();
            //DeathRunStart();
            //GetComponent<PlayerControls>().ScrapAmount += argScrap;
        }
    }
    void Scraped()
    {
        //photonView.RPC("RpcIsDead", RpcTarget.AllViaServer, true);
        IsDead = true;
        KillSound.Play();
        DeathRunStart();
    }
    public void DeathRunStart()
    {
        StartCoroutine(GetComponent<PlayerControls>().DeathPause());
        RemoveAbility(gameObject.transform);
    }
    public void RespawnProcedure()
    {
        int sideSelect = Random.Range(1, 5);
        string Spawntxt;
        GameObject SpawnGaOb;
        Spawntxt = "SpawnLocation" + sideSelect;
        SpawnGaOb = GameObject.Find(Spawntxt);
        gameObject.transform.position = new Vector2(SpawnGaOb.transform.position.x, SpawnGaOb.transform.position.y);
        gameObject.transform.rotation = SpawnGaOb.transform.rotation;
    }
    public bool NowIsDead()
    {
        return IsDead;
    }
    public void RemoveAbility(Transform parent)
    {
        //if (gameObject.transform.childCount == 15)
        //{
        //GameObject deadChild= gameObject.transform.GetChild(15).gameObject;
        //Destroy(deadChild);
        //}
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == "AbiR3")
            {
                Destroy(child.gameObject);
                //actors.Add(child.gameObject);
            }
            if (child.tag == "AbiB1")
            {
                Destroy(child.gameObject);
                //actors.Add(child.gameObject);
            }
            if (child.tag == "AbiB2")
            {
                Destroy(child.gameObject);
                //actors.Add(child.gameObject);
            }
            if (child.tag == "AbiB3")
            {
                Destroy(child.gameObject);
                //actors.Add(child.gameObject);
            }
            //if (child.childCount > 0)
            //{
            //    GetChildObject(child, _tag);
            //}
        }
    }

    [PunRPC]
    void RpcIsDead(bool argDead)
    {
        IsDead = argDead;
    }
    [PunRPC]
    void RpcActivity(bool argActive)
    {
        //XArea.SetActive(argActive);
        FrontBox.SetActive(argActive);
        BackBox.SetActive(argActive);
    }
    void Activity(bool argActive)
    {
        //XArea.SetActive(argActive);
        FrontBox.SetActive(argActive);
        BackBox.SetActive(argActive);
    }
}
