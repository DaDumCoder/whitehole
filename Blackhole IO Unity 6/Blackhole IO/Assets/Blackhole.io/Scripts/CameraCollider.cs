using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Player player;
    [SerializeField] private Material transparentMaterial;

    private List<Renderer> transparentRenderers = new List<Renderer>();
    private List<int> opaqueBufferIndices = new List<int>();
    private List<Material> coverObjectsMaterials = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessHitRenderers();   
    }

    private void ProcessHitRenderers()
    {
        Renderer[] hitRenderers = GetHitRenderers();

        for (int i = 0; i < hitRenderers.Length; i++)
        {
            if (transparentRenderers.Contains(hitRenderers[i]))
                continue;

            transparentRenderers.Add(hitRenderers[i]);
            coverObjectsMaterials.Add(hitRenderers[i].material);

            hitRenderers[i].material = transparentMaterial;
        }

        for (int k = 0; k < transparentRenderers.Count; k++)
        {
            bool shouldBeOpaque = true;

            for (int j = 0; j < hitRenderers.Length; j++)
            {
                if (hitRenderers[j] == transparentRenderers[k])
                {
                    shouldBeOpaque = false;
                    break;
                }
            }

            if(shouldBeOpaque)
            {
                opaqueBufferIndices.Add(k);

                if(transparentRenderers[k] != null)
                    transparentRenderers[k].material = coverObjectsMaterials[k];
            }
        }

        try 
        {
            for (int i = 0; i < opaqueBufferIndices.Count; i++)
            {
                transparentRenderers.RemoveAt(opaqueBufferIndices[i]);
                coverObjectsMaterials.RemoveAt(opaqueBufferIndices[i]);
            }
        }
        catch 
        {
            Debug.Log("error removing stuff");
        }
        
        opaqueBufferIndices.Clear();

        // 2. Check if we should set previous objects opaque again
    }  

    

    List<Renderer> hitRenderers = new List<Renderer>();
    private Renderer[] GetHitRenderers()
    {
        hitRenderers.Clear();

        float cameraPlayerDistance = Vector3.Distance(transform.position, player.transform.position);

        RaycastHit[] detectedHits = Physics.RaycastAll(transform.position, (player.transform.position - transform.position), cameraPlayerDistance * .99f);

        for (int i = 0; i < detectedHits.Length; i++)
        {
            Renderer objectRenderer = detectedHits[i].collider.GetComponent<Renderer>();

            if (objectRenderer == null)
                continue;

            // Additionnal check, if the object renderer's bounds are smaller than the player, don't add it
            Vector2 projectedBoundsSize = new Vector2(objectRenderer.bounds.size.x, objectRenderer.bounds.size.z);
            if (projectedBoundsSize.magnitude < player.GetSize() * 1.5f)
                continue;
            else
            {
                // Otherwise, add it to the list
                hitRenderers.Add(objectRenderer);
            }

        }

        return hitRenderers.ToArray();
    }
}
