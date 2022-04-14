using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStates;//���˽�۲���ģʽ ����ע��
    private CinemachineFreeLook freeLook;
    private Transform cameraMainTransform;

    //��д�����з���
    protected override void Awake()
    {
        //��������� ���͵ĵ�ʵ������Ҫ����
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
