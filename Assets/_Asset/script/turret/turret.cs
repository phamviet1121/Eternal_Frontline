using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    public float detectRadius = 10f;
    public string monsterTag = "Monster";
    public float deslay = 1f;
    public float fireRate = 1f; // mỗi bao lâu bắn 1 lần
    public float bulletSpeed = 10f;



    public GameObject gunbarrel;// nong sung

    public GameObject bulletPrefab;//vien dan 

    public GameObject[] gunHeads;

    public Transform[] firePoints; // vị trí bắn đạn

    private List<GameObject> targetList = new List<GameObject>();
    private float fireCooldown;
    private bool canAttack = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectMonstersInRange();
        CleanupTargetList();
        Attack();
    }
    // Quét các đối tượng có tag Monster trong bán kính
    void DetectMonstersInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(monsterTag) && !targetList.Contains(hit.gameObject))
            {
                targetList.Add(hit.gameObject); // thêm theo thứ tự phát hiện
            }
        }
    }

    // Xóa những con quái đã chết hoặc ra khỏi phạm vi
    void CleanupTargetList()
    {
        for (int i = targetList.Count - 1; i >= 0; i--)
        {
            GameObject target = targetList[i];
            if (target == null || Vector3.Distance(transform.position, target.transform.position) > detectRadius)
            {
                targetList.RemoveAt(i);
            }
        }
    }

    void Attack()
    {
        if (targetList.Count == 0 || !canAttack) return;

        GameObject currentTarget = targetList[0];
        if (currentTarget == null) return;

        // Quay về phía mục tiêu
        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;

        direction.y = 0f; // Giữ không xoay theo trục Y nếu không cần
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            gunbarrel.transform.rotation = targetRotation * Quaternion.Euler(0, -90f, 0);

        }


        canAttack = false;
        StartCoroutine(ShootBurst(currentTarget));
    }


    IEnumerator ShootBurst(GameObject currentTarget)
    {
        int bulletsToFire = Mathf.RoundToInt(fireRate);
        float timeBetweenBullets = (fireRate > 1) ? 0.1f : 0f;

        for (int i = 0; i < bulletsToFire; i++)
        {
            foreach (Transform firePoint in firePoints)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                HomingBullet homing = bullet.GetComponent<HomingBullet>();
                if (homing != null)
                {
                    homing.target = currentTarget.transform;
                    homing.speed = bulletSpeed;
                }
            }
            if (fireRate > 1f && gunHeads != null)
            {
                StartCoroutine(RotateGunHead());
            }

            if (timeBetweenBullets > 0f)
                yield return new WaitForSeconds(timeBetweenBullets);
        }

        // Sau khi bắn xong loạt → đợi delay mới được bắn tiếp
        yield return new WaitForSeconds(deslay); // deslay = 2f chẳng hạn
        canAttack = true;
    }
    IEnumerator RotateGunHead()
    {
        float duration = 0.2f;
        float speed = 100f; // độ/giây
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float deltaTime = Time.deltaTime;
            float deltaAngle = speed * deltaTime;

            foreach (GameObject gun in gunHeads)
            {
                if (gun != null)
                    gun.transform.Rotate(Vector3.right * deltaAngle);
            }

            elapsed += deltaTime;
            yield return null;
        }
    }




    //IEnumerator AttackDelay()
    //{
    //    yield return new WaitForSeconds(deslay); // delay 2 giây
    //    canAttack = true;
    //}



    // Vẽ bán kính trong scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }


}
