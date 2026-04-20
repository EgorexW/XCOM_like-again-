using System.Collections.Generic;
using UnityEngine;

public static class MyExtentions{
    public static void Shuffle<T>(this List<T> list){
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i){
            var r = UnityEngine.Random.Range(i, count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    public static T Random<T>(this List<T> list, bool pop = false){
        if (list.Count < 1){
            Debug.LogWarning("Trying to get random element from empty list");
            return default;
        }
        var i = UnityEngine.Random.Range(0, list.Count);
        var obj = list[i];
        if (pop){
            list.RemoveAt(i);
        }
        return obj;
    }

    public static T WeightedRandom<T>(this Dictionary<T, float> list){
        var totalWeight = 0f;

        foreach (var kvp in list) totalWeight += Mathf.Max(kvp.Value, 0f);

        var roll = UnityEngine.Random.Range(0f, totalWeight);

        foreach (var kvp in list){
            var weight = Mathf.Max(kvp.Value, 0f);
            if (roll <= weight){
                return kvp.Key;
            }
            roll -= weight;
        }

        Debug.LogWarning("Invalid Weights");
        return default;
    }

    public static T WeightedRandom<T>(this Dictionary<T, int> list){
        var totalWeight = 0;

        foreach (var kvp in list) totalWeight += Mathf.Max(kvp.Value, 0);

        var roll = UnityEngine.Random.Range(0, totalWeight);

        foreach (var kvp in list){
            var weight = Mathf.Max(kvp.Value, 0);

            if (roll < weight){
                return kvp.Key;
            }
            roll -= weight;
        }

        Debug.LogWarning("Invalid Weights");
        return default;
    }

    public static IReadOnlyList<T> ReadOnly<T>(this List<T> list){
        return new List<T>(list);
    }

    public static List<T> Copy<T>(this List<T> list){
        return new List<T>(list);
    }
}