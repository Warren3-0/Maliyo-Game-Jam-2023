using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera mainCamera;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public ParticleSystem muzzleFlash;

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
        muzzleFlash.Play();
        RaycastHit hit;
        Vector3 direction;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            EnemyController enemy = hit.transform.GetComponent<EnemyController>();
            direction = hit.point - transform.position;
            /*if (enemy != null)
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
            }*/
        }
        else
        {
            direction = transform.forward;
        }
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Destroy(bullet, 2f);
        StartCoroutine(MoveBullet(bullet, direction));
    }

    private IEnumerator MoveBullet(GameObject bullet, Vector3 direction)
    {
        float distance = 0f;

        while (distance < range)
        {
            float moveDistance = bulletSpeed * Time.deltaTime;
            bullet.transform.position += direction.normalized * moveDistance;
            distance += moveDistance;

            Collider[] colliders = Physics.OverlapSphere(bullet.transform.position, 0.05f);
            foreach (Collider collider in colliders)
            {
                EnemyController enemy = collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Destroy(bullet);
                    yield break;
                }

                Asteroid asteroid = collider.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    asteroid.TakeDamage(damage);
                    Destroy(bullet);
                    yield break;
                }
            }

            yield return null;
        }

        Destroy(bullet);
    }
}