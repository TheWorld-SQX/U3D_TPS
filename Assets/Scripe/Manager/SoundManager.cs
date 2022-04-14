using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgmSource;
    public AudioSource audioSource;
    [Header("PlayerSound")]
    [SerializeField] private AudioClip walkAClip,runAclip,shootAclip, getHitAclip,deadAClip,boomAClip,bgmAClip;

    //void Start()
    //{
    //    audioSource = this.GetComponent<AudioSource>();
    //}
    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }
    public void WalkAClip()
    {
        //audioSource.clip = walkAClip;
        //audioSource.Play();
        AudioSource.PlayClipAtPoint(walkAClip, this.transform.position);
    }
    public void RunAClip()
    {
        audioSource.clip = runAclip;
        audioSource.Play();
    }

    public void ShootAClip()
    {
        audioSource.clip = shootAclip;
        audioSource.Play();
    }
    public void GetHitAclip()
    {
        audioSource.clip = getHitAclip;
        audioSource.Play();
    }
    public void DeadAclip()
    {
        audioSource.clip = deadAClip;
        audioSource.Play();
    }
    public void BoomAclip()
    {
        audioSource.clip = boomAClip;
        audioSource.Play();
    }
    public void BgmAclip()
    {
        audioSource.clip = bgmAClip;
        bgmSource.Play();
    }
    public void StopBgm()
    {
        audioSource.clip = bgmAClip;
        bgmSource.Pause();
        //AudioSource.PlayClipAtPoint(bgmAClip, this.transform.position);
    }
}
