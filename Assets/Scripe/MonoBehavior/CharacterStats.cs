using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;

    //获取CharacterStats
    public CharacterData_SO characterData;
    [SerializeField]
    private CharacterData_SO characterStatsTemple;
    public AttackData_SO attackData;
    private void Awake()
    {
        //if (characterStatsTemple != null)
        //{
        //    characterData = Instantiate(characterStatsTemple);
        //}
        if (characterStatsTemple != null)
        {
            characterData = Instantiate(characterStatsTemple) ;
            //CharacterData_SO.CreateInstance("CharacterData_SO");
        }
    }

    //不想逐级.调用;
    #region Read from Character_SO
    public int MaxHealth 
    {
        get
        {
            if (characterData != null)
            {
                return characterData.maxHealth;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.maxHealth = value;
        }
    }
    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentHealth;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.currentHealth = value;
        }
    }
    public int BaseDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.baseDefence;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.baseDefence = value;
        }
    }
    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
            {
                return characterData.currentDefense;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.currentDefense = value;
        }
    }
    #endregion

    #region Character Combat
    public void TakeDamage(CharacterStats attacker,CharacterStats defener)
    {
        //伤害减去防御值不能为负，使用Math.Max保证最小值值为0（生命值减去伤害同样也是）
        int damage = Math.Max(attacker.CurrentDamage() - defener.CurrentDefence,0);
        CurrentHealth = Math.Max(CurrentHealth - damage, 0);
        //Debug.Log("damage" + damage);
        //TODO:UpdateUI
        if (CurrentHealth>=0)
        {
            UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        }
        if (CurrentHealth == 0)
        {
            
            if (attacker.characterData != null)
            {
                attacker.characterData.UpdateLevel(characterData.killedPoint);
            }
        }
    }

    //获取随机攻击力为当前攻击力
    private int CurrentDamage()
    {
        int norDamage = attackData.normalDamage;
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage,attackData.maxDamage);
        return norDamage;
    }
    #endregion
   
}
