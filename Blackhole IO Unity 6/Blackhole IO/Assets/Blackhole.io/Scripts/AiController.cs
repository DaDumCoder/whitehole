using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetSystems;

public class AiController : MonoBehaviour
{

    [Header(" Managers ")]
    [SerializeField] private Player player;

    [Header(" Physics ")]
    [SerializeField] private Rigidbody rig;
    [SerializeField] private float maxMoveSpeed;
    private bool canMove;

    [Header(" Detection ")]
    [SerializeField] private LayerMask eatableObjectsMask;
    [SerializeField] private float minDetectionRadius;
    [Range(1f, 5f)]
    [SerializeField] private float searchSpeed;
    private float detectionRadius;

    private void Awake()
    {
        TimersManager.OnBeforeGameTimerEnded += StartMoving;
        TimersManager.OnGameTimerEnded += StopMoving;
    }

    private void OnDestroy()
    {
        TimersManager.OnBeforeGameTimerEnded -= StartMoving;
        TimersManager.OnGameTimerEnded -= StopMoving;
    }

    // Start is called before the first frame update
    void Start()
    {
        detectionRadius = minDetectionRadius;
    }

    private void StartMoving()
    {
        canMove = true;
    }

    private void StopMoving()
    {
        canMove = false;
        rig.linearVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            Move();
    }

    private void Move()
    {
        // 1. Get the desired direction
        GameObject closestEatableObject = GetClosestEatableObject();

        if (closestEatableObject == null)
            return;

        Vector3 direction = (closestEatableObject.transform.position.With(y: 0) - transform.position.With(y: 0)).normalized;

        Move(direction);
    }

    private GameObject GetClosestEatableObject()
    {
        Collider[] detectedEatableObjects = GetCloseEatableObjects();

        if (detectedEatableObjects == null)
            return null;

        // 1. Filter the list to remove the objects we cant eat
        List<Collider> potentialEatableObjects = new List<Collider>();

        for (int i = 0; i < detectedEatableObjects.Length; i++)
        {
            Renderer eatableObjectRenderer = detectedEatableObjects[i].GetComponent<Renderer>();

            if (eatableObjectRenderer == null)
                continue;

            float eatableObjectBoundsMagnitude = new Vector2(eatableObjectRenderer.bounds.size.x, eatableObjectRenderer.bounds.size.z).magnitude;

            if (eatableObjectBoundsMagnitude <= player.GetSize() * 1f)
            {
                potentialEatableObjects.Add(detectedEatableObjects[i]);
                detectionRadius = minDetectionRadius;
            }
        }

        if (potentialEatableObjects.Count <= 0)
        {
            detectionRadius += Time.deltaTime * searchSpeed;
            return null;
        }

        return Utilsjet.GetClosestTransformInArray(transform, Utilsjet.ColliderToTransformArray(potentialEatableObjects.ToArray())).gameObject;
    }

    private Collider[] GetCloseEatableObjects()
    {
        Collider[] detectedEatableObjects = Physics.OverlapSphere(transform.position, detectionRadius, eatableObjectsMask);

        if (detectedEatableObjects.Length <= 0)
        {
            detectionRadius += Time.deltaTime * searchSpeed;
            return null;
        }
        

        return detectedEatableObjects;
    }

    private void Move(Vector3 direction)
    {
        Vector3 velocity = new Vector3(direction.x, 0, direction.z) * maxMoveSpeed;
        rig.linearVelocity = velocity;

        //playerRenderer.forward = Vector3.Lerp(playerRenderer.forward, velocity.normalized, .1f * Time.deltaTime * 60);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Mathf.Max(detectionRadius, minDetectionRadius));
    }
}
