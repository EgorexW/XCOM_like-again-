using System.IO;
using UnityEngine;

public static class JsonHelper
{
    public static T LoadFromFile<T>(string fileName) where T : class
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"File not found: {filePath}");
            return null;
        }

        try
        {
            string jsonText = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonText);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load JSON file: {e.Message}");
            return null;
        }
    }

    public static T LoadFromResources<T>(string resourceName) where T : class{
        // Crucial: Leave off the ".json" extension when using Resources.Load
        TextAsset jsonAsset = Resources.Load<TextAsset>(resourceName);

        if (jsonAsset == null){
            Debug.LogError($"Failed to load JSON file: {resourceName}");
            return null;
        }
        
        try
        {
            return JsonUtility.FromJson<T>(jsonAsset.text);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load JSON file: {e.Message}");
            return null;
        }
    }
}