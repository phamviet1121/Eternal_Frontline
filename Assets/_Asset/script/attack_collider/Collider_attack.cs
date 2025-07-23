using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider_attack : MonoBehaviour
{
    public float dame;
    public string nameTag;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameTag))
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(dame);
            }
            gameObject.SetActive(false);



        }
    }
}
