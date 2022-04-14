using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class DialogueBehavior : PlayableBehaviour
{
    private PlayableDirector playableDirector;

    public string dialogueName;
    [TextArea(8,1)] public string dialogueline;
    public int dialogueSize;

    private bool isClipPlayed;//轨道中 当前片段是否正在播放；
    public bool requirePause;//只有按下某个键后才能进行后才能继续对话；
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

            //requirePause 真 该片段是需要需要暂停的片段
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
        //如果资源处于静止状态，一直未被使用
        isClipPlayed = false;
        if (pauseScheduled)
        {
            pauseScheduled = false;
            //暂停timeline的播放；
            TimeManager.instance_TM.PauseTimeline(playableDirector);
        }
        else
        {
            AnimUI.instance_UM.ToggleDialogueBox(false);//将对话框 关闭
        }
    }

}
