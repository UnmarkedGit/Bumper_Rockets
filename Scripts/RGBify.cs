using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RGBify : MonoBehaviour
{
    //[SerializeField]
    private bool cycleDone;
    float timeLeft;
    Color targetColor;
    [SerializeField]
    private Image PimpedOutObject;
    [SerializeField]
    private float Speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        cycleDone = true;
        PimpedOutObject = this.GetComponent<Image>();
    }

    void Update()
    {
        Flashing();
    }

    void Flashing()
    {

        //PimpedOutObject.color = RSIColBlock;

        if (cycleDone)
        {
            StartCoroutine(TheFlash());
        }
        if (timeLeft <= Time.deltaTime)
        {

            PimpedOutObject.color = targetColor;

            timeLeft = Speed;
        }
        else
        {

            PimpedOutObject.color = Color.Lerp(PimpedOutObject.color, targetColor, Time.deltaTime / timeLeft);

            timeLeft -= Time.deltaTime;
        }
    }

    IEnumerator TheFlash()
    {
        cycleDone = false;
        targetColor = Color.yellow;
        yield return new WaitForSeconds(Speed);
        targetColor = Color.green;
        yield return new WaitForSeconds(Speed);
        targetColor = Color.blue;
        yield return new WaitForSeconds(Speed);
        targetColor = Color.magenta;
        yield return new WaitForSeconds(Speed);
        targetColor = Color.red;
        yield return new WaitForSeconds(Speed);
        cycleDone = true;
    }
}
