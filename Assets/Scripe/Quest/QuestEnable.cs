using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����������ű���������Ϸ�����ϣ�
public class QuestEnable : MonoBehaviour
{
    public Quest quest;

    public bool isFinished;

    public QuestTarget questTarget;

    //��������ķ���
    public void DelegateQuest()
    {
        //�Ƿ��������
        //���û���������
        if (isFinished == false)
        {
            //û�н���.�ȴ���������
            if (quest.questStatus == Quest.QuestStatus.Waitting)
            {
                quest.questStatus = Quest.QuestStatus.Accepted;//����ί��ʱ���������Ϊ�����ա�״̬
                PlayerQuest.Instance.questList.Add(quest);
                //������ռ���������Ҫ������
                if (quest.questType == Quest.QuestType.Gathering)
                {
                    questTarget.CheckQuestIsComplete();

                    #region
                    //������ռ������񣬸�����
                    if (DialogueManager.Instance.GetQuestResult() == true)
                    {
                        DialogueManager.Instance.ShowDialogue(DialogueManager.Instance.canTalk.congratsLines, DialogueManager.Instance.canTalk.hasName);
                        isFinished = true;
                        OfferRewards();
                    }
                    #endregion
                }
            }
            //����Ѿ���������
            else
            {
                Debug.Log(string.Format("QUEST: {0} has accepted already!", quest.questName));
            }
        }
        //����������
        else
        {
            Debug.Log("You have Finished THIS QUEST BRO!");
        }
        //if�ж�֮����������б�
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
