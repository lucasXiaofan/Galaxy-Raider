using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] Transform target;
    private float rotateSpeed = 5f;
    NavMeshAgent nav;
    enmeyHealth health;
    float distanceToTarget = Mathf.Infinity;
    bool activated = false;
    [SerializeField] Transform bossBody;
    [SerializeField] float enemyDamage = 40f;
    //shield
    [SerializeField] Powershield powershield;
    //
    //enemy jump
    public float jumpCoolDown;
    public float nextJumpTime;
    public float minJumpDistance = 3f;
    public float maxJumpDistance = 30f;
    public AnimationCurve heightCurve;
    [SerializeField] Transform AoeIndicator;
    //
    // laser spin
    [SerializeField] Transform laser;

    //
    private State state;
    private enum State
    {
        shieldedWithMissle,
        unshieldedMovingWithShooting,
        jumpAttack,
        laserSpin,
        dead,

    }

    // Start is called before the first frame update
    void Start()
    {
        AoeIndicator.gameObject.SetActive(false);
        nav = GetComponent<NavMeshAgent>();
        health = GetComponent<enmeyHealth>();
        state = State.shieldedWithMissle;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.isDead())
        {
            enabled = false;
            nav.enabled = false;
            state = State.dead;
        }
        switch (state)
        {
            default:
            case State.shieldedWithMissle:
                facePlayer();
                Shielded();
                break;
            case State.unshieldedMovingWithShooting:
                facePlayer();
                chasePlayer();
                break;
            case State.jumpAttack:
                jAttack();
                break;
            case State.laserSpin:
                //StartCoroutine(spin(bossBody, laser));
                break;
            case State.dead:
                bossDie();
                break;

        }

    }
    // private IEnumerator spin(Transform boss, Transform laser)
    // {

    // }
    private bool canJumpAttack(Transform boss, Transform player)
    {
        jumpCoolDown += Time.deltaTime;
        float distance = Vector3.Distance(boss.position, player.position);
        if (jumpCoolDown > nextJumpTime && distance >= minJumpDistance && distance <= maxJumpDistance)
        {
            return true;
        }
        return false;
    }

    private void jAttack()
    {
        Vector3 jumpDestination = target.position;
        AoeIndicator.gameObject.SetActive(true);
        AoeIndicator.position = jumpDestination;

    }
    // private IEnumerator bossJump(Vector3 targetPos, Transform boss)
    // {
    //     NavMeshAgent.enabled = false;

    // }

    private void Shielded()
    {
        if (powershield.shielded() == false)
        {
            state = State.unshieldedMovingWithShooting;
        }
    }

    private void facePlayer()
    {
        Vector3 curDirection = (target.position - transform.position);
        Quaternion TargetRotation = Quaternion.LookRotation(new Vector3(curDirection.x, 0, curDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.deltaTime * rotateSpeed);
    }

    private void chasePlayer()
    {
        nav.SetDestination(target.position);
    }

    private void bossDie()
    {
        bossBody.gameObject.SetActive(false);
    }
}
