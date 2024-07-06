using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExtendGeometry;

/// <summary>
/// 挂在飞机下的BulletPoint节点下
/// <br/>设置子弹的位置
/// <br/>主要是Player在用,1条直直的,2条V(有弧度,是椭圆),3条是鸡爪(有弧度,是椭圆),子弹不会旋转,竖直的
/// </summary> 
public class BulletPointsCalcEllipseComponent :MonoBehaviour
{
	[SerializeField] private Vector3[] _pointArr;
	//[SerializeField] EllipsePathCalc _ellipseTrajectory;
	[SerializeField] SpriteRenderer _sr;
	[SerializeField] Transform _muzzleTrans;
	public BulletPointsCalcEllipseComponent InitComponent(SpriteRenderer sr,Transform muzzleTrans)
	{
		  _sr = sr;
		_muzzleTrans=muzzleTrans;
		//_ellipseTrajectory =InitTrajectory(sr.BoundsSizeX());

		return this;
	}

	#region pub
	/// <summary>多少列子弹射击方向.向霰弹枪一样,发射一圈又一圈</summary>
	public Vector3[] GetPointArr(int columCnt, Vector3 muzzlePos, float boundsSizeX, EDir muzzleDir)
	{
		Vector3[] posArr = GetPointOffsetArr(columCnt, boundsSizeX,muzzleDir);
		//更新位置
		Vector3[]  tempArr = new Vector3[columCnt];
		for (int i = 0; i < columCnt; i++)
		{
			tempArr[i] = ExtendVector3.Vector3Add(muzzlePos, posArr[i]);
		}


		return tempArr;
	}
	#endregion



	#region pri
	private void OnDisable()
	{
		_pointArr = null;
	}

	/// <summary>
	/// 因为理解错误,敌人发生过Y值过高的bug
	/// <br/>这里椭圆中心为Vector3,与muzzle的作用在外面计算
	/// </summary>
	private Vector3[] GetPointOffsetArr(int colCnt, float boundsSizeX ,EDir muzzleDir)
	{
		if (_pointArr != null && _pointArr.Length == colCnt   ) //跟之前的列数一样	.这一段决定了是初始时的坐标,所以外面要加上muzzle的实时坐标,来更新位置
		{
			return _pointArr;
		}          
		if (colCnt == 0)
		{
			throw new System.Exception("数值不能为0异常");
		}		
		_pointArr = new Vector3[colCnt];
		if (colCnt == 1)
		{
			_pointArr[0] = Vector3.zero;
			return _pointArr;
		}
		//
		float xRadius = boundsSizeX / 2.0f;
		float yRadius = 0.3f;
		Ellipse ellipse  = new Ellipse(xRadius, yRadius, Vector2.zero);;
		//
		float xHalf = boundsSizeX / 4;	//自定义初始一行子弹的初始宽度
		float xWidth = xHalf * 2;		//初始一行子弹的宽度
		float offset = xWidth / (colCnt-1);//2个,间隔及时全部;3个,间隔就是一半;4个,间隔就是1/3;5个,间隔就是1/4
		float xMin = -xHalf -offset;         // 初始一行子弹第一个的x值	;-offset是方便后面循环,相当于第0或-1个子弹,反正就是第1的前面
		float x  = xMin + ellipse.Top.x; //从左到右的第一个x
		float y;
		//
		if (muzzleDir == EDir.DOWN)
		{
			for (int i = 0; i < colCnt; i++)
			{
				x += offset;
				y = ellipse.GetYBottom(x); //椭圆底部端点的y值									
                _pointArr[i] = new Vector3(x, y);
			}
		}
		else if (muzzleDir == EDir.UP)
		{
			for (int i = 0; i < colCnt; i++)
			{
				x += offset;
				y = ellipse.GetYTop(x); //椭圆顶部端点的y值
				_pointArr[i] = new Vector3(x, y);
			}
		}
		else
		{

			throw new System.Exception("未定义");
		}
			return _pointArr;
	}

	
																				   

	/// <summary>以sr的宽度作为椭圆的x轴长</summary>
	private EllipsePathCalc InitTrajectory(float xDiameter,  float yDiameter=0.6f)
	{
		EllipsePathData data = new EllipsePathData();
		data.XRadius = xDiameter / 2.0f;
		data.YRadius = yDiameter/ 2.0f;
		//
		var pos = _muzzleTrans.position;  // _muzzleTrans是Y轴上的端点(上方的端点)
		pos.y -= (float)data.YRadius;	 //端点减掉y轴半径就是椭圆中心
		data.Center = pos;
	   //
		EllipsePathCalc ellipsePathCalc = new EllipsePathCalc();
		ellipsePathCalc.Init(data);
		return ellipsePathCalc;
	}


	#endregion
}
