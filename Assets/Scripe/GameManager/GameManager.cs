using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStates;//待了解观察者模式 反向注册
    private CinemachineFreeLook freeLook;
    private Transform cameraMainTransform;

    //重写基类中方法
    protected override void Awake()
    {
        //生成这个类 类型的的实例，不要销毁
        base.Awake();
    }

    public void RigsterPlayer(CharacterStats player)
    {
        playerStates = player;
        freeLook = FindObjectOfType<CinemachineFreeLook>();
        cameraMainTransform = FindObjectOfType<CinemachineBrain>().transform;

        if (freeLook != null)
        {
            freeLook.Follow = playerStates.transform;
            freeLook.LookAt = playerStates.transform.GetChild(1);
        }
    }

}
