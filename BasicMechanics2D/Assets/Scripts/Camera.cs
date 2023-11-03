using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject TargetObject;
    public float followSpeed = 9;

    private void LateUpdate()
    {
        if (TargetObject != null)
        {
            Vector3 targetDistance = TargetObject.transform.position - transform.position;
            targetDistance.z = 0f;
            transform.position += targetDistance * followSpeed * Time.deltaTime;
        }
    }
}

