using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("�����ȡ")]
    public Transform itemsCollectTransform;//��Ʒ���ռ���

    //[Header("��������")]

    
    /// <summary>
    /// ����
    /// </summary>
    public void SettingUI()
    {
        //ֹͣ��Ϸ
        Time.timeScale = 0f;

        //����SettingUI
    }

    /// <summary>
    /// ��ϵ����
    /// </summary>
    public void CloseSettingUI()
    {
        //�ָ���Ϸ
        Time.timeScale = 1f;
    }
}
