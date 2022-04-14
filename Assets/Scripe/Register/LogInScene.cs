using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
//xml文件的命名空间
using System.IO;
using System.Xml;
public class LogInScene : MonoBehaviour
{
    public Image registerPl;
    public Image image_pro;
    public Text text_info;
    public Text text_pro;
    private string path;
    //注册框内同名变量
    public InputField InputField_name_zc;
    public InputField InputField_pass_zc;
    public InputField InputField_pass1_zc;
    //引用登陆账号密码同名变量
    public InputField InputField_name;
    public InputField InputField_pass;

    private void Start()
    {
        registerPl.gameObject.SetActive(false);
        //Application.dataPath指的是生成exe文件后文件夹的位置
        //Application.dataPath资源文件夹下位置
        //path一定要初始化赋值,否则会出错
        path = Application.dataPath + "/user.xml";
        //判断_xmlpath文件存不存在,若不存在就创建
        if (!File.Exists(path))
        {
            //新建一个xml实例(文档对象)
            XmlDocument xmlDoc = new XmlDocument();
            //创建一个根节点
            XmlElement root = xmlDoc.CreateElement("Root");

            //创建用户子节点
            XmlElement user = xmlDoc.CreateElement("User");
            //创建用户特性(名字,密码等)
            user.SetAttribute("user_name", "Admin");
            user.SetAttribute("user_pass", "123456");
            //将用户节点添加到root节点中
            root.AppendChild(user);
            //将root节点添加到文本中
            xmlDoc.AppendChild(root);
            //保存到_xmlpath(自动创建)
            xmlDoc.Save(path);

            Debug.Log("xml creat success!");
        }
        Debug.Log("xml:" + path);

    }
    //界面退出
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        //退出游戏
        Application.Quit();
    }
    //打开注册面板
    public void OpenRegistered()
    {
        registerPl.gameObject.SetActive(true);

    }
    //关闭注册面板
    public void CloseRegistered()
    {
        registerPl.gameObject.SetActive(false);
    }

    //打开提示框并显示信息
    public void ProDisplay_open(string str)
    {
        image_pro.gameObject.SetActive(true);
        text_info.text = str;
        text_pro.text = str;
    }
    //关闭提示框
    public void ProDisplay_close()
    {
        image_pro.gameObject.SetActive(false);
    }
    public void CreatRegistered()
    {
        // 判断注册内容是否为空
        if (InputField_name_zc.text == "" || InputField_pass1_zc.text == "" || InputField_pass_zc.text == "")
        {
            ProDisplay_open("注册信息有选项为空,请完善信息!");
            return;
        }
        //判断输入的两次密码是否一致
        if (InputField_pass1_zc.text != InputField_pass_zc.text)
        {
            ProDisplay_open("两次输入的密码不一致!!");
            return;
        }
        //检测账号是否被占用
        XmlDocument xmlDoc = new XmlDocument();
        //打开用户文件
        xmlDoc.Load(path);
        //选择"Root"下的所有子节点
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("Root").ChildNodes;
        //遍历所有子节点
        foreach (XmlElement xe in nodeList)
        {
            if (xe.GetAttribute("user_name") == InputField_name_zc.text)
            {
                ProDisplay_open("该用户名已被占用!请更换其他用户名!");
                return;
            }
        }
        //取得"Root"根节点
        XmlNode root = xmlDoc.SelectSingleNode("Root");
        //创建新的子节点
        XmlElement user = xmlDoc.CreateElement("User");
        //设置属性
        user.SetAttribute("user_name", InputField_name_zc.text);
        user.SetAttribute("user_pass", InputField_pass_zc.text);
        //将新节点加入到根节点
        root.AppendChild(user);
        //保存文档
        xmlDoc.Save(path);
        //提示框显示成功注册的消息
        ProDisplay_open("注册成功!");

        //清空已注册信息
        InputField_name_zc.text = "";
        InputField_pass1_zc.text = "";
        InputField_pass_zc.text = "";
    }
    //游戏登陆函数
    public void GameLanding()
    {

        //判断用户名和密码是否为空
        if (InputField_name.text == "" || InputField_pass.text == "")
        {
            ProDisplay_open("用户名或密码不能为空!");
            return;
        }
        XmlDocument xmlDoc = new XmlDocument();
        //打开用户文件
        xmlDoc.Load(path);
        //选择"Root"下的所有子节点
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("Root").ChildNodes;
        //遍历所有子节点
        foreach (XmlElement xe in nodeList)
        {
            //判断用户名是否注册
            if (xe.GetAttribute("user_name") == InputField_name.text)
            {
                var name_pass = xe.GetAttribute("user_pass");
                //判断密码是否正确
                if (InputField_pass.text == name_pass)
                {
                    ProDisplay_open("登录成功!");
                    //Application.OpenURL("https://123.sogou.com/");
                    SceneManager.LoadSceneAsync(1);
                    return;
                }
                else
                {
                    ProDisplay_open("密码不正确,请重新输入!");
                    return;
                }
            }
            else
            {
                ProDisplay_open("登陆失败!密码错误!");
            }
            ProDisplay_open("此用户账号未被注册!请注册后再登录!");

        }
    }

}

