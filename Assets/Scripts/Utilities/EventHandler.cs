using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{
    //֪ͨ���ȫ�ֹ���
    public static event Action<Vector3> BaseGlobal;

    //������¼�
    public static event Action<Vector3> GetupLightEvent;

    /// <summary>
    /// ����ȫ�ֹ���
    /// </summary>
    /// <param name="basePos"></param>
    public static void CallBaseGlobal(Vector3 basePos)
    {
        BaseGlobal?.Invoke(basePos);
    }

    /// <summary>
    /// �����
    /// </summary>
    /// <param name="_Pos"></param>
    public static void RaisedGetupLightEvent(Vector3 _Pos)
    {
        GetupLightEvent?.Invoke(_Pos);
    }
}
