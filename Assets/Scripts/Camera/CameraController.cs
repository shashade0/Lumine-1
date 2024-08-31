using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���ִ���
/// </summary>
public enum TurnDirctionTwoDir
{
    Horizontal,
    Vertical
}

/// <summary>
/// ����С����
/// </summary>
public enum TurnDirctionFourDir
{
    TurnTop,
    TurnBottom,
    TurnLeft,
    TurnRight,
    None
}

public class CameraController : MonoBehaviour
{
    [Header("�����ȡ")]
    private BoxCollider2D coll;

    [Header("��������")]
    public TurnDirctionFourDir turnDir;
    public float cameraHeight;
    public float cameraWidth;

    private void Start()
    {
        cameraHeight = 2f * Camera.main.orthographicSize;
        //cameraWidth = Screen.width / Screen.height * cameraHeight * 2f;//TODO:���Ȳ���
        cameraWidth = Camera.main.orthographicSize * Screen.width / Screen.height * 2;
        //Debug.Log("cameraWidth: + " + cameraWidth);
    }
 }
