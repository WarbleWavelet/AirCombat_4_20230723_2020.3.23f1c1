using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using QFramework;
using QFramework.AirCombat;
using UnityEngine;



public struct GameDataMgrInitedEvent { }
public class GameDataMgr : NormalSingleton<GameDataMgr>  ,ICanGetUtility,ICanGetSystem	,ICanSendEvent
{
	private Dictionary<Type, object> _bulletDataDic = new Dictionary<Type, object>();

	private BulletName[] _bossBullets =
	{
		BulletName.ENEMY_BOSS_0,
		BulletName.ENEMY_BOSS_1
	};

	public void Init( )
	{
		InitData<AllBulletData>(ResourcesPath.CONFIG_BULLET_CONFIG,InitBulletData);
		this.SendEvent<GameDataMgrInitedEvent>();
	}
    #region pub


	public T Get<T>()
	{
		Type type = typeof(T);
		if (_bulletDataDic.ContainsKey(type))
		{
			return (T) _bulletDataDic[type];
		}
		else
		{
			Debug.LogError("当前类型未初始化，类型名："+type);
			return default(T);
		}
	}
    #endregion

	#region pri



    private void InitData<T>(string path,Action callback)
	{
        this.GetUtility<ILoadUtil>().LoadConfig(path, json =>
		{
			var data = JsonMapper.ToObject<T>((string) json);
			_bulletDataDic.Add(typeof(T),data);
			callback.DoIfNotNull();
		});
	}

	private void InitBulletData()
	{
		
		IReader reader = this.GetUtility<IReaderUtil>().GetReader(ResourcesPath.CONFIG_BULLET_CONFIG);

		foreach (BulletName bullet in _bossBullets)
		{
			TaskQueueMgr<JsonData>.Single.AddQueue(()=> reader[bullet.ToString()]["Events"] );
		}
		
		TaskQueueMgr<JsonData>.Single.Execute(CallBack);
	}
	private void CallBack(JsonData[] data)
	{
		Dictionary<BulletName,List<BulletEventData> > 
			datas = new Dictionary<BulletName, List<BulletEventData>>();
		for (int i = 0; i < data.Length; i++)
		{
			datas[_bossBullets[i]] = new List<BulletEventData>();
			foreach (JsonData jsonData in data[i])
			{
				var json = jsonData["Data"].ToJson();
				var type = (BulletEventType) int.Parse(jsonData["Type"].ToJson());
				BulletEventData temp;
				switch (type)
				{
					case BulletEventType.CHANGE_SPEED:
						temp = JsonMapper.ToObject<ChangeSpeedData>(json);
						break;
					case BulletEventType.CHANGE_TRAJECTORY:
						temp = JsonMapper.ToObject<ChangeTrajectoryData>(json);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
						
				datas[_bossBullets[i]].Add(temp);
			}
		}
		
		
		AllBulletData bulletData = _bulletDataDic[typeof(AllBulletData)] as AllBulletData;
		foreach (var pair in datas)
		{
			switch (pair.Key)
			{
				case BulletName.ENEMY_BOSS_0:
					for (int i = 0; i < bulletData.ENEMY_BOSS_0.Events.Length; i++)
					{
						bulletData.ENEMY_BOSS_0.Events[i].Data = pair.Value[i];
					}
					break;
				case BulletName.ENEMY_BOSS_1:
					for (int i = 0; i < bulletData.ENEMY_BOSS_1.Events.Length; i++)
					{
						bulletData.ENEMY_BOSS_1.Events[i].Data = pair.Value[i];
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
	}
	#endregion	

    public IArchitecture GetArchitecture()
    {
		return AirCombatApp.Interface;
    }
}
