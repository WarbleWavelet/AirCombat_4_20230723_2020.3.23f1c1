/****************************************************
    文件：ExtendTween.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/12 18:56:25
	功能：
*****************************************************/

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public static partial class ExtendTween
{
    public static void Example(Transform t,Vector3 tar, float duration)
    {
        t.DOMove(tar, duration);
        t.DOLocalMove(tar, duration);
        t.DORotate(tar, duration);
        t.DOLocalRotate(tar, duration);
        t.DORotateQuaternion(Quaternion.identity, duration);
        t.DOLocalRotateQuaternion(Quaternion.identity, duration);
        //

    }
}
public static partial class ExtendTween 
{
    public static void Example_Image(Image image)
    {
        image.DOKill();
        image.DOColor(Color.black, 2f);
    }


}

public static partial class ExtendTween
{
    public static void Move(this Transform t, Vector3 tarPos, float spendTime)
    {
        t.DOMove(tarPos, spendTime);
    }

    public static void LocalMove(this Transform t, Vector3 tarPos, float spendTime)
    {
        t.DOLocalMove(tarPos, spendTime);
    }
}


public static partial class ExtendTween
{
    public static void InitPlayBack<T>(this T t,bool back=false) where T:Tween
    {
        t.SetAutoKill(false);
        if (back)
        {
            t.PlayBackwards();
        }
        else
        {
            t.PlayForward();
        }
    }
}


