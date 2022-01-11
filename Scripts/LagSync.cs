using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LagSync : MonoBehaviourPun, IPunObservable
{
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;

    //private Vector2 networkPosition = Vector2.zero;
    //private Quaternion networkRotation;
    private bool latestIsDead;
    private bool latestIsRush;
    float latestscrap;
    int latestsize;
    public float delaySmoother;
    //float scrapAtLastPacket = 0;
    //int sizeAtLastPacket = 0;

    public void Awake()
    {
        bool flag = false;
        using (List<Component>.Enumerator enumerator = base.photonView.ObservedComponents.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                if(enumerator.Current == this)
                {
                    flag = true;
                    break;
                }
            }
        }
        if (!flag)
        {
            Debug.LogWarning(this + " is not observed");
        }
        //if(!photonView.IsMine && GetComponent<PlayerControls>() != null)
        //{
        //    Destroy(GetComponent<PlayerControls>());
        //}
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(transform.position);
            //stream.SendNext(transform.rotation);
            stream.SendNext(GetComponent<PlayerControls>().ScrapAmount);
            stream.SendNext(GetComponent<ShipInteractions>().IsDead);
            stream.SendNext(GetComponent<PlayerControls>().isRushing);
            stream.SendNext(GetComponent<PlayerControls>().size);
            //stream.SendNext(GetComponent<Rigidbody2D>().velocity);
            return;
        }
        else
        {
            //this.networkPosition = (Vector2)stream.ReceiveNext();
            //this.networkRotation = (Quaternion)stream.ReceiveNext();
            latestscrap = (float)stream.ReceiveNext();
            latestIsDead = (bool)stream.ReceiveNext();
            latestIsRush = (bool)stream.ReceiveNext();
            latestsize = (int)stream.ReceiveNext();
            //GetComponent<Rigidbody2D>().velocity = (Vector2)stream.ReceiveNext();

            //float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTimestamp));//timestamp
            //networkPosition = (GetComponent<Rigidbody2D>().velocity * lag);
        }
    }

    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            //Lag compensation
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            //base.transform.position = Vector2.Lerp(base.transform.position, this.networkPosition, Time.fixedDeltaTime * this.delaySmoother);
            //base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.networkRotation, Time.fixedDeltaTime * this.delaySmoother);
            GetComponent<PlayerControls>().ScrapAmount = latestscrap;
            GetComponent<ShipInteractions>().IsDead = latestIsDead;
            GetComponent<PlayerControls>().isRushing = latestIsRush;
            GetComponent<PlayerControls>().size = latestsize;
            this.transform.localScale = new Vector3(.5f + (0.05f * (latestsize - 1)), .5f + (0.05f * (latestsize - 1)), 1.0f);
        }
    }

}
