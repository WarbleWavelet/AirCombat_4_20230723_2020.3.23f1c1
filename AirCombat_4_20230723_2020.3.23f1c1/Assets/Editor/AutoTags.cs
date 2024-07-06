using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[InitializeOnLoad]
public class AutoTags
{
    static AutoTags()
    {
        AddUnityTag();
        AddPrefabTag();
    }

    private static void AddUnityTag()
    {
        var type = typeof(Tags);
        var infos = type.GetFields();
        foreach (var info in infos)
        { 
            InternalEditorUtility.AddTag((string) info.GetRawConstantValue());
        }
    }

    private static void AddPrefabTag()
    {
        var prefab = Resources.Load<GameObject>(ResourcesPath.PREFAB_PLANE);
        prefab.tag = Tags.PLAYER;
        prefab = Resources.Load<GameObject>(ResourcesPath.PREFAB_BULLET);
        prefab.tag = Tags.BULLET;
        prefab = Resources.Load<GameObject>(ResourcesPath.EFFECT_SHIELD);
        prefab.tag = Tags.SHIELD;
        foreach (var o in Resources.LoadAll<GameObject>(ResourcesPath.ENEMY_FOLDER))
        { 
            o.tag = Tags.ENEMY;
        }
        
        prefab = Resources.Load<GameObject>(ResourcesPath.PREFAB_ITEM_ITEM);
        prefab.tag = Tags.ITEM;
        prefab = Resources.Load<GameObject>(ResourcesPath.PREFAB_ENEMY_MISSILE);
        prefab.tag = Tags.ENEMY;
    }
}