using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public bool canDash;

    void Start()
    {
        canDash = true;
    }


    void Update()
    {
        if (Input.GetButton("Dash") && canDash)
        { 
            StartCoroutine(DashTest());
        }
    }


    IEnumerator DashTest()
    {
        canDash = false;
        float startTime = Time.time;
        float elapsedTime = 0f;
        float dashDuration = 0.15f; // Dash süresi

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(2f, 0f, 0f); // Örneğin, x ekseninde 1 birimlik bir dash mesafesi

        while (elapsedTime < dashDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / dashDuration);

            elapsedTime = Time.time - startTime;

            yield return null;
        }

        // Eğer buraya geldiyse, dash işlemi tamamlandı.
        canDash = true;
    }

}
