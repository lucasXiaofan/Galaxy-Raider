
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
    public float minJumpDistance = 3f;
    public float maxJumpDistance = 30f;
    public AnimationCurve heightCurve;
    public float jumpSpeed = 2;
    public float coolDown = 0.0f;
    public float nextJumpTime = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxJumpDistance);

    }

    private IEnumerator jumpAttack(Transform enemyTest, Transform target)
    {

        navMeshAgent.enabled = false;
        // having this to store the position temporarily 
        Vector3 startingPosition = enemyTest.position;
        Vector3 jumpEnd = target.position;

        for (float time = 0; time < 1; time += Time.deltaTime * jumpSpeed)
        {
            enemyTest.position = Vector3.Lerp(startingPosition, jumpEnd, time)
                + Vector3.up * heightCurve.Evaluate(time);
            //enemyTest.rotation = Quaternion.Slerp(enemyTest.rotation, Quaternion.LookRotation(target.position - enemyTest.position), time);
            yield return null;
        }
        //reset cooldown each time after jump
        coolDown = 0;
        navMeshAgent.enabled = true;

        if (NavMesh.SamplePosition(target.position, out NavMeshHit hit, 1f, navMeshAgent.areaMask))
        {
            navMeshAgent.Warp(hit.position);
            isProvoked = true;
            Debug.Log("chase mode");

        }
    }

    public bool canJumpAttack(Transform enemyt, Transform player)
    {
        coolDown += Time.deltaTime; // more research about time.deltatime
        //what is the difference between Time.time and Time.deltaTime
        float distance = Vector3.Distance(enemyt.position, player.position);
        // cooldown > nextJumptime 
        if (coolDown > nextJumpTime && distance >= minJumpDistance && distance <= maxJumpDistance)
        {
            return true;
        }
        return false;
    }


    private void EngageTarget()
    {
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            if (distanceToTarget < 9f)
            {
                enemyChase();
                print("engaging" + Time.deltaTime);
            }
            else
            {
                enemyChase();
                if (canJumpAttack(EnemyTest, target))
                {
                    StartCoroutine(jumpAttack(EnemyTest, target));
                    Debug.Log("jumpattack " + Time.time);
                }
            }

        }

    }

    private void enemyChase()
    {
        navMeshAgent.SetDestination(target.position);
    }
}
