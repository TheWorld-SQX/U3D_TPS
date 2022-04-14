using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class DialogueBehavior : PlayableBehaviour
{
    private PlayableDirector playableDirector;

    public string dialogueName;
    [TextArea(8,1)] public string dialogueline;
    public int dialogueSize;

    private bool isClipPlayed;//����� ��ǰƬ���Ƿ����ڲ��ţ�
    public bool requirePause;//ֻ�а���ĳ��������ܽ��к���ܼ����Ի���
    private bool pauseScheduled;

    public override void OnPlayableCreate(Playable playable)
    {
        playableDirector = playable.GetGraph().GetResolver() as PlayableDirector;
    }
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (isClipPlayed == false && info.weight >0)
        {
            AnimUI.instance_UM.SetupDialogue(dialogueName, dialogueline, dialogueSize);

            //requirePause �� ��Ƭ������Ҫ��Ҫ��ͣ��Ƭ��
            if (requirePause)
            {
                pauseScheduled = true;
            }
            AnimUI.instance_UM.ToggleNextDialogue(false);
            isClipPlayed = true;
        }
    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        //�����Դ���ھ�ֹ״̬��һֱδ��ʹ��
        isClipPlayed = false;
        if (pauseScheduled)
        {
            pauseScheduled = false;
            //��ͣtimeline�Ĳ��ţ�
            TimeManager.instance_TM.PauseTimeline(playableDirector);
        }
        else
        {
            AnimUI.instance_UM.ToggleDialogueBox(false);//���Ի��� �ر�
        }
    }

}
