using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatableObject : MonoBehaviour
{
    Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePhysics()
    {
        if (rig != null)
            return;

        GetComponent<MeshCollider>().convex = true;
        rig = gameObject.AddComponent<Rigidbody>();
    }
}
