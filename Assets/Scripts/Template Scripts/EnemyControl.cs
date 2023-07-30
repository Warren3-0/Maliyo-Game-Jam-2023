using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera mainCamera;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float bulletLifetime = 3f;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot(); 
        }
    }

    public void Shoot()
    {
        RaycastHit hit;
        Vector3 shootDirection;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            shootDirection = (hit.point - transform.position).normalized;
            EnemyController enemy  = hit.transform.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            else
            {
                Asteroid asteroid = hit.transform.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.TakeDamage(damage);
                }
            }
        }
        else
        {
            shootDirection = transform.forward;
        }
    }
}