using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrbitingObjectsSkill : Skill
{

    public Mover mover;
    public Control_attack control_Attack;

    public GameObject orbitingPrefab;    // Prefab để xoay quanh
    public float orbitRadius = 2f;       // Khoảng cách tới player
    public float rotateSpeed = 90f;      // Tốc độ xoay quanh (độ/giây)
    public int numberOfObjects = 3;      // Số lượng vật thể
    public float duration = 5f;          // thời gian tồn tại 
    public float dame;

    private List<GameObject> orbitingObjects = new List<GameObject>();

    public float cooldown = 30f;
    public float remainingTime;
    private bool canUse = true;
    private Transform playerTransform;
    public override void Use()
    {
        if (!control_Attack.isAttacking)
        {
            if (!canUse)
            {
                Debug.Log("❌ Kỹ năng đang hồi chiêu...");
                return;
            }
            canUse=false;
            playerTransform = this.transform;
            StartCoroutine(SpawnAndOrbit());
          
        }
    }

    private IEnumerator SpawnAndOrbit()
    {
        // Xoá cũ nếu có
        foreach (var obj in orbitingObjects)
            Destroy(obj);
        orbitingObjects.Clear();

        // Tạo các đối tượng xoay quanh
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitRadius;
            offset.y = 1f;
            Vector3 spawnPos = playerTransform.position + offset;

            GameObject go = Instantiate(orbitingPrefab, spawnPos, Quaternion.identity);
            Collider_obj collider_Obj = go.GetComponent<Collider_obj>();
            if(collider_Obj != null)
            {
                collider_Obj.dame = dame;
            }    
           // go.transform.parent = this.transform; // Gắn vào player để xoay quanh
            orbitingObjects.Add(go);
        }

        float timer = 0f;

        // Xoay trong khoảng thời gian
        while (timer < duration)
        {
            float anglePerSecond = rotateSpeed * Mathf.Deg2Rad;

            for (int i = 0; i < orbitingObjects.Count; i++)
            {
                float angle = (i * Mathf.PI * 2 / numberOfObjects) + (anglePerSecond * timer);
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitRadius;
                offset.y = 1f;
                orbitingObjects[i].transform.localPosition = playerTransform.position + offset;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Hết thời gian thì huỷ các object
        foreach (var obj in orbitingObjects)
            Destroy(obj);
        StartCoroutine(CooldownTimer());
    }

    private IEnumerator CooldownTimer()
    {
        remainingTime = cooldown;


        // Debug.Log("⏳ Đang hồi chiêu...");
        while (remainingTime > 0)
        {
            //Debug.Log($"⏳ Còn lại: {remainingTime:F1}s"); // In 1 chữ số thập phân
            yield return new WaitForSeconds(0.1f);       // Cập nhật mỗi 0.1 giây
            remainingTime -= 0.1f;
        }
        canUse = true;
        //Debug.Log("✅ Kỹ năng sẵn sàng!");
    }

}
