using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI chargeText;

    public void UpdateHealthHUD(int current, int max)
    {
        healthText.text = current.ToString() + "/" + max.ToString();
        healthSlider.value = (float) current / max;
    }

    public void UpdateChargeHUD(int current, int max)
    {
        chargeText.text = current.ToString() + "/" + max.ToString();
    }

    public void DisableHUD()
    {
        gameObject.SetActive(false);
    }

}
