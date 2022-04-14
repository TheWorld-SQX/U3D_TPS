using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSlime : MonoBehaviour
{
    [SerializeField] private GameObject enemyObj1,fX1;
    [SerializeField] private GameObject enemyObj2,fX2;
    [SerializeField] private Transform centerPosition;
    [SerializeField] private float patrolRange;
    [SerializeField] float generatetimeE, generatetimeF;


    private void OnTriggerStay(Collider other)
    {
        float randomX = UnityEngine.Random.Range(-patrolRange, patrolRange);
        float randomZ = UnityEngine.Random.Range(-patrolRange, patrolRange);
        //巡逻的随机点为初始位置 + 新随机生成点
        Vector3 randomPosition1 = new Vector3(centerPosition.position.x + randomX, -0.0001f, centerPosition.position.z + randomZ);
        Vector3 randomPosition2 = new Vector3(centerPosition.position.x + 2f + randomX, -0.001f, centerPosition.position.z + 1.5f + randomZ);
        Vector3 randomPosition3 = new Vector3(centerPosition.position.x + 0.65f + randomX, -0.0001f, centerPosition.position.z + 1.2f + randomZ);
        Vector3 randomPosition4 = new Vector3(centerPosition.position.x + 0.35f + randomX, -0.001f, centerPosition.position.z + 1.8f + randomZ);

        if (other.gameObject.CompareTag("Player"))
        {
            generatetimeE -= Time.deltaTime;
            generatetimeF -= Time.deltaTime;
        }
        if (other.gameObject.CompareTag("Player") && generatetimeF> 0)
        {
            PoolManager.Release(enemyObj1,randomPosition1);
            PoolManager.Release(enemyObj2, randomPosition2);
        }
        if (other.gameObject.CompareTag("Player") && generatetimeE > 0)
        {
            PoolManager.Release(fX1, randomPosition3);
            PoolManager.Release(fX2, randomPosition4);
        }

    }
}
