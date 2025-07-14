using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    // Danh sách k? n?ng gán trong Editor
    public List<Skill> skills;

    // G?i k? n?ng theo ch? s?
    public void UseSkill(int index)
    {
        // Ki?m tra index h?p l?
        if (index >= 0 && index < skills.Count)
        {
            Skill skill = skills[index];
            if (skill != null)
                skill.Use();
        }
        else
        {
            Debug.LogWarning("Skill index out of range for hero: " + name);
        }
    }
}
