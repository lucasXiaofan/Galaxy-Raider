using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// STAMINA

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // Stamina only depleted after every Jump.
        {
            StaminaBar.instance.UseStamina(15);

        }
    }
}
