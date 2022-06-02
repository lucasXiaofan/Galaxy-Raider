using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Image healthbar;
    public void UpdateHealth(float fraction)
    {
        healthbar.fillAmount = fraction;
    }
}
