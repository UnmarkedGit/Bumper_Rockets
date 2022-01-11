using UnityEngine;
using Photon.Pun;

public class PlanetOrbits : MonoBehaviourPunCallbacks
{ 
    float TimeCounter = 0;

    [SerializeField]
    float speed;
    [SerializeField]
    float xRadius;
    [SerializeField]
    float yRadius;
    [SerializeField]
    float xOffset;
    [SerializeField]
    float yOffset;
    //[SyncVar]
    float x;
    //[SyncVar]
    float y;

    void Update()
    {
        PlanetOrbit();
    }
    void PlanetOrbit()
    {
        TimeCounter += Time.deltaTime * speed;

        x = Mathf.Cos(TimeCounter) * xRadius;
        y = Mathf.Sin(TimeCounter) * yRadius;

        GetComponent<Rigidbody2D>().position = new Vector2(x + xOffset, y + yOffset);
    }
}
