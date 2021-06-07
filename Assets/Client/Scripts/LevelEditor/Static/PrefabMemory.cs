using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class PrefabMemory
{
    private static ListEditorPrefab list = new ListEditorPrefab();
    private const string name = "prefab_memory";

    public static EditorPrefab GetPrefabMemory(int index) { return list.EditorPrefabs[index]; }
    public static List<EditorPrefab> GetPrefabMemory() { return list.EditorPrefabs; }

    public static void Save()
    {
        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(GetPath(), json);
    }
    public static void Load()
    {
        list = JsonUtility.FromJson<ListEditorPrefab>(File.ReadAllText(GetPath()));
    }
    public static string GetPath()
    {
        return Application.dataPath + "/" + name + ".json";
    }
}
