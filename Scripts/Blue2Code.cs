using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue2Code : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("AbiR1") || col.gameObject.CompareTag("AbiR2") || col.gameObject.CompareTag("AbiR3")
            || col.gameObject.CompareTag("AbiY2"))
        {
            col.transform.parent = col.transform;
            col.gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
        }
    }
}
