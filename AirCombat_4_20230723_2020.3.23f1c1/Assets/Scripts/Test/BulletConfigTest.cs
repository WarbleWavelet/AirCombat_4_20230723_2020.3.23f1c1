using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LitJson;
using QFramework;
using QFramework.AirCombat;
using UnityEngine;

public class BulletConfigTest : ITest    ,ICanGetUtility
{
    public IEnumerator Execute()
    {
        bool complete = false;
        this.GetUtility<ILoadUtil>().LoadConfig(ResourcesPath.CONFIG_BULLET_CONFIG, (value) =>
        {
            bool change = false;
            string json = (string) value;
            //
            JsonData  data = json.JsonStr2JsonData_JsonMapper();
          

            
            foreach (string iKey in data.Keys)
            {
                if (iKey == BulletType.PLAYER.Enum2String())
                { 
                    continue;
                }
                change = ChangeValue(data[iKey]);

                foreach (string jKey in data[iKey].Keys) //  "ENEMY_NORMAL_0": {等
                {
                    if (jKey == JsonKey.Events)
                    {
                        foreach (JsonData jsonData in data[iKey][jKey])
                        {
                            if (jsonData[JsonKey.Data].Keys.Contains(JsonKey.trajectory))
                            {
                                if (ChangeValue(jsonData[JsonKey.Data]))
                                {
                                    change = true;
                                }
                            }
                        }
                    }
                }
            }

            if (change)
            {
                File.WriteAllText(ResourcesPath.CONFIG_BULLET_CONFIG,data.ToJson());
            }
            complete = true;
        });

        while (!complete)
        {
            yield return null;
        }
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    private bool ChangeValue(JsonData jsonData)
    {
        if (!jsonData[JsonKey.trajectory][0].IsArray) //不是数组
        { 
            return false;
        }
        bool change = false;
        //
        TempTrajectoryData temp  = jsonData.JsonData2Object_JsonMapper<TempTrajectoryData>();
        //

        ReverseIfMoreZero(temp,change);
        if (change)
        {
            Save2(temp, jsonData);
        }

        return change;
    }



    #region pri
    void ReverseIfMoreZero (TempTrajectoryData temp, bool change)
    {
        foreach (int[] array in temp.trajectory)
        {
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j] > 0)
                {
                    array[j] = -array[j];
                    change = true;
                }
            }
        }
    }
    void Save2(TempTrajectoryData from, JsonData to)
    {
        to[JsonKey.trajectory].Clear();
        for (int i = 0; i < from.trajectory.Length; i++)
        {
            to[JsonKey.trajectory].Add(i);
            to[JsonKey.trajectory][i] = new JsonData();
            for (int j = 0; j < from.trajectory[i].Length; j++)
            {
                to[JsonKey.trajectory][i].Add(j);
                to[JsonKey.trajectory][i][j] = from.trajectory[i][j];
            }
        }
    }
    #endregion

}

public class TempTrajectoryData
{
    public int[][] trajectory;
}
