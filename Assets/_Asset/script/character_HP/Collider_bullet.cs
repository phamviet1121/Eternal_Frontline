using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider_bullet : MonoBehaviour
{
    public float dame;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(dame);
            }
            // Huỷ viên đạn
            Destroy(gameObject);

            // Nếu muốn huỷ cả quái:
            // Destroy(other.gameObject);
        }
    }
}
