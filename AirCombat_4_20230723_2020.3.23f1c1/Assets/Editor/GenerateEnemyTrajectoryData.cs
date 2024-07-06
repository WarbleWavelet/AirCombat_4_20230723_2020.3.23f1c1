using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LitJson;
using UnityEditor;
using UnityEngine;

public class GenerateEnemyTrajectoryData {

	[MenuItem(MenuItemPath.Tools_GenerateEnemyTrajectoryData)]
	private static void Execute()
	{
		PathDataMgr data =  new PathDataMgr();
		data.PathDataDic = new Dictionary<TrajectoryType, IPathData[]>();
		data.PathDataDic[TrajectoryType.STRAIGHT] = InitStraightData(data);
		data.PathDataDic[TrajectoryType.W] = InitWData(data);
		data.PathDataDic[TrajectoryType.ELLIPSE] = InitEllipseData(data);
        //
		string json = JsonUtil.Dic2Json_Trajectory(data.PathDataDic);
		string path = ResourcesPath.CONFIG_ENEMY_TRAJECTORY;

        File.WriteAllText(path,json);
		(path.SubStringStartWith(StringMark.Assets)).ShootAt();

	}

    private static IPathData[] InitStraightData(PathDataMgr data)
    {
        List<IPathData> list = new List<IPathData>();
        list.Add(SetStraightData(100f));
        list.Add(SetStraightData(90f));
        list.Add(SetStraightData(80f));
        return list.ToArray();
    }

    private static IPathData[] InitWData(PathDataMgr data)
	{
		List<IPathData> list = new List<IPathData>();
		list.Add(SetEllipseData());
		return list.ToArray();
	}

    private static IPathData[] InitEllipseData(PathDataMgr data)
    {
        List<IPathData> list = new List<IPathData>();
        list.Add(SetWData(20f));
        return list.ToArray();
    }

    #region pri
    private static StraightPathData SetStraightData(float angle)
	{
		StraightPathData data = new StraightPathData();
		data.Angle = angle;
		return data;
	}
    private static WPathData SetWData(float angle)
    {
        WPathData data = new WPathData();
        data.Angle = angle;
        return data;
    }

    private static EllipsePathData SetEllipseData()
    {
        EllipsePathData data = new EllipsePathData()
        {
            YRatioInScreen = 0.8f,
            XRadius =0.5f,// 1,
            YRadius = 1.0f,//0.5f,
            Precision = 20
        };
        return data;
    }
    #endregion


}
