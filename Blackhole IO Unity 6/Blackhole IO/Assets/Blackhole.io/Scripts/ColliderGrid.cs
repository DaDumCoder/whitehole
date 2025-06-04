using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderGrid : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject colliderPrefab;

    [Header(" Settings ")]
    [SerializeField] private LayerMask collidersMask;
    [SerializeField] private float gridSize;
    [SerializeField] private int xSubdivisions;
    [SerializeField] private int ySubdivisions;


    [Header(" Performace ")]
    HashSet<Collider> disabledColliders = new HashSet<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void GenerateTerrain()
    {
        while (transform.childCount > 0)
        {
            Transform t = transform.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        float xStep = gridSize;
        float zStep = gridSize;

        Vector3 initialPos = Vector3.left * xStep / 2 * (xSubdivisions - 1);

        initialPos.y = -.5f;
        initialPos.z = -zStep / 2 * (ySubdivisions - 1);

        for (int x = 0; x < xSubdivisions; x++)
        {
            for (int y = 0; y < ySubdivisions; y++)
            {
                Vector3 spawnPos = initialPos + x * Vector3.right * xStep + y * Vector3.forward * zStep;

                GameObject colliderInstance = Instantiate(colliderPrefab, spawnPos, Quaternion.identity, transform);
                colliderInstance.transform.localScale = new Vector3(gridSize, 1, gridSize);
            }
        }
    }

    /*
    public void UpdateTerrain(Vector3[] playersPositions, int[] playersRadiuses)
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform currentCollider = transform.GetChild(i);
            bool colliderIsEnabled = true;

            for (int j = 0; j < playersPositions.Length; j++)
            {
                Vector3 currentPlayerPosition = playersPositions[j];

                if(Vector3.Distance(currentCollider.position, currentPlayerPosition) < playersRadiuses[j])
                {
                    currentCollider.gameObject.SetActive(false);
                    colliderIsEnabled = false;
                    break;
                }
            }

            if (colliderIsEnabled)
                currentCollider.gameObject.SetActive(true);
        }
    }
    */

    public void UpdateTerrain(Vector3[] playersPositions, float[] playersRadiuses)
    {
        for (int i = 0; i < playersPositions.Length; i++)
        {
            Collider[] detectedColliders = Physics.OverlapSphere(playersPositions[i], playersRadiuses[i], collidersMask);

            for (int j = 0; j < detectedColliders.Length; j++)
            {
                disabledColliders.Add(detectedColliders[j]);
                detectedColliders[j].gameObject.SetActive(false);
            }
        }

        EnableDisabledColliders(playersPositions, playersRadiuses);
    }

    HashSet<Collider> collidersToRemoveFromDisabledCollidersList = new HashSet<Collider>();
    private void EnableDisabledColliders(Vector3[] playersPositions, float[] playersRadiuses)
    {
        collidersToRemoveFromDisabledCollidersList.Clear();

        foreach (Collider disabledCollider in disabledColliders)
        {
            if (ShouldEnable(disabledCollider, playersPositions, playersRadiuses))
            {
                disabledCollider.gameObject.SetActive(true);
                collidersToRemoveFromDisabledCollidersList.Add(disabledCollider);
            }
        }

        foreach (Collider colliderToUpdate in collidersToRemoveFromDisabledCollidersList)
            disabledColliders.Remove(colliderToUpdate);
    }

    private bool ShouldEnable(Collider disabledCollider, Vector3[] playersPositions, float[] playersRadiuses)
    {
        for (int i = 0; i < playersPositions.Length; i++)
            if (Vector3.Distance(playersPositions[i], disabledCollider.transform.position) < playersRadiuses[i])
                return false;
           
        return true;
    }
}
