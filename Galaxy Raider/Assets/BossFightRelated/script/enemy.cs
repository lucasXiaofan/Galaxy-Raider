
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class enemy : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float rotateSpeed = 5f;
    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    // experiment for jump attack
    [SerializeField] Transform EnemyTest;
    [SerializeField] float enemyDamage = 40f;
    public float minJumpDistance = 3f;
    public float maxJumpDistance = 30f;
    public AnimationCurve heightCurve;
    public float jumpSpeed = 1;
    [SerializeField] float coolDown = 0.0f;
    public float nextJumpTime = 3.0f;
    //health
    enmeyHealth health;
    [SerializeField] Transform AoeIndicator;
    bool isJumping = false;


    // Start is called before the first frame update
    void Start()
    {
        AoeIndicator.gameObject.SetActive(false);
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<enmeyHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isJumping) {
            return;
        }
        
        if (health.isDead())
        {
            enabled = false;
            navMeshAgent.enabled = false;
            EnemyTest.gameObject.SetActive(false);

        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);


        if (isProvoked) //&& !canJumpAttack(EnemyTest, target))
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
        else
        {
            isProvoked = false;
        }

        coolDown += Time.deltaTime;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxJumpDistance);

    }

    private IEnumerator jumpAttack(Transform enemyTest, Transform target)
    {
        isJumping = true;
        navMeshAgent.enabled = false;
        // having this to store the position temporarily 
        Vector3 startingPosition = enemyTest.position;
        Vector3 jumpEnd = target.position;
        AoeIndicator.gameObject.SetActive(true);
        AoeIndicator.position = jumpEnd;
        yield return new WaitForSeconds(1);

        for (float time = 0; time < 1; time += Time.deltaTime * jumpSpeed)
        {
            enemyTest.position = Vector3.Lerp(startingPosition, jumpEnd, time)
                + Vector3.up * heightCurve.Evaluate(time);
            //enemyTest.rotation = Quaternion.Slerp(enemyTest.rotation, Quaternion.LookRotation(target.position - enemyTest.position), time);
            yield return null;
        }
        //reset cooldown each time after jump
        AoeIndicator.gameObject.SetActive(false);

        if (NavMesh.SamplePosition(jumpEnd, out NavMeshHit hit, 1f, navMeshAgent.areaMask))
        {
            navMeshAgent.Warp(hit.position);
        }
        navMeshAgent.enabled = true;
        isJumping = false;
    }


    public bool canJumpAttack(Transform enemyt, Transform player)
    {
        float distance = Vector3.Distance(enemyt.position, player.position);
        // cooldown > nextJumptime 
        return coolDown > nextJumpTime && distance >= minJumpDistance && distance <= maxJumpDistance;   
    }


    private void EngageTarget()
    {
        FaceTarget();

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            enemyAttack();
            return;
        }

        if (distanceToTarget < 9f)
        {
            enemyChase();
            return;
        }
        
        if (canJumpAttack(EnemyTest, target))
        {
            StartCoroutine(jumpAttack(EnemyTest, target));
            coolDown = 0;
        }
    }

    private void enemyAttack()
    {

        GetComponent<Animator>().SetBool("Spin", true);
        GetComponent<Animator>().SetBool("Normal", false);
    }

    private void enemyChase()
    {

        navMeshAgent.SetDestination(target.position);
        GetComponent<Animator>().SetBool("Spin", false);
        GetComponent<Animator>().SetBool("Normal", true);
    }
    void FaceTarget()
    {
        Vector3 curDirection = (target.position - transform.position);
        Quaternion TargetRotation = Quaternion.LookRotation(new Vector3(curDirection.x, 0, curDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.deltaTime * rotateSpeed);
    }
}
