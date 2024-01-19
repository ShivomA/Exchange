using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 5;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;

        Destroy(gameObject, 5);
    }
}
