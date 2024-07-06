using QFramework;
using QFramework.AirCombat;
using UnityEngine;

public class LevelRoot : ViewBase ,ICanGetUtility ,ICanGetSystem
{
    protected override void InitChild()
    {
        IReader reader = this.GetUtility<IReaderUtil>().GetReader(ResourcesPath.CONFIG_LEVEL_CONFIG);
        for (Name i = 0; i < Name.COUNT; i++)
        {
            var index = i;
            TaskQueueMgr<int>.Single.AddQueue(() => reader[index.ToString()]);
        }

        TaskQueueMgr<int>.Single.Execute(SpawnLevelItem);
    }


    #region 辅助
    private void SpawnLevelItem(int[] values)
    {
        if (values.Length != (int) Name.COUNT)
        {
            Debug.LogError("返回数据数量不匹配，正确数量:"
                + (int) Name.COUNT 
                + " 当前数量："
                + values.Length
            );
            return;
        }

        int index = values[(int) Name.levelCount];
                                                                  
        SpawnItem(index);
    }

    private void SpawnItem(int count)
    {
        GameObject go;
        LevelItem item;
        for (var i = 0; i < count; i++)
        {
             go = this.GetSystem<IGameObjectSystem>().Instantiate(ResourcesPath.PREFAB_LEVEL_ITEM, LoadType.RESOURCES);
            go.SetParent(transform);
            go.AddComponent<LevelItem>();
        }

        Reacquire();
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion


    public enum Name
    {
        levelCount = 0,
        COUNT
    }
}