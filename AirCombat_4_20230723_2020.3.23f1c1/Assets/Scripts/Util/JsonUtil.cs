using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using QAssetBundle;
using UnityEngine;

public class JsonUtil
{
    public static string Dic2Json_Trajectory(Dictionary<TrajectoryType, IPathData[]> dic)
    {
        JsonData jsonData = new JsonData();
        foreach (var item in dic)
        {
            string trajectoryType = item.Key.Enum2String();
            jsonData[trajectoryType] = new JsonData();
            foreach (IPathData trajectoryData in item.Value)
            {
                string json = JsonMapper.ToJson(trajectoryData);
                JsonData after = JsonMapper.ToObject(json);
                jsonData[trajectoryType].Add(after);
                if (trajectoryType != global::TrajectoryType.STRAIGHT.Enum2String())
                {
                    Debug.Log($"{json}\n{after}\n");
                }
            }
        }

        return jsonData.ToJson();
    }

    public static Dictionary<TrajectoryType, IPathData[]> Json2Dic_Trajectory(string json)
    {
        Dictionary<TrajectoryType, IPathData[]> dic = new Dictionary<TrajectoryType, IPathData[]>();
        JsonData data = JsonMapper.ToObject(json);
        for (TrajectoryType type = TrajectoryType.STRAIGHT; type < TrajectoryType.COUNT; type++)
        {
            string tmpKey = type.Enum2String();
            if (data.Keys.Contains(tmpKey))
            {
                dic[type] = GetTrajectoryDataArray(type, tmpKey, data);
            }
            else
            {

                Debug.LogWarning($"读取轨迹类型缺失：{tmpKey}(这个不影响，因为有的后面飞机不会用到)");
            }
        }


        return dic;
    }

    private static IPathData[] GetTrajectoryDataArray(TrajectoryType type, string tmpKey , JsonData data)
    {
        string json = data[tmpKey].ToJson();
        switch (type)
        {
            case TrajectoryType.STRAIGHT:
                return JsonMapper.ToObject<StraightPathData[]>(json);
            case TrajectoryType.W:
                return JsonMapper.ToObject<VPathData[]>(json);
            case TrajectoryType.ELLIPSE:
                return JsonMapper.ToObject<EllipsePathData[]>(json);
            default:

                throw new System.Exception($"异常:{tmpKey}");
                //throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

}
