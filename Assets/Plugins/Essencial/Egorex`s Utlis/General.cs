using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class General : MonoBehaviour{
    public const int Iterationlimit = 10;

    // static General instance;
    //
    // static General GetInstance(){
    //     if (instance == null){
    //         instance = new GameObject("General").AddComponent<General>();
    //         DontDestroyOnLoad(instance);
    //     }
    //     return instance;
    // }

    public static readonly Vector2[] MainDirections2D = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };

    public static readonly Vector3[] MainDirections3D = { Vector3.right, Vector3.left, Vector3.forward, Vector3.back };

    public static float GetAngleFromVector(Vector2 dir){
        dir = dir.normalized;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0){
            angle += 360;
        }
        return angle;
    }

    public static Vector2 GetMousePos(){
        return Mouse.current.position.ReadValue();
    }

    public static Vector2 GetMouseWorldPos(){
        return GetMouseWorldPos(GetMousePos());
    }

    public static Vector2 GetMouseWorldPos(Vector2 mousePos){
        Debug.Assert(Camera.main != null, "Camera.main == null");
        var pos = Camera.main.ScreenToWorldPoint(mousePos);
        return pos;
    }

    public static Vector2 RandomPointOnCircle(){
        var angle = Random.value * 2 * Mathf.PI;
        return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }

    static IEnumerator StartAfterSecondsCoroutine(MonoBehaviour monoBehaviour, IEnumerator coroutine, float seconds){
        yield return new WaitForSeconds(seconds);
        if (monoBehaviour == null || !monoBehaviour.gameObject.activeInHierarchy){
            yield break;
        }
        monoBehaviour.StartCoroutine(coroutine);
    }

    static IEnumerator CallAfterSecondsCoroutine(UnityAction action, float seconds){
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }

    public static string TimeToString(float nr){
        var minutes = Mathf.FloorToInt(nr / 60);
        var seconds = nr - minutes * 60;
        seconds = Mathf.Round(seconds);
        var text = minutes + ":" + seconds;
        if (seconds < 10){
            text = minutes + ":0" + seconds;
        }
        return text;
    }

    public static Vector2Int RoundVector(Vector2 pos){
        return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }

    public static TComponent GetComponentFromCollider<TComponent>(Collider2D collider){
        if (collider == null){
            return default;
        }
        return !collider.TryGetComponent(out TComponent component2) ? default : component2;
    }

    public static TComponent GetComponentFromCollider<TComponent>(Collider collider){
        if (collider == null){
            return default;
        }
        return !collider.TryGetComponent(out TComponent component2) ? default : component2;
    }

    static GameObject GetGameObjectFromCollider(Component collider){
        return collider == null ? null : collider.gameObject;
    }

    public static void WorldText(string text, Vector2 pos, float size, float time = 0.01f){
        WorldText(text, pos, size, time, Color.white);
    }

    public static void WorldText(string text, Vector2 pos, float size, float time, Color color){
        var gameObject = new GameObject("WorldText"){
            transform ={
                position = pos
            },
            // hideFlags = HideFlags.HideInHierarchy
        };
        var textMeshPro = gameObject.AddComponent<TextMeshPro>();
        textMeshPro.color = color;
        textMeshPro.text = text;
        textMeshPro.fontSize = size;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        Destroy(gameObject, time);
    }

    public static TComponent GetRootComponent<TComponent>(GameObject gameObject, bool mustBeFound = true){
        return GetRootComponent<TComponent>(gameObject.transform, mustBeFound);
    }

    public static TComponent GetRootComponent<TComponent>(Transform transform, bool mustBeFound = true){
        var objectRoot = GetObjectRoot(transform, false);
        if (objectRoot == null){
            var localComponent = transform.GetComponent<TComponent>();
            Debug.Assert(!mustBeFound || localComponent != null, typeof(TComponent) + " is null", transform);
            return localComponent;
        }
        var component = objectRoot.GetRootComponent<TComponent>();
        Debug.Assert(!mustBeFound || component != null, typeof(TComponent) + " is null", transform);
        return component;
    }

    public static ObjectRoot GetObjectRoot(Transform transform, bool mustBeFound = true){
        var root = transform.GetComponentInParent<ObjectRoot>();
        if (mustBeFound && root == null){
            Debug.LogError("Object Root not found", transform);
        }
        return root;
    }

    public static Vector2 GetClosestPos(Vector2 centerPos, List<Vector2> poses){
        var closestPos = Vector2.zero;
        var smallestDis = Mathf.Infinity;
        foreach (var pos in poses){
            var dis = Vector2.Distance(centerPos, pos);
            if (dis < smallestDis){
                smallestDis = dis;
                closestPos = pos;
            }
        }
        return closestPos;
    }

    public static Vector2 GetClosestPos(Vector3 transformPosition, List<GameObject> enemies){
        var poses = enemies.ConvertAll(input => (Vector2)input.transform.position);
        return GetClosestPos(transformPosition, poses);
    }

    public static bool IsMouseOverUI(){
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static float RandomRange(Vector2 vector){
        return Random.Range(vector.x, vector.y);
    }

    public static int RandomRange(Vector2Int vector){
        return Random.Range(vector.x, vector.y);
    }

    public static HashSet<T> GetUniqueRootComponents<T>(Collider[] colliders){
        var components = new HashSet<T>();
        foreach (var obj in colliders){
            var component = GetRootComponent<T>(obj.gameObject, false);
            if (component != null){
                components.Add(component);
            }
        }
        return components;
    }

    public static Quaternion RotateLeftOrRight(Quaternion rotation, Quaternion targetRotation, float delta){
        var flatDefaultRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        return Quaternion.RotateTowards(rotation, flatDefaultRotation, delta);
    }

    public static Collider[] OverlapBounds(Bounds bounds){
        var result = Physics.OverlapBox(bounds.center, bounds.extents);
        return result;
    }

    public static List<T> GetComponentsFromColliders<T>(Collider[] colliders){
        var components = new List<T>();
        foreach (var collider in colliders){
            var component = GetComponentFromCollider<T>(collider);
            if (component != null){
                components.Add(component);
            }
        }
        return components;
    }

    public static Vector2 RandomPointInsideCollider2D(Collider2D collider2D){
        Vector2 pos = collider2D.bounds.center;
        for (var i = 0; i < Iterationlimit; i++){
            var posTmp = new Vector2(
                Random.Range(collider2D.bounds.min.x, collider2D.bounds.max.x),
                Random.Range(collider2D.bounds.min.y, collider2D.bounds.max.y)
            );
            if (!collider2D.OverlapPoint(posTmp)){
                continue;
            }
            pos = posTmp;
            break;
        }
        return pos;
    }

    public static List<T> GetComponentsFromColliders<T>(Collider2D[] colliders){
        var components = new List<T>();
        foreach (var collider in colliders){
            var component = GetComponentFromCollider<T>(collider);
            if (component != null){
                components.Add(component);
            }
        }
        return components;
    }

    public static string EnumDescription<T>() where T : Enum {
        StringBuilder sb = new StringBuilder();

        // Detect if this Enum is using the [Flags] attribute
        bool isFlag = typeof(T).IsDefined(typeof(FlagsAttribute), false);

        // Using basic Rich Text to bold the header
        sb.AppendLine(isFlag ? "<b>List Index ➔ Name</b>" : "<b>Index ➔ Name</b>");

        foreach (T value in Enum.GetValues(typeof(T))) {
            int rawValue = Convert.ToInt32(value);

            if (isFlag) {
                if (rawValue == 0) {
                    sb.AppendLine($"[--] {value}");
                } 
                else if ((rawValue & (rawValue - 1)) == 0) {
                    int listIndex = (int)Math.Log(rawValue, 2);
                    sb.AppendLine($"[{listIndex}] {value}");
                }
            } else {
                // Standard Enum behavior
                sb.AppendLine($"[{rawValue}] {value}"); 
            }
        }

        return sb.ToString().TrimEnd();
    }
}