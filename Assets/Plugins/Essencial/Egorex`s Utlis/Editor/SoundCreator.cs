using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

// AI Generated Code
// This script creates a Sound asset from selected AudioClip(s) in the Unity Editor.

public class SoundCreator{
    [MenuItem("Assets/Create/Egorex/Sound from AudioClip", true)]
    static bool ValidateCreateSound(){
        return Selection.objects.All(obj => obj is AudioClip) && Selection.objects.Length > 0;
    }

    [MenuItem("Assets/Create/Egorex/Sound from AudioClip")]
    static void CreateSound(){
        var clips = Selection.objects.OfType<AudioClip>().ToArray();
        if (clips.Length == 0){
            Debug.LogError("Selected object(s) are not AudioClip(s).");
            return;
        }

        // Create the Sound asset
        var soundAsset = ScriptableObject.CreateInstance<Sound>();
        soundAsset.clips = new List<AudioClip>(clips);

        // Generate path for the new asset
        string newPath;

        var path = AssetDatabase.GetAssetPath(clips[0]);
        newPath = Path.Combine(Path.GetDirectoryName(path), clips[0].name + ".asset");

        AssetDatabase.CreateAsset(soundAsset, newPath);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = soundAsset;
    }
}