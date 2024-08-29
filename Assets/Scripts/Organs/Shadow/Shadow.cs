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
    //private float changeAmount = 0.05f;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        ////Ѱ��Player
        //if(this.gameObject.activeInHierarchy == true)
        //{

        //}

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //�ı���Ӱ��λ��
        ChangeShadowPosition();
    }

    /// <summary>
    /// ����Player��λ�øı���Ӱ��λ��
    /// </summary>
    public void ChangeShadowPosition()
    {
        float tmp_Distance = (this.transform.parent.transform.GetChild(1).transform.position - playerTransform.position).magnitude;

        if(playerTransform.position.x <= this.transform.parent.transform.GetChild(1).transform.position.x)
        {
            this.transform.localPosition = new Vector2(Mathf.Abs(tmp_Distance), -playerTransform.position.y);
        }
        else
        {
            this.transform.localPosition = new Vector2(-1 * Mathf.Abs(tmp_Distance), -playerTransform.position.y);
        }
        
        ChangeShadowScale(tmp_Distance);
    }

    /// <summary>
    /// �ı�Shadow��Ӱ�Ĵ�С
    /// </summary>
    public void ChangeShadowScale(float _distance)
    {
        float tmp_ScaleX = Mathf.Lerp(1.4f, 4.2f, (10f - _distance) / 10);
        float tmp_ScaleY = Mathf.Lerp(1.4f, 4.2f, (10f - _distance) / 10);

        this.transform.localScale = new Vector2(tmp_ScaleY, tmp_ScaleX);
    }
}
