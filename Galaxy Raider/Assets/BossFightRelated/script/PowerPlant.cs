using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : MonoBehaviour
{

    enmeyHealth health;
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(true);
        health = GetComponent<enmeyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.isDead() == true)
        {
            print("yeas");
            transform.gameObject.SetActive(false);
        }

    }
}
