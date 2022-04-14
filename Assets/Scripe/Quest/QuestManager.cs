using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

//����ű� ���ڡ�UI Canvas����Ϸ�����ϣ����¡���ʾ�����б�
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

    //��ҽ��������������������������������б�
    //PS:�������Ҫ������ɵ������Ƴ�UI�����б����ǾͲ�����ô���������Ǳ����ж���UI������ TODO
    public void UpdateQuestList()
    {
        //forѭ����ʾ������ϵ������ж��ٸ�������ʾ���ٸ�List���������ж���List��ʾ���ٸ�����
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
        //������ʾ�ɿ��ر�
        if (questAction.action.WasPressedThisFrame() && questAction.action.IsPressed())
        {
            //Debug.Log("������Ctrl��");
            questPanel.SetActive(true);
        }
        else if (questAction.action.WasReleasedThisFrame())
        {
            //Debug.Log("�ɿ���Ctrl��");
            questPanel.SetActive(false);
        }
        //��������� ������ʱ��ʱ"ʧ��"
        //if (questAction.action.WasPerformedThisFrame())
        //{
        //    questPanel.SetActive(!questPanel.activeInHierarchy);
        //}

        //SOLVED �޸�����������UI�����б�ʱ����NPC�����Ի���UI�����б�������������
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
