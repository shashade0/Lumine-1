using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TurnDirctionTwoDir
{
    Horizontal,
    Vertical
}

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
    public Camera camera;
    private BoxCollider2D coll;

    [Header("��������")]
    public TurnDirctionFourDir turnDir;
    public float cameraHeight;
    public float cameraWidth;

    private void Start()
    {
        cameraHeight = 2f * camera.orthographicSize;
        //cameraWidth = Screen.width / Screen.height * cameraHeight * 2f;//TODO:���Ȳ���
        cameraWidth = camera.orthographicSize * Screen.width / Screen.height * 2;
        //Debug.Log("cameraWidth: + " + cameraWidth);
    }
 }
