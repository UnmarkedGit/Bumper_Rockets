using UnityEngine;
using Photon.Pun;

public class Red2Code : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float fuse;
    [SerializeField]
    private float BoomSize;
    [SerializeField]
    private float Timer;
    [SerializeField]
    ParticleSystem Idle;
    [SerializeField]
    ParticleSystem Boom;

    [Header("Sounds")]
    [SerializeField]
    AudioSource red2;

    public void Start()
    {
        Timer = 0;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 1);
        Boom.Pause();
        red2.PlayDelayed(fuse+0.5f);
    }
    public void FixedUpdate()
    {
        LaunchRed2();
    }

    void LaunchRed2()
    {
        if (fuse <= Timer)
        {
            Explosion();
            Boom.Play();
            Idle.Stop();
        }
        else
        {
            Timer += .01f;
            Idle.Play();
        }
    }

    void Explosion()
    {
        if (gameObject.transform.localScale.x >= BoomSize) 
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.transform.localScale += new Vector3(.2f, .2f, 1);
        }
    }
}
