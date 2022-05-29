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
    [SerializeField] float chaseRange = 20f;
    //shield
    [SerializeField] Powershield powershield;
    //
    //enemy jump
    [SerializeField] float jumpCoolDown = 0.0f;
    [SerializeField] float nextJumpTime = 5f;
    [SerializeField] float minJumpDistance = 3f;
    [SerializeField] float maxJumpDistance = 30f;
    [SerializeField] AnimationCurve heightCurve;
    [SerializeField] Transform AoeIndicator;
    private bool Notjumped = true;
    //
    // laser spin
    private float laserScale = 1f;
    [SerializeField] Transform laser;
    [SerializeField] float laserSize = 30f;
    [SerializeField] float laserCoolDown = 0.0f;
    [SerializeField] float nextLaser = 15f;
    [SerializeField] float laserSpeed = 90f;
    private bool Notlasered = false;
    //deal damage distance
    [SerializeField] float damageRange = 10f;
    [SerializeField] float bossDamage = 3f;
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
        jumpCoolDown+= Time.deltaTime;
        laserCoolDown+=Time.deltaTime;
        switch (state)
        {
            //distanceToTarget = Vector3.Distance(target.position, transform.position);

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
                StartCoroutine(jAttack(bossBody,target));
                
                Notjumped = false;
                Notlasered = true;
                jumpCoolDown = 0f;
                
                break;
            case State.laserSpin:
                facePlayer();
                StartCoroutine(spin(bossBody, laser));
                
                Notlasered = false;
                Notjumped = true;
                laserCoolDown = 0f;
                
                break;
            case State.dead:
                bossDie();
                break;
            
        }
        

    }
    private IEnumerator spin(Transform boss, Transform laser)
    {
        float duration = 1f;
        nav.enabled = false;
        Vector3 aimposition = target.position;
        yield return new WaitForSeconds(.2f);
        JumpAndSpinDamageToPlayer();
        laserScale += laserSpeed*Time.deltaTime;
        
        laser.localScale = new Vector3(1, 1, laserScale);
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
        
        float distance = Vector3.Distance(boss.position, player.position);
        return jumpCoolDown > nextJumpTime && distance >= minJumpDistance && distance <= maxJumpDistance;
    }
    private void JumpAndSpinDamageToPlayer()
    {
        float distance = Vector3.Distance(bossBody.position, target.position);
        print("the distance between boss and player "+distance);
        if(distance<=damageRange)
        {
            playerHealth player = target.GetComponent<playerHealth>();
            if(player == null) return;
            player.playerTakeDamge(bossDamage);
        }
    }

    private IEnumerator jAttack(Transform boss,Transform target)

    {
        nav.enabled = false;
        Vector3 startPosition = boss.position;
        Vector3 jumpDestination = target.position;
        AoeIndicator.gameObject.SetActive(true);
        AoeIndicator.position = jumpDestination;
        yield return new WaitForSeconds(0.5f);

        for (float time= 0; time <1; time += Time.deltaTime*1.0f)
        {
            boss.position = Vector3.Lerp(startPosition,jumpDestination,time)
                +Vector3.up*heightCurve.Evaluate(time);
            yield return null;
        }
        AoeIndicator.gameObject.SetActive(false);
        JumpAndSpinDamageToPlayer();
        if (NavMesh.SamplePosition(jumpDestination, out NavMeshHit hit, 1f,nav.areaMask))
        {
            nav.Warp(hit.position);
        }
        nav.enabled = true;
        state = State.unshieldedMovingWithShooting;
    }
    

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
        float distance = Vector3.Distance(bossBody.position, target.position);

        laser.localScale = new Vector3(1, 1, 1);
        if (distance <= chaseRange+15)
        {
            nav.SetDestination(target.position);
        }
        
        if (distance >= chaseRange && laserCoolDown >= nextLaser && Notlasered)
        {
            
            
            state = State.laserSpin;
        }
        else if (distance >= chaseRange && jumpCoolDown>= nextJumpTime && Notjumped)
        {
            
            
            state = State.jumpAttack;
        }
    }

    private void bossDie()
    {
        bossBody.gameObject.SetActive(false);
    }
}
