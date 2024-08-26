using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [Header("�����ȡ")]
    private Transform playerTransform;

    [Header("��������")]
    private float distance;
    private float changeAmount = 0.05f;

    private void Update()
    {
        //Ѱ��Player
        if(this.gameObject.activeInHierarchy == true)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            //�ı���Ӱ��λ��
            ChangeShadowPosition();
        }
    }

    /// <summary>
    /// ����Player��λ�øı���Ӱ��λ��
    /// </summary>
    public void ChangeShadowPosition()
    {
        float tmp_Distance = (this.transform.parent.transform.GetChild(1).transform.position - playerTransform.position).magnitude;
        this.transform.localPosition = new Vector2(Mathf.Abs(tmp_Distance), -playerTransform.position.y);
    }

    /// <summary>
    /// �ı�Shadow��Ӱ�Ĵ�С
    /// </summary>
    public void ChangeShadowScale()
    {

    }
}
