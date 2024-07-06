using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;



#region PathMgr
/// <summary>
/// 管理一架飞机，路径的初始位置，方向等
/// 管理两种路径,EnterPath的进场路径,平时PathCalc的常规路径
/// </summary>
public class PathMgr     :ICanGetUtility ,ICanSendQuery	,IPath
{
	#region 字属构造

	[SerializeField]	private PathBase _pathBase;


	/// <summary>
	/// 根据飞机的初始位置,生成相应的pathMgr
	/// 所以需要全部生成完再移动,这样避免第一种情况
	/// </summary>
	public PathMgr(Transform enemyTrans, EnemyData enemyData, IPathData pathData,Vector3 creatorPos)
	{
		//以下是顶部左右两个creator的straight轨迹的飞机
		enemyTrans.SetPosX(creatorPos.x);	//x需要creator传过来,不动的的,所以不能取y
		enemyTrans.SetPosY(  GetY(enemyTrans) );  //y需要跟随相机的移动(竖屏),加上一点点偏移
		_pathBase = PathFactory.GetPath(enemyData.trajectoryType) ;
		_pathBase.Init(enemyTrans, pathData);//这里trans穿进去了
	}
    #endregion


    #region IPath
    public Vector3 GetFromPos(int idxInRange)
	{
		return _pathBase.GetFromPos(idxInRange);
	}

	public Vector2 GetDir()
	{
		return _pathBase.GetDir();
	}
    public PathState GetPathState()
    {
        return _pathBase.GetPathState();
    }

    public bool FollowCamera()
	{
		return _pathBase.FollowCamera();
    }
    #endregion

    #region pub


    #endregion

    #region pri


    float GetY(Transform enemyTrans)
	{

		float posY = this.GetUtility<IGameUtil>().CameraMaxPoint().y;
		float height = enemyTrans.GetComponent<SpriteRenderer>().BoundsHeight() / 2.0f;
		return  posY+height ;
	}
	#endregion


	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}

    public string PathName()
    {
        return _pathBase.PathName();
    }
}
#endregion  



















