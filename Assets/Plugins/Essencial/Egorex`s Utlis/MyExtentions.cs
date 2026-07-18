using System.Collections.Generic;
using UnityEngine;

public static class MyExtentions{
    /// <summary>
    /// Shuffles the elements of the list in place using the Fisher-Yates algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle.</param>
    public static void Shuffle<T>(this List<T> list){
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i){
            var r = UnityEngine.Random.Range(i, count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    /// <summary>
    /// Returns a random element from the read-only list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The read-only list to select from.</param>
    /// <returns>A random element of type <typeparamref name="T"/>, or default if the list is empty.</returns>
    public static T Random<T>(this IReadOnlyList<T> list){
        if (list.Count < 1){
            Debug.LogWarning("Trying to get random element from empty list");
            return default;
        }
        var i = UnityEngine.Random.Range(0, list.Count);
        var obj = list[i];
        return obj;
    }

    /// <summary>
    /// Selects a random key from the dictionary based on its float weight value.
    /// </summary>
    /// <typeparam name="T">The type of keys in the dictionary.</typeparam>
    /// <param name="list">The dictionary containing elements and their corresponding float weights.</param>
    /// <returns>The selected key of type <typeparamref name="T"/>, or default if selection fails.</returns>
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

    /// <summary>
    /// Selects a random key from the dictionary based on its integer weight value.
    /// </summary>
    /// <typeparam name="T">The type of keys in the dictionary.</typeparam>
    /// <param name="list">The dictionary containing elements and their corresponding integer weights.</param>
    /// <returns>The selected key of type <typeparamref name="T"/>, or default if selection fails.</returns>
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

    /// <summary>
    /// Creates and returns a new list containing the same elements as the read-only list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The read-only list to copy.</param>
    /// <returns>A new list of type <see cref="List{T}"/> containing the elements of the source list.</returns>
    public static List<T> Copy<T>(this IReadOnlyList<T> list){
        return new List<T>(list);
    }

    /// <summary>
    /// Returns a random float between the x (inclusive) and y (inclusive) components of the Vector2.
    /// </summary>
    /// <param name="vector">The Vector2 containing the min (x) and max (y) values.</param>
    /// <returns>A random float value.</returns>
    public static float Random(this Vector2 vector){
        return UnityEngine.Random.Range(vector.x, vector.y);
    }

    /// <summary>
    /// Returns a random integer between the x (inclusive) and y (inclusive) components of the Vector2Int.
    /// </summary>
    /// <param name="vector">The Vector2Int containing the min (x) and max (y) values.</param>
    /// <returns>A random integer value.</returns>
    public static int Random(this Vector2Int vector){
        return UnityEngine.Random.Range(vector.x, vector.y + 1);
    }
}