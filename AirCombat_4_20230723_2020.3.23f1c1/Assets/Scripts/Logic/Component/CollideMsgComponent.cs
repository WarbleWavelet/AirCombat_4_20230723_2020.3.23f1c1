/****************************************************
	文件：CollideMsgComponent.cs
	作者：lenovo
	邮箱: 
	日期：2024/6/10 14:33:6
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
																 
public class CollideMsgComponent : MonoBehaviour
{
	Trigger2DComponent _trigger2DComponent;
	public CollideMsgComponent InitComponent(Trigger2DComponent trigger2DComponent)
	{
		_trigger2DComponent	= trigger2DComponent;	
		trigger2DComponent.EnterLst.Add(A);
		return this;
	}



	private void A(Collider2D otherCollider)
	{
		//Debug.Log(CommonClass.Log_ClassFunction() + $"\n:{otherCollider.gameObject.name}碰撞{gameObject.name}");
		Transform self = _trigger2DComponent.transform;
		Transform other = otherCollider.transform;
		//
		List<ICollideMsg> selfMsg = self.GetComponentsInChildren<ICollideMsg>().ToList();
		List<ICollideMsg> otherMsg = otherCollider.GetComponentsInChildren<ICollideMsg>().ToList();
		//需要判空
		selfMsg.ForEach(colliderMsg => colliderMsg.CollideMsg(other));
		otherMsg.ForEach(colliderMsg => colliderMsg.CollideMsg(self));
	}

}



