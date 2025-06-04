using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    [Header(" Components ")]
    [SerializeField] private DiggableTerrainGenerator diggableTerrainGenerator;
    private Camera mainCamera;

    [Header(" Settings ")]
    [SerializeField] private int holeRadius;
    private Vector3 previousHitPoint;
    private bool canStorePreviousHitPoint = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput();
    }

    private void ManageInput()
    {
        if (Input.GetMouseButton(0))
            TryDigging(Input.mousePosition);
        else if (Input.GetMouseButtonUp(0))
            canStorePreviousHitPoint = true;
    }


    private void TryDigging(Vector3 mousePosition)
    {
        RaycastHit hit;
        Physics.Raycast(mainCamera.ScreenPointToRay(mousePosition), out hit, Mathf.Infinity);

        if (hit.collider != null && hit.collider.GetComponent<RaycastablePlane>())
        {
            if(canStorePreviousHitPoint)
                previousHitPoint = hit.point;

            canStorePreviousHitPoint = false;

            TerrainHitCallback(hit.point);

            previousHitPoint = hit.point;
        }

    }

    private void TerrainHitCallback(Vector3 hitPos)
    {
        diggableTerrainGenerator.DigLine(hitPos, previousHitPoint, holeRadius);
        //diggableTerrainGenerator.DigAtPosition(hitPos, holeRadius, true);
    }
}
