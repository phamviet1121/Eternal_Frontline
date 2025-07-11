using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator anim;
    public int indexAttack;

    private float lastAttackTime = 0f;
    public float comboDelay = 1f;
    public float delayAfterAttack = 0.3f;

    public bool isattack;

    void Start()
    {
        indexAttack = 0;
        isattack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastAttackTime > comboDelay)
        {
            indexAttack = 0;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {

            if (isattack)
            {
                // Gửi trigger dựa trên indexAttack
                if (indexAttack == 0)
                {
                    anim.SetTrigger("attack1_0");

                    isattack = false;
                }
                else if (indexAttack == 1)
                {
                    anim.SetTrigger("attack1_1");
                    isattack = false;
                }
                else if (indexAttack == 2)
                {
                    anim.SetTrigger("attack1_2");
                    isattack = false;
                }


            }

            lastAttackTime = Time.time;
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
        isattack = true;
    }

}
