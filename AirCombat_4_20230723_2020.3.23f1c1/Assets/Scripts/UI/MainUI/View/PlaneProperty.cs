using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections.Generic;

public class PlaneProperty : ViewBase   ,ICanGetSystem
{
    public enum Property
    {
        attack = 0,
        fireRate,
        life,
        COUNT
    }

    private List<PropertyItem> _items;

    protected override void InitChild()
    {
        _items = new List<PropertyItem>((int) Property.COUNT);

        for (Property i = 0; i < Property.COUNT; i++)
        {
            var go = this.GetSystem<IGameObjectSystem>().Instantiate(ResourcesPath.PREFAB_PROPERTY_ITEM,LoadType.RESOURCES);
            go.SetParent(transform);
            var cpt = go.AddComponent<PropertyItem>();
            cpt.Init(i.ToString());
            _items.Add(cpt);
        }
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}