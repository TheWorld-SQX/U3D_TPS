using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "Character States/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("State Info")]
    public  int maxHealth;
    public  int currentHealth;
    public  int baseDefence;
    public  int currentDefense;
    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int referExperience;
    public int currenExp;
    public float delta;
    public int killedPoint; 

    private float DeltaExp
    {
        get{ return 1 + (currentLevel - 1) * delta; }    
    }

    public void UpdateLevel(int point)
    {
        currenExp += point;
        if (currenExp>referExperience)
        {
            LevelUp();
        }
    }
    private void LevelUp()
    {
        currentLevel = Mathf.Clamp(currentLevel + 1,0,maxLevel);
        referExperience += (int)(referExperience * DeltaExp);
        maxHealth += (int)(maxHealth * delta);
        currentHealth = maxHealth;
        Debug.Log("maxHealth" + maxHealth + "currentLevel" + currentLevel);
    }

}
