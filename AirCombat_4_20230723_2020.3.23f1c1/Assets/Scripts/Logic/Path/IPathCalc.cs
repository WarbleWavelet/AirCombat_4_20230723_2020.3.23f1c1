using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using static ExtendGeometry;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;



#region 接口
/// <summary>
/// 轨迹接口，提供基础图形轨迹的计算方法
/// </summary>
public interface IPathCalc
{
	void Init(IPathData data);
	/// <summary>比如椭圆,一个y有两个x坐标对应</summary>
	float[] GetXArr(float y, Vector2 firstPos);
	/// <summary>比如椭圆,一个x有两个y坐标对应</summary>
	float[] GetYArr(float x, Vector2 firstPos);
	Vector2 GetDir();

	float GetZRotate();
}

public abstract class PathCalcBase : IPathName
{
	public string PathName()
	{
		return this.GetType().Name;
	}
}
#endregion



#region 实现类




/// <summary>
/// 椭圆 ,圆,扇形,鸡爪,竹叶
/// <br/>01 ↑ 
/// <br/>02 ↖↗ 
/// <br/>03 ↖↑↗
/// <br/>04 ↖↑↑↗
/// <br/>05 ↖↖↑↗↗
/// </summary>
public class EllipsePathCalc : PathCalcBase, IPathCalc 
{
	private EllipsePathData _data;
	private float _perimeter;
	//
	/// <summary>图形边长的点坐标</summary>
	private Vector3[] _sidePointPosArr;
	/// <summary>
	/// 边上点的数量.初始化一块赋值._sidePointPosArr有变化得一起改
	/// //截取 "Precision": 20
	/// </summary>
	private int _sidePointCnt;
	//
	private int _middleIndex;
	private int _curIndex;
	/// <summary>用一个自定义的熟悉图形类来代入</summary>
	ExtendGeometry.Ellipse _ellipse;


	public void Init(IPathData data)
	{
		if (data is EllipsePathData)
		{
			_data = data as EllipsePathData;
		}
		else
		{
			Debug.LogError("当前数据不是EllipseData类型，类型为:" + data);
			return;
		}

		//float y2 = (float)_data.YRadius + _data.Center.y; //0.5+4.8=5.3
		float y = (float)_data.YRadius + _data.Center.y;
		Vector2 pos = new Vector2(_data.Center.x, y);
		_ellipse = new Ellipse(_data.XRadius, _data.YRadius, pos); //原来定义Y是端的,x是长的
		//
		_sidePointPosArr = InitSidePointPosArr(_data);
		_sidePointCnt = _sidePointPosArr.Length;
		//
		STest.LogArr(_sidePointPosArr);
		//
		_middleIndex = _sidePointPosArr.Length / 2;
		_curIndex = GetStartIndex(_sidePointPosArr, _data.StartPos);
	}



	#region pub

	public float[] GetXArr(float y, Vector2 startPos)
	{
		if (_ellipse == null)
			return null;
        float[] pre = _ellipse.GetXArr(y,startPos);
        return pre;
    }


	public float[] GetYArr(float x, Vector2 startPos)
	{
		if (_ellipse == null)
			return null;
		float[] pre=   _ellipse.GetYArr(x, startPos);
		return pre;
	}




	public Vector2 GetDir()
	{
		_curIndex = NextIndex(_curIndex);
		int nextIndex = (_curIndex + 1).Round(_sidePointCnt) ;


		return  _sidePointPosArr.Dir(_curIndex, nextIndex);
	}


	public float GetZRotate()
	{
		return 0;
	}
	#endregion



	#region pri    


	/// <summary>图形边长的点坐标</summary>	
	private Vector3[] InitSidePointPosArr(EllipsePathData ellipse)
	{

		#region 数据
		/**
		"ELLIPSE": [
		{
			"YRatioInScreen": 0.8,
			"XRadius": 1,
			"YRadius": 0.5,
			"Precision": 20
		}        
		*/
		#endregion  
		int precision = (ellipse.Precision).MultipleMore( 4);//一圈的精确度 20
		float xLeft = _ellipse.Left.x;

		//x轴上的坐标分成多少份，举例，顶点为4个，x轴上坐标要被分成2份
		float xTmp = xLeft;//这个循环中变量
		float[] yArr;
		Vector3[] posArr = new Vector3[precision];
		int halfPrecision = precision / 2;  //上下两份,理解为半圈的精确度 10
		float xOffset = (float)_ellipse.XAxis / halfPrecision; // 2/10=0.2
		int symIdx = 0;//对称的索引,精确度20个点, 0对19, 1对18, 2对17
		//posArr[14] = new Vector3(0,-10);//没设置到默认为0

        for (int i = 0; i < halfPrecision + 1; i++)//+1包括了可能正在两个半圈中间的那个
		{
			yArr = GetYArr(xTmp, Vector3.zero);//只需要方向,用原来的坐标(所以不需要移动中心)就可以求一样的方向

			if ((yArr[0] - _data.Center.y).Abs() < 0.01f)//自定义的是(1,0.5,(0,5.3)),_data时老的中心(0.6*0.8),误打误撞有撞击效果,所以不改了 
            {
				posArr[i] = new Vector3(xTmp, yArr[0]);
			}
			else 
			{
				symIdx = posArr.Length - i - 1;
				//排大小 ,我有自制的Ellipse类,已经排好了大小,索引小,值就小
				posArr[i] = new Vector3(xTmp, yArr[1]);
				posArr[symIdx] = new Vector3(xTmp, yArr[0]);  //对称的

				//if (yArr[0] < _data.Center.y) //下面的
				//{
				//    posArr[i] = new Vector3(xTmp, yArr[1]);
				//    posArr[symIdx] = new Vector3(xTmp, yArr[0]);  //对称的
				//}
				//else if (yArr[0] > _data.Center.y)//上面的
				//{
				//    posArr[i] = new Vector3(xTmp, yArr[0]);
				//    posArr[symIdx] = new Vector3(xTmp, yArr[1]);  //对称的
				//}            
			}



			xTmp += xOffset;
		}
		return posArr;
	}


	private int GetStartIndex(Vector3[] posArr, Vector2 startPos)
	{
		int index = 0;
		float distance = 0;
		float oldDistance = 0;

		int offset = posArr.Length / 4;
		List<KeyValuePair<int, float>> points = new List<KeyValuePair<int, float>>(4);
		for (int i = 0; i < 4; i++)
		{
			distance = Vector3.Distance(posArr[index], startPos);
			points.Add(new KeyValuePair<int, float>(index, distance));

			index += offset;
		}

		points = points.OrderBy(pair => pair.Value).ToList();
		points.RemoveRange(2, points.Count - 2);
		points = points.OrderBy(pair => pair.Key).ToList();

		bool mark = true;
		index = points[0].Key;
		oldDistance = Vector3.Distance(posArr[index], startPos);
		for (int i = points[0].Key + 1; i <= points[1].Key; i++)
		{
			distance = Vector3.Distance(posArr[i], startPos);
			mark = oldDistance > distance;

			if (!mark)
			{
				break;
			}
			else
			{
				index = i;
			}
		}

		return index;
	}



	private int NextIndex(int curIndex)
	{
		float nextX = _sidePointPosArr[(curIndex + 1).Round(_sidePointCnt)].x;
		if (curIndex < _middleIndex)
		{
			if (nextX < _data.XPos)
			{
				curIndex++;
			}
		}
		else
		{
			if (nextX > _data.XPos)
			{
				curIndex++;
			}
		}

		curIndex = (curIndex).Round(_sidePointCnt);
		return curIndex;
	}


	#endregion


}


/// <summary>
/// 直线轨迹
/// </summary>
public class StraightPathCalc : PathCalcBase, IPathCalc
{
	/// <summary>Vector2( cos(rad),sin(rad) ) </summary>
	private Vector2 _dir;
	/// <summary>tan(rad)斜率 比率</summary>
	private float _k;
	private float _zRotate;
	/// <summary>x,y互求的tmp</summary>
	private float[] _temp;


	public void Init(IPathData data)
	{
		if (data is StraightPathData)
		{
			var tmp = (StraightPathData)data;
			Init(tmp.Angle);
		}
		else
		{
			Debug.LogError("当前传入数据不是直线轨迹StraightTrajectory:" + data);
		}
	}


	#region pub IPathCalc


	public void Init(double angle)
	{
		Init((float)angle);
	}


	public void Init(float degree)
	{
		float radian = degree.Degree2Radian();
		_temp = new float[1];
		_dir = new Vector2(radian.Cos(), radian.Sin()).normalized;
		_k = radian.Tan();
		_zRotate = degree - 90;
	}

	public float[] GetXArr(float y, Vector2 startPos)
	{
		_temp[0] = ExtendGeometry.Straight.GetX(y, _k, startPos);
		return _temp;
	}

	public float[] GetYArr(float x, Vector2 startPos)
	{
		_temp[0] = ExtendGeometry.Straight.GetY(x, _k, startPos);
		return _temp;
	}


	public Vector2 GetDir()
	{
		return _dir;
	}


	public float GetZRotate()
	{
		return _zRotate;
	}
	#endregion


}


/// <summary>V W之类</summary>
public class VPathCalc : PathCalcBase, IPathCalc
{
	private VPathData _data;
	public StraightPathCalc[] _straights;
	private float[] _xPos = new float[3];
	private StraightPathCalc _curTrajectory;

	public void Init(IPathData data)
	{
		if (data is VPathData)
		{
			_data = data as VPathData;
		}
		else
		{
			Debug.LogError("当前传入数据类型错误，传入类型为：" + data);
		}

		_xPos = _data.XPos;
		InitStraights(_data);
		_curTrajectory = _straights[0];
	}

	private void InitStraights(VPathData data)
	{
		_straights = new StraightPathCalc[2];
		_straights[0] = new StraightPathCalc();
		_straights[0].Init(data.Angle);
		_straights[1] = new StraightPathCalc();
		_straights[1].Init(-data.Angle);
	}

	public float[] GetYArr(float x, Vector2 startPos)
	{
		_curTrajectory = _straights[GetTrajectoryIndex(x)];
		return _curTrajectory.GetYArr(x, startPos);
	}

	private int GetTrajectoryIndex(float x)
	{
		if (x < _xPos[1])
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}

	public float[] GetXArr(float y, Vector2 startPos)
	{
		Debug.LogError("W轨迹的GetX方法无法获取正确的值");
		return null;
	}

	public bool SetCurTrajectory(float x)
	{
		if (x >= _xPos[0] && x <= _xPos[2])
		{
			_curTrajectory = _straights[GetTrajectoryIndex(x)];
			return true;
		}
		else
		{
			return false;
		}
	}

	public Vector2 GetDir()
	{
		return _curTrajectory.GetDir();
	}

	public float GetZRotate()
	{
		return 0;
	}

}





public class RotatePathCalc : PathCalcBase, IPathCalc
{
	public void Init(IPathData data)
	{
		throw new System.NotImplementedException();
	}

	public float[] GetYArr(float x, Vector2 startPos)
	{
		throw new System.NotImplementedException();
	}

	public float[] GetXArr(float y, Vector2 startPos)
	{
		throw new System.NotImplementedException();
	}

	public Vector2 GetDir()
	{
		throw new System.NotImplementedException();
	}

	public float GetZRotate()
	{
		throw new System.NotImplementedException();
	}
}
#endregion
