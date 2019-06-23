using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlate : MonoBehaviour
{
    [SerializeField]
    public bool isTriggered = false;
 
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            GetComponentInParent<TriggerGateControl>().Triggered(gameObject);
        }
    }

    public bool GetTrigger()
    {
        return isTriggered;
    }
}
