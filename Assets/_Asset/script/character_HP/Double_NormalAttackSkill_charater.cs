using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Double_NormalAttackSkill_charater : Skill
{
    public UnityEvent UnityEventSkill;
    public Attack_character_HP attack_character_HP;
    public TagBasedDetector tagBasedDetector;
    public Control_attack control_Attack;

    public float cooldown = 5f;

    public float remainingTime;
    private bool canUse = true;
    private void Start()
    {
        // onSkill = false;

    }
    public override void Use()
    {
        if (control_Attack.isAllowsAttack)
        {
            if (!canUse)
            {
                Debug.Log("❌ Kỹ năng đang hồi chiêu...");
                return;
            }

            canUse = false;
            attack_character_HP.DoubleNormalAttack();
            StartCoroutine(CooldownTimer());
        }
    }
    private void Update()
    {
        if (attack_character_HP.b)
        {
            tagBasedDetector.DetectAndRotateEvent(UnityEventSkill);
        }

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
