using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectSync : MonoBehaviourPun
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
    }
}
