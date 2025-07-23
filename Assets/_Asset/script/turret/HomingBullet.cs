using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public float rotateSpeed = 200f;
    public float dame;
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // 🔥 Nếu mục tiêu bị hủy thì hủy đạn
            return;
        }

        // 🔄 Tính hướng bay đến mục tiêu
        Vector3 targetOffset = target.position + Vector3.up * 1f;
        Vector3 direction = (targetOffset - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(dame);
            }
            // 💥 Có thể thêm hiệu ứng nổ, sát thương tại đây
            Destroy(gameObject);
        }
    }
}
