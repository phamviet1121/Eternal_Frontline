using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackSkill : Skill
{
    public Attack attack;
    public override void Use()
    {
        attack.NormalAttack();
    }
}
