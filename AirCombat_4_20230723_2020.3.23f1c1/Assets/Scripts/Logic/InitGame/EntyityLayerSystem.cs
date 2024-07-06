using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameLayerSystem : QFramework.ISystem
{
    /// <summary>可以船枚举类型，值来设定PosZ</summary>
    public void SetLayerParent(IGameView view);
}
public class EntyityLayerSystem : QFramework.AbstractSystem, IGameLayerSystem

{


    #region 字属


    private Dictionary<Entity2DLayer, Transform> _gameLayerDic;
    #endregion

    protected override void OnInit()
    {
        if (this.GetSystem<ISceneSystem>().IsScene(ESceneName.Game)==false)
        {
            var scene = this.GetModel<IAirCombatAppStateModel>().CurScene.Value ;
            Debug.LogError($"GameLayerSystem只能在Game场景下运行，当前场景：{scene}");
            return ;
        }
        Transform gameRoot = this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(GameObjectName.GameRoot);
        Init(gameRoot);
      
    }


    #region pub IGameLayerSystem
    public void SetLayerParent(IGameView view)
    {
        Entity2DLayer layer = view.E_Entity2DLayer;

        if (_gameLayerDic.ContainsKey(layer))
        {
            view.Transform.SetParent(_gameLayerDic[layer]);
        }
        else
        { 
            Debug.LogError("当前层级并不存在，layer:" + layer);
        }
    }
    #endregion


    #region pri


    void Init(Transform gameRoot)
    {
        _gameLayerDic = new Dictionary<Entity2DLayer, Transform>();

        foreach (Entity2DLayer layer in Enum.GetValues(typeof(Entity2DLayer)))
        {
            int idx = layer.Enum2Int(); //  0 -1 -2
            string str = layer.Enum2String();
            Transform layerTrans=gameRoot.FindOrNew(str);
            layerTrans.Parent(gameRoot)
                .SetPos(Vector3.forward * idx)
                .SetAsLastSibling();
            _gameLayerDic.Add(layer, layerTrans);
        }
    }


    #endregion


    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


    #endregion

}