using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powershield : MonoBehaviour
{
    // Start is called before the first frame update
    enmeyHealth shieldHealth;
    bool shieldExist;
    void Start()
    {
        shieldExist = true;
        shieldHealth = GetComponent<enmeyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldHealth.isDead())
        {
            enabled = false;

            transform.gameObject.SetActive(false);
            shieldExist = false;

        }
    }

    public bool shielded()
    {
        return shieldExist;
    }
}
