using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//这个脚本 放在【UI Canvas】游戏物体上；更新、显示任务列表；
public class QuestManager : Singleton<QuestManager>
{
    //public static QuestManager instance_PQ;

    public GameObject[] questArray;

    public GameObject questPanel;

    public Text expText, goldText;
    [SerializeField]
    private InputActionReference questAction;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }
    private void OnEnable()
    {
        questAction.action.Enable();
    }
    private void OnDisable()
    {
        questAction.action.Disable();
    }
    private void Start()
    {
        UpdateQuestList();
        questPanel.SetActive(false);
    }

    //玩家接收任务；完成任务用这个方法更新任务列表
    //PS:如果我们要将【完成的任务】移出UI任务列表，我们就不能这么遍历，而是遍历有多少UI任务栏 TODO
    public void UpdateQuestList()
    {
        //for循环显示玩家身上的任务；有多少个任务显示多少个List，而不是有多少List显示多少个任务
        for (int i = 0; i < PlayerQuest.Instance.questList.Count; i++)
        {
            questArray[i].transform.GetChild(0).GetComponent<Text>().text = PlayerQuest.Instance.questList[i].questName;

            if (PlayerQuest.Instance.questList[i].questStatus == Quest.QuestStatus.Accepted)
            {
                questArray[i].transform.GetChild(1).GetComponent<Text>().text
                = "<color=red>" + PlayerQuest.Instance.questList[i].questStatus + "</color>";
            }
            else if (PlayerQuest.Instance.questList[i].questStatus == Quest.QuestStatus.Completed)
            {
                questArray[i].transform.GetChild(1).GetComponent<Text>().text
                = "<color=lime>" + PlayerQuest.Instance.questList[i].questStatus + "</color>";
            }
        }
    }

    private void Update()
    {
        //按下显示松开关闭
        if (questAction.action.WasPressedThisFrame() && questAction.action.IsPressed())
        {
            //Debug.Log("按下了Ctrl键");
            questPanel.SetActive(true);
        }
        else if (questAction.action.WasReleasedThisFrame())
        {
            //Debug.Log("松开了Ctrl键");
            questPanel.SetActive(false);
        }
        //这个方法会 闪，会时不时"失灵"
        //if (questAction.action.WasPerformedThisFrame())
        //{
        //    questPanel.SetActive(!questPanel.activeInHierarchy);
        //}

        //SOLVED 修复：当开启【UI任务列表】时，和NPC开启对话【UI任务列表】还开启的问题
        if (questPanel.activeInHierarchy && DialogueManager.Instance.dialoguePanel.activeInHierarchy)
        {
            questPanel.SetActive(false);
        }
    }

    public void UpdateUIText()
    {
        expText.text = "EXP: " + PlayerQuest.Instance.exp;
        goldText.text = "GOLD: " + PlayerQuest.Instance.gold;
    }
}
