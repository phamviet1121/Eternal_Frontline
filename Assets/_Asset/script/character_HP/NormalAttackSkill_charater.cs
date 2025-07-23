using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackSkill_charater : Skill
{
    public Attack_character_HP attack_character_HP;
    public override void Use()
    {
        attack_character_HP.NormalAttack();
    }
}
