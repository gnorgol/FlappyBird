using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detect and gain point");
        if (other.gameObject.GetComponent<Bird>())
        {
            EventManager.instance.TriggerGainPoint();
        }
    }
}
