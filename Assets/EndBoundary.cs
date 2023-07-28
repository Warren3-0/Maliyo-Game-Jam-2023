using UnityEngine;

public class EndBoundary : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Obstacle") || collider.CompareTag("Enemy"))
        {
                Destroy(collider.gameObject);
        }
    }
}