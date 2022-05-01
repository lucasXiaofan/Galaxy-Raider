
using UnityEngine;

public class Weapon_test_code : MonoBehaviour
{
    [SerializeField] Camera FPSCamera;
    [SerializeField] float range = 50f;
    [SerializeField] float damage = 30f;
    [SerializeField] GameObject hiteffect;
    public float bullet_live_time = 3.0f;
    public bool canShoot = true;
    public float fireRate = 15f;
    private float nextTimeToShoot = 0f;





    private void OnEnable()
    {
        canShoot = true;
    }


    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireRate;
            ShootAuto();
        }

    }


    private void ShootAuto()
    {
        processRayCast();
    }

    private void processRayCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward, out hit, range))
        {
            PlayhitEffect(hit);
            Debug.Log(hit.transform.name);
            enmeyHealth target = hit.transform.GetComponent<enmeyHealth>();
            if (target == null)
            {
                return;
            }
            target.takeDamage(damage);
        }
        else
        {
            return;
        }

    }

    private void PlayhitEffect(RaycastHit hit)
    {
        GameObject impact = Instantiate(hiteffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, bullet_live_time);

    }
}
