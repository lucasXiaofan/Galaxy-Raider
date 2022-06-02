using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLookAtCamera : MonoBehaviour
{
    [SerializeField] Transform playerView;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2 * transform.position - playerView.position);
    }
}
