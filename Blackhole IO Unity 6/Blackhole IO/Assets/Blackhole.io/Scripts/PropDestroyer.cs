using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PropDestroyer : MonoBehaviour
{
    public static UnityAction<Vector3, float> OnPropDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetComponent<Rigidbody>())
        {
            Vector3 propPosition = collision.collider.transform.position;
            float propSizeMagnitude = collision.collider.GetComponent<Renderer>().bounds.size.magnitude;
            
            OnPropDestroyed?.Invoke(propPosition, propSizeMagnitude);

            //collision.collider.gameObject.SetActive(false);
            Destroy(collision.collider.gameObject);
        }
    }
}
