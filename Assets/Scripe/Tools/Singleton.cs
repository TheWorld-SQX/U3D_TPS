using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T :Singleton<T>
{
    //���͵���
    private static T instance;//private ������ⲿ���� ��дһ��public static T,����
    public static T Instance
    {
        //���� �ɶ��� ���ܱ����� ֻ��get û��set
        get
        {
            return instance;
        }
    }
    //��������Լ̳в���д,protect����ɷ��ʣ���дvirtual override;
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }
    //�������Ƿ�֪�������ĵĵ����Ѿ�������
    public static bool  IsInstialize
    {
        //��=null ֻ��Ҫ֪���Ѿ�������
        get{ return instance != null; }
    }

    //������Ը��ڵ�������ʱ��Щ����
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
