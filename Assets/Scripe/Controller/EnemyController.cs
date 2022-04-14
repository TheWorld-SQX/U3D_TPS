using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public enum EnemyState { GUARD, PATROL, CHASE, DEAD };
    private EnemyState enemyState;
    private NavMeshAgent agent;

    [Header("EnemySetting")]
    [SerializeField]
    private float sightRadius;
    [SerializeField]
    private bool isGuard;

    private Animator anim;

    private bool isRun;
    private bool isChase;
    private bool isFollow;
    private bool isDead;

    private float speed;

    [Header("Patrol State")]
    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 guardPostion;
    private Quaternion guardRotation;

    //Ѳ��ʱѲ��ʱ�� ����ʱ��
    [SerializeField]
    private float lookTime;
    private float remainLookAtTime;

    //��������
    private GameObject attackTarget;

    private CharacterStats characterStats;

    private float lastAttackTime;
    public GameObject slime_bullet;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //Ĭ���ٶ�Ϊ�����ٶ�
        speed = agent.speed;
        guardPostion = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookTime;

        characterStats = GetComponent<CharacterStats>();
    }
    private void Start()
    {
        if (isGuard)
        {
            enemyState = EnemyState.GUARD;
        }
        else
        {
            enemyState = EnemyState.PATROL;
            //��ʼ�����Ѳ��ʱ ������һ������Ϊ��ʼλ��
            GetNewWayPoint();
        }
    }
    private void Update()
    {
        SwitchState();
        SwitchAnimator();
        lastAttackTime -= Time.deltaTime;

        if (characterStats.CurrentHealth == 0)
        {
            isDead = true;
        }
    }

    //�л�����
    private void SwitchAnimator()
    {
        anim.SetBool("Run", isRun);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Death", isDead);
    }

    private void SwitchState()
    {
        if (isDead)
        {
            enemyState = EnemyState.DEAD;
        }
        else if (FoundPlayer())
        {
            enemyState = EnemyState.CHASE;
            //Debug.Log("find player postion");
        }

        switch (enemyState)
        {
            case EnemyState.GUARD:
                isChase = false;
                if (transform.position != guardPostion)
                {
                    isRun = true;
                    agent.isStopped = false;
                    //��ԭ��λ���߹�ȥ
                    agent.destination = guardPostion;
                    //�ߵ�ԭλ��ֹͣ
                    if (Vector3.SqrMagnitude(guardPostion - transform.position) <= agent.stoppingDistance)
                    {
                        isRun = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.02f);
                    }
                }
                break;
            case EnemyState.PATROL:
                //TODO:Ѳ��
                isChase = false;
                agent.speed = speed * 0.5f;
                //�ж��Ƿ������Ѳ�ߵ�
                if (Vector3.SqrMagnitude(wayPoint - transform.position)<=agent.stoppingDistance)
                {
                    isRun = false;
                    //ʱ��ֵ����
                    if (remainLookAtTime > 0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else
                    //������һ����
                    GetNewWayPoint(); 
                }
                else
                {
                    isRun = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyState.CHASE:
                //������Χ�ڹ���

                isRun = false;
                isChase = true;
                agent.speed = speed;
                //FoudPlayer ����ֵΪfalse, Player ������ھ����趨ֵ
                if (!FoundPlayer())
                {
                    //������Χ�ص�ĳ��״̬
                    isFollow = false;
                    if (remainLookAtTime>0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                        agent.destination = transform.position;
                    }
                    //�ص���Ӧ״̬������ Ѳ�ߣ�
                    else if (isGuard)
                    {
                        enemyState = EnemyState.GUARD;
                    }
                    else
                    {
                        enemyState = EnemyState.PATROL;
                    }
                }
                else
                {
                    //�Ƿ񲥷�Follow����Clip
                    isFollow = true;
                    //����NavMesh agent
                    agent.isStopped = false;
                    //׷���
                    agent.destination = attackTarget.transform.position;
                }

                //�ڼ��ܷ�Χ�ڹ���
                if (TargetInSkillRange())
                {
                    //���ٸ��沢ֹͣ
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime< 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;
                        Attack();
                        //Debug.Log("����ֵ"+attackTarget.GetComponent<CharacterStats>().CurrentHealth);
                    }
                }
                break;
            case EnemyState.DEAD:
                agent.enabled =false;
                Destroy(gameObject, 3f);
                break;
            default:
                break;
        }
    }

    private void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if (TargetInSkillRange())
        {
            //Զ�̹���
            anim.SetTrigger("Skill");
            Instantiate(slime_bullet,transform.position,Quaternion.identity);
            
            //Debug.Log("Slime_Sphere" + slime_bullet.GetComponent<Slime_Sphere>().isPlayer);
            //if ( slime_bullet.GetComponent<Slime_Sphere>().isPlayer)
            //{
            //    attackTarget.GetComponent<CharacterStats>().TakeDamage(characterStats, attackTarget.GetComponent<CharacterStats>());
            //}
        }
    }

    //������� ���뾶��Χ�ڣ�
    private bool FoundPlayer()
    {
        var coliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var target in coliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }
    //Զ���빥��
    private bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            //���������ڹ����ߵķ�Χ�ڣ�����true;
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        }
        else
            return false;
    }

    //���Ѳ��
    private void GetNewWayPoint()
    {
        //��ԭʱ��ֵ
        remainLookAtTime = lookTime;

        float randomX = UnityEngine.Random.Range(-patrolRange, patrolRange);
        float randomZ = UnityEngine.Random.Range(-patrolRange, patrolRange);

        //Ѳ�ߵ������Ϊ��ʼλ�� + ��������ɵ�
        Vector3 randomPosition = new Vector3(guardPostion.x + randomX, transform.position.y, guardPostion.z + randomZ);
        //wayPoint = randomPosition;

        //�ܿ�û�е�����ĵ�
        NavMeshHit hit;
        wayPoint =  NavMesh.SamplePosition(randomPosition, out hit, patrolRange, 1) ? hit.position : transform.position;

    }

    //Scene������ʾ ��Χ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}
