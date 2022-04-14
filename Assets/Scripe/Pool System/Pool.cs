using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这类干什么：对象池思想就是这个 有哪些方法：对象池该有的方法，
//比如说：生成对象Copy()、初始化对象Initialize(Transform parent)、入队成为可用对象AvailableObject()、启用可用对象PreparedObject()
[System.Serializable]
public class Pool
{
    public GameObject Prefab => prefab;
    public int Size => size;
    public int RuntimeSize => queue.Count;

    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;

    Queue<GameObject> queue;

    Transform parent;

    //初始化
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (var i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }

    //复制 生成所需要的对象
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }

    //启用可用对象
    //第一步；可用对象入列
    GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if (queue.Count > 0 && !queue.Peek().activeSelf)//当队列中的元素大于0并且第一个元素是未启用的状态
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }

        queue.Enqueue(availableObject);

        return availableObject;
    }
    //第二步，启用可用对象，这个方法有三个重载，方便不同需求下使用
    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }

    //用完后回到对象池 可以写个方法调用 如下面注释；但是可以让对象出队后 就 归队，在出队候入队AvailableObject方法中queue.Enqueue(availableObject);
    //public void Return(GameObject gameObject)
    //{
    //    queue.Enqueue(gameObject);
    //}
}
