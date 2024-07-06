using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections.Generic;
using UnityEngine;
using static ExtendAudio;


#region 

public interface  IGameView :ITransform,IEntity2DLayer

{

}

/// <summary></summary>
public abstract class GameLevelViewBase : MonoBehaviour, IGameView, QFramework.ICanGetUtility,ICanGetSystem
{


	#region IColliderEntity2D

	public RectTransform RectTransform
	{
		get { return gameObject.GetComponent<RectTransform>(); }
	}

	public SpriteRenderer SpriteRenderer 
	{ 
		get { return gameObject.GetOrAddComponent<SpriteRenderer>(); }
	}


	#endregion



	#region IGameView
	public abstract Entity2DLayer E_Entity2DLayer { get; }
	public Transform Transform
	{
		get { return transform; }
	}

	#endregion

	/// <summary>设置了层级</summary>
	protected virtual void OnEnable()                                                                                                                                                                          
	{
		
		this.GetSystem<IGameLayerSystem>().SetLayerParent(this);
		transform.SetPosZ(E_Entity2DLayer.Enum2Int());                                                                                                                         
		Init();
	}                      

	public virtual void Init()
	{
		InitComponent();
	}

	#region 
	/// <summary>在基类中会被Init自动调用,Init又在OnEnable中被调用</summary>
	protected virtual void InitComponent()
	{
	}

	#endregion



	#region QF
	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
	#endregion
}



#region GameViewBase  IGameRoot
/// <summary>在GameRoot/PLANE的物体</summary>
public  class PlaneLevelView : GameLevelViewBase, ICanGetSystem  ,IGameRoot
{
	public override Entity2DLayer E_Entity2DLayer { get { return Entity2DLayer.PLANE; } }
	//public EnemyType _enemyType;

	protected override  void OnEnable()
	{
		base.OnEnable();
		SpriteRenderer sr =gameObject.GetComponent<SpriteRenderer>();
		//if (sr.IsNotNullObject())//涉及碰撞,不能这样做
		//{
		//    sr.sortingOrder = _enemyType.Enum2Int();
		//}
	}


}

public  class BGLevelView : GameLevelViewBase, IGameRoot
{
	public override Entity2DLayer E_Entity2DLayer { get { return Entity2DLayer.BACKGROUND; } }
}

public  class EffectLevelView : GameLevelViewBase, IGameRoot
{
	public override Entity2DLayer E_Entity2DLayer { get { return Entity2DLayer.EFFECT; } }
}

#endregion

/// <summary>不破坏双边的命名习惯</summary>
public interface IEffect : IEffectLife
{ }


/// <summary>预制体是Item,但是节点GameRoot/PLANE下</summary>
public abstract class ItemInPlaneLevelViewBase : PlaneLevelView
{
	private IEffectContainer _effectView;
	/// <summary>联系ItemView和各自的EffectContainer</summary>
	public abstract EItemType E_ItemType { get; }

	protected override void InitComponent()
	{
		transform.FindOrNew(GameObjectName.Life).GetOrAddComponent<AutoDespawnOtherComponent>().Init(transform, ResourcesPath.PREFAB_ITEM_ITEM, SpriteRenderer); //后面回收需要key，但用的是一样的预制体
		Trigger2DComponent trigger2DComponent= transform.GetOrAddComponent<Trigger2DComponent>().InitComponent(transform.GetOrAddComponent<Rigidbody2D>());
		transform.FindOrNew(GameObjectName.Collider).GetOrAddComponent<CollideMsgComponent>().InitComponent(trigger2DComponent);
		transform.FindOrNew(GameObjectName.Collider).GetOrAddComponent<CollideMsgFromItemComponent>().Init(CollideEvent);
		if (_effectView == null)
			_effectView = GetEffectContainer();

		_effectView.Init(transform);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_effectView.Begin();
		InitSprite();
	}

	private void InitSprite()
	{
		SpriteRenderer.sprite = this.GetSystem<ISpriteSystem>().Load(SpritePath(),LoadType.RESOURCES);
	}


	/// <summary>找特效的n张图</summary>
	protected abstract IEffectContainer GetEffectContainer();
	/// <summary>原来的用枚举</summary>
	protected abstract string GetGameAudio();

	protected virtual void ItemLogic()
	{
		Destroy(gameObject);
	}
	protected abstract string SpritePath();

	protected virtual void OnDisable()
	{
		gameObject.name = GameObjectName.Item;
		_effectView.Hide();
	}

	private void CollideEvent()
	{
	   this.GetSystem<IAudioSystem>().PlaySound(GetGameAudio().ToString());
		_effectView.Stop(ItemLogic);
	}

}


/// <summary>
/// 根据层次枚举Entity2DLayer设置posZ
/// <br/>因为强相关枚举,所以时统一的全局Pos,不要用LocalPos,违背原则
/// </summary>
public abstract class SetPosZByLayerLevelView : GameLevelViewBase
{
	protected override void OnEnable()
	{
		float z = E_Entity2DLayer.Enum2Int();
		transform.SetPosZ(z);
		Init();
	}

}



#endregion





