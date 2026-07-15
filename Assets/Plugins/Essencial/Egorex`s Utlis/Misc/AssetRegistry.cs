using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AssetRegistryBase : ScriptableObject{
#if UNITY_EDITOR
    public abstract void RefreshFromProject();
#endif
}

public abstract class AssetRegistry<T> : AssetRegistryBase where T : UnityEngine.Object{
    [SerializeField] protected List<AssetRegistryEntry<T>> entries = new();

    public string GetGuid(T asset){
        foreach (var entry in entries)
            if (entry.asset == asset) return entry.guid;
        Debug.LogWarning($"{typeof(T).Name} '{asset.name}' is not registered in {name}.");
        return null;
    }

    public T GetAsset(string guid){
        foreach (var entry in entries)
            if (entry.guid == guid) return entry.asset;
        Debug.LogWarning($"No {typeof(T).Name} registered in {name} for guid {guid}.");
        return null;
    }

#if UNITY_EDITOR
    protected virtual void OnValidate(){
        for (var i = 0; i < entries.Count; i++){
            var entry = entries[i];
            if (entry.asset == null) continue;
            entry.guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(entry.asset));
            entries[i] = entry;
        }
    }

    // Unity's AssetDatabase search syntax, eg. "t:Equipment" or "t:Prefab".
    protected abstract string AssetSearchFilter{ get; }

    // Override to narrow down which assets found by AssetSearchFilter actually belong in this registry,
    // eg. only prefabs that carry a specific component.
    protected virtual bool IsValidAsset(T asset) => true;

    [Button("Refresh From Project")]
    public override void RefreshFromProject(){
        entries.RemoveAll(entry => entry.asset == null);
        foreach (var guid in AssetDatabase.FindAssets(AssetSearchFilter)){
            var asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
            if (asset == null || !IsValidAsset(asset)) continue;
            if (entries.Exists(entry => entry.asset == asset)) continue;
            entries.Add(new AssetRegistryEntry<T>{ asset = asset });
        }
        OnValidate();
        EditorUtility.SetDirty(this);
    }
#endif
}

[Serializable]
public struct AssetRegistryEntry<T> where T : UnityEngine.Object{
    public T asset;
    [ReadOnly] public string guid;
}
