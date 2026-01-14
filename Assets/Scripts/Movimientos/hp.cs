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

    private float timer = 0f; // Variable para contar el tiempo

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

        // Lógica para bajar la vida cada 1 segundo
        if (currHealth > 0)
        {
            timer += Time.deltaTime; // Suma el tiempo que pasa entre frames

            if (timer >= 1f) // Si ha pasado 1 segundo o más
            {
                currHealth -= 1; // Resta 1 de vida
                timer = 0f;      // Reinicia el contador para el siguiente segundo
            }
        }
    }
}