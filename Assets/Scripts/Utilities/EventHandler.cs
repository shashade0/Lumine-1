using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{
    //֪ͨ���ȫ�ֹ���
    public static event Action<Vector3> BaseGlobal;
    public static void CallBaseGlobal(Vector3 basePos)
    {
        BaseGlobal?.Invoke(basePos);
    }
}
