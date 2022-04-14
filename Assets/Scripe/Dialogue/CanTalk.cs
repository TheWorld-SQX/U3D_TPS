using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//这个脚本放在 可对话的游戏物体上
public class CanTalk : MonoBehaviour
{
    public bool hasName;
    [SerializeField]
    private bool isEnter;
    [SerializeField]
    private InputActionReference communicationAction;
    public QuestEnable questEnable;//游戏物体 可 发放任务
    public QuestTarget questTarget;//这个脚本中并没有访问，但是在DialogueManager脚本中有使用到这个变量

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
            //NPC身上没有任务
            if (questEnable == null)
            {
                DialogueManager.Instance.ShowDialogue(dialogueLines, hasName);
                //Debug.Log("BOARD LINES");
            }
            //NPC含有任务
            else
            {
                //任务完成
                if (questEnable.quest.questStatus == Quest.QuestStatus.Completed)
                {
                    DialogueManager.Instance.ShowDialogue(completedLines, hasName);
                    //Debug.Log("COMPLETED LINES");显示完成任务的对话
                }
                //任务没完成
                else
                {
                    DialogueManager.Instance.ShowDialogue(dialogueLines, hasName);
                    //Debug.Log("NORMAL NPC LINES");显示普通对话
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
            //DialogueManager.instance_DM.canTalk = null;//如果不让他为空，人物带着canTalk在场景乱跑；
        }
    }
}
