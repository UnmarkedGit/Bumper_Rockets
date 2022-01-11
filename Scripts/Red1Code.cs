using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class Red1Code : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float speed;

    [Header("Sounds")]
    [SerializeField]
    AudioSource red1;
    public void Update()
    {
        GetComponent<Rigidbody2D>().transform.Translate(0.0f, speed * Time.deltaTime, 0.0f);
    }
    public void Start()
    {
        red1.Play();
    }
}
