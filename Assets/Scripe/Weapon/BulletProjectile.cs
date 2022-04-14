using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody bulletRigibody;
    public bool shooted;

    [SerializeField]
    private Transform vFXHit;
    [SerializeField]
    private Transform vFXNoHit;

    private void Awake()
    {
        bulletRigibody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        float speed = 10f;
        bulletRigibody.velocity = transform.forward * speed ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BulletTarget"))
        {
            Instantiate(vFXHit, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Untagged"))
        {
            Instantiate(vFXNoHit, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
