using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using IAddTypeByAttribute = ExtendAttribute.IAddTypeByAttribute;



public class StCustomAttributes : IInit  ,ICanGetUtility  
{

    public void Init()
    {
        ExtendAttribute.InitData<BindPrefabAttribute>(this.GetUtility<IBindPrefabUtil>().Init);
        ExtendAttribute.InitData<BulletAttribute>(this.GetUtility<IBulletModelUtil>().Init);
    }


    #region 实现
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}


#region BulletModelUtil
public interface IBulletModelUtil :IUtility, IAddTypeByAttribute
{
    IBulletModel GetBulletModel(BulletType type);
}

public   class BulletModelUtil : IBulletModelUtil
{
    private static Dictionary<BulletType, IBulletModel> _bulletDic = new Dictionary<BulletType, IBulletModel>();

    #region IBulletUtil
    /// <summary>
    ///  bulletType,类前标签
    ///  type ,实现类BossBullet:IBullet
    ///  这里字典Dic<A,B>,A是attribute中的一个字段类型
    /// </summary>
    public  void Init(Attribute atb, Type type)
    {
        BulletAttribute after = atb as BulletAttribute;
        if (!_bulletDic.ContainsKey(after.E_BulletType))
        {
            _bulletDic.Add(after.E_BulletType, ExtendAttribute.GetInstance<IBulletModel>(type));
        }
        else
        {
            Debug.LogError("当前数据绑定类型存在重复，重复的类名称为：" + _bulletDic[after.E_BulletType] + "和" + type);
        }
    }

    public  IBulletModel GetBulletModel(BulletType type)
    {
        if (_bulletDic.ContainsKey(type))
        {
            return _bulletDic[type];
        }
        else
        {
            Debug.LogError("BulletUtil当前未绑定对应类型的数据，类型为：" + type);
            return null;
        }
    }
    #endregion

}
#endregion


#region BindPrefabUtil
public interface IBindPrefabUtil : IUtility, IAddTypeByAttribute
{
    List<Type> GetType(string path);
}
public  class BindPrefabUtil : IBindPrefabUtil 
{
    private static readonly Dictionary<string, List<Type>> _pathDic = new Dictionary<string, List<Type>>();
    private static readonly Dictionary<Type, int> _priorityDic = new Dictionary<Type, int>();
    #region 辅助
    /// <summary>初始化内部字典</summary>
    public void Init(Attribute atb, Type type)
    {
        BindPrefabAttribute after= atb as BindPrefabAttribute;
        string path = after.Path;
        if (!_pathDic.ContainsKey(path))
        {
            _pathDic.Add(path, new List<Type>());
        }

        if (!_pathDic[path].Contains(type))
        {
            _pathDic[path].Add(type);
            _priorityDic.Add(type, after.Priority);
            _pathDic[path].Sort(new BindPriorityComparer());
        }
       
    }


    /// <summary>根据路径返回类型</summary>
    public  List<Type> GetType(string path)
    {
        if (_pathDic.ContainsKey(path))
        {
            return _pathDic[path];
        }

        Debug.LogError("当前数据中未包含路径：" + path);
        return null;
    }
    #endregion



    #region 内部类
    /// <summary>预制体优先级比较器</summary>
    class BindPriorityComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            if (x == null)
            {
                return 1;
            }

            if (y == null)
            {
                return -1;
            }

            return _priorityDic[x] - _priorityDic[y];
        }
    }

    #endregion
}
#endregion
