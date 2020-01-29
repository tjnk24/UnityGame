using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarHandler : MonoBehaviour
{
    public Image ImgHealthBar;

    public void ChangePercentageUI(Damageable damageable)
    {
        float currentPercent;
        float currentAmount;

        float max = ImgHealthBar.fillAmount;
        float maxHealth = damageable.startingHealth;
        float currentHealth = damageable.currentHealth;


        //найти процент currenthealth от maxHealth
        //найти число, соответствующее проценту от max(числа 1)
        //присвоить это число в ImgHealthBar.fillAmount
        if (currentHealth <= 0)
        {
            ImgHealthBar.fillAmount = 0;
            return;
        }

        currentPercent = (currentHealth / maxHealth) * 100;
        currentAmount = max * (currentPercent / 100);

        ImgHealthBar.fillAmount = currentAmount;
    }
}
