using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public static class ForceReserialize
{
    [MenuItem("ESF/Reserialize/Force Reserialize All Assets")]
    public static void ReserializeAll()
    {
        AssetDatabase.ForceReserializeAssets();
    }

    [MenuItem("ESF/Reserialize/Force Reserialize Selected Folders")]
    public static void ReserializeSelectedFolders()
    {
        var paths = new List<string>();

        foreach (var obj in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path)) continue;

            if (AssetDatabase.IsValidFolder(path))
                paths.AddRange(AssetDatabase.FindAssets("", new[] { path })
                    .Select(AssetDatabase.GUIDToAssetPath));
            else
                paths.Add(path);
        }

        paths = paths.Distinct().ToList();

        if (paths.Count == 0)
        {
            UnityEngine.Debug.LogWarning("No folders/assets selected.");
            return;
        }

        AssetDatabase.ForceReserializeAssets(paths,
            ForceReserializeAssetsOptions.ReserializeAssetsAndMetadata);

        UnityEngine.Debug.Log($"Reserialized {paths.Count} assets.");
    }

    [MenuItem("ESF/Reserialize/Force Reserialize Selected Folders", true)]
    public static bool ValidateReserializeSelectedFolders()
    {
        return Selection.objects.Length > 0;
    }
}