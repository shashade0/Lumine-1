using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    [Header("�����ȡ")]
    public GameObject button;
    public GameObject talkUI;

    private DialogSystem dialogSystem;

    [Header("��������")]
    private string targetCharacterName;

    private void Awake()
    {
        dialogSystem = GetComponent<DialogSystem>();

        targetCharacterName = this.gameObject.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        button.SetActive(false);
    }

    private void Update()
    {
        if(button.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            //��ȡ��ǰ���������
            dialogSystem.targetCharacterName = targetCharacterName;

            //չʾ�Ի���
            talkUI.SetActive(true);
        }
    }
}
