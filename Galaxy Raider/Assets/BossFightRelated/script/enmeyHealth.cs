using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enmeyHealth : MonoBehaviour
{
    [SerializeField] float hitPoint = 200f;
    bool Dead = false;
    public bool isDead()
    {
        return Dead;
    }
    public void takeDamage(float damage)
    {
        hitPoint -= damage;

        if (hitPoint <= 0)
        {
            if (Dead) return;
            Dead = true;
        }
        print(" from enemy health" + isDead());
    }

}
