using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnimUI : MonoBehaviour
{
    public static AnimUI instance_UM;
    public GameObject dialogueBox;
    public GameObject nextDialogue;
    public Button quitButton;

    public Text characterNameText;
    public Text dialogueLineText;

    private void Awake()
    {
        if (instance_UM == null)
        {
            instance_UM = this;
        }
        else
        {
            if (instance_UM != this)
            {
                Destroy(gameObject);
            }
        }
    }
    public void ToggleDialogueBox(bool _isActive)
    {
       dialogueBox.SetActive(_isActive);
    }
    public void ToggleNextDialogue(bool _isActive)
    {
        if (nextDialogue != null)
        {
            nextDialogue.SetActive(_isActive);
        }
    }
    public void SetupDialogue(string _name,string _line,int _size)
    {
        characterNameText.text = _name;
        dialogueLineText.text = _line;
        dialogueLineText.fontSize = _size;

        //赋值好每一句对话的台词、名字、字体大小后，开启对话框；
        ToggleDialogueBox(true);
    }
    public void Quit()
    {
        SceneManager.LoadScene(1);
    }
}
