using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator anim;
    public int indexAttack;
    public float dame;


    private float lastAttackTime = 0f;
    public float comboDelay = 1f;
    public float delayAfterAttack = 0.3f;
    public float delayatt;

    public bool isattack;
  //  public bool a;

    public Control_attack control_Attack;
    public Control_collider_attack control_Collider_Attack;
    // public TagBasedDetector tagBasedDetector;

    void Start()
    {
        indexAttack = 0;
        isattack = true;
        control_Attack.a = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - lastAttackTime > comboDelay)
        {
            indexAttack = 0;
        }
        //if (Input.GetKeyDown(KeyCode.K))
        //{
          
        //    if (isattack && control_Attack.isAllowsAttack)
        //    {  
        //        a = true;
        //        // control_Attack.ismoverAttack=true;
        //        // Gửi trigger dựa trên indexAttack
        //        if (indexAttack == 0)
        //        {

        //            // anim.SetTrigger("attack1_0");
        //            isattack = false;
        //            control_Attack.isAttacking = true;
        //            //tagBasedDetector.DetectAndRotate(anim, "attack1_0");
        //        }
        //        else if (indexAttack == 1)
        //        {
        //            // anim.SetTrigger("attack1_1");
        //            isattack = false;
        //            control_Attack.isAttacking = true;
        //            //tagBasedDetector.DetectAndRotate(anim, "attack1_1");
        //        }
        //        else if (indexAttack == 2)
        //        {
        //            // anim.SetTrigger("attack1_2");
        //            isattack = false;
        //            control_Attack.isAttacking = true;
        //            //tagBasedDetector.DetectAndRotate(anim, "attack1_2");
        //        }


        //    }

        //    lastAttackTime = Time.time;
        //}


    }

    public void NormalAttack()
    {
        if (isattack && control_Attack.isAllowsAttack&&!control_Attack.isAttacking)
        {
            control_Attack.a = true;
            // control_Attack.ismoverAttack=true;
            // Gửi trigger dựa trên indexAttack
            if (indexAttack == 0)
            {

                // anim.SetTrigger("attack1_0");
                isattack = false;
                control_Attack.isAttacking = true;
                //tagBasedDetector.DetectAndRotate(anim, "attack1_0");
            }
            else if (indexAttack == 1)
            {
                // anim.SetTrigger("attack1_1");
                isattack = false;
                control_Attack.isAttacking = true;
                //tagBasedDetector.DetectAndRotate(anim, "attack1_1");
            }
            else if (indexAttack == 2)
            {
                // anim.SetTrigger("attack1_2");
                isattack = false;
                control_Attack.isAttacking = true;
                //tagBasedDetector.DetectAndRotate(anim, "attack1_2");
            }


        }

        lastAttackTime = Time.time;
    }    







  //  int b;
    public void annimattack()
    {
        if (control_Attack.a == true)
        {
           // b++;
          //  Debug.Log(b);


            if (indexAttack == 0 && control_Attack.isAttacking)
            {
                anim.SetTrigger("attack1_0");
                control_Collider_Attack.dame = dame ;
                //control_Collider_Attack.onCollider_Attack(dame);
                isattack = false;

            }
            else if (indexAttack == 1 && control_Attack.isAttacking)
            {
                anim.SetTrigger("attack1_1");
                control_Collider_Attack.dame = dame + dame * 0.2f;
                //control_Collider_Attack.onCollider_Attack();
                isattack = false;

            }
            else if (indexAttack == 2 && control_Attack.isAttacking)
            {
                anim.SetTrigger("attack1_2");
                control_Collider_Attack.dame = dame + dame * 0.5f;
                // control_Collider_Attack.onCollider_Attack();
                isattack = false;

            }
            control_Attack.a = false;

        }


    }
    public void delay()
    {
        StartCoroutine(AttackCooldown(delayAfterAttack));
    }
    IEnumerator AttackCooldown(float delay)
    {
        // Chờ animation kết thúc, thường bạn dùng thời lượng clip hoặc animation event để xác nhận
        // Sau đó chờ thêm 0.3s
        yield return new WaitForSeconds(delay);
        isattack = true;
    }


    public void onattack1_1()
    {
        indexAttack = 2;
    }
    public void onattack1_2()
    {
        indexAttack = 0;
    }
    public void onattack1_0()
    {
        indexAttack = 1;
    }

    public void onAttack()
    {
        //isattack = true;
    }
    public void isAtacking()
    {
        control_Attack.isAttacking = true;
    }
    public void isNotAtacking()
    {
        StartCoroutine(Attackdelay(delayatt));
        // control_Attack.isAttacking = false;
    }
    IEnumerator Attackdelay(float delay)
    {

        // Chờ animation kết thúc, thường bạn dùng thời lượng clip hoặc animation event để xác nhận
        // Sau đó chờ thêm 0.3s
        yield return new WaitForSeconds(delay);
        control_Attack.isAttacking = false;
    }
}
