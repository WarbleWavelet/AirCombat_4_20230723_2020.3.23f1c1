using QFramework;
using QFramework.AirCombat;
using System.Collections.Generic;
using UnityEngine;

public class NSPlaneSpritesModel : NormalSingleton<NSPlaneSpritesModel>   ,ICanGetUtility
{


    #region 字属

    /// <summary>
    /// 类似于
    /// (飞机种类,飞机等级)
    /// <br/>(0,(0_0,0_1,0_2,0_3))
    /// <br/>(1,(1_0,1_1,1_2,1_3))
    /// <br/>(2,(2_0,2_1,2_2,2_3))
    /// <br/>(3,(3_0,3_1,3_2,3_3))
    /// </summary>
    private Dictionary<int, List<Sprite>> _planeSpriteDic;
    /// <summary>不同ID，等级的飞机</summary>
    public Sprite this[int id, int level] => GetPlaneSprite(id, level);


    /// <summary>不同的飞机数，planeId的数量</summary>
    public int Count
    {
        get
        {
            if (_planeSpriteDic == null)
                return 0;
            return _planeSpriteDic.Count;
        }
    }

    #endregion


    public void Init( )
    {
        LoadSprite();
    }

    private void LoadSprite()
    {
        _planeSpriteDic = new Dictionary<int, List<Sprite>>();
        var sprites = this.GetUtility<ILoadUtil>().LoadAll<Sprite>(ResourcesPath.PICTURE_PLAYER_PICTURE_FOLDER);

        foreach (var sprite in sprites)
        {
            var idData = sprite.name.Split('_');
            var playerId = int.Parse(idData[0]);
            if (!_planeSpriteDic.ContainsKey(playerId)) 
                _planeSpriteDic[playerId] = new List<Sprite>();

            _planeSpriteDic[playerId].Add(sprite);
        }
    }

    private Sprite GetPlaneSprite(int id, int level)
    {
        int count = _planeSpriteDic[id].Count;
        if (!_planeSpriteDic.ContainsKey(id) || level >= count)
        {
            Debug.LogError("当前id或等级错误,等级"+level);
            level = count - 1;
        }

        return _planeSpriteDic[id][level];
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}