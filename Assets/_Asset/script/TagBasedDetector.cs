using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagBasedDetector : MonoBehaviour
{
    public float detectRadius = 5f;
    public float rotateRadius = 2f;
    private GameObject targetMonster;

    public float stopDistance = 2f;      // Dừng lại ở khoảng này
    public float moveSpeed = 3f;         // Tốc độ di chuyển
    public float rotateSpeed = 5f;
    public Control_attack control_Attack;
    public Mover mover;
    public Attack attack;

    void OnDrawGizmosSelected()
    {
        // Màu vàng cho vùng bắt Monster (5f)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        // Màu đỏ cho vùng quay mặt (2f)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rotateRadius);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectAndRotate();
    }

    public void DetectAndRotate(/*Animator anim, string name)*/)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);

        float closestDistance = Mathf.Infinity;
        GameObject closestMonster = null;


        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Monster"))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);

                // Bắt con gần nhất trong bán kính 5f
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestMonster = col.gameObject;
                }

                //// Nếu có con gần trong 2f thì quay mặt về hướng đó
                //if (distance <= rotateRadius)
                //{
                //    Vector3 direction = (col.transform.position - transform.position).normalized;
                //    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                //}
            }


        }
        if (closestMonster != null)
        {
            control_Attack.attack_target = true;
            targetMonster = closestMonster;
          //  Debug.Log("Đã bắt con gần nhất: " + targetMonster.name);

            Vector3 direction = (targetMonster.transform.position - transform.position).normalized;
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);

            if (control_Attack.isAttacking && attack.a)
            {
                if (flatDirection.magnitude > 0.01f)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(flatDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
                }
                float distanceToTarget = Vector3.Distance(transform.position, targetMonster.transform.position);
                if (distanceToTarget > stopDistance)
                {
                    mover.ismoverattck = true;
                    Vector3 moveTarget = transform.position + flatDirection * moveSpeed * Time.deltaTime;
                    mover.movertagget(moveTarget);

                    //transform.position += flatDirection * moveSpeed * Time.deltaTime;
                }
                else
                {
                    mover.ismoverattck = false;
                    attack.annimattack();
                    //anim.SetTrigger(name);
                }
            }



        }
        else
        {
            targetMonster = null;
            control_Attack.attack_target = false;
            attack.annimattack();
        }




    }



}
