using UnityEngine;
using Photon.Pun;

public class BlowUpFlashCode : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 1);
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale += new Vector3(.2f, .2f, 0);
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, .01f);
    }

}
