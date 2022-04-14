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
        //在方法ShowDialogue()内末尾激活面板显示
        if (dialoguePanel.activeInHierarchy)
        {
            if (speakAction.action.WasPressedThisFrame())
            {
                if (isScrolling == false)
                {
                    currentDiadlogueLine++;
                    //如果当前对话小于整个对话数组长度；还没对话完
                    if (currentDiadlogueLine < dialogueLines.Length)
                    {
                        CheckSetName();
                        //dialogueText.text = dialogueLines[currentDiadlogueLine];
                        StartCoroutine(ScrollingText());
                    }
                    //如果对话到最后一句
                    else
                    {
                        //dialoguePanel.SetActive(false);
                        //如果当前对话的这个任务【已经完成】
                        if (GetQuestResult() && canTalk.questEnable.isFinished == false)
                        {
                            ShowDialogue(canTalk.congratsLines, canTalk.hasName);//祝福的台词
                            canTalk.questEnable.isFinished = true;//开关，保证一次
                            print(string.Format("QUEST: {0} HAS COMPLETED", canTalk.questEnable.quest.questName));
                            canTalk.questEnable.OfferRewards();

                            //MARKER 补充：任务完成以后可以将原先的任务，从questList中移除，不移除也可以，根据游戏要求决定
                            //for(int i = 0; i < Player.instance.questList.Count; i++)
                            //{
                            //    if(Player.instance.questList[i].questName == talkable.questable.quest.questName)
                            //    {
                            //        Player.instance.questList.RemoveAt(i);
                            //    }
                            //}
                        }
                        //如果当前对话的这个任务【没有完成】
                        else
                        {
                            dialoguePanel.SetActive(false);
                            //如果游戏物体没有任务
                            if (canTalk.questEnable == null)
                            {
                                Debug.Log("There is no Quest on this person");
                            }
                            //如果游戏物体有任务没有发放
                            else
                            {
                                canTalk.questEnable.DelegateQuest();

                                //MARKER 这里其实是针对【收集类类型】的任务，如果在DelegateQuest方法调用以后
                                //我们直接判断我们是否已经完成了这个任务
                                //这部分先转移到DelegateQuest方法中试试
                                //if (GetQuestResult() && talkable.questable.isFinished == false)
                                //{
                                //    ShowDialogue(talkable.congratsLines, talkable.hasName);
                                //    talkable.questable.isFinished = true;
                                //}
                            }

                            //这部分是当我们和任务要求的游戏对象，比如隐藏NPC对话时，hasTalked等于True
                            //如果游戏物体上的任务不为空
                            if (canTalk.questTarget != null)
                            {
                                for (int i = 0; i < PlayerQuest.Instance.questList.Count; i++)
                                {
                                    //如果游戏物体上的任务名和玩家身上的任务名一样；
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

    //获取任务完成情况
    public bool GetQuestResult()
    {
        Debug.Log("canTalk" + canTalk);
        Debug.Log("canTalk.questEnable" + canTalk.questEnable);
        //如果游戏物体 不能发放任务
        if (canTalk.questEnable == null)
        {
            Debug.Log("canTalk" + canTalk);
            Debug.Log("canTalk.questEnable" + canTalk.questEnable);
            return false;
        }
        //遍历（一个一个）判断玩家接收的任务 有没有完成
        for (int i = 0; i < PlayerQuest.Instance.questList.Count; i++)
        {
            //如果游戏物体上的任务名和玩家上的任务名一样，玩家完成了任务
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
