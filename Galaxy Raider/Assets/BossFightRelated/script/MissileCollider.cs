using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ground")
        {
            Destroy(gameObject);
        }
    }

}
