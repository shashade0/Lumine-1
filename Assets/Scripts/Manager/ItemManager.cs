using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [Header("�����ȡ")]
    public Animator animator;

    [Header("��������")]
    public List<Item> currentItemsList = new List<Item>();//���ռ��ĵ��߼���
    public List<Item> allItemsList = new List<Item>();//�ܵ��ߵļ���

    private int currentIndex;
    private Item currentItem;//��ǰװ���ĵ���

    /// <summary>
    /// �л�����
    /// </summary>
    public void SwitchItems()
    {
        //�л����߰������
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //�������ж�

            //��һ������

            //���ص�ǰ���� 

            //��ֵ��ǰ�����ĵ���
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //�������ж�

            //��һ������

            //���ص�ǰ���� 

            //��ֵ��ǰ�����ĵ���
        }

        //װ����ǰ�ĵ���
        //EquippedItem();
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    public void GetItem(Item _item)
    {
        currentItemsList.Add(_item);

        //�޸ĵ�����UI

        //װ���ȡ�ĵ���

    }

    /// <summary>
    /// װ����ǰ�ĵ���
    /// </summary>
    public void EquippedItem(Item _item)
    {
        //װ����ǰ���ߵĶ�����
    }

}
