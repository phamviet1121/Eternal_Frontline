using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSkill : Skill
{
    public override void Use()
    {
        Debug.Log("🔥 Fireball được thi triển!");

        // TODO: Thêm hiệu ứng particle/Instantiate/animation ở đây
        // Ví dụ: Instantiate(fireballPrefab, transform.position, Quaternion.identity);
    }
}
