using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : Skill
{
    public override void Use()
    {
        Debug.Log("💚 Heal được thi triển!");

        // TODO: Hồi máu, hiển thị hiệu ứng
        // Ví dụ: currentHP += 20; Instantiate(healEffect, ...)
    }
}
