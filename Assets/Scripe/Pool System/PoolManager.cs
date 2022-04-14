using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;
    [SerializeField] Pool[] vFXPools;
    [SerializeField] Pool[] lootItemPools;

    static Dictionary<GameObject, Pool> dictionary;

    void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();

        Initialize(enemyPools);
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);
        Initialize(vFXPools);
        Initialize(lootItemPools);
    }

#if UNITY_EDITOR
    void OnDestroy()
    {
        CheckPoolSize(enemyPools);
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vFXPools);
        CheckPoolSize(lootItemPools);
    }
#endif

    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(
                    string.Format("Pool: {0} has a runtime size {1} bigger than its initial size {2}!",
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }

    void Initialize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same prefab in multiple pools! Prefab: " + pool.Prefab.name);

                continue;
            }
#endif
            dictionary.Add(pool.Prefab, pool);

            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    /// <summary>
    /// <para>Return a specified<paramref name="prefab"></paramref>gameObject in the pool.</para>
    /// <para>���ݴ����<paramref name="prefab"></paramref>���������ض������Ԥ���õ���Ϸ����</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>ָ������Ϸ����Ԥ���塣</para>
    /// </param>
    /// <returns>
    /// <para>Prepared gameObject in the pool.</para>
    /// <para>�������Ԥ���õ���Ϸ����</para>
    /// </returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].PreparedObject();
    }

    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position.</para>
    /// <para>���ݴ����prefab��������position����λ���ͷŶ������Ԥ���õ���Ϸ����</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>ָ������Ϸ����Ԥ���塣</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>ָ���ͷ�λ�á�</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position);
    }

    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position and rotation.</para>
    /// <para>���ݴ����prefab������rotation��������position����λ���ͷŶ������Ԥ���õ���Ϸ����</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>ָ������Ϸ����Ԥ���塣</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>ָ���ͷ�λ�á�</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>ָ������תֵ��</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation);
    }

    /// <summary>
    /// <para>Release a specified prepared gameObject in the pool at specified position, rotation and scale.</para>
    /// <para>���ݴ����prefab����, rotation������localScale��������position����λ���ͷŶ������Ԥ���õ���Ϸ����</para> 
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab.</para>
    /// <para>ָ������Ϸ����Ԥ���塣</para>
    /// </param>
    /// <param name="position">
    /// <para>Specified release position.</para>
    /// <para>ָ���ͷ�λ�á�</para>
    /// </param>
    /// <param name="rotation">
    /// <para>Specified rotation.</para>
    /// <para>ָ������תֵ��</para>
    /// </param>
    /// <param name="localScale">
    /// <para>Specified scale.</para>
    /// <para>ָ��������ֵ��</para>
    /// </param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
    
}
