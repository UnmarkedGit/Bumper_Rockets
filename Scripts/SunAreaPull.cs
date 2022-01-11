using UnityEngine;

public class SunAreaPull : MonoBehaviour
{
    [SerializeField]
    private float GravPull = 4f;
    [SerializeField]
    private GameObject PullTowardsObject;

    public void OnTriggerStay2D(Collider2D col)
    {
        //this is a list of stuff that the sun will effect
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Sc0")
            || col.gameObject.CompareTag("Sc1") || col.gameObject.CompareTag("Sc2")
            || col.gameObject.CompareTag("Satellite"))
        {

            //Vector2 direction = col.transform.position - PullTowardsObject.transform.position;
            //Vector2 newvector = direction.normalized * -(GravPull * Time.deltaTime);
            //col.GetComponent<Rigidbody2D>().velocity += newvector;
            //Vector2 newvector = Vector2.MoveTowards(col.transform.position, PullTowardsObject.transform.position, (GravPull * Time.deltaTime));
            //col.GetComponent<Rigidbody2D>().MovePosition(newvector);
        }
    }
}
