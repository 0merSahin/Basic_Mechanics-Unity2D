using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    private float moveSpeed;
    public float waitTime = 1f;
    private bool runable = false;
    private float elapsedTime = 0f;

    private void Start()
    {
        targetObject = GameObject.FindGameObjectWithTag("Player");
        moveSpeed = 5;
    }
    
    private void Update()
    {
        if (targetObject != null)
        {
            if (!runable)
            {
                if (elapsedTime <= waitTime)
                {
                    elapsedTime += Time.deltaTime;
                }
                else
                {
                    runable = true;
                }
            }
            else
            {
                Vector3 targetPosition = targetObject.transform.position;
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }
    }
}

