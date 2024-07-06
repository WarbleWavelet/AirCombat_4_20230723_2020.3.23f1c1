using QFramework;
using QFramework.AirCombat;
using System.Collections;
using UnityEngine;

public class LevelRootController : ControllerBase  ,ICanGetUtility  ,ICanGetSystem
{
    protected override void InitChild()
    {
        IReader reader = this.GetUtility<IReaderUtil>().GetReader(ResourcesPath.CONFIG_LEVEL_CONFIG);
        string key = LevelRoot.Name.levelCount.Enum2String();
        reader[key].Get<int>(levelCount =>
        { 
            this.GetSystem<ICoroutineSystem>().StartOutter(WaitAfterInstantiateLevelItem(levelCount));
        });
    }

    private IEnumerator WaitAfterInstantiateLevelItem(int levelCount)
    {
        yield return new WaitUntil(() => transform.childCount >= levelCount);
        AddComponent();
        Reacquire();
    }

    private void AddComponent()
    {
                                                                 
        foreach (Transform trans in transform)
        { 
            trans.AddComponent<LevelItemController>();
        }
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}