using UnityEngine;

//�����������Ϸ����
//����˵���ռ�����Ʒ�����ص�NPC��̽���������
public class QuestTarget : MonoBehaviour
{
    public string questName;

    public enum QuestType { Gathering, Talk, Reach };
    public QuestType questType;

    [Header("Talk Type Quest")]//�Ի�������
    public bool hasTalked;

    [Header("Reach Type Quest")]//�ص�������
    public bool hasReach;

    //���������ø÷��������� NPC�Ի���ɺ󡢵���̽�������ռ�����Ʒ
    public void CheckQuestIsComplete()
    {
        //������񣬱�����������״̬
        for (int i = 0; i < PlayerQuest.Instance.questList.Count; i++)
        {
            //���������Ϸ������������������ϵ�������һ��(��ҽ�������)����������״̬Ϊ����
            if (questName == PlayerQuest.Instance.questList[i].questName
             && PlayerQuest.Instance.questList[i].questStatus == Quest.QuestStatus.Accepted)
            {
                //ͨ��QuestManager������������
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
