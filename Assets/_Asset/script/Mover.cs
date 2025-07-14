using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mover : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    public Animator anim;
    public Control_attack control_Attack;

    public bool ismoverattck;
    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        ismoverattck = false;

    }

    // Update is called once per frame
    void Update()
    {



        float hoz = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(hoz, 0, ver).normalized;

        // được di chuyenr (ko có mục tiêu )
        if (!control_Attack.attack_target)
        {
            //nếu di chuyển thì ko tấn công 
            if (moveDirection.magnitude > 0.01f)
            {
                if (control_Attack.isAttacking == false)
                {

                    //khóa tấn công bật di chuyển 
                    control_Attack.isMover = true;
                    control_Attack.isAllowsAttack = false;

                }
            }
            // nếu 0 di chuyển thì đc tấn công 
            else
            {
                control_Attack.isAllowsAttack = true;

                // (nếu tấn công xét b = false khóa di chuyển đến khi tấn công xong )
                //tấn công 
                if (control_Attack.isAttacking == true)
                {
                    control_Attack.isMover = false;
                    if (!ismoverattck)
                    {
                        anim.SetFloat("mover", 0);
                    }

                }
                //else
                //{
                //    control_Attack.isMover = true;
                //}   
            }
        }
        // nếu có mục tiêu 
        else
        {
            // control_Attack.isMoverAttack = true;

            // di chuyển được phép tấn công 
            if (moveDirection.magnitude > 0.01f)
            {
                //xét xem có đang tấn công hay ko 
                if (control_Attack.isAttacking == false)
                {

                    // được phép di chuyển khi ko tấn công 
                    control_Attack.isMover = true;
                }
                else
                {
                    //ko được phép di chuyển khi  tấn công 
                    control_Attack.isMover = false;
                    if (!ismoverattck)
                    {
                        anim.SetFloat("mover", 0);
                    }

                }

                // được phép tấn công 
                control_Attack.isAllowsAttack = true;

            }
            // nếu 0 di chuyển thì đc tấn công 
            else
            {
                //được phép tấn công
                control_Attack.isAllowsAttack = true;

                // (nếu tấn công xét b = false khóa di chuyển đến khi tấn công xong )
                //tấn công 
                if (control_Attack.isAttacking == true)
                {
                    control_Attack.isMover = false;
                    if (!ismoverattck)
                    {
                        anim.SetFloat("mover", 0);
                    }

                }
                //else
                //{
                //    control_Attack.isMover = true;
                //}
            }
        }



        if (control_Attack.isMover == true)
        {

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

    public void movertagget(Vector3 moveTarget)
    {
        float distance = Vector3.Distance(transform.position, moveTarget);
        //Debug.Log(distance);
       
        if (ismoverattck)
        {
            rb.MovePosition(moveTarget);
            anim.SetFloat("mover", moveTarget.magnitude);
        }
        else
        {
            anim.SetFloat("mover",0);
        }

    }
}
