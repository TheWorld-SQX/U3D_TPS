using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//����ű����� �ɶԻ�����Ϸ������
public class CanTalk : MonoBehaviour
{
    public bool hasName;
    [SerializeField]
    private bool isEnter;
    [SerializeField]
    private InputActionReference communicationAction;
    public QuestEnable questEnable;//��Ϸ���� �� ��������
    public QuestTarget questTarget;//����ű��в�û�з��ʣ�������DialogueManager�ű�����ʹ�õ��������

    [TextArea(1, 4)]
    public string[] completedLines;
    [TextArea(1, 3)]
    public string[] dialogueLines;
    [TextArea(1, 4)]
    public string[] congratsLines;

    private void OnEnable()
    {
        communicationAction.action.Enable();
    }
    private void OnDisable()
    {
        communicationAction.action.Disable();
    }
    private void Update()
    {
        if (isEnter && communicationAction.action.WasPressedThisFrame()
            && DialogueManager.Instance.dialoguePanel.activeInHierarchy == false)
        {
            //NPC����û������
            if (questEnable == null)
            {
                DialogueManager.Instance.ShowDialogue(dialogueLines, hasName);
                //Debug.Log("BOARD LINES");
            }
            //NPC��������
            else
            {
                //�������
                if (questEnable.quest.questStatus == Quest.QuestStatus.Completed)
                {
                    DialogueManager.Instance.ShowDialogue(completedLines, hasName);
                    //Debug.Log("COMPLETED LINES");��ʾ�������ĶԻ�
                }
                //����û���
                else
                {
                    DialogueManager.Instance.ShowDialogue(dialogueLines, hasName);
                    //Debug.Log("NORMAL NPC LINES");��ʾ��ͨ�Ի�
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = true;
            DialogueManager.Instance.canTalk = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = false;
            //DialogueManager.instance_DM.canTalk = null;//���������Ϊ�գ��������canTalk�ڳ������ܣ�
        }
    }
}
