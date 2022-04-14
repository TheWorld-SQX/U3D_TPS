using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ʲô�������˼�������� ����Щ����������ظ��еķ�����
//����˵�����ɶ���Copy()����ʼ������Initialize(Transform parent)����ӳ�Ϊ���ö���AvailableObject()�����ÿ��ö���PreparedObject()
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

    //��ʼ��
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for (var i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }

    //���� ��������Ҫ�Ķ���
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }

    //���ÿ��ö���
    //��һ�������ö�������
    GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if (queue.Count > 0 && !queue.Peek().activeSelf)//�������е�Ԫ�ش���0���ҵ�һ��Ԫ����δ���õ�״̬
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
    //�ڶ��������ÿ��ö�������������������أ����㲻ͬ������ʹ��
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

    //�����ص������ ����д���������� ������ע�ͣ����ǿ����ö�����Ӻ� �� ��ӣ��ڳ��Ӻ����AvailableObject������queue.Enqueue(availableObject);
    //public void Return(GameObject gameObject)
    //{
    //    queue.Enqueue(gameObject);
    //}
}
