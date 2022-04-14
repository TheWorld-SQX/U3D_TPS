using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这个脚本放在玩家上；玩家身上的任务 类；
public class PlayerQuest : Singleton<PlayerQuest>
{
    //public static PlayerQuest instance_PQ;
    //itenNum表示玩家身上的
    public int exp, gold, itemNum;
    //把泛型理解为 通用型Generic，万能型；
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
