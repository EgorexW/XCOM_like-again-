using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ObjectsPool : CountUI{
    [SerializeField] protected GameObject prefab;

    [FoldoutGroup("Events")] public UnityEvent<GameObject> onCreateObject = new();

    readonly List<GameObject> activeObjs = new();
    readonly Queue<GameObject> inactiveObjs = new();

    protected virtual void Awake(){
        SetPrefab();
        if (prefab.transform.parent == transform){
            prefab.SetActive(false);
        }
    }

    protected void OnValidate(){
        SetPrefab();
    }

    void SetPrefab(){
        if (prefab != null){
            return;
        }
        if (transform.childCount <= 0){
            return;
        }
        prefab = transform.GetChild(0).gameObject;
    }

    public override void SetCount(int count){
        base.SetCount(count);
        while (activeObjs.Count > count) RemoveObject();
        while (activeObjs.Count < count) AddObject();
    }

    public GameObject AddObject(){
        if (inactiveObjs.Count < 1){
            CreateObjectUI();
        }
        var obj = inactiveObjs.Dequeue();
        obj.SetActive(true);
        activeObjs.Add(obj);
        if (obj.TryGetComponent(out IPoolable poolable)){
            poolable.OnPoolActivate();
        }
        return obj;
    }

    public void RemoveObject(GameObject obj = null){
        if (obj == null){
            obj = activeObjs[^1];
        }
        if (obj == null || !activeObjs.Contains(obj)){
            return;
        }
        if (obj.TryGetComponent(out IPoolable poolable)){
            poolable.OnPoolDeactivate();
        }
        obj.SetActive(false);
        activeObjs.Remove(obj);
        inactiveObjs.Enqueue(obj);
    }

    public IReadOnlyList<GameObject> GetActiveObjs(){
        return activeObjs.AsReadOnly();
    }

    public GameObject GetActiveObject(int index){
        if (index < 0 || index >= activeObjs.Count){
            Debug.LogWarning("Index out of range: " + index);
            return null;
        }
        return activeObjs[index];
    }

    protected virtual GameObject CreateObjectUI(){
        var newObj = Instantiate(prefab, transform);
        inactiveObjs.Enqueue(newObj);
        onCreateObject.Invoke(newObj);
        newObj.SendMessage("OnObjectPoolCreate", SendMessageOptions.DontRequireReceiver);
        return newObj;
    }

    public void Clear(){
        SetCount(0);
    }
}

public interface IPoolable{
    void OnPoolActivate();
    void OnPoolDeactivate();
}