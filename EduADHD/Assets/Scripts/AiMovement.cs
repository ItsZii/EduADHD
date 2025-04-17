using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AiMovement : MonoBehaviour
{
    public List<Transform> destinations;
    public float pauseDuration = 10f; // time to wait at each destination
    public float rotationSpeed = 5f;
    public bool isTeacher;
    

    private NavMeshAgent theAgent;
    private int currentDestinationIndex = 0;
    private bool hasArrived = false;
    private bool isRotating = false;
    private bool started = false;
    private float pauseTimer = 0f;
    private float failSafe = 0f;
    private float teacherWait = 0f;

    void Start()
    {
        theAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (started && isTeacher) 
        {
            teacherWait += Time.deltaTime;
        }
        if (teacherWait >= 10f && isTeacher) 
        {
            isTeacher = false;
        }

        if (started && !isTeacher) 
        {
            failSafe += Time.deltaTime;
        }
        
        if (failSafe >= 30f) 
        {
            currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Count;
            MoveToNextDestination();
            failSafe = 0f;
        }
        // Start walking with L
        if (!started && Input.GetKeyDown(KeyCode.L))
        {
            started = true;
            if(!isTeacher)
            {
                MoveToNextDestination();
            }
        }

        if (!started || destinations.Count == 0)
            return;

        // Check if agent reached the destination
        if (!hasArrived && !theAgent.pathPending && theAgent.remainingDistance <= theAgent.stoppingDistance)
        {
            hasArrived = true;
            isRotating = true;
            pauseTimer = 0f; // Start pause
        }

        // Rotate after arriving
        if (isRotating)
        {
            Quaternion targetRotation = destinations[currentDestinationIndex].rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * 100 * Time.deltaTime);

            // Check if rotation is done
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                isRotating = false;
            }
        }

        // Pause before moving to next destination
        if (hasArrived)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration)
            {
                currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Count;
                MoveToNextDestination();
                failSafe = 0f;
            }
        }
    }

    void MoveToNextDestination()
    {
        theAgent.SetDestination(destinations[currentDestinationIndex].position);
        hasArrived = false;
        isRotating = false;
    }
}
