using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameEditor : MonoBehaviour
{
    private TimelineWindow timelineWindow;
    private WindowManager windowManager;
    private Raycaster raycaster;

    private void Awake()
    {
        timelineWindow = GetComponent<TimelineWindow>();
        windowManager = GetComponent<WindowManager>();
        raycaster = GetComponent<Raycaster>(); 
    }

    private void Start()
    {
        LevelManager.Load("0 demo level");
        windowManager.Init(LevelManager.Load());
        float length = LevelManager.level.LevelData.EndFadeOut;

        raycaster.Init(windowManager);
        timelineWindow.Init(length);
        CoroutineManager.Init(this);
        EditorTimer.Init(length);
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
        GUILayout.TextField("Second current: " + EditorTimer.SecCurrent.ToString());
        GUILayout.TextField("Frame current: " + Utils.Sec2Frame(EditorTimer.SecCurrent).ToString());
    }
}
#endif