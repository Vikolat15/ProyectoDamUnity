using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthBarSlider;
    public TextMeshProUGUI healthBarValueText;
    public int maxHealth = 100;
    public int currHealth;
    void Start()
    {
        currHealth = maxHealth;
    }

    void Update()
    {
        // Actualizar visualmente (Slider y Texto)
        healthBarValueText.text = currHealth.ToString() + "/" + maxHealth.ToString();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currHealth;

    }

    public void changeHealthBar(int hp)
    {
        currHealth = hp;
        healthBarValueText.text = currHealth.ToString() + "/" + maxHealth.ToString();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currHealth;
    }
}