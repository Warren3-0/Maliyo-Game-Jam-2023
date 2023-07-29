using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogError("Laser Hit Player");
            Destroy(gameObject);
        }
    }
}