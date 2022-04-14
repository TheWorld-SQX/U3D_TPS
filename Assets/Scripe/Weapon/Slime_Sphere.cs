using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Slime_Sphere : MonoBehaviour
{
    private Rigidbody rigid_SSPhere;
    [SerializeField]
    private float forceOnSS;
    //[SerializeField]
    private GameObject attackTarget;
    [SerializeField]
    private Vector3 attackHigth;
    private Vector3 direction;
    private float deadtime = 10f;
    [SerializeField]
    private Transform vFXHit;
    //public bool isPlayer;
    [SerializeField]
    private CharacterStats slimeData;
    private void Start()
    {
        //slimeData = gameObject.GetComponent<CharacterStats>();
        rigid_SSPhere = GetComponent<Rigidbody>();
        ShootSS();
    }
    private void Update()
    {
        deadtime -= Time.deltaTime;
        if (deadtime <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void ShootSS()
    {
        if (attackTarget == null)
        {
            attackTarget = FindObjectOfType<PlayerController>().gameObject;
        }
        direction = (attackTarget.transform.position + attackHigth - gameObject.transform.position ).normalized;
        rigid_SSPhere.AddForce(direction * forceOnSS, ForceMode.Impulse);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //attackTarget.GetComponent<Animator>().SetTrigger("GetHit");
            if (attackTarget != null && slimeData != null)
            {
                attackTarget.GetComponent<CharacterStats>().TakeDamage(slimeData, attackTarget.GetComponent<CharacterStats>());
            }
            Instantiate(vFXHit, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bullet") == false )
        {
            Destroy(gameObject, 10f);
        }
    }

}
