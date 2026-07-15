#if UNITY_EDITOR
using UnityEditor;

public class AssetRegistryPostprocessor : AssetPostprocessor{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths){
        if (importedAssets.Length == 0 && deletedAssets.Length == 0) return;

        foreach (var guid in AssetDatabase.FindAssets("t:AssetRegistryBase")){
            var registry = AssetDatabase.LoadAssetAtPath<AssetRegistryBase>(AssetDatabase.GUIDToAssetPath(guid));
            registry.RefreshFromProject();
        }
    }
}
#endif
