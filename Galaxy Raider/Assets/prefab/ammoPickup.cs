using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickup : MonoBehaviour
{
    [SerializeField] int ammoAmount = 5;
    [SerializeField] ammoType atype;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player pick up the " + ammoAmount + " " + atype + "!");
            //ammo.IncreaseAmmo(ammoType, ammoAmount);
            FindObjectOfType<Ammo>().IncreaseAmmo(atype, ammoAmount);
            Destroy(gameObject);
        }
    }
}
