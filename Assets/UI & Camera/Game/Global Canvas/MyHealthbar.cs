using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyHealthbar : MonoBehaviour
{
    // "our" healthbar on the screen

    private Image healthbarImage;

    void Start()
    {
        healthbarImage = GetComponent<Image>();
    }

    public void UpdateMyHealthBar(float currentHealth, float maxHealth)
    {
        healthbarImage.fillAmount = currentHealth / maxHealth;
    }
}
