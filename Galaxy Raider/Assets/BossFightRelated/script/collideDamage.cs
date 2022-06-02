using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collideDamage : MonoBehaviour
{
    [SerializeField] float damage = 50f;
    [SerializeField] playerHealth player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.playerTakeDamge(damage);
        }
    }
}
