using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLookAtCamera : MonoBehaviour
{
    [SerializeField] Transform playerView;
    [SerializeField] Transform healthbarCanvas;
    // Update is called once per frame
    void Update()
    {
        healthbarCanvas.LookAt(2 * transform.position - playerView.position);
    }
}
