using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRotation : MonoBehaviour
{
    Quaternion rotation;
    void Awake()
    {
        //rotation = transform.rotation;
        rotation = GameObject.FindGameObjectWithTag("NetManager").transform.rotation;
    }
    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
