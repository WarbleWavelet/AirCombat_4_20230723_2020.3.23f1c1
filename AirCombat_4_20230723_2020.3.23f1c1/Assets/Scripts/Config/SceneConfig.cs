using QFramework;
using QFramework.AirCombat;
using UnityEngine;

public class SceneConfig : NormalSingleton<SceneConfig>, IInit ,QFramework.IController  
{


    public void Init()
    {
        OnLoaded();
    }



    private void OnLoaded()
    {     


        this.GetSystem<ISceneSystem>().AddSceneLoaded(ESceneName.Game, callBack =>
        {
            this.GetSystem<IGameObjectPoolSystem>().Init(callBack);
        });

        this.GetSystem<ISceneSystem>().AddSceneLoaded(ESceneName.Game, callBack =>
        {
           // var go = this.GetUtility<ILoadUtil>().LoadAndInstantiateGameObject(ResourcesPath.PREFAB_GAMEROOT);

            //ResLoader loader = ResLoader.Allocate();
            //GameObject go = loader.LoadSync<GameObject>(GameObjectName.GameRoot);      
            //loader.Dispose();
            //GameObject go = GameObject.Find(GameObjectName.GameRoot);
            //go.GetOrAddComponent<GameRoot>().InitComponentEnemy();
            //go.name = GameObjectName.GameRoot;
       
            callBack();
        });
    }



    #region ÖØÐ´
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    #endregion  
}

public interface ISceneConfigSystem : QFramework.ISystem
{ }
public class SceneConfigSystem : AbstractSystem, ISceneConfigSystem
{


    protected override void OnInit()
    {
        OnLoaded();
    }

    private void OnLoaded()
    {
        ISceneSystem sys = this.GetSystem<ISceneSystem>();
        //SceneSystem sys = this.GetSystem<SceneSystem>();
        sys.AddSceneLoaded(ESceneName.Game, callBack =>
        {
            this.GetSystem<IGameObjectPoolSystem>().Init(callBack);
        });

        sys.AddSceneLoaded(ESceneName.Game, callBack =>
        {
            //ResLoader resLoader = ResLoader.Allocate();
            //var go =resLoader.LoadSync<GameObject>(ResourcesName.Prefab.GameRoot);
            //resLoader.Dispose();       
            //go.name = GameObjectName.GameRoot;
            //go.GetOrAddComponent<GameRoot>().InitComponentEnemy();

            //resLoader.Recycle2Cache();
            callBack();
        });
    }
}