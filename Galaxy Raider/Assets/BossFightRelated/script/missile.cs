using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] Transform launchPlace;
    private float MissileCoolDown = 0.0f;
    private float nextLunach = 3f;
    public float missileSpeed = 3f;
    private void Update()
    {
        MissileCoolDown += Time.deltaTime;
        if (MissileCoolDown > nextLunach)
        {
            MissileCoolDown = 0;
            GameObject rocket = Instantiate(missilePrefab, launchPlace.position, missilePrefab.transform.rotation);
            rocket.transform.LookAt(player);
            StartCoroutine(SendHomingMissile(rocket));

        }

    }
    public IEnumerator SendHomingMissile(GameObject rocket)
    {
        while (Vector3.Distance(player.position, rocket.transform.position) > 0.3f)
        {
            rocket.transform.position += (player.position - rocket.transform.position).normalized * missileSpeed * Time.deltaTime;
            rocket.transform.LookAt(player);
            yield return null;
        }
        Destroy(rocket);
    }
    // [SerializeField] private Transform targetCube;
    // [SerializeField] private float force;
    // [SerializeField] private float rotationForce;
    // [SerializeField] private float secondsBeforeHoming;
    // [SerializeField] private float launchForce;
    // [SerializeField] GameObject particleEffect;
    // private bool shouldFollow;
    // private Rigidbody rb;

    // private void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    //     StartCoroutine(WaitBeforeHoming());

    // }
    // private void FixedUpdate()
    // {
    //     if (shouldFollow)
    //     {
    //         if (targetCube != null)
    //         {
    //             Vector3 direction = targetCube.position - rb.position;
    //             direction.Normalize();
    //             Vector3 rotationAmount = Vector3.Cross(transform.up, direction);
    //             rb.angularVelocity = rotationAmount * rotationForce;
    //             rb.velocity = transform.up * force;
    //         }
    //     }
    // }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     //Destroy(collision.collider.gameObject);
    //     Destroy(gameObject);
    // }

    // private IEnumerator WaitBeforeHoming()
    // {
    //     rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
    //     yield return new WaitForSeconds(secondsBeforeHoming);
    //     shouldFollow = true;
    // }
}