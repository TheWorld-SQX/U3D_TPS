using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance_TM;
    [SerializeField]
    private InputActionReference speakControl;

    //此处使用枚举代替bool类型，表示动画播放模式 和 对话暂停模式
    public enum GameMode
    {
        GamePlay,
        DialogueMoment
    }
    public GameMode gameMode;
    private PlayableDirector currentplayableDirector;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    DontDestroyOnLoad(this);
    //}
    public void Awake()
    {
        if (instance_TM == null)
        {
            instance_TM = this;
        }
        else
        {
            if (instance_TM != this)
            {
                Destroy(gameObject);
            }
        }
        //DontDestroyOnLoad(gameObject);
        gameMode = GameMode.GamePlay;
    }
    private void OnEnable()
    {
        speakControl.action.Enable();
    }
    private void OnDisable()
    {
        speakControl.action.Disable();
    }

    //按下之后才能继续对话， Timeline的播放
    private void Update()
    {
        if (gameMode == GameMode.DialogueMoment)
        {
            if (speakControl.action.IsPressed())
            {
                ResumeTimeline();
            }
        }
    }
    public void PauseTimeline(PlayableDirector _playableDirector)
    {
        currentplayableDirector = _playableDirector;

        gameMode = GameMode.DialogueMoment;
        if (currentplayableDirector != null)
        {
            currentplayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        }
        //currentplayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        AnimUI.instance_UM.ToggleNextDialogue(true);
    }
    public void ResumeTimeline()
    {
        gameMode = GameMode.GamePlay;
        currentplayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);

        AnimUI.instance_UM.ToggleNextDialogue(false);
        AnimUI.instance_UM.ToggleDialogueBox(true);//显示对话框
    }
}
