using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : Singleton<SceneController>
{
    /*
    属性：玩家位置，传送门地点，传送目的地，传送目的名字，场景传送所属类别
    方法：传送；获取传送门地点；获取传送目的地；判断场景传送所属类别
    */
    private GameObject player;
    public GameObject playerPrefab;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    //判断场景传送所属类别
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

    //得到传送目的地名字
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
