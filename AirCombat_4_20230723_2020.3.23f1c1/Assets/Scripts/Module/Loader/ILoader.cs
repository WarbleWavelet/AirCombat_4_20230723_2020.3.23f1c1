using System;
using UnityEngine;
using Object = UnityEngine.Object;

public interface ILoader
{
    void LoadConfig(string path, Action<object> complete);
    T Load<T>(string path) where T : Object;
    T[] LoadAll<T>(string path) where T : Object;
}