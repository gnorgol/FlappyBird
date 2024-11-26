using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaCollider : MonoBehaviour
{
    //collider for the arena


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Bird>())
        {
            Debug.Log("Bird has left the arena");
            
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
