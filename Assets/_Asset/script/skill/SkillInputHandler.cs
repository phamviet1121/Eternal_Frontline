using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInputHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            GameManager.Instance.UseHeroSkill(0);

        if (Input.GetKeyDown(KeyCode.U))
            GameManager.Instance.UseHeroSkill(1);

        if (Input.GetKeyDown(KeyCode.I))
            GameManager.Instance.UseHeroSkill(2);

        if (Input.GetKeyDown(KeyCode.O))
            GameManager.Instance.UseHeroSkill(3);
    }
}
