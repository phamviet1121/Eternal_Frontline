using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackSkill : Skill
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int bulletCount = 10;
    public float angleRange = 30f; // góc hình quạt
    public float bulletSpeed = 10f;
    public float fireRate = 0.1f; // thời gian giữa các viên
    public float timeBulletPrefab = 1f;

    public Control_attack control_Attack;
    public TagBasedDetector tagBasedDetector;
    public Animator anim;
    public string nameAnim;

    public float cooldown = 5f;

    public float remainingTime;
    private bool canUse = true;
    public override void Use()
    {
        if (!control_Attack.isAttacking)
        {
            if (!canUse)
            {
                Debug.Log("❌ Kỹ năng đang hồi chiêu...");
                return;
            }
            control_Attack.isAttacking = true;
            control_Attack.isMover = false;
            canUse = false;
            anim.SetBool(nameAnim, true);
            StartCoroutine(FireBullets());
        }
    }

    IEnumerator FireBullets()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            // Góc ngẫu nhiên trong phạm vi ±angleRange/2
            float randomAngle = Random.Range(-angleRange / 2, angleRange / 2);
            Quaternion rotation;
            // Tính hướng bắn mới dựa trên góc xoay quanh trục Y
            if (tagBasedDetector.closestMonster != null)
            {
                Vector3 direction1 = tagBasedDetector.closestMonster.transform.position - transform.position;
                direction1.y = 0f;
                if (direction1 != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction1);

                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 50f);
                }
            }

            rotation = Quaternion.Euler(0, randomAngle, 0);
           // Debug.Log($"{rotation}");
            Vector3 direction = rotation * firePoint.forward;

            // Tạo viên đạn
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));

            // Gắn velocity cho viên đạn
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction.normalized * bulletSpeed;
            }
            Destroy(bullet, timeBulletPrefab);
            yield return new WaitForSeconds(fireRate);
        }
        anim.SetBool(nameAnim, false);
        StartCoroutine(delayAttack());
        StartCoroutine(CooldownTimer());
    }

    private IEnumerator delayAttack()
    {
        yield return new WaitForSeconds(0.3f);
        control_Attack.isAttacking = false;
        control_Attack.isMover = true;
    }
    private IEnumerator CooldownTimer()
    {
        remainingTime = cooldown;
        while (remainingTime > 0)
        {

            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }
        canUse = true;

    }

}
