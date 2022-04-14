using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//发放任务类脚本；放在游戏物体上；
public class QuestEnable : MonoBehaviour
{
    public Quest quest;

    public bool isFinished;

    public QuestTarget questTarget;

    //发放任务的方法
    public void DelegateQuest()
    {
        //是否完成任务
        //如果没有完成任务
        if (isFinished == false)
        {
            //没有接收.等待接受任务
            if (quest.questStatus == Quest.QuestStatus.Waitting)
            {
                quest.questStatus = Quest.QuestStatus.Accepted;//初次委托时将任务更改为【接收】状态
                PlayerQuest.Instance.questList.Add(quest);
                //如果是收集类任务，需要给奖励
                if (quest.questType == Quest.QuestType.Gathering)
                {
                    questTarget.CheckQuestIsComplete();

                    #region
                    //完成了收集型任务，给奖励
                    if (DialogueManager.Instance.GetQuestResult() == true)
                    {
                        DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.canTalk.congratsLines, DialogueManager.Instance.canTalk.hasName);
                        isFinished = true;
                        OfferRewards();
                    }
                    #endregion
                }
            }
            //如果已经接收任务
            else
            {
                Debug.Log(string.Format("QUEST: {0} has accepted already!", quest.questName));
            }
        }
        //如果完成任务
        else
        {
            Debug.Log("You have Finished THIS QUEST BRO!");
        }
        //if判断之后更新任务列表
        QuestManager.Instance.UpdateQuestList();
    }

    
    public void OfferRewards()
    {
        PlayerQuest.Instance.exp += quest.expReward;
        PlayerQuest.Instance.gold += quest.goldReward;
        QuestManager.Instance.UpdateUIText();
        Debug.Log("$*$*$*****Bonus*****$*$*$");
    }

}
