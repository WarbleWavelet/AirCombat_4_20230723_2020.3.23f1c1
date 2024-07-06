using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


/// <summary>检测MsgEvent是否有重复项</summary>
public class MsgEventTest : ITest
{
    public IEnumerator Execute()
    {
        (typeof(MsgEvent)).RepeatConstException();
        yield return null;
    }

}