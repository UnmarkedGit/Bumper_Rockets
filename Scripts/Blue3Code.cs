using UnityEngine;
using Photon.Pun;

public class Blue3Code : MonoBehaviourPunCallbacks
{
    [SerializeField]
    float fireRate = 0.1f;
    [SerializeField]
    private float lastShot = 0.0f;
    [SerializeField]
    private GameObject Red1Prefab;
    [SerializeField]
    private GameObject LeftTurret;
    [SerializeField]
    private GameObject RightTurret;

    void Update()
    {
        if (Time.time > fireRate + lastShot)
        {
            RotateTurrets();
            RpcRight1Activate();
            RpcLeft1Activate();
            lastShot = Time.time;
            fireRate = Random.Range(.05f, .15f);
        }
    }
    [PunRPC]
    void RpcRight1Activate()
    {
        GameObject shot1 = Instantiate(Red1Prefab, RightTurret.transform.position, RightTurret.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        //NetworkServer.Spawn(shot1);
        shot1.transform.parent = null;
        Destroy(shot1, .5f);
    }
    [PunRPC]
    void RpcLeft1Activate()
    {
        GameObject shot1 = Instantiate(Red1Prefab, LeftTurret.transform.position, LeftTurret.transform.rotation);
        shot1.transform.parent = gameObject.transform;
        //NetworkServer.Spawn(shot1);
        shot1.transform.parent = null;
        Destroy(shot1, .5f);
    }
    void RotateTurrets()
    {
        RightTurret.transform.rotation = this.transform.rotation;
        LeftTurret.transform.rotation = this.transform.rotation;
        RightTurret.transform.Rotate(0.0f, 0.0f, Random.Range(170.0f, 190.0f));
        LeftTurret.transform.Rotate(0.0f, 0.0f, Random.Range(170.0f, 190.0f));
    }
}
