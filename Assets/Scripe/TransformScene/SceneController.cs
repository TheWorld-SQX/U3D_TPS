using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : Singleton<SceneController>
{
    /*
    ���ԣ����λ�ã������ŵص㣬����Ŀ�ĵأ�����Ŀ�����֣����������������
    ���������ͣ���ȡ�����ŵص㣻��ȡ����Ŀ�ĵأ��жϳ��������������
    */
    private GameObject player;
    public GameObject playerPrefab;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    //�жϳ��������������
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name,transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
            default:
                break;
        }
    }
    private IEnumerator Transition(string sceneName,TransitionDestination.DestinationTag destinationTag)
    {
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            //player = GameManager.Instance.playerStates.gameObject;
            //player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            yield break;
        }
        else 
        {
            player = GameManager.Instance.playerStates.gameObject;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            yield return new WaitForSeconds(0.3f);
        }
    }

    //�õ�����Ŀ�ĵ�����
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrance = FindObjectsOfType<TransitionDestination>();
        for (int i = 0; i < entrance.Length; i++)
        {
            if (entrance[i].destinationTag == destinationTag)
            {
                return entrance[i];
            }
        }
        return null;
    }
}
