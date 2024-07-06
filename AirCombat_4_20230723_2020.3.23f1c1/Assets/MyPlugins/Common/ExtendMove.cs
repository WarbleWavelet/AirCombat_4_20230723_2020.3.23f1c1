/****************************************************
    文件：ExtendMove.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/22 0:9:47
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 

public static class ExtendMove 
{
    public static void Translate(this Transform t
        , float speed
        , Vector3 dir
        , Space space=Space.World)
    {
        t.Translate(speed * dir.normalized * Time.deltaTime, space);
    }




    #region Move、SimpleMove


    /// <summary>
    /// <para /> 无限制
    /// <para /> 移动时候需要注意乘以时间
    /// <para /> 需要自己做重力
    /// </summary> 
    public static void Move(this CharacterController ctrl
    , float speed
    , Vector3 dir)
    {
        ctrl.Move(speed * dir.normalized * Time.deltaTime);
    }

    /// <summary>
    /// <para />无限制
    /// <para />移动时候需要注意乘以时间
    /// <para />需要自己做重力
    /// </summary> 
    public static void Move(this Transform t
        , float speed
        , Vector3 dir)
    {
        CharacterController ctrl = t.gameObject.GetOrAddComponent<CharacterController>();
        ctrl.Move(speed * dir.normalized * Time.deltaTime);
    }


    /// <summary>
    ///  <para />只能在平面移动，不带Y轴移动
    ///  <para />默认每帧，移动时不需要乘以deltaTime
    ///  <para />重力自动施加。 如果该角色落地，则返回
    /// </summary>
    public static void SimpleMove(this CharacterController ctrl
        , float speed
        , Vector3 dir)
    {
        ctrl.SimpleMove( speed * dir.normalized );
    }

    /// <summary>
    ///  <para />只能在平面移动，不带Y轴移动
    ///  <para />默认每帧，移动时不需要乘以deltaTime
    ///  <para />重力自动施加。 如果该角色落地，则返回
    /// </summary>
    public static void SimpleMove(this Transform t
        , float speed
        , Vector3 dir)
    {
        CharacterController ctrl = t.gameObject.GetOrAddComponent<CharacterController>();
        ctrl.SimpleMove(speed * dir.normalized);

    }
    #endregion

    public static void MovePosition(this Transform t
        , float speed
        , Vector3 dir)
    {
        Rigidbody rbg = t.gameObject.GetOrAddComponent<Rigidbody>();
        rbg.MovePosition(speed * dir.normalized);
    }


    public static void MovePosition(this Rigidbody  rbg
        , float speed
        , Vector3 dir)
    {
        rbg.MovePosition(speed * dir.normalized);
    }

}




