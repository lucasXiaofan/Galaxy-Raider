using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    [SerializeField] float lightDecayRate = 1f;
    [SerializeField] float lightDecayAngle = 0.1f;
    [SerializeField] int minimumAngle = 30;
    public Light Flashlight;
    private void Start()
    {
        Flashlight = GetComponent<Light>();
    }

    private void Update()
    {
        LightDecay();
        AngleDecay();
    }

    private void LightDecay()
    {
        Flashlight.intensity -= lightDecayRate * Time.deltaTime;
    }

    private void AngleDecay()
    {
        if (Flashlight.spotAngle <= minimumAngle)
        {
            return;
        }
        else
        {
            Flashlight.spotAngle -= lightDecayAngle * Time.deltaTime;
        }

    }



}
