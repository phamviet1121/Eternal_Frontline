using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject damageTextPrefab;
    public Transform textSpawnPoint;
    public Transform canvasTransform;

    public UnityEvent event_die;
    public UnityEvent event_hurt;
    public bool isdie;

    void Start()
    {
        isdie = false;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
    // hien thi 
    void LateUpdate()
    {
        // Giữ scale dương
        Vector3 scale = healthSlider.transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        healthSlider.transform.localScale = scale;

        // Giữ nguyên rotation hoặc luôn quay về camera
        healthSlider.transform.forward = Camera.main.transform.forward;
    }
    // tru mau 
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;

        ShowDamageText(damage); // gọi text

        event_hurt.Invoke();

        // timeSinceLastDamage = 0f;
        if (currentHealth <= 0 && isdie == false)
        {
            Die();
            isdie = true;
        }
    }

    void Die()
    {
        // Debug.Log("Chết rồi!");
        event_die.Invoke();
        // Xử lý chết ở đây (ẩn object, respawn, game over...)
    }
    // text dinh dame
    void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            GameObject textObj = Instantiate(damageTextPrefab, textSpawnPoint.position, Quaternion.identity, canvasTransform);
            TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = damage.ToString("F0"); // hiện sát thương là số nguyên
            }
        }
    }
    // txt hoi mau
    void ShowHealingText(float healing)
    {
        if (damageTextPrefab != null)
        {
            GameObject textObj = Instantiate(damageTextPrefab, textSpawnPoint.position, Quaternion.identity, canvasTransform);
            TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.color = Color.green;
                tmp.text = " + " + healing.ToString("F0"); // hiện sát thương là số nguyên

            }
        }
    }
    //txt them mau
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;

        // Nếu muốn tăng máu hiện tại theo lượng mới

        //currentEnergy += amount;


        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Cập nhật UI
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // Gọi hồi máu hiển thị text (tuỳ chọn)

        ShowHealingText(amount);

    }


}
