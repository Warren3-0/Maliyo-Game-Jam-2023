using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    private float damage;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}