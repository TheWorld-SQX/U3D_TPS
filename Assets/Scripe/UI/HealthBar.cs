using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject hpBarPrefabs;//血条预设体
    private Transform hpBarPrefabsTrans;//血条预设体自身位置
    public Transform hpPosePosition;//血条预设体放置位置

    private Image hpSlider;//图片类型为填充
    private Transform camTransform;
    private CharacterStats currentState;
    public bool alwaysVisual;
    public float visualTime;
    private void Awake()
    {
        currentState = GetComponent<CharacterStats>();
        currentState.UpdateHealthBarOnAttack += UpdateHPBar;
    }
    //脚本调用时生成UIBar
    private void OnEnable()
    {
        camTransform = Camera.main.transform;
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                hpBarPrefabsTrans = Instantiate(hpBarPrefabs, canvas.transform).transform;
                hpSlider = hpBarPrefabsTrans.GetChild(0).GetComponent<Image>();
                hpBarPrefabsTrans.gameObject.SetActive(alwaysVisual);
            }
        }
    }
    private void UpdateHPBar(int currentHealth, int maxHealth)
    {
        if (currentHealth < 0)
        {
            Destroy(hpBarPrefabsTrans.gameObject);
        }
        if (currentHealth == 0)
        {
            hpBarPrefabsTrans.gameObject.SetActive(false);
        }
        if (currentHealth>0)
        {
            hpBarPrefabsTrans.gameObject.SetActive(true);
            float sliderPercent = (float)currentHealth / maxHealth;
            hpSlider.fillAmount = sliderPercent;
        }
    }
    private void LateUpdate()
    {
        if (hpBarPrefabsTrans != null)
        {
            hpBarPrefabsTrans.position = hpPosePosition.position;
            hpBarPrefabsTrans.forward = -camTransform.forward;
        }
    }
}
