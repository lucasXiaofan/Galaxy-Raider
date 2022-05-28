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
    public float chaseRange = 20f;
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
    public float laserSize = 30f;

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
                chasePlayer(laser);
                break;
            case State.jumpAttack:
                jAttack();

                break;
            case State.laserSpin:
                StartCoroutine(spin(bossBody, laser));
                facePlayer();
                break;
            case State.dead:
                bossDie();
                break;

        }

    }
    private IEnumerator spin(Transform boss, Transform laser)
    {
        float duration = 2f;
        nav.enabled = false;
        laser.localScale = new Vector3(1, 1, laserSize);
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            boss.eulerAngles = new Vector3(boss.eulerAngles.x, yRotation,
            boss.eulerAngles.z);
            yield return null;
        }
        
        nav.enabled = true;
        yield return new WaitForSeconds(1);
        state = State.unshieldedMovingWithShooting;
    }
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

    private void chasePlayer(Transform laser)
    {
        nav.SetDestination(target.position);
        laser.localScale = new Vector3(1, 1, 1);
        float distance = Vector3.Distance(bossBody.position, target.position);
        if (distance >= chaseRange)
        {
            state = State.laserSpin;
        }
    }

    private void bossDie()
    {
        bossBody.gameObject.SetActive(false);
    }
}
