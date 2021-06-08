using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameEditor : MonoBehaviour
{
    private WindowManager windowManager;
    private EditorTimer editorTimer;

    private void Awake()
    {
        windowManager = GetComponent<WindowManager>();
        editorTimer = GetComponent<EditorTimer>();
    }

    private void Start()
    {
        LevelManager.Load("0 demo level");
        windowManager.Init(LevelManager.Load());
        editorTimer.Init(LevelManager.level.LevelData.EndFadeOut);
    }

    public void Quit()
    {
        SaveWindow.Instance.Open();
        SceneStatic.LoadMenu();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameEditor))]
public class CustomButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameEditor editor = (GameEditor)target;

        if (GUILayout.Button("Save Test Level"))
        {
            LevelManager.CreateTestLevel();
        }
    }
}
#endif