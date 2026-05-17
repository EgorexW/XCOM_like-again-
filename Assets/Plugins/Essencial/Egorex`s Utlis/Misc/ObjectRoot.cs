using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectRoot<T> : MonoBehaviour, IObjectRoot<T> where T : class
{
    private readonly Dictionary<Type, object> _componentsCache = new();
    
    public new T1 GetComponent<T1>() where T1 : T
    {
        var componentsList = GetComponents<T1>();
        if (componentsList.Count == 0)
        {
            return default;
        }
        if (componentsList.Count > 1)
        {
            Debug.LogWarning($"Multiple components of type {typeof(T1)} found on {gameObject.name}. Returning the first one.");
        }
        return componentsList[0];
    }
    
    public new List<T1> GetComponents<T1>() where T1 : T
    {
        Type type = typeof(T1);
        if (_componentsCache.TryGetValue(type, out var cachedObject))
        {
            return (List<T1>)cachedObject;
        }
        
        T1[] fetchedComponents = GetComponentsInChildren<T1>(true);
        List<T1> newList = new List<T1>(fetchedComponents);
        
        _componentsCache[type] = newList;
        
        return newList;
    }
    
    public void ClearCache()
    {
        _componentsCache.Clear();
    }
}

public interface IObjectRoot<T> where T : class{
    public T1 GetComponent<T1>() where T1 : T;
    public List<T1> GetComponents<T1>() where T1 : T;
}