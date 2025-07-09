using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    public Animator anim;
    void Start()
    {
       // rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float hoz = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(hoz, 0, ver).normalized;
        if (moveDirection.magnitude > 0.01f)
        {
            // Tạo hướng quay theo hướng di chuyển
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Chỉ giữ lại góc quay quanh trục Y (khóa X, Z)
            Vector3 euler = targetRotation.eulerAngles;
            euler.x = 0;
            euler.z = 0;
            targetRotation = Quaternion.Euler(euler);

            // Quay vật thể mượt theo hướng đó
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        Vector3 targetPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);
        anim.SetFloat("mover", moveDirection.magnitude);


    }
}
