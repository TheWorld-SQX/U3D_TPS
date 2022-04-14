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

    //巡逻时巡视时间 及计时器
    [SerializeField]
    private float lookTime;
    private float remainLookAtTime;

    //攻击对象
    private GameObject attackTarget;

    private CharacterStats characterStats;

    private float lastAttackTime;
    public GameObject slime_bullet;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //默认速度为代理速度
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
            //开始随机点巡逻时 先生成一个点作为初始位置
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

    //切换动画
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
                    //向原来位置走过去
                    agent.destination = guardPostion;
                    //走到原位置停止
                    if (Vector3.SqrMagnitude(guardPostion - transform.position) <= agent.stoppingDistance)
                    {
                        isRun = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.02f);
                    }
                }
                break;
            case EnemyState.PATROL:
                //TODO:巡逻
                isChase = false;
                agent.speed = speed * 0.5f;
                //判断是否到了随机巡逻点
                if (Vector3.SqrMagnitude(wayPoint - transform.position)<=agent.stoppingDistance)
                {
                    isRun = false;
                    //时间值减少
                    if (remainLookAtTime > 0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else
                    //重新找一个点
                    GetNewWayPoint(); 
                }
                else
                {
                    isRun = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyState.CHASE:
                //攻击范围内攻击

                isRun = false;
                isChase = true;
                agent.speed = speed;
                //FoudPlayer 返回值为false, Player 距离大于距离设定值
                if (!FoundPlayer())
                {
                    //超出范围回到某个状态
                    isFollow = false;
                    if (remainLookAtTime>0)
                    {
                        remainLookAtTime -= Time.deltaTime;
                        agent.destination = transform.position;
                    }
                    //回到相应状态（守卫 巡逻）
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
                    //是否播放Follow动画Clip
                    isFollow = true;
                    //开启NavMesh agent
                    agent.isStopped = false;
                    //追玩家
                    agent.destination = attackTarget.transform.position;
                }

                //在技能范围内攻击
                if (TargetInSkillRange())
                {
                    //不再跟随并停止
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime< 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;
                        Attack();
                        //Debug.Log("生命值"+attackTarget.GetComponent<CharacterStats>().CurrentHealth);
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
            //远程攻击
            anim.SetTrigger("Skill");
            Instantiate(slime_bullet,transform.position,Quaternion.identity);
            
            //Debug.Log("Slime_Sphere" + slime_bullet.GetComponent<Slime_Sphere>().isPlayer);
            //if ( slime_bullet.GetComponent<Slime_Sphere>().isPlayer)
            //{
            //    attackTarget.GetComponent<CharacterStats>().TakeDamage(characterStats, attackTarget.GetComponent<CharacterStats>());
            //}
        }
    }

    //发现玩家 （半径范围内）
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
    //远距离攻击
    private bool TargetInSkillRange()
    {
        if (attackTarget != null)
        {
            //攻击对象在攻击者的范围内，返回true;
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        }
        else
            return false;
    }

    //随机巡逻
    private void GetNewWayPoint()
    {
        //还原时间值
        remainLookAtTime = lookTime;

        float randomX = UnityEngine.Random.Range(-patrolRange, patrolRange);
        float randomZ = UnityEngine.Random.Range(-patrolRange, patrolRange);

        //巡逻的随机点为初始位置 + 新随机生成点
        Vector3 randomPosition = new Vector3(guardPostion.x + randomX, transform.position.y, guardPostion.z + randomZ);
        //wayPoint = randomPosition;

        //避开没有导航格的点
        NavMeshHit hit;
        wayPoint =  NavMesh.SamplePosition(randomPosition, out hit, patrolRange, 1) ? hit.position : transform.position;

    }

    //Scene窗口显示 范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}
