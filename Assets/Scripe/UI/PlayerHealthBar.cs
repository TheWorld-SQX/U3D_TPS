using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHealthBar : MonoBehaviour
{
    private Text levelText;
    private Image hpBar;
    private Image expBar;
    private void Awake()
    {
        levelText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        hpBar = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>();
        expBar = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Image>();
    }
    private void Update()
    {
        UpdateHP();
        UpdateExp();
    }
    private void UpdateExp()
    {
        float hpPercent = (float)GameManager.Instance.playerStates.CurrentHealth / GameManager.Instance.playerStates.MaxHealth;
        hpBar.fillAmount = hpPercent;
        levelText.text = "LV:" + GameManager.Instance.playerStates.characterData.currentLevel.ToString();
    }
    private void UpdateHP()
    {
        float expPercent = (float)GameManager.Instance.playerStates.characterData.currenExp / GameManager.Instance.playerStates.characterData.referExperience;
        expBar.fillAmount = expPercent;
    }
    public void Exit()
    {
        SaveManager.Instance.SavePlayerData();
        SceneManager.LoadSceneAsync(1);
    }

}
