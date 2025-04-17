using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class AiMovement : MonoBehaviour
{
    public GameObject theDestination;
    NavMeshAgent theAgent;
    void Start()
    {
        theAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            theAgent.SetDestination(theDestination.transform.position);
        }
        
    }
}
