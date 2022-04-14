using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogIn : MonoBehaviour
{
    //在学习UI框架时 重点关注 干掉拖拽赋值这类垃圾操作 
    // 最垃圾拖拽赋值或者另一个垃圾办法获取父物体找到子物体 登录 注册 账号名 密码 跳转
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
        //点击注册发生。。。
    }
    private void EnterBn()
    {
        //点击确定发生。。。
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
