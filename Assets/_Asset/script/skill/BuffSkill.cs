using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : Skill
{
    public Mover mover;
    public Control_attack control_Attack;
    public float addspeed;
    public float addspeedAnim;
    public float duration = 3f;
    public float cooldown = 5f;

    private float originalSpeed;
    public float remainingTime;
    private bool canUse = true;

    private void Start()
    {
        mover.anim.SetFloat("addspeed", 1f);
        originalSpeed = mover.speed;
        canUse = true;
    }
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
            // originalSpeed = mover.speed;
            mover.speed += addspeed;
            mover.anim.SetFloat("addspeed", addspeedAnim);
            mover.anim.SetTrigger("roar");
            canUse = false;

            StartCoroutine(ResetBuffAfterDelay());
            // StartCoroutine(CooldownTimer());
        }

    }

    private System.Collections.IEnumerator ResetBuffAfterDelay()
    {
        yield return new WaitForSeconds(duration);

        // Khôi phục tốc độ ban đầu
        mover.speed = originalSpeed;

        // Reset animation (tuỳ theo thiết kế animator của bạn)
        mover.anim.SetFloat("addspeed", 1f); // Hoặc giá trị bình thường
        //Debug.Log("⏳ Buff đã hết, trở lại trạng thái bình thường");
        StartCoroutine(CooldownTimer());
    }

    // Hết hồi chiêu sau Y giây
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
