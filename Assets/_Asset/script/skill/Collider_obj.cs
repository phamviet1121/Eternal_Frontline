using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider_obj : MonoBehaviour
{
    public float dame;
    public string nameTag;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameTag))
        {
            Debug.Log("co bat dc ko ");
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                Debug.Log("co gay dame ko ");
                healthSystem.TakeDamage(dame);
            }
         
        }
    }
}
