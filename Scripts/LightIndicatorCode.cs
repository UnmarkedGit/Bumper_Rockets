using System.Collections;
using UnityEngine;
using Photon.Pun;


public class LightIndicatorCode : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public Vector3 LightSize;
    [SerializeField]
    public Vector3 MaxSize;
    [SerializeField]
    public Vector3 MinSize;
    [SerializeField]
    private GameObject Light1;
    [SerializeField]
    private GameObject Light2;
    [SerializeField]
    private GameObject Light3;
    [SerializeField]
    private PlayerControls PlayConScript;
    [SerializeField]
    private bool cycleDone;

    [SerializeField]
    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        PlayConScript = GetComponent<PlayerControls>();
    }
    public void Start()
    {
        cycleDone = true;
    }
    public void Update()
    {
        if (!view.IsMine)
        {
            return;
        }
        Light();
    }
    
    void Light()
    {
        if (PlayConScript.GetAbilityTime() >= 500)
        {
            CmdFlashing(false);
            CmdColorAmountSet(PlayConScript.GetAbility(), PlayConScript.GetAbilityTime());
        }
        else
        {
            CmdFlashing(true);
        }
    }
    void CmdColorAmountSet(int argAbil, float argPower)
    {
        photonView.RPC("RpcColorAmountSet", RpcTarget.All, argAbil, argPower);
    }
    [PunRPC]
    void RpcColorAmountSet(int argAbil, float argPower)
    {
        float Power = argPower / 500;
        if (argAbil == 0)
        {
            if (PlayConScript.GetStage2())
            {
                if (PlayConScript.GetStage3())
                {
                    FirstLight(1.0f, 0.0f, 0.0f, Power);
                    SecondLight(1.0f, 0.0f, 0.0f, Power);
                    ThirdLight(1.0f, 0.0f, 0.0f, Power);
                    return;
                }
                FirstLight(1.0f, 0.0f, 0.0f, Power);
                SecondLight(1.0f, 0.0f, 0.0f, Power);
                ThirdLight(1.0f, 1.0f, 1.0f, 0.0f);
                return;
            }
            FirstLight(1.0f, 0.0f, 0.0f, Power);
            SecondLight(1.0f, 1.0f, 1.0f, 0.0f);
            ThirdLight(1.0f, 1.0f, 1.0f, 0.0f);
            return;
        }
        if (argAbil == 1)
        {
            if (PlayConScript.GetStage2())
            {
                if (PlayConScript.GetStage3())
                {
                    FirstLight(0.0f, 0.0f, 1.0f, Power);
                    SecondLight(0.0f, 0.0f, 1.0f, Power);
                    ThirdLight(0.0f, 0.0f, 1.0f, Power);
                    return;
                }
                FirstLight(0.0f, 0.0f, 1.0f, Power);
                SecondLight(0.0f, 0.0f, 1.0f, Power);
                ThirdLight(1.0f, 1.0f, 1.0f, 0.0f);
                return;
            }
            FirstLight(0.0f, 0.0f, 1.0f, Power);
            SecondLight(1.0f, 1.0f, 1.0f, 0.0f);
            ThirdLight(1.0f, 1.0f, 1.0f, 0.0f);
            return;
        }
        if (argAbil == 2)
        {
            if (PlayConScript.GetStage2())
            {
                if (PlayConScript.GetStage3())
                {
                    FirstLight(0.0f, 1.0f, 0.0f, Power);
                    SecondLight(0.0f, 1.0f, 0.0f, Power);
                    ThirdLight(0.0f, 1.0f, 0.0f, Power);
                    return;
                }
                FirstLight(0.0f, 1.0f, 0.0f, Power);
                SecondLight(0.0f, 1.0f, 0.0f, Power);
                ThirdLight(1.0f, 1.0f, 1.0f, 0.0f);
                return;
            }
            FirstLight(0.0f, 1.0f, 0.0f, Power);
            SecondLight(1.0f, 1.0f, 1.0f, 0.0f);
            ThirdLight(1.0f, 1.0f, 1.0f, 0.0f);
            return;
        }
        if (argAbil == 3)
        {
            if (PlayConScript.GetStage2())
            {
                if (PlayConScript.GetStage3())
                {
                    FirstLight(1.0f, 1.0f, 0.0f, Power);
                    SecondLight(1.0f, 1.0f, 0.0f, Power);
                    ThirdLight(1.0f, 1.0f, 0.0f, Power);
                    return;
                }
                FirstLight(1.0f, 1.0f, 0.0f, Power);
                SecondLight(1.0f, 1.0f, 0.0f, Power);
                ThirdLight(1.0f, 1.0f, 1.0f, 0.0f);
                return;
            }
            FirstLight(1.0f, 1.0f, 0.0f, Power);
            SecondLight(1.0f, 1.0f, 1.0f, 0.0f);
            ThirdLight(1.0f, 1.0f, 1.0f, 0.0f);
            return;
        }

    }

    void FirstLight(float argRed, float argGreen, float argBlue, float argVisibility)
    {
        Light1.GetComponent<SpriteRenderer>().color = new Color(argRed, argGreen, argBlue, argVisibility);
    }
    void SecondLight(float argRed, float argGreen, float argBlue, float argVisibility)
    {
        Light2.GetComponent<SpriteRenderer>().color = new Color(argRed, argGreen, argBlue, argVisibility);
    }
    void ThirdLight(float argRed, float argGreen, float argBlue, float argVisibility)
    {
        Light3.GetComponent<SpriteRenderer>().color = new Color(argRed, argGreen, argBlue, argVisibility);
    }

    void CmdFlashing(bool argNotFullPow)
    {
        photonView.RPC("RpcFlashing", RpcTarget.All, argNotFullPow);
    }
    [PunRPC]
    void RpcFlashing(bool argNotFullPow)
    {
        float Speed = 1f;
        Light1.transform.localScale = Vector3.Lerp(Light1.transform.localScale, LightSize, Time.deltaTime / Speed);
        Light2.transform.localScale = Vector3.Lerp(Light2.transform.localScale, LightSize, Time.deltaTime / Speed);
        Light3.transform.localScale = Vector3.Lerp(Light3.transform.localScale, LightSize, Time.deltaTime / Speed);
        if (argNotFullPow && cycleDone)
        {
            StartCoroutine(TheFlash());
        }
    }

    IEnumerator TheFlash()
    {
        cycleDone = false;
        LightSize = MaxSize;
        //Light1.GetComponent<SpriteRenderer>().color.r = 1.0f;
        FirstLight(Light1.GetComponent<SpriteRenderer>().color.r, Light1.GetComponent<SpriteRenderer>().color.g, Light1.GetComponent<SpriteRenderer>().color.b, 0.0f);
        SecondLight(Light2.GetComponent<SpriteRenderer>().color.r, Light2.GetComponent<SpriteRenderer>().color.g, Light2.GetComponent<SpriteRenderer>().color.b, 0.0f);
        ThirdLight(Light3.GetComponent<SpriteRenderer>().color.r, Light3.GetComponent<SpriteRenderer>().color.g, Light3.GetComponent<SpriteRenderer>().color.b, 1.0f);
        yield return new WaitForSeconds(.2f);
        FirstLight(Light1.GetComponent<SpriteRenderer>().color.r, Light1.GetComponent<SpriteRenderer>().color.g, Light1.GetComponent<SpriteRenderer>().color.b, 0.0f);
        SecondLight(Light2.GetComponent<SpriteRenderer>().color.r, Light2.GetComponent<SpriteRenderer>().color.g, Light2.GetComponent<SpriteRenderer>().color.b, 1.0f);
        ThirdLight(Light3.GetComponent<SpriteRenderer>().color.r, Light3.GetComponent<SpriteRenderer>().color.g, Light3.GetComponent<SpriteRenderer>().color.b, 0.0f);
        //LightSize = MinSize;
        yield return new WaitForSeconds(.2f);
        FirstLight(Light1.GetComponent<SpriteRenderer>().color.r, Light1.GetComponent<SpriteRenderer>().color.g, Light1.GetComponent<SpriteRenderer>().color.b, 1.0f);
        SecondLight(Light2.GetComponent<SpriteRenderer>().color.r, Light2.GetComponent<SpriteRenderer>().color.g, Light2.GetComponent<SpriteRenderer>().color.b, 0.0f);
        ThirdLight(Light3.GetComponent<SpriteRenderer>().color.r, Light3.GetComponent<SpriteRenderer>().color.g, Light3.GetComponent<SpriteRenderer>().color.b, 0.0f);
        yield return new WaitForSeconds(.2f);
        FirstLight(Light1.GetComponent<SpriteRenderer>().color.r, Light1.GetComponent<SpriteRenderer>().color.g, Light1.GetComponent<SpriteRenderer>().color.b, 0.0f);
        SecondLight(Light2.GetComponent<SpriteRenderer>().color.r, Light2.GetComponent<SpriteRenderer>().color.g, Light2.GetComponent<SpriteRenderer>().color.b, 1.0f);
        ThirdLight(Light3.GetComponent<SpriteRenderer>().color.r, Light3.GetComponent<SpriteRenderer>().color.g, Light3.GetComponent<SpriteRenderer>().color.b, 0.0f);
        yield return new WaitForSeconds(.2f);
        cycleDone = true;
    }

}
