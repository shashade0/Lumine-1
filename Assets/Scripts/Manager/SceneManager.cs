using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    private void Start()
    {
        EnterMenu();
    }

    /// <summary>
    /// ������Ϸ�˵�
    /// </summary>
    public void EnterMenu()
    {
        //���ز˵�����
    }

    /// <summary>
    /// ������һ������
    /// </summary>
    public void LoadNextScene()
    {

    }

    /// <summary>
    /// ���ڼ��س�����
    /// </summary>
    public void OnLoadNextScene()
    {

    }

    /// <summary>
    /// �������ؽ���
    /// </summary>
    public void AfterLoadScene()
    {

    }
}
