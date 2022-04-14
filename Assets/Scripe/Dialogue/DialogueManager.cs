using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueManager : Singleton<DialogueManager>
{
    //public static DialogueManager instance_DM;

    public GameObject dialoguePanel;
    public Text dialogueText,nameText;
    [SerializeField]
    private int currentDiadlogueLine;
    [TextArea(1, 3)]
    public string[] dialogueLines;
    [SerializeField]
    private InputActionReference speakAction;
    [SerializeField]
    private float textWaiteTime;
    private bool isScrolling;

    public CanTalk canTalk;
    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }
    //private void Awake()
    //{
    //    if (instance_DM == null)
    //    {
    //        instance_DM = this;
    //    }
    //    else
    //    {
    //        if (instance_DM != null)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //    DontDestroyOnLoad(gameObject);
    //}
    private void OnEnable()
    {
        speakAction.action.Enable();
    }
    private void OnDisable()
    {
        speakAction.action.Disable();
    }

    private void Start()
    {
        dialogueText.text = dialogueLines[currentDiadlogueLine];
    }
    private void Update()
    {
        //�ڷ���ShowDialogue()��ĩβ���������ʾ
        if (dialoguePanel.activeInHierarchy)
        {
            if (speakAction.action.WasPressedThisFrame())
            {
                if (isScrolling == false)
                {
                    currentDiadlogueLine++;
                    //�����ǰ�Ի�С�������Ի����鳤�ȣ���û�Ի���
                    if (currentDiadlogueLine < dialogueLines.Length)
                    {
                        CheckSetName();
                        //dialogueText.text = dialogueLines[currentDiadlogueLine];
                        StartCoroutine(ScrollingText());
                    }
                    //����Ի������һ��
                    else
                    {
                        //dialoguePanel.SetActive(false);
                        //�����ǰ�Ի�����������Ѿ���ɡ�
                        if (GetQuestResult() && canTalk.questEnable.isFinished == false)
                        {
                            ShowDialogue(canTalk.congratsLines, canTalk.hasName);//ף����̨��
                            canTalk.questEnable.isFinished = true;//���أ���֤һ��
                            print(string.Format("QUEST: {0} HAS COMPLETED", canTalk.questEnable.quest.questName));
                            canTalk.questEnable.OfferRewards();

                            //MARKER ���䣺��������Ժ���Խ�ԭ�ȵ����񣬴�questList���Ƴ������Ƴ�Ҳ���ԣ�������ϷҪ�����
                            //for(int i = 0; i < Player.instance.questList.Count; i++)
                            //{
                            //    if(Player.instance.questList[i].questName == talkable.questable.quest.questName)
                            //    {
                            //        Player.instance.questList.RemoveAt(i);
                            //    }
                            //}
                        }
                        //�����ǰ�Ի����������û����ɡ�
                        else
                        {
                            dialoguePanel.SetActive(false);
                            //�����Ϸ����û������
                            if (canTalk.questEnable == null)
                            {
                                Debug.Log("There is no Quest on this person");
                            }
                            //�����Ϸ����������û�з���
                            else
                            {
                                canTalk.questEnable.DelegateQuest();

                                //MARKER ������ʵ����ԡ��ռ������͡������������DelegateQuest���������Ժ�
                                //����ֱ���ж������Ƿ��Ѿ�������������
                                //�ⲿ����ת�Ƶ�DelegateQuest����������
                                //if (GetQuestResult() && talkable.questable.isFinished == false)
                                //{
                                //    ShowDialogue(talkable.congratsLines, talkable.hasName);
                                //    talkable.questable.isFinished = true;
                                //}
                            }

                            //�ⲿ���ǵ����Ǻ�����Ҫ�����Ϸ���󣬱�������NPC�Ի�ʱ��hasTalked����True
                            //�����Ϸ�����ϵ�����Ϊ��
                            if (canTalk.questTarget != null)
                            {
                                for (int i = 0; i < PlayerQuest.Instance.questList.Count; i++)
                                {
                                    //�����Ϸ�����ϵ���������������ϵ�������һ����
                                    if (canTalk.questTarget.questName == PlayerQuest.Instance.questList[i].questName)
                                    {
                                        canTalk.questTarget.hasTalked = true;
                                        canTalk.questTarget.CheckQuestIsComplete();
                                    }
                                }
                            }
                            else
                            {
                                return;
                            }
                        }

                    }
                }
            }
        }
    }

    //��ȡ����������
    public bool GetQuestResult()
    {
        Debug.Log("canTalk" + canTalk);
        Debug.Log("canTalk.questEnable" + canTalk.questEnable);
        //�����Ϸ���� ���ܷ�������
        if (canTalk.questEnable == null)
        {
            Debug.Log("canTalk" + canTalk);
            Debug.Log("canTalk.questEnable" + canTalk.questEnable);
            return false;
        }
        //������һ��һ�����ж���ҽ��յ����� ��û�����
        for (int i = 0; i < PlayerQuest.Instance.questList.Count; i++)
        {
            //�����Ϸ�����ϵ�������������ϵ�������һ����������������
            if (canTalk.questEnable.quest.questName == PlayerQuest.Instance.questList[i].questName
                && PlayerQuest.Instance.questList[i].questStatus == Quest.QuestStatus.Completed)
            {
                canTalk.questEnable.quest.questStatus = Quest.QuestStatus.Completed;
                return true;
            }
        }
        return false;
    }

    public void ShowDialogue(string[] _newLines,bool hasName)
    {
        dialogueLines = _newLines;
        currentDiadlogueLine = 0;
        CheckSetName();
        //dialogueText.text= dialogueLines[currentDiadlogueLine];
        StartCoroutine(ScrollingText());
        dialoguePanel.SetActive(true);
        //nameText.gameObject.SetActive(true);
        //nameText.gameObject.SetActive(false);
        nameText.gameObject.SetActive(hasName);
    }
    private IEnumerator ScrollingText()
    {
        isScrolling = true;
        dialogueText.text = "";
        foreach (char letter in dialogueLines[currentDiadlogueLine].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textWaiteTime);
        }
        isScrolling = false;
    }
    private void CheckSetName()
    {
        if (dialogueLines[currentDiadlogueLine].StartsWith("n-"))
        {
            nameText.text = dialogueLines[currentDiadlogueLine].Replace("n-","");
            currentDiadlogueLine++;
        }
    }
}
