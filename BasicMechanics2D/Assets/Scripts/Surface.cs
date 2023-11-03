using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    Movement MovementScript;
    private bool isLeftSurface;

    private void Start()
    {
        MovementScript = FindObjectOfType<Movement>();
        if (gameObject.name.Equals("SurfaceLeft"))
            isLeftSurface = true;
        else if (gameObject.name.Equals("SurfaceRight"))
            isLeftSurface = false;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isLeftSurface)
        {
            MovementScript.canLeftMove = false;
        }
        else
        {
            MovementScript.canRightMove = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isLeftSurface)
        {
            MovementScript.canLeftMove = true;
        }
        else
        {
            MovementScript.canRightMove = true;
        }
    }
}
