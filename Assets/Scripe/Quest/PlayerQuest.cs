using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����ű���������ϣ�������ϵ����� �ࣻ
public class PlayerQuest : Singleton<PlayerQuest>
{
    //public static PlayerQuest instance_PQ;
    //itenNum��ʾ������ϵ�
    public int exp, gold, itemNum;
    //�ѷ������Ϊ ͨ����Generic�������ͣ�
    public List<Quest> questList = new List<Quest>();
    //public Dictionary<string, Quest> questDictionary = new Dictionary<string, Quest>();//OPTIONAL

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }
    //{
    //    if (instance_PQ == null)
    //    {
    //        instance_PQ = this;
    //    }
    //    else
    //    {
    //        if (instance_PQ != this)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //    DontDestroyOnLoad(gameObject);
    //}

}
