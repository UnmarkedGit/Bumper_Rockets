using UnityEngine;

public class Blue1Code : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("AbiR1") || col.gameObject.CompareTag("AbiR2") || col.gameObject.CompareTag("AbiR3")
            || col.gameObject.CompareTag("AbiY2"))
        {
            Destroy(col.gameObject);
        }
    }
    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("AbiR1") || col.gameObject.CompareTag("AbiR2") || col.gameObject.CompareTag("AbiR3")
            || col.gameObject.CompareTag("AbiY2"))
        {
            Destroy(col.gameObject);
        }
    }
}
