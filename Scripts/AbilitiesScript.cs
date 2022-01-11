using System.Collections;
using UnityEngine;
using Photon.Pun;

public class AbilitiesScript : MonoBehaviourPunCallbacks
{

    [Header("Colors&Effects")]
    [SerializeField]
    private SpriteRenderer ShipColor;
    [SerializeField]
    private SpriteRenderer ShipBackground;
    [SerializeField]
    private ParticleSystem JetTrail;


    [Header("ActivityBools")]
    [SerializeField]
    private bool abilStillActive;
    [SerializeField]
    private bool Blue1Active;
    [SerializeField]
    private bool Blue2Active;
    [SerializeField]
    private bool Blue3Active;
    [SerializeField]
    private bool Green3Active;

    [Header("Ability GameObjects")]
    [SerializeField]
    private GameObject Red1Prefab;
    [SerializeField]
    private GameObject Red2Prefab;
    [SerializeField]
    private GameObject Red3Prefab;
    [SerializeField]
    private GameObject Blue1Prefab;
    [SerializeField]
    private GameObject Blue2Prefab;
    [SerializeField]
    private GameObject Blue3Prefab;
    [SerializeField]
    private GameObject Yellow2Prefab;
    [SerializeField]
    private GameObject Yellow3Prefab;
    [SerializeField]
    private GameObject Green1Prefab;
    [SerializeField]
    private GameObject Green2Prefab;

    [Header("Sounds")]
    [SerializeField]
    AudioSource Red3;
    [SerializeField]
    AudioSource Blue1;
    [SerializeField]
    AudioSource Blue2;
    [SerializeField]
    AudioSource Blue3;
    [SerializeField]
    AudioSource Green1;
    [SerializeField]
    AudioSource Green2;
    [SerializeField]
    AudioSource Yellow1;
    [SerializeField]
    AudioSource Yellow2;
    [SerializeField]
    AudioSource Yellow3;

    [Header("Scripts")]
    [SerializeField]
    private PlayerControls PlayConScript;
    [SerializeField]
    private ShipInteractions ShipInter;

    private void Awake()
    {
        PlayConScript = GetComponent<PlayerControls>();
        ShipInter = GetComponent<ShipInteractions>();
    }
    public void Update()
    {

        if (!photonView.IsMine)
        {
            return;
        }
        if (ShipInter.IsDead)
        {
            StopCoroutine(Red3Abil());
            StopCoroutine(Blue1Abil());
            StopCoroutine(Blue2Abil());
            StopCoroutine(Blue3Abil());
            StopCoroutine(Green3Abil());
            StopCoroutine(Yellow1Abil());
        }
    }
    public bool GetBlue1Act()
    {
        return Blue1Active;
    }
    public bool GetBlue2Act()
    {
        return Blue2Active;
    }
    public bool GetBlue3Act()
    {
        return Blue3Active;
    }
    public bool GetGreen3Act()
    {
        return Green3Active;
    }
    public bool GetAbiStillAct()
    {
        return abilStillActive;
    }
    public void CmdAbilitySelection(int argAbil)
    {
        if (argAbil == 0)
        {
            if (PlayConScript.GetStage2())
            {
                if (PlayConScript.GetStage3())
                {
                    photonView.RPC("RpcRed3Activate", RpcTarget.All);
                    return;
                }
                photonView.RPC("RpcRed2Activate", RpcTarget.All);
                return;
            }
            photonView.RPC("RpcRed1Activate", RpcTarget.All);
            return;
        }


        if (argAbil == 1)
        {
            if (PlayConScript.GetStage2())
            {
                if (PlayConScript.GetStage3())
                {
                    photonView.RPC("RpcBlue3Activate", RpcTarget.All);
                    return;
                }
                photonView.RPC("RpcBlue2Activate", RpcTarget.All);
                return;
            }
            photonView.RPC("RpcBlue1Activate", RpcTarget.All);
            return;
        }


        if (argAbil == 2)
        {
            if (PlayConScript.GetStage2())
            {
                if (PlayConScript.GetStage3())
                {
                    photonView.RPC("RpcGreen3Activate", RpcTarget.All);
                    return;
                }
                photonView.RPC("RpcGreen2Activate", RpcTarget.All);
                return;
            }
            photonView.RPC("RpcGreen1Activate", RpcTarget.All);
            return;
        }


        if (argAbil == 3)
        {
            if (PlayConScript.GetStage2())
            {
                if (PlayConScript.GetStage3())
                {
                    photonView.RPC("RpcYellow3Activate", RpcTarget.All);
                    return;
                }
                photonView.RPC("RpcYellow2Activate", RpcTarget.All);
                return;
            }
            photonView.RPC("RpcYellow1Activate", RpcTarget.All);
            return;
        }
    }
    private Vector3 AbilitySizeChange(float argInitalSizeX, float argInitalSizeY, float argGrowth, int argShipSize)
    {
        return new Vector3(argInitalSizeX + (argGrowth * argShipSize), argInitalSizeY + (argGrowth * argShipSize), 1.0f);
    }
    private Vector3 AbilityPlacement(bool argBack, float argInitalLocation, float argGrowth, int argShipSize)
    {
        if (argBack)
        {
            //Back side
            return new Vector3(0.0f, -(argInitalLocation + (argGrowth * argShipSize)), 0.0f);
        }
        else
        {
            //Front side
            return new Vector3(0.0f, argInitalLocation + (argGrowth * argShipSize), 0.0f);
        }
    }

    [PunRPC]
    void RpcRed1Activate()
    {
        GameObject shot1 = Instantiate(Red1Prefab, this.transform.position, this.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        shot1.transform.localScale = AbilitySizeChange(4f, 2.5f, 0, PlayConScript.GetSize());
        shot1.transform.Translate(AbilityPlacement(false, 2.5f, 0.5f, PlayConScript.GetSize()));
        shot1.transform.parent = null;
        Destroy(shot1, .5f);
    }
    [PunRPC]
    void RpcRed2Activate()
    {
        GameObject shot1 = Instantiate(Red2Prefab, this.transform.position, this.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        shot1.transform.localScale = AbilitySizeChange(1.0f, 1.0f, 0f, PlayConScript.GetSize());
        shot1.transform.Translate(AbilityPlacement(true, 2.0f, 0.5f, PlayConScript.GetSize()));
        shot1.transform.parent = null;
    }
    [PunRPC]
    void RpcRed3Activate()
    {
        Red3.Play();
        StartCoroutine(Red3Abil());
    }
    [PunRPC]
    void RpcBlue1Activate()
    {
        StartCoroutine(Blue1Abil());
    }
    [PunRPC]
    void RpcBlue2Activate()
    {
        StartCoroutine(Blue2Abil());
    }
    [PunRPC]
    void RpcBlue3Activate()
    {
        StartCoroutine(Blue3Abil());
    }
    [PunRPC]
    void RpcGreen1Activate()
    {
        GameObject shot1 = Instantiate(Green1Prefab, this.transform.position, this.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        shot1.transform.localScale = AbilitySizeChange(2.5f, 2.5f, 0, PlayConScript.GetSize());
        Green1.Play();
        Destroy(shot1, 1f);

        int sideSelect = Random.Range(1, 5);
        string Spawntxt;
        GameObject SpawnGaOb;
        Spawntxt = "SpawnLocation" + sideSelect;
        SpawnGaOb = GameObject.Find(Spawntxt);
        gameObject.transform.position = new Vector2(SpawnGaOb.transform.position.x, SpawnGaOb.transform.position.y);
        gameObject.transform.rotation = SpawnGaOb.transform.rotation;
    }
    [PunRPC]
    void RpcGreen2Activate()
    {
        GameObject shot1 = Instantiate(Green2Prefab, this.transform.position, this.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        shot1.transform.localScale = AbilitySizeChange(8f, 8f, 0f, PlayConScript.GetSize());
        shot1.transform.parent = null;
        Green2.Play();
        Destroy(shot1, 1f);
    }
    [PunRPC]
    void RpcGreen3Activate()
    {
        StartCoroutine(Green3Abil());
    }
    [PunRPC]
    void RpcYellow1Activate()
    {
        StartCoroutine(Yellow1Abil());
    }
    [PunRPC]
    void RpcYellow2Activate()
    {
        gameObject.GetComponent<PlayerControls>().AddAbilityTime(500);
        Yellow2.Play();
        GameObject shot1 = Instantiate(Yellow2Prefab, this.transform.position, this.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        shot1.transform.localScale = AbilitySizeChange(20f, 20f, 0f, PlayConScript.GetSize());
        Destroy(shot1, 1f);
    }
    [PunRPC]
    void RpcYellow3Activate()
    {
        Yellow3.Play();
        GameObject shot1 = Instantiate(Yellow3Prefab, this.transform.position, this.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        shot1.transform.localScale = AbilitySizeChange(10f, 7.5f, 0f, PlayConScript.GetSize());
        shot1.transform.Translate(AbilityPlacement(false, 4.5f, .5f, PlayConScript.GetSize()));
        Destroy(shot1, 1f);
    }
    IEnumerator Red3Abil()
    {
        abilStillActive = true;
        GameObject shot1 = Instantiate(Red3Prefab, this.transform.position, this.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        shot1.transform.localScale = AbilitySizeChange(8f, 8f, 0.0f, PlayConScript.GetSize());
        shot1.transform.Translate(AbilityPlacement(false, 7f, .75f, PlayConScript.GetSize()));
        yield return new WaitForSeconds(5);
        Destroy(shot1);
        abilStillActive = false;
    }
    IEnumerator Blue1Abil()
    {
        abilStillActive = true;
        Blue1Active = true;
        GameObject shot1 = Instantiate(Blue1Prefab, this.transform.position, this.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        shot1.transform.localScale = AbilitySizeChange(3f, 3f, 0f, PlayConScript.GetSize());
        Blue1.mute = false;
        yield return new WaitForSeconds(5);
        Destroy(shot1);
        Blue1Active = false;
        abilStillActive = false;
        Blue1.mute = true;
    }
    IEnumerator Blue2Abil()
    {
        abilStillActive = true;
        Blue2Active = true;
        GameObject shot1 = Instantiate(Blue2Prefab, this.transform.position, this.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        shot1.transform.localScale = AbilitySizeChange(6f, 3f, .0f, PlayConScript.GetSize());
        Blue2.mute = false;
        yield return new WaitForSeconds(5);
        Destroy(shot1);
        Blue2Active = false;
        abilStillActive = false;
        Blue2.mute = true;
    }
    IEnumerator Blue3Abil()
    {
        abilStillActive = true;
        Blue3Active = true;
        GameObject shot1 = Instantiate(Blue3Prefab, this.transform.position, this.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        shot1.transform.localScale = AbilitySizeChange(7f, 3f, .0f, PlayConScript.GetSize());
        Blue3.mute = false;
        yield return new WaitForSeconds(5);
        Destroy(shot1);
        Blue3Active = false;
        abilStillActive = false;
        Blue3.mute = true;
    }
    IEnumerator Green3Abil()
    {
        abilStillActive = true;
        Green3Active = true;
        yield return new WaitForSeconds(10);
        Green3Active = false;
        abilStillActive = false;

    }
    IEnumerator Yellow1Abil()
    {
        abilStillActive = true;
        ShipColor.color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, .1f);
        ShipBackground.color = new Color(1, 1, 1, .1f); 
        JetTrail.startColor = new Color(1, 1, 1, .1f);
        Yellow1.mute = false;
        yield return new WaitForSeconds(5);
        ShipColor.color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
        ShipBackground.color = new Color(1, 1, 1, 1f);
        JetTrail.startColor = new Color(1, 1, 1, 1f);
        abilStillActive = false;
        Yellow1.mute = true;
    }
}