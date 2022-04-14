using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogIn : MonoBehaviour
{
    //��ѧϰUI���ʱ �ص��ע �ɵ���ק��ֵ������������ 
    // ��������ק��ֵ������һ�������취��ȡ�������ҵ������� ��¼ ע�� �˺��� ���� ��ת
    [Header("Attribute")]
    [SerializeField]
    private Button register, enter;
    private InputField accountIF,passwordIF;
    private string accountText, passwordText;

    private void Update()
    {
        accountIF.onValueChanged.AddListener(GetAccountIF);
        passwordIF.onValueChanged.AddListener(GetPasswordIF);

    }
    private void RegisterBn()
    {
        //���ע�ᷢ��������
    }
    private void EnterBn()
    {
        //���ȷ������������
    }
    private void GetAccountIF(string value)
    {
        accountIF.text = value;
    }
    private void GetPasswordIF(string value)
    {
        passwordIF.text = value;
    }
}
