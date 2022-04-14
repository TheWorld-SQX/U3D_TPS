using UnityEngine;

//含有任务的游戏物体
//比如说可收集的物品、隐藏的NPC、探索的区域等
public class QuestTarget : MonoBehaviour
{
    public string questName;

    public enum QuestType { Gathering, Talk, Reach };
    public QuestType questType;

    [Header("Talk Type Quest")]//对话类任务
    public bool hasTalked;

    [Header("Reach Type Quest")]//地点类任务
    public bool hasReach;

    //完成任务调用该方法；比如 NPC对话完成后、到达探索区域、收集完物品
    public void CheckQuestIsComplete()
    {
        //完成任务，遍历更新任务状态
        for (int i = 0; i < PlayerQuest.Instance.questList.Count; i++)
        {
            //含任务的游戏物体任务名与玩家身上的任务名一样(玩家接收任务)；更新任务状态为接收
            if (questName == PlayerQuest.Instance.questList[i].questName
             && PlayerQuest.Instance.questList[i].questStatus == Quest.QuestStatus.Accepted)
            {
                //通过QuestManager更新三种任务
                switch (questType)
                {
                    case QuestType.Gathering:
                        if (PlayerQuest.Instance.itemNum >= PlayerQuest.Instance.questList[i].requireAmount)
                        {
                            PlayerQuest.Instance.questList[i].questStatus = Quest.QuestStatus.Completed;
                            QuestManager.Instance.UpdateQuestList();
                            Debug.Log("UPDATE");
                        }
                        break;

                    case QuestType.Talk:
                        if (hasTalked)
                        {
                            PlayerQuest.Instance.questList[i].questStatus = Quest.QuestStatus.Completed;
                            QuestManager.Instance.UpdateQuestList();
                        }
                        break;

                    case QuestType.Reach:
                        if (hasReach)
                        {
                            PlayerQuest.Instance.questList[i].questStatus = Quest.QuestStatus.Completed;
                            QuestManager.Instance.UpdateQuestList();
                        }
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < PlayerQuest.Instance.questList.Count; i++)
            {
                if (PlayerQuest.Instance.questList[i].questName == questName)
                {
                    if (questType == QuestType.Reach)
                    {
                        hasReach = true;
                        CheckQuestIsComplete();
                    }
                }
            }
        }
    }
}
