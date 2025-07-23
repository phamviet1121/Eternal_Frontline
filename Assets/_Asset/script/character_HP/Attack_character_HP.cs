using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_character_HP : MonoBehaviour
{
    public Animator anim;

    public float delayAfterAttack = 0.3f;
    public float delayatt;

    public bool isattack;
    //public bool a;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Control_attack control_Attack;
    public TagBasedDetector tagBasedDetector;
    public float bulletSpeed = 10f;

    void Start()
    {

        isattack = true;
        control_Attack.a = false;

    }



    public void NormalAttack()
    {
        if (isattack && control_Attack.isAllowsAttack && !control_Attack.isAttacking)
        {
            control_Attack.a = true;
            isattack = false;
            control_Attack.isAttacking = true;

        }
    }

    public bool b;
    public void DoubleNormalAttack()
    {
        if (isattack && control_Attack.isAllowsAttack && !control_Attack.isAttacking)
        {
            b = true;
            isattack = false;
            control_Attack.isAttacking = true;

        }
    }
    public void Doubleannimattack()
    {
        if (b == true)
        {
            anim.SetTrigger("attack2");
            isattack = false;
            b = false;

        }


    }


    public void annimattack()
    {
        if (control_Attack.a == true)
        {
            anim.SetTrigger("attack1_0");
            isattack = false;
            control_Attack.a = false;

        }


    }
    public void spambullet()
    {

        Vector3 direction;

        if (tagBasedDetector.closestMonster != null)
        {
            // Tính hướng đến mục tiêu gần nhất
            Transform target = tagBasedDetector.closestMonster.transform;
            direction = (target.position - firePoint.position).normalized;
        }
        else
        {
            // Bắn thẳng về phía trước
            direction = firePoint.forward;
        }

        // Tạo viên đạn tại firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));

        // Gắn velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }

        // Hủy sau 1 giây
        Destroy(bullet, 1f);



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





    public void isAtacking()
    {
        control_Attack.isAttacking = true;
    }
    public void isNotAtacking()
    {
        StartCoroutine(Attackdelay(delayatt));
    }
    IEnumerator Attackdelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        control_Attack.isAttacking = false;
    }
}
