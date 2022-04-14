using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
//xml�ļ��������ռ�
using System.IO;
using System.Xml;
public class LogInScene : MonoBehaviour
{
    public Image registerPl;
    public Image image_pro;
    public Text text_info;
    public Text text_pro;
    private string path;
    //ע�����ͬ������
    public InputField InputField_name_zc;
    public InputField InputField_pass_zc;
    public InputField InputField_pass1_zc;
    //���õ�½�˺�����ͬ������
    public InputField InputField_name;
    public InputField InputField_pass;

    private void Start()
    {
        registerPl.gameObject.SetActive(false);
        //Application.dataPathָ��������exe�ļ����ļ��е�λ��
        //Application.dataPath��Դ�ļ�����λ��
        //pathһ��Ҫ��ʼ����ֵ,��������
        path = Application.dataPath + "/user.xml";
        //�ж�_xmlpath�ļ��治����,�������ھʹ���
        if (!File.Exists(path))
        {
            //�½�һ��xmlʵ��(�ĵ�����)
            XmlDocument xmlDoc = new XmlDocument();
            //����һ�����ڵ�
            XmlElement root = xmlDoc.CreateElement("Root");

            //�����û��ӽڵ�
            XmlElement user = xmlDoc.CreateElement("User");
            //�����û�����(����,�����)
            user.SetAttribute("user_name", "Admin");
            user.SetAttribute("user_pass", "123456");
            //���û��ڵ���ӵ�root�ڵ���
            root.AppendChild(user);
            //��root�ڵ���ӵ��ı���
            xmlDoc.AppendChild(root);
            //���浽_xmlpath(�Զ�����)
            xmlDoc.Save(path);

            Debug.Log("xml creat success!");
        }
        Debug.Log("xml:" + path);

    }
    //�����˳�
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        //�˳���Ϸ
        Application.Quit();
    }
    //��ע�����
    public void OpenRegistered()
    {
        registerPl.gameObject.SetActive(true);

    }
    //�ر�ע�����
    public void CloseRegistered()
    {
        registerPl.gameObject.SetActive(false);
    }

    //����ʾ����ʾ��Ϣ
    public void ProDisplay_open(string str)
    {
        image_pro.gameObject.SetActive(true);
        text_info.text = str;
        text_pro.text = str;
    }
    //�ر���ʾ��
    public void ProDisplay_close()
    {
        image_pro.gameObject.SetActive(false);
    }
    public void CreatRegistered()
    {
        // �ж�ע�������Ƿ�Ϊ��
        if (InputField_name_zc.text == "" || InputField_pass1_zc.text == "" || InputField_pass_zc.text == "")
        {
            ProDisplay_open("ע����Ϣ��ѡ��Ϊ��,��������Ϣ!");
            return;
        }
        //�ж���������������Ƿ�һ��
        if (InputField_pass1_zc.text != InputField_pass_zc.text)
        {
            ProDisplay_open("������������벻һ��!!");
            return;
        }
        //����˺��Ƿ�ռ��
        XmlDocument xmlDoc = new XmlDocument();
        //���û��ļ�
        xmlDoc.Load(path);
        //ѡ��"Root"�µ������ӽڵ�
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("Root").ChildNodes;
        //���������ӽڵ�
        foreach (XmlElement xe in nodeList)
        {
            if (xe.GetAttribute("user_name") == InputField_name_zc.text)
            {
                ProDisplay_open("���û����ѱ�ռ��!����������û���!");
                return;
            }
        }
        //ȡ��"Root"���ڵ�
        XmlNode root = xmlDoc.SelectSingleNode("Root");
        //�����µ��ӽڵ�
        XmlElement user = xmlDoc.CreateElement("User");
        //��������
        user.SetAttribute("user_name", InputField_name_zc.text);
        user.SetAttribute("user_pass", InputField_pass_zc.text);
        //���½ڵ���뵽���ڵ�
        root.AppendChild(user);
        //�����ĵ�
        xmlDoc.Save(path);
        //��ʾ����ʾ�ɹ�ע�����Ϣ
        ProDisplay_open("ע��ɹ�!");

        //�����ע����Ϣ
        InputField_name_zc.text = "";
        InputField_pass1_zc.text = "";
        InputField_pass_zc.text = "";
    }
    //��Ϸ��½����
    public void GameLanding()
    {

        //�ж��û����������Ƿ�Ϊ��
        if (InputField_name.text == "" || InputField_pass.text == "")
        {
            ProDisplay_open("�û��������벻��Ϊ��!");
            return;
        }
        XmlDocument xmlDoc = new XmlDocument();
        //���û��ļ�
        xmlDoc.Load(path);
        //ѡ��"Root"�µ������ӽڵ�
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("Root").ChildNodes;
        //���������ӽڵ�
        foreach (XmlElement xe in nodeList)
        {
            //�ж��û����Ƿ�ע��
            if (xe.GetAttribute("user_name") == InputField_name.text)
            {
                var name_pass = xe.GetAttribute("user_pass");
                //�ж������Ƿ���ȷ
                if (InputField_pass.text == name_pass)
                {
                    ProDisplay_open("��¼�ɹ�!");
                    //Application.OpenURL("https://123.sogou.com/");
                    SceneManager.LoadSceneAsync(1);
                    return;
                }
                else
                {
                    ProDisplay_open("���벻��ȷ,����������!");
                    return;
                }
            }
            else
            {
                ProDisplay_open("��½ʧ��!�������!");
            }
            ProDisplay_open("���û��˺�δ��ע��!��ע����ٵ�¼!");

        }
    }

}

