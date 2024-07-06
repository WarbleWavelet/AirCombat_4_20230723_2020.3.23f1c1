/****************************************************
	文件：IEnterPath.cs
	作者：lenovo
	邮箱: 
	日期：2024/6/11 19:52:26
	功能：
*****************************************************/

using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;


#region EnterPathFactory : IPath


/// <summary>Path中包含EnetrPath</summary>
public class EnterPathMgr : IPath, ICanGetUtility
{

	#region 字段 内部类
	public enum MoveDir
	{
		UP_TO_DOWN,
		LEFT_TO_RIGHT,
		RIGHT_TO_LEFT
	}

	private IEnterPath _enterPath;
	private Vector3 _fromPos;
	#endregion


	#region 生命



	public void Init(Transform trans, float x, float yRatio, MoveDir moveDir, float yRadius = 0f)
	{
		float topY = this.GetUtility<IGameUtil>().CameraSizeHeight() * yRatio + yRadius;
        _enterPath = EnterPathFactory.GetEnterPath(moveDir);
		if (_enterPath == null)
			return;
		_fromPos = _enterPath.Init(trans, x, topY);

	}


	#endregion


	#region IPath
	public Vector3 GetFromPos(int id)
	{
		return _fromPos;
	}

	public Vector2 GetDir()
	{
		if (_enterPath == null)
		{
			return Vector2.zero;

		}

		return _enterPath.EnterDir();
	}
	public PathState GetPathState()
	{
		return PathState.NULL;
	}
	public bool FollowCamera()
	{
		return true;
	}
	#endregion



	#region 说明

	#endregion	

	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}

	public string PathName()
	{
		return ((PathNameBase)_enterPath).PathName();
	}
}


public static class EnterPathFactory
{
	public static IEnterPath GetEnterPath(EnterPathMgr.MoveDir moveDir)
	{
		switch (moveDir)
		{
			case EnterPathMgr.MoveDir.UP_TO_DOWN:
				return new Up2DownEnterPath();
			case EnterPathMgr.MoveDir.LEFT_TO_RIGHT:
				return new Left2RightEnterPath();
			case EnterPathMgr.MoveDir.RIGHT_TO_LEFT:
				return new Right2LeftEnterPath();
			default:
				Debug.LogError("当前类型未进行配置，名称为：" + moveDir);
				return null;
		}
	}

	public static Vector3 InitEnterPath(IEnterPath enterPath, Transform trans, float x, float topY)
	{
		Vector3 fromPos = enterPath.Init(trans, x, topY);
		return fromPos;
	}
}
#endregion  

#region IEnterPath ，IEnterPath实例类




public interface IEnterPath 
{
	/// <summary>标准的offset就是高或宽的一半</summary>
	Vector3 Init(Transform t, float x, float topY);
	/// <summary>不然与Path.GetDir()难区别</summary>
	Vector2 EnterDir();

}

public interface IPathName
{
	string PathName();
}
public abstract class PathNameBase : IPathName
{
	public virtual string PathName()
	{
		return this.GetType().Name;
	}
}
public class Up2DownEnterPath : PathNameBase, IEnterPath, ICanGetUtility
{
	private Transform _trans;
	private float _halfHeight;
	private Vector3 _fromPos;
	private float _fromY;
	private float mToY 
	{ get { return this.GetUtility<IGameUtil>().CameraMinPoint().y + _halfHeight; } }

	public Vector3 Init(Transform trans,float x ,float halfHeight)
	{
		_trans = trans;
		_halfHeight = halfHeight;

		var size = _trans.GetComponent<SpriteRenderer>().bounds.size;
		_fromY = this.GetUtility<IGameUtil>().CameraMaxPoint().y + size.y / 2;
		_fromPos = new Vector3(x, _fromY, _trans.position.z);

		return _fromPos;

	}



	public Vector2 EnterDir()
	{
		if (_trans.position.y > mToY)
		{
			return Vector2.down;   //向下走
		}
		else
		{
			return Vector2.zero;     //停止
		}
	}



	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
}

public class Left2RightEnterPath : PathNameBase, IEnterPath, ICanGetUtility
{
	private Transform _trans;
	private float _offsetY;
	private Vector3 _fromPos;
	private float _halfWidth;
	private float _fromX;
	private float mToX
	{
		get
		{
			return this.GetUtility<IGameUtil>().CameraMinPoint().x - _halfWidth;
		}
	}

	public Vector3 Init(Transform trans,float x, float offsetY)
	{
		_trans = trans;
		_offsetY = offsetY;

		var size = _trans.GetComponent<SpriteRenderer>().bounds.size;
		_halfWidth = size.x / 2;
		_fromX = this.GetUtility<IGameUtil>().CameraMinPoint().x - _halfWidth;
		//
		float y = this.GetUtility<IGameUtil>().CameraMinPoint().y + _offsetY;
		_fromPos = new Vector3(_fromX,  y,  _trans.position.z);
		return _fromPos;

	}



	public Vector2 EnterDir()
	{
		if (_trans.position.x > mToX)
		{
			return Vector2.left;
		}
		else
		{
			return Vector2.zero;
		}
	}


	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
}

public class Right2LeftEnterPath : PathNameBase, IEnterPath, ICanGetUtility
{
	private Transform _trans;
	private float _offsetY;
	private Vector3 _fromPos;
	private float _fromX;
	float _halfWidth;
	private float mToX
	{
		get
		{
			return this.GetUtility<IGameUtil>().CameraMinPoint().x + _halfWidth;
		}
	}

	public Vector3 Init(Transform trans,float x, float offsetY)
	{
		_trans = trans;
		_offsetY = offsetY;
		var size = _trans.GetComponent<SpriteRenderer>().bounds.size;
		_halfWidth = size.x / 2;
		_fromX = this.GetUtility<IGameUtil>().CameraMaxPoint().x + _halfWidth;
		//
		float y = this.GetUtility<IGameUtil>().CameraMinPoint().y + _offsetY;
		_fromPos = new Vector3(_fromX,y, _trans.position.z );


		return _fromPos;
	}



	public Vector2 EnterDir()
	{
		return (_trans.position.x < mToX)  ?  Vector2.right : Vector2.zero;

	}



	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
}
#endregion  


