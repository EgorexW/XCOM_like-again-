using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ObjectsFactory : UIElement
{
    [SerializeField] GameObject prefab;

    [SerializeField] bool sendMessageInsteadOfDestroy;

    [FoldoutGroup("Events")] public UnityEvent<GameObject> onCreateObject = new();
    [FoldoutGroup("Events")] public UnityEvent<GameObject> onRemoveObject = new();

    readonly List<GameObject> objects = new();

    void Awake()
    {
        SetPrefab();
        if (prefab.transform.parent == transform){
            prefab.SetActive(false);
        }
    }

    void OnValidate()
    {
        SetPrefab();
    }

    void SetPrefab()
    {
        if (prefab != null){
            return;
        }
        if (transform.childCount <= 0){
            return;
        }
        prefab = transform.GetChild(0).gameObject;
    }

    public void SetCount(int count)
    {
        while (objects.Count > count) RemoveObject();
        while (objects.Count < count) AddObject();
    }

    public GameObject AddObject()
    {
        var obj = Instantiate(prefab, transform);
        obj.SetActive(true);
        objects.Add(obj);
        onCreateObject.Invoke(obj);
        obj.SendMessage("OnFactoryAdd", SendMessageOptions.DontRequireReceiver);
        return obj;
    }

    public void RemoveObject(GameObject obj = null)
    {
        if (obj == null){
            if (objects.Count == 0){
                return;
            }
            obj = objects[^1];
        }
        else if (!objects.Contains(obj)){
            return;
        }
        onRemoveObject.Invoke(obj);
        if (sendMessageInsteadOfDestroy){
            obj.SendMessage("OnFactoryRemove", SendMessageOptions.RequireReceiver);
        }
        else{
            Destroy(obj);
        }
        objects.Remove(obj);
    }

    public void Clear()
    {
        SetCount(0);
    }

    public List<GameObject> GetObjects()
    {
        return new List<GameObject>(objects);
    }

    public GameObject GetObject(int index)
    {
        return objects[index];
    }
}