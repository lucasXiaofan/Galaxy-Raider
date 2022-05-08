using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHit : MonoBehaviour
{
    [SerializeField] Transform attackTarget;
    [SerializeField] float enemyDamage = 40f;
    private void Start()
    {

    }
    public void AttackHitEvent()
    {
        playerHealth target = attackTarget.GetComponent<playerHealth>();
        if (attackTarget == null) return;
        target.playerTakeDamge(enemyDamage);
    }
}
