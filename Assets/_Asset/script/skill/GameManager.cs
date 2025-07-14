using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public HeroController currentHero;

    public HeroController hero1;

    void Start()
    {
        SelectHero(hero1); // gán tướng mặc định khi bắt đầu
    }

    private void Awake()
    {
        Instance = this;
    }

    // Gọi để chọn tướng
    public void SelectHero(HeroController hero)
    {
        currentHero = hero;
        Debug.Log("Selected hero: " + hero.name);
    }

    // Gọi kỹ năng theo chỉ số
    public void UseHeroSkill(int index)
    {
        if (currentHero != null)
        {
            currentHero.UseSkill(index);
        }
    }
}
