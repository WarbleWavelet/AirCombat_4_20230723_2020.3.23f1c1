/****************************************************
    文件：GameObjectSystem.cs
	作者：lenovo
    邮箱: 
    日期：2024/2/23 15:59:52
	功能：
*****************************************************/

using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameObjectSystem;
using static LevelRoot;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Random = UnityEngine.Random;


public enum LoadType
{
    AB,
    RESOURCES,
    RESLOADER

}
public interface ISpriteSystem : QFramework.ISystem
{
    Sprite Load(string abNameOrResourcesPath, LoadType loadType);
    Sprite GetCurIDPlaneSprite( );
}

public class SpriteSystem : QFramework.AbstractSystem, ISpriteSystem
{

    static Dictionary<string, Sprite> _enemyDic;
    Dictionary<int, List<Sprite>> _playerDic ;

    ResourceLoader _loader;
   


    protected override void OnInit()
    {
        _playerDic = new Dictionary<int, List<Sprite>>(); //0_0
        _enemyDic = new Dictionary<string, Sprite>(); 
        _loader = new ResourceLoader();
        LoadSprite(ResourcesPath.PICTURE_PLAYER_PICTURE_FOLDER,out _playerDic);
        LoadEnemySprite(ResourcesPath.PICTURE_ENEMY_FOLDER,out _enemyDic);
    }



    public Sprite Load(string path, LoadType loadType)
    {
        return _loader.Load<Sprite>(path);
    }

    public Sprite GetCurIDPlaneSprite()
    {
        IAirCombatAppStateModel mode = this.GetModel<IAirCombatAppStateModel>();
        string spriteName=  ($"{mode.SelectedPlaneSpriteLevel}"); //0123
        string pre = ResourcesPath.PICTURE_PLAYER_BULLET_FOLDER;
        string path = pre + spriteName;
        return _loader.Load<Sprite>(path);
    }


    #region Enemy
    string GetEnemyKey(int type, int id)
    {
        return $"{type}_{id}";
    }

    string GetEnemyKey(EnemyType type, int id)
    {
        int t = type.Enum2Int();
        return GetEnemyKey(type,id);
    }

    /// <summary></summary>
    private void LoadEnemySprite(string path, out Dictionary<string, Sprite> dic)
    {
        var sprites = _loader.LoadAll<Sprite>(path);
        dic = new Dictionary<string, Sprite>();
        foreach (var sprite in sprites)
        {
            //Enemy_Boss_0
            var arr = sprite.name.Split('_');
            EnemyType type ;
            if (!Enum.TryParse(arr[1], out type))
            {
                return;
            }
            var id = int.Parse(arr[2]);
            string key =GetEnemyKey(type,id);
            if (!dic.ContainsKey(key))
            {
                dic[key] = sprite;
            }
        }
    }
                       
    private Sprite GetEnemySprite(EnemyType type, int id)
    {
        Sprite sprite = null;
        string key =  GetEnemyKey(type, id);
        if (_enemyDic.TryGetValue(key, out sprite))
        {
            sprite = _enemyDic[key];   
            return sprite;
        }
        else
        {
            throw new System.Exception("异常");
        }
    }
    #endregion



    #region player



    private void LoadSprite(string path,out Dictionary<int, List<Sprite>> dic )
    {
        var sprites = _loader.LoadAll<Sprite>(path);
        dic=new Dictionary<int, List<Sprite>>();
        foreach (var sprite in sprites)
        {
            var idData = sprite.name.Split('_');
            var playerId = int.Parse(idData[0]);
            if (!dic.ContainsKey(playerId))
            { 
                 dic[playerId] = new List<Sprite>();
            } 

            dic[playerId].Add(sprite);
        }
    }


    private Sprite GetPlayerSprite(int id, int level)
    {
        int count = _playerDic[id].Count;
        if (!_playerDic.ContainsKey(id) || level >= count)
        {
            Debug.LogError("当前id或等级错误,等级" + level);
            level = count - 1;
        }

        return _playerDic[id][level];
    }

    #endregion
}


#region IGameObjectSystem
public interface IGameObjectSystem : QFramework.ISystem
{
    GameObject Instantiate(string abNameOrResourcesPath, LoadType loadType);
    GameObject Load(string abNameOrResourcesPath, LoadType loadType);
}

public class GameObjectSystem : QFramework.AbstractSystem, IGameObjectSystem
{

    static Dictionary<string,GameObject> _dic;
    ResLoader _loader;
    protected override void OnInit()
    {
        _dic = new Dictionary<string, GameObject>();
        _loader = ResLoader.Allocate();
    }
    



    public GameObject Instantiate(string abNameOrResourcesPath, LoadType loadType)
    {
        
        switch ( loadType )
        {
            case LoadType.RESOURCES: return InstantiateByResources(abNameOrResourcesPath); ;
            case LoadType.AB: return InstantiateByAB(abNameOrResourcesPath); ;
            default: throw new System.Exception("异常");
        }
    }

    /// <summary这种不是></summary>
    public GameObject Load(string abNameOrResourcesPath, LoadType loadType)
    {

        switch (loadType)
        {
            case LoadType.RESOURCES: return LoadByResources(abNameOrResourcesPath); ;
            default: throw new System.Exception("异常");
        }
    }
    #region pri

    GameObject LoadByResources(string path)
    {
        GameObject prefab;
        if (_dic.TryGetValue(path, out prefab))
        {

        }
        else
        {
            prefab = Resources.Load<GameObject>(path);
            _dic.Add(path, prefab);
        }
        //
        return prefab;
    }

    GameObject InstantiateByResources(string path)
    {
        GameObject prefab;
        if (_dic.TryGetValue(path, out prefab))
        {

        }
        else
        {
            prefab = Resources.Load<GameObject>(path);
            _dic.Add(path, prefab);
        }         
        if (prefab.IsNullObject())
        {

            throw new System.Exception("加载预制体异常："+ path);
        }
        GameObject go = GameObject.Instantiate(prefab);
        //
        return go;
    }



    GameObject InstantiateByAB(string abName)
    {
        GameObject prefab;
        if (_dic.TryGetValue(abName, out prefab))
        {

        }
        else
        {
            prefab = _loader.LoadSync<GameObject>(abName);
            _dic.Add(abName, prefab);
        }
        GameObject go = GameObject.Instantiate(prefab);
        return go;
    }
    #endregion  

}
#endregion  





