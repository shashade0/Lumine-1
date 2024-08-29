using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����һ��Ҫ������Ⱦ����������ܸı�͸����
[RequireComponent(typeof(SpriteRenderer))]

public class ItemFader : MonoBehaviour
{
    private SpriteRenderer sr;
    public Color originColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originColor = sr.color;
    }

    /// <summary>
    /// �𽥻ָ���ɫ
    /// </summary>
    public bool FadeIn()
    {
        Color targetColor = originColor;
        sr.DOColor(targetColor, Settings.itemFadeDuration);
        return true;
    }

    /// <summary>
    /// ����ȫ͸��
    /// </summary>
    public bool FadeOut()
    {
        Color targetColor = originColor;
        targetColor.a = Settings.targetAlpha;
        sr.DOColor(targetColor, Settings.itemFadeDuration);
        return true;
    }
}
