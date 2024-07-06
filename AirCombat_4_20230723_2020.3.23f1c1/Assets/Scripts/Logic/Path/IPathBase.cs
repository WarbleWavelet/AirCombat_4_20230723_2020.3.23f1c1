/****************************************************
	文件：IPathBase.cs
	作者：lenovo
	邮箱: 
	日期：2024/6/11 18:54:35
	功能：
*****************************************************/

using QFramework.AirCombat;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static ExtendGeometry;


#region IPath ，PathBase
/// <summary>
/// 这名字原本是IPathBase的,
/// 有部分方法PathMgr完全一样,所以再拆分组合</summary>
public interface IPath
{ 
	Vector3 GetFromPos(int id);
	Vector2 GetDir();
	PathState GetPathState();
	bool FollowCamera();
    string PathName();
}

/// <summary>
/// 路径接口，提供具体的路径的计算方法
/// </summary>
public interface IPathBase :IPath  ,IPathName
{
	void Init(Vector3 startPos, SpriteRenderer sr, IPathData pathData);
	void Init(Transform t, IPathData pathData);

}

public abstract class PathBase : IPathBase
{


	protected PathState _pathState;
	protected IPathCalc _pathCalc;
	protected IPathData _pathData;

	/// <summary>队头的飞机.需要初始位置和SR的综合考虑</summary>
	protected Vector3 _startPos;
	protected SpriteRenderer _sr;
	/// <summary>这种的原因是飞机是会动的</summary>
	protected Transform _enemyTrans;

	/// <summary>
	/// 根据图片大小设置出场位置
	/// </summary>
	public virtual void Init(Transform t, IPathData pathData)
	{
		_enemyTrans = t;
		DoByChild(pathData);
	}
	public virtual void Init(Vector3 startPos, SpriteRenderer sr, IPathData pathData)
	{
		_startPos = startPos;
		_sr = sr;
		DoByChild(pathData);
	}

	/// <summary>没加飞机在队伍中的位置</summary>
	public abstract Vector3 GetFromPos(int id);
	public abstract Vector2 GetDir();
	public abstract bool FollowCamera();

	/// <summary>因为IPathData未确定</summary>
	void DoByChild(IPathData pathData)
	{
		//_pathData = pathData;
		//_pathCalc.InitComponent(pathData);
	}

	public virtual PathState GetPathState()
	{
		return PathState.NULL;
	}

	public virtual string PathName()
	{ 
		 return this.GetType().Name;
	}
}

#endregion


#region  PathBase实例类

public class EllipsePath : PathBase, QFramework.ICanGetUtility
{
	private EnterPathMgr _enterPathMgr = new EnterPathMgr();
	private EllipsePathData _data;
	string _pathName;
	public override void Init(Transform trans, IPathData data)
	{
		base.Init(trans, data);
		if (data is EllipsePathData)
		{
			_data = data as EllipsePathData;
		}
		else
		{
			Debug.LogError("当前传入椭圆形路径的数据类型错误，类型为：" + data);
			return;
		}
		float cameraHeight = this.GetUtility<IGameUtil>().CameraSizeHeight();//打点看一下==6
		float centerY = cameraHeight * (float)_data.YRatioInScreen;//6*0.8=4.8
		float topY = centerY + (float)_data.YRadius; //上端点
		_enterPathMgr.Init(_enemyTrans, 0, (float)_data.YRatioInScreen, EnterPathMgr.MoveDir.UP_TO_DOWN, (float)_data.YRadius);
		_data.Center = new Vector2(0, centerY);
		//
		Vector2 startPos = new Vector2(_enemyTrans.position.x, topY);
		_data.StartPos = startPos;
		//
		//
		_pathCalc = new EllipsePathCalc();
		_pathCalc.Init(_data);
	}

	public override Vector3 GetFromPos(int id)
	{
		return _enterPathMgr.GetFromPos(id);
	}

	public override Vector2 GetDir()
	{
		_data.XPos = _enemyTrans.position.x;
		STest.IsBossPlane(_enemyTrans);
		if (_enterPathMgr.GetDir() == Vector2.zero)
		{
			_pathName = ((PathCalcBase)_pathCalc).PathName();
            return _pathCalc.GetDir();
		}
		else
		{
			_pathName = _enterPathMgr.PathName();
            return _enterPathMgr.GetDir();
		}

	}

	public override bool FollowCamera()
	{
		return true;
	}
    public override string PathName()
    {
		return _pathName;
    }
    public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
}

public class StraightPath : PathBase
{
	float _xOffset;
	float _yOffset;


	#region 生命
	public override void Init(Transform t, IPathData pathData)
	{
		base.Init(t, pathData);
		_pathCalc = new StraightPathCalc();
		_pathCalc.Init(pathData);
	}
	public override void Init(Vector3 startPos, SpriteRenderer sr, IPathData pathData)
	{
		base.Init(startPos, sr, pathData);
		_pathCalc = new StraightPathCalc();
		_pathCalc.Init(pathData);
	}
	#endregion





	#region 实现
	public override Vector3 GetFromPos(int idxInRange)
	{
		//方便后面修改
		var sr = _enemyTrans.GetComponent<SpriteRenderer>();
		Vector3 pos = _enemyTrans.position;
		//
		float height = sr.BoundsHeight();
		_yOffset = height * 0.5f;//按中心点的,自定义
		float y = (pos.y) + (height / 2.0f + _yOffset + height / 2.0f) * idxInRange; //这样写明显点
		float x = _pathCalc.GetXArr(y, pos)[0];
		float z = Entity2DLayer.PLANE.Enum2Int();

		return new Vector3(x, y, z);
	}

	public override Vector2 GetDir()
	{
		return -_pathCalc.GetDir();
	}

	public override bool FollowCamera()
	{
		return false;
	}




    #endregion

}




public class WPath : PathBase, ICanGetUtility
{

	private float _leftX;
	private float _rightX;
	private List<Vector3> _startPosOffset;
	private List<float> _xPos = new List<float>();
	private List<VPathCalc> _wTrajectory;
	private int _vCount;
	private EnterPathMgr _enterPath = new EnterPathMgr();

	#region PathBase

	public override void Init(Transform trans, IPathData pathData)
	{
		base.Init(trans, pathData);
		_pathState = PathState.ENTER;
		_vCount = 2;
		InitData();
		InitXRange();
		var datas = InitPathData(pathData);
		InitVTrajectory(datas);
		_enterPath.Init(trans, 0, 0.8f, EnterPathMgr.MoveDir.LEFT_TO_RIGHT);
	}


	public override Vector3 GetFromPos(int id)
	{
		if (id > 0)
		{
			Debug.LogError("W阵型只支持飞机单飞");
			return Vector3.zero;
		}

		return _enterPath.GetFromPos(id);
	}

	public override Vector2 GetDir()
	{
		switch (_pathState)
		{
			case PathState.ENTER:
				Vector3 dir = _enterPath.GetDir();
				if (dir == Vector3.zero)
				{
					_pathState = PathState.FORWARD_MOVING;
					return Vector2.zero;
				}
				else
				{
					return dir;
				}
			case PathState.FORWARD_MOVING:
			case PathState.BACK_MOVING:
				SetMovingState();
				return MovingDir(GetBaseDir());
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public override PathState GetPathState()
	{ 
		return _pathState;
	}


    public override bool FollowCamera()
	{
		return true;
	}
	#endregion



	#region pri


	private void InitVTrajectory(IPathData[] datas)
	{
		_wTrajectory = new List<VPathCalc>();
		VPathCalc temp = null;
		foreach (var data in datas)
		{
			temp = new VPathCalc();
			temp.Init(data);
			_wTrajectory.Add(temp);
		}
	}

	private void InitData()
	{
		var halfWidth = _enemyTrans.GetComponent<SpriteRenderer>().bounds.size.x / 2;
		_leftX = this.GetUtility<IGameUtil>().CameraMinPoint().x + halfWidth;
		_rightX = this.GetUtility<IGameUtil>().CameraMaxPoint().x - halfWidth;

	}

	private void InitXRange()
	{
		float offsetX = (_rightX - _leftX) / (_vCount * 2);
		int count = GetPointCount();
		for (int i = 0; i < count; i++)
		{
			_xPos.Add(_leftX + offsetX * i);
		}
	}

	private int GetPointCount()
	{
		int count = 3;
		for (int i = 0; i < _vCount - 1; i++)
		{
			count += 2;
		}

		return count;
	}

	private IPathData[] InitPathData(IPathData data)
	{
		VPathData source = null;
		if (data is VPathData)
		{
			source = data as VPathData;
			source.Angle = -source.Angle;
		}
		else
		{
			Debug.LogError("当前传入数据类型错误，传入类型为：" + data);
		}

		IPathData[] datas = new IPathData[_vCount];
		VPathData temp = null;
		for (int i = 0; i < _vCount; i++)
		{
			temp = new VPathData();
			temp.Angle = source.Angle;
			temp.XPos = new float[3];
			temp.XPos[0] = _xPos[i * 2];
			temp.XPos[1] = _xPos[i * 2 + 1];
			temp.XPos[2] = _xPos[i * 2 + 2];
			datas[i] = temp;
		}

		return datas;
	}
	private Vector2 GetBaseDir()
	{
		foreach (VPathCalc trajectory in _wTrajectory)
		{
			if (trajectory.SetCurTrajectory(_enemyTrans.position.x))
			{
				return trajectory.GetDir();
			}
		}

		return _wTrajectory[_wTrajectory.Count - 1].GetDir();
	}

	private Vector2 MovingDir(Vector2 direction)
	{
		if (_pathState == PathState.BACK_MOVING)
		{
			if (_enemyTrans.position.x < _leftX)
			{
				return direction;
			}
			else
			{
				return direction.Reversal();
			}
		}
		else if (_pathState == PathState.FORWARD_MOVING)
		{
			if (_enemyTrans.position.x > _rightX)
			{
				return direction.Reversal();
			}
			else
			{
				return direction;
			}
		}
		else
		{
			return Vector2.zero;
		}
	}

	private void SetMovingState()
	{
		if (_enemyTrans.position.x < _leftX)
		{
			_pathState = PathState.FORWARD_MOVING;
		}
		else if (_enemyTrans.position.x > _rightX)
		{
			_pathState = PathState.BACK_MOVING;
		}
	}
	#endregion



	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
}





public class StayOnTopPath : PathBase
{
	private EnterPathMgr _enterPath = new EnterPathMgr();

	public override void Init(Transform trans, IPathData trajectory)
	{
		base.Init(trans, trajectory);
		_enterPath.Init(trans, 0, 0.8f, EnterPathMgr.MoveDir.UP_TO_DOWN);
	}

	public override Vector3 GetFromPos(int id)
	{
		return _enterPath.GetFromPos(id);
	}

	public override Vector2 GetDir()
	{
		return _enterPath.GetDir();
	}

    public override bool FollowCamera()
	{
		return true;
	}
}





#endregion

#region PathFactory
public static class PathFactory
{
	/// <summary>通过Path类型实例一份Path实例类</summary>
	public static PathBase GetPath(TrajectoryType trajectoryType)
	{
		switch (trajectoryType)
		{
			case TrajectoryType.STRAIGHT: return new StraightPath();
			case TrajectoryType.W: return new WPath();
			case TrajectoryType.STAY_ON_TOP: return new StayOnTopPath();
			case TrajectoryType.ELLIPSE: return new EllipsePath();
			case TrajectoryType.ROTATE: Debug.LogError("PathFactory未初始该轨道：" + trajectoryType); return null;
			default: Debug.LogError("PathFactory的当前轨迹未添加，名称：" + trajectoryType); return null;
		}
	}
}
#endregion










