using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T :Singleton<T>
{
    //泛型单例
    private static T instance;//private 如何在外部访问 再写一个public static T,如下
    public static T Instance
    {
        //单例 可读的 不能被更改 只有get 没有set
        get
        {
            return instance;
        }
    }
    //在子类可以继承并重写,protect子类可访问，重写virtual override;
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
    //其他类是否知道这个类的的单例已经生成了
    public static bool  IsInstialize
    {
        //！=null 只需要知道已经生成了
        get{ return instance != null; }
    }

    //子类可以更在单例销毁时做些更改
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
