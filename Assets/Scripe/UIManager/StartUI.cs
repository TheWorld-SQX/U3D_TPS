using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public GameObject helpCanvas;
    public GameObject quitCanvas;
    public GameObject tipImage;
    public GameObject musicpanel;
    public Button quitButton;
    public Button playButtoon;
    public Button helpButton;
    public Toggle musicToggle;
    public void HelpClick()
    {
        helpCanvas.SetActive(true);
        playButtoon.enabled = false;
        helpButton.enabled = false;
        quitButton.enabled = false;
    }
    public void HelpExit()
    {
        helpCanvas.SetActive(false);
        playButtoon.enabled = true;
        quitButton.enabled = true;
        helpButton.enabled = true;
    }
    public void TipClick()
    {
        tipImage.SetActive(true);
    }
    public void TipClose()
    {
        tipImage.SetActive(false);
    }
    public void PlayClick()
    {
        SceneManager.LoadScene(2);
    }
    public void StartClick()
    {
        SceneManager.LoadScene(3);
    }
    public void ContinueClick()
    {
        SceneManager.LoadScene(3);
        //SaveManager.Instance.LoadPlayerPosition();
    }

    public void QuitClick()
    {
        quitCanvas.SetActive(true);
        playButtoon.enabled = false;
        quitButton.enabled = false;
        helpButton.enabled = false;
    }
    public void QuitYes()
    {
        Application.Quit();
    }
    public void QuitNo()
    {
        quitCanvas.SetActive(false);
        playButtoon.enabled = true;
        quitButton.enabled = true;
        helpButton.enabled = true;
    }
    public void MusicClick()
    {
        musicpanel.SetActive(true);
        if (musicToggle.isOn == true)
        {
            SoundManager.Instance.BgmAclip();
        }
        else
        {
            SoundManager.Instance.StopBgm();
        }
    }
    public void MusicClose()
    {
        musicpanel.SetActive(false);

    }
}
