using System.Collections;
using UnityEngine;
using Photon.Pun;


public class PlayerControls : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public int size;
    [SerializeField]
    public int Ability;

    [SerializeField]
    public GameObject target;

    [SerializeField]
    private AbilitiesScript AbilityScript;
    [SerializeField]
    private ShipInteractions ShipInterScript;
    [SerializeField]
    private PlayerUI UIFoPlayer;

    [SerializeField]
    public bool isRushing = false;
    [SerializeField]
    private bool stage2 = false;
    [SerializeField]
    private bool stage3 = false;
    [SerializeField]
    private bool AbilityCycleDone = false;
    [SerializeField]
    private bool LeftButtonPressed = false;
    [SerializeField]
    private bool RightButtonPressed = false;
    [SerializeField]
    private bool RushButtonPressed = false;
    [SerializeField]
    private bool SpawnSizeButtonPressed = false;
    [SerializeField]
    private bool AbilityButtonPressed = false;
    [SerializeField]
    private bool SlowButtonPressed = false;
    [SerializeField]
    private bool isDeathPaused = false;

    [SerializeField]
    public float green;
    [SerializeField]
    public float blue;
    [SerializeField]
    public float red;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float turningSpeed;
    [SerializeField]
    private float RushTimer = 500f;
    [SerializeField]
    public float AbilityTimer = 1f;
    [SerializeField]
    public float ScrapAmount = 0f;
    [SerializeField]
    private float RushDistance;

    [Header("Sounds")]
    [SerializeField]
    AudioSource HardJets;
    [SerializeField]
    AudioSource SoftJets;
    [SerializeField]
    AudioSource GreenJets;

    [Header("Particles")]
    [SerializeField]
    ParticleSystem SoftRock;
    [SerializeField]
    ParticleSystem HardRock;
    [SerializeField]
    ParticleSystem GreenSpark;
    [SerializeField]
    ParticleSystem GreenJet;
    [SerializeField]
    ParticleSystem UpgradeEffect;

    [Header("Pun Stuff")]
    //private PhotonView photonView;
#pragma warning disable 0109
    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Renderer renderer;
    private bool controllable = true;

    public void Awake()
    {
        AbilityScript = GetComponent<AbilitiesScript>();
        ShipInterScript = GetComponent<ShipInteractions>();
        UIFoPlayer = GetComponent<PlayerUI>();
        //photonView = GetComponent<PhotonView>();
        renderer = GetComponent<Renderer>();
    }
    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Sun");
        AbilityCycleDone = true;
        Ability = Random.Range(0, 4);
        size = 1;
        GreenJet.Stop();
        GreenSpark.Stop();
        //HardRock.Stop();
        HardRock.Play();
        SoftRock.Stop();
        HardJets.Play();
        SoftJets.Play();
        GreenJets.Play();
        HardJets.mute = true;
        SoftJets.mute = true;
        GreenJets.mute = true;
        //StartCoroutine(AbilitiesActivator());
    }
    public void FixedUpdate()
    {
        if (!photonView.AmOwner || !controllable)
        {
            return;
        }
        if (this.photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }

        if (!isDeathPaused)
        {
            if (ShipInterScript.IsDead)
            {
                speed = 20;
                DeadPlayerControls();
            }
            if (!ShipInterScript.IsDead)
            {
                speed = 15;
                AlivePlayerControls();
            }
        }
        if (isDeathPaused)
        {
            GetComponent<Rigidbody2D>().velocity = (new Vector2(0,0));
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
        RGBData();
        GreenAbil3Stuff();
        CmdAbilityManager(true);
    }

    private void GreenAbil3Stuff()
    {
        if (AbilityScript.GetGreen3Act())
        {
            photonView.RPC("RpcGreenSpark", RpcTarget.All, true);
            photonView.RPC("RpcGreenJets", RpcTarget.All, true);
        }
        else
        {
            photonView.RPC("RpcGreenJets", RpcTarget.All, false);
            photonView.RPC("RpcGreenSpark", RpcTarget.All, false);
        }
    }
    private void RGBData()
    {
        red = UIFoPlayer.RedInput;
        green = UIFoPlayer.GreenInput;
        blue = UIFoPlayer.BlueInput;
    }
    private void DeadPlayerControls()
    {

        Quaternion rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().rotation);
        Vector2 TheDirection = rotation * Vector2.up;

        if (!Input.GetKey("w") || !RushButtonPressed)
        {
            GetComponent<Rigidbody2D>().velocity =  TheDirection * speed;
            //Vector2 localVelocity = Vector2.ClampMagnitude(TheDirection, 1) * speed;
            //GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(localVelocity);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
        UseHardJets(false);
        if (Input.GetKey("w")|| RushButtonPressed)
        {
            //GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + (TheDirection * (speed * 4) * Time.deltaTime));
            GetComponent<Rigidbody2D>().velocity = (TheDirection * (speed*4));
            //Vector2 localVelocity = Vector2.ClampMagnitude(TheDirection, 1) * speed * 4;
            //GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(localVelocity);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
        if (Input.GetKey("a")|| LeftButtonPressed)
        {
            GetComponent<Rigidbody2D>().MoveRotation(GetComponent<Rigidbody2D>().rotation + turningSpeed * Time.deltaTime);
        }
        if (Input.GetKey("d") || RightButtonPressed)
        {
            GetComponent<Rigidbody2D>().MoveRotation(GetComponent<Rigidbody2D>().rotation - turningSpeed * Time.deltaTime);
        }
        if (Input.GetKey("s") || SlowButtonPressed)
        {
            //GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + (TheDirection * (-speed) * Time.deltaTime));
            GetComponent<Rigidbody2D>().velocity = TheDirection * -speed;
            //Vector2 localVelocity = Vector2.ClampMagnitude(TheDirection, 1) * -speed;
            //GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(localVelocity);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }

    }
    private void AlivePlayerControls()
    {
        if (UIFoPlayer.GetDeathMenu() || UIFoPlayer.GetStartMenu()) return;

        Quaternion rotation = Quaternion.Euler(0, 0, GetComponent<Rigidbody2D>().rotation);
        Vector2 TheDirection = rotation * Vector2.up;

        RushMoveBool(TheDirection);
        UseHardJets(true);
        Controls(TheDirection);
        RushTimeAdjuster();


        //AbilityChange();
        SizeChange(size);
        //}
    }
    //AlivePlayerControls features
    public void RushMoveBool(Vector2 Direction)
    {
        if (isRushing)
        {
            GetComponent<Rigidbody2D>().velocity = Direction * (speed*3);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Direction * speed;
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
    }
    public void Controls(Vector2 Direction)
    {
        if (Input.GetKey("w") || RushButtonPressed)
        {
            Rushing();
            //GetComponent<Rigidbody2D>().transform.Translate(0.0f, (speed) * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey("s") || SlowButtonPressed)
        {
            GetComponent<Rigidbody2D>().velocity = Direction * (speed/2);
            GetComponent<Rigidbody2D>().angularVelocity = 0;
        }
        if (Input.GetKey("a") || LeftButtonPressed)
        {
            GetComponent<Rigidbody2D>().MoveRotation(GetComponent<Rigidbody2D>().rotation + turningSpeed * Time.deltaTime);
        }
        if (Input.GetKey("d") || RightButtonPressed)
        {
            GetComponent<Rigidbody2D>().MoveRotation(GetComponent<Rigidbody2D>().rotation - turningSpeed * Time.deltaTime);
        }
        if (Input.GetKey("e") || SpawnSizeButtonPressed)
        {
            PlanetAbilSwap(ShipInterScript.RPLA, ShipInterScript.BPLA, ShipInterScript.GPLA, ShipInterScript.YPLA);

        }
        if (Input.GetKeyDown("q"))
        {
            AbilitiesSwitch();
        }
        if (Input.GetKey(KeyCode.Space) || AbilityButtonPressed)
        {
            AbilityUse(Ability);
        }

    }
    public void RushTimeAdjuster()
    {
        if (!Input.GetKey("w"))
        {
            //isRushing = false;
            if (RushTimer < 500)
            {
                RushTimer += 2;
            }
        }
        if (AbilityScript.GetGreen3Act())
        {
            //isRushing = true;
            if (RushTimer < 500)
            {
                RushTimer += 1;
            }
        }
        if (ShipInterScript.SunAreaC && AbilityTimer < 500)
        {
            AbilityTimer += 1;
        }
        if (AbilityTimer < 500 && !AbilityScript.GetAbiStillAct())
        {
            AbilityTimer += 1;
        }
    }


    //Mobile controls
    public void RushButtonMD()
    {
        RushButtonPressed = true;
        //Rushing();
    }
    public void RushButtonMU()
    {
        RushButtonPressed = false;
        //Rushing();
    }
    public void LeftButtonMD()
    {
        LeftButtonPressed = true;
        //GetComponent<Rigidbody2D>().transform.Rotate(Vector3.forward * turningSpeed * Time.deltaTime);
    }
    public void LeftButtonMU()
    {
        LeftButtonPressed = false;
        //GetComponent<Rigidbody2D>().transform.Rotate(Vector3.forward * turningSpeed * Time.deltaTime);
    }
    public void RightButtonMD()
    {
        RightButtonPressed = true;
        //GetComponent<Rigidbody2D>().transform.Rotate(Vector3.back * turningSpeed * Time.deltaTime);
    }
    public void RightButtonMU()
    {
        RightButtonPressed = false;
        //GetComponent<Rigidbody2D>().transform.Rotate(Vector3.back * turningSpeed * Time.deltaTime);
    }
    public void SpawnSizeButtonMD()
    {
        SpawnSizeButtonPressed = true;
    }
    public void SpawnSizeButtonMU()
    {
        SpawnSizeButtonPressed = false;
    }
    public void AbilityButtonMD()
    {
        AbilityButtonPressed = true;
        //AbilityUse(Ability);
    }
    public void AbilityButtonMU()
    {
        AbilityButtonPressed = false;
        //AbilityUse(Ability);
    }
    public void SlowButtonMD()
    {
        SlowButtonPressed = true;
        //GetComponent<Rigidbody2D>().transform.Translate(0.0f, -(speed / 2) * Time.deltaTime, 0.0f);
    }
    public void SlowButtonMU()
    {
        SlowButtonPressed = false;
        //GetComponent<Rigidbody2D>().transform.Translate(0.0f, -(speed / 2) * Time.deltaTime, 0.0f);
    }


    public void UseSoftJets(bool argActive)
    {
        if (argActive)
        {
            SoftRock.Play();
            SoftJets.mute = false;
        }
        else
        {
            SoftRock.Stop();
            SoftJets.mute = true;
        }
    }
    public void UseHardJets(bool argActive)
    {
        if (argActive)
        {
            HardRock.Play();
            HardJets.mute = false;
        }
        else
        {
            HardRock.Stop();
            HardJets.mute = true;
        }
    }
    public void UseGreenJets(bool argActive)
    {
        if (argActive)
        {
            GreenJet.Play();
            GreenJets.mute = false;
        }
        else
        {
            GreenJet.Stop();
            GreenJets.mute = true;
        }
    }
    public void UseGreenSpark(bool argActive)
    {
        if (argActive)
        {
            GreenSpark.Play();
        }
        else
        {
            GreenSpark.Stop();
        }
    }
    [PunRPC]
    public void RpcSoftJets(bool argActive)
    {
        if (argActive)
        {
            SoftRock. Play();
            SoftJets.mute = false;
        }
        else
        {
            SoftRock.Stop();
            SoftJets.mute = true;
        }
    }
    [PunRPC]
    public void RpcHardJets(bool argActive)
    {
        if (argActive)
        {
            HardRock.Play();
            HardJets.mute = false;
        }
        else
        {
            HardRock.Stop();
            HardJets.mute = true;
        }
    }
    [PunRPC]
    public void RpcGreenJets(bool argActive)
    {
        if (argActive)
        {
            GreenJet.Play();
            GreenJets.mute = false;
        }
        else
        {
            GreenJet.Stop();
            GreenJets.mute = true;
        }
    }
    [PunRPC]
    public void RpcGreenSpark(bool argActive)
    {
        if (argActive)
        {
            GreenSpark.Play();
        }
        else
        {
            GreenSpark.Stop();
        }
    }

    //getters
    public float GetScrap()
    {
        return ScrapAmount;
    }
    public float GetRush()
    {
        return RushTimer;
    }
    public int GetSize()
    {
        return size;
    }
    public float GetAbilityTime()
    {
        return AbilityTimer;
    }
    public void AddAbilityTime(float argPowGain)
    {
        AbilityTimer = AbilityTimer + argPowGain;
    }
    public int GetAbility()
    {
        return Ability;
    }
    public bool GetStage2()
    {
        return stage2;
    }
    public bool GetStage3()
    {
        return stage3;
    }
    public bool GetIsRushing()
    {
        return isRushing;
    }

    //Complex getters
    public float NextScrapNeeded()
    {
        return 100 * size;
    }
    public bool ScrapFull()
    {
        if (GetScrap() >= NextScrapNeeded())
        {
            return true;
        }
        return false;
    }

    //Not Getters
    void ClearScrap()
    {
        ScrapAmount = 0;
        stage2 = false;
        stage3 = false;
    }
    void Dashforward()
    {
        StartCoroutine(RushCycle());

    }
    void Rushing()
    {
        /*if (isRushing)
        {
            return;
        }*/
        //GetComponent<Rigidbody2D>().transform.Translate(0.0f, (speed * 2) * Time.deltaTime, 0.0f);
        if (AbilityScript.GetGreen3Act())
        {
            Dashforward();
        }
        else
        {
            if (RushTimer >= 200)
            {
                RushTimer = RushTimer - 200;//(float)Mathf.Lerp(RushTimer, RushTimer - 200, Time.fixedDeltaTime * 1);
                Dashforward();
            }
        }
    }



    public void AbilityUse(int argAbility)
    {
        if (AbilityTimer >= 500)
        {
            AbilityTimer = 0;
            this.AbilityScript.CmdAbilitySelection(argAbility);
        }
    }
    void PlanetAbilSwap(bool ArgR, bool ArgB, bool ArgG, bool ArgY)
    { 
        if(ScrapFull())
        {
            if (ArgR)
            {
                //photonView.RPC("RpcSize", RpcTarget.AllViaServer, size + 1);
                size = size + 1;
                ClearScrap();
                Ability = 0;
            }
            if (ArgB)
            {
                //photonView.RPC("RpcSize", RpcTarget.AllViaServer, size + 1);
                size = size + 1;
                ClearScrap();
                Ability = 1;
            }
            if (ArgG)
            {
                //photonView.RPC("RpcSize", RpcTarget.AllViaServer, size + 1);
                size = size + 1;
                ClearScrap();
                Ability = 2;
            }
            if (ArgY)
            {
                //photonView.RPC("RpcSize", RpcTarget.AllViaServer, size + 1);
                size = size + 1;
                ClearScrap();
                Ability = 3;
            }
            ShipInterScript.RemoveAbility(gameObject.transform);
            SizeChange(size);
            UpgradeEffect.Play();
        }
    }
    void SizeChange(int argSize)
    {
        gameObject.transform.localScale = new Vector3( .5f + (0.05f * (argSize - 1)), .5f + (0.05f * (argSize - 1)), 1.0f);
        GetComponent<PlayerCameraFollow>().CamSizeChange(argSize);
    }
    public void FinnishedWiping()
    {
        if (!UIFoPlayer.GetFinishMenuActive())
        {
            size = 1;
            //GetComponent<PlayerCameraFollow>().SetCamSizeWipe();
            GetComponent<ShipInteractions>().RespawnProcedure();
            ShipInterScript.RespawnProcedure();
        }
    }
    [PunRPC]
    void RpcStages(bool argS2,bool argS3)
    {
            stage2 = argS2;
            stage3 = argS3;
    }
    [PunRPC]
    void RpcSize(int argSize)
    {
        size = argSize;
    }
    void setStages(bool argS2, bool argS3)
    {
        stage2 = argS2;
        stage3 = argS3;
    }
    void setSize(int argSize)
    {
        size = argSize;
    }
    void CmdAbilityManager(bool argStart)
    {
        photonView.RPC("RpcAbilityManager", RpcTarget.All, argStart);
    }
    [PunRPC]
    void RpcAbilityManager(bool argStart)
    {
        if (argStart && AbilityCycleDone)
        {
            StartCoroutine(AbilitiesActivator());
        }
    }
    void AbilitiesSwitch()
    {
        if (stage2)
        {
            if (stage3)
            {
                setStages(false, false);
                return;
            }
            setStages(true, true);
            return;
        }
        setStages(true, false);
        return;
    }

    //Enumerators
    IEnumerator AbilitiesActivator()
    {
        AbilityCycleDone = false;
        yield return new WaitForSeconds(1.5f);
        AbilitiesSwitch();
        AbilityCycleDone = true;
    }
    IEnumerator RushCycle()
    {
        isRushing = true;
        photonView.RPC("RpcSoftJets", RpcTarget.All, true);
        yield return new WaitForSeconds(RushDistance); 
        photonView.RPC("RpcSoftJets", RpcTarget.All, false);
        isRushing = false;
    }
    public IEnumerator DeathPause()
    {
        isDeathPaused = true;
        Vector3 Placement = new Vector3(gameObject.transform.position.x +Random.Range(-1.5f, 1.5f), gameObject.transform.position.y + Random.Range(-1.5f, 1.5f), 0);
        PhotonNetwork.InstantiateRoomObject("BlowUpFlash", Placement, Quaternion.identity);
        yield return new WaitForSeconds(.25f);
        Placement = new Vector3(gameObject.transform.position.x + Random.Range(-1.5f, 1.5f), gameObject.transform.position.y + Random.Range(-1.5f, 1.5f), 0);
        PhotonNetwork.InstantiateRoomObject("BlowUpFlash", Placement, Quaternion.identity);
        yield return new WaitForSeconds(.25f);
        Placement = new Vector3(gameObject.transform.position.x + Random.Range(-1.5f, 1.5f), gameObject.transform.position.y + Random.Range(-1.5f, 1.5f), 0);
        PhotonNetwork.InstantiateRoomObject("BlowUpFlash", Placement, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        isDeathPaused = false;
    }


}
