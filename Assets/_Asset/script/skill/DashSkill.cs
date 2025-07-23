using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashSkill : Skill
{  

    public Mover mover;
    public Control_attack control_Attack;
    public string nameAnim;
    public float dashDistance = 5f; // Khoảng cách lướt
    public float dashDuration = 1f;

    public float dame;
    public Control_collider_attack control_Collider_Attack;
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
            canUse = false;
            mover.anim.SetBool(nameAnim, true);
            if(control_Collider_Attack!=null )
            {
                control_Collider_Attack.dame=dame;
                control_Collider_Attack.onCollider_Attack();
            }    


            StartCoroutine(PerformDash());
           

        }
    }
    private IEnumerator PerformDash()
    {
       

        Vector3 startPos = transform.position;
        Vector3 targetPos = transform.position + transform.forward * dashDistance;

        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
           
            // Lerp vị trí theo thời gian
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / dashDuration);
            elapsed += Time.deltaTime;
            yield return null; // chờ frame tiếp theo
        }

        // Đảm bảo đến đúng vị trí cuối
        transform.position = targetPos;
        mover.anim.SetBool(nameAnim, false);
        if (control_Collider_Attack != null)
        {
            control_Collider_Attack.offCollider_Attack();
        }
        control_Attack.isAttacking = false;

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
