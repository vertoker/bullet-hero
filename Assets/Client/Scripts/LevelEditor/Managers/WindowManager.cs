using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public interface IWindow
{
    public RectTransform Open();
    public void Close();
}
public interface IInit
{
    public void Init();
}
public enum EditorRenderType
{
    None = 0, EditorLR = 1, EditorLRD = 2
}
public class WindowManager : MonoBehaviour
{
    public static Color butEnableBG = new Color(1f, 1f, 1f);
    public static Color butNotSelectBG = new Color(0.85f, 0.85f, 0.85f);
    public static Color butDisableBG = new Color(0.7f, 0.7f, 0.7f);
    public static Color butEnable = new Color(0.1f, 0.1f, 0.1f);
    public static Color butDisable = new Color(0f, 0f, 0f);

    private Dictionary<string, IWindow> left_windows, right_windows, game_windows;
    private IWindow left_active_window, right_active_window, game_active_window;

    [Header("EditorLeft")]
    [SerializeField] private ScrollRect editor_left_scroll_rect;
    [SerializeField] private RectTransform editor_left_rect_null;
    private RectTransform editor_left_rect;

    [SerializeField] private ObjectEditorWindow object_editor;
    [SerializeField] private MarkerEditorWindow marker_editor;
    [SerializeField] private CheckpointEditorWindow checkpoint_editor;
    [SerializeField] private CreateMarkerWindow create_marker;
    [SerializeField] private CreateCheckpointWindow create_checkpoint;
    [SerializeField] private Window create_prefab;
    [SerializeField] private ListWindow create_prefab_standard;
    [SerializeField] private ListWindow create_prefab_level;
    [SerializeField] private ListWindow create_prefab_memory;

    [Header("EditorRight")]
    [SerializeField] private ScrollRect editor_right_scroll_rect;
    [SerializeField] private RectTransform editor_right_rect_null;
    private RectTransform editor_right_rect;

    [SerializeField] private ListWindow marker_list;
    [SerializeField] private ListWindow checkpoint_list;
    [SerializeField] private EditPosWindow edit_pos;
    [SerializeField] private EditScaWindow edit_sca;
    [SerializeField] private EditRotWindow edit_rot;
    [SerializeField] private EditClrWindow edit_clr;
    [SerializeField] private ListWindow prefab_list;
    [Header("GameEditor")]
    [SerializeField] private GameObject game_window;
    [SerializeField] private LevelDataWindow level_data_window;
    [SerializeField] private NewLevelWindow new_level_window;
    [SerializeField] private OpenLevelWindow open_level_window;
    [SerializeField] private SaveWindow save_window;
    [Header("Other and Important")]
    [SerializeField] private ContentMarkersWindow content_markers_window;
    private EditorRenderType renderType = EditorRenderType.None;

    private static WindowManager Instance;
    private void Awake() { Instance = this; }
    public static EditorRenderType RenderType 
    {
        get { return Instance.renderType; }
        set
        {
            Instance.renderType = value;
            switch (value)
            {
                case EditorRenderType.None:
                    Debug.Log("Nothing");
                    break;
                case EditorRenderType.EditorLR:
                    Instance.editor_left_rect.sizeDelta = new Vector2(480, 640);
                    Instance.editor_right_rect.sizeDelta = new Vector2(480, 640);
                    Instance.content_markers_window.Close();
                    break;
                case EditorRenderType.EditorLRD:
                    Instance.editor_left_rect.sizeDelta = new Vector2(480, 390);
                    Instance.editor_right_rect.sizeDelta = new Vector2(480, 390);
                    Instance.content_markers_window.Open();
                    break;
            }
        }
    }

    public void Init(bool load_level)
    {
        // Windows Dictionarys
        left_windows = new Dictionary<string, IWindow>()
        {
            { "object_editor", object_editor },
            { "marker_editor", marker_editor },
            { "checkpoint_editor", checkpoint_editor },
            { "create_marker", create_marker },
            { "create_checkpoint", create_checkpoint },
            { "create_prefab", create_prefab },
            { "create_prefab_standard", create_prefab_standard },
            { "create_prefab_level", create_prefab_level },
            { "create_prefab_memory", create_prefab_memory }
        };
        right_windows = new Dictionary<string, IWindow>()
        {
            { "marker_list", marker_list },
            { "checkpoint_list", checkpoint_list },
            { "edit_pos", edit_pos },
            { "edit_sca", edit_sca },
            { "edit_rot", edit_rot },
            { "edit_clr", edit_clr },
            { "prefab_list", prefab_list }
        };
        game_windows = new Dictionary<string, IWindow>()
        {
            { "level_data_window", level_data_window },
            { "new_level_window", new_level_window },
            { "open_level_window", open_level_window },
            { "save_window", save_window }
        };

        editor_left_rect = editor_left_scroll_rect.GetComponent<RectTransform>();
        editor_right_rect = editor_right_scroll_rect.GetComponent<RectTransform>();

        // Windows Init
        object_editor.Init();
        marker_editor.Init();
        checkpoint_editor.Init();
        create_marker.Init();
        create_checkpoint.Init();
        ListWindowsInit();

        edit_pos.Init();
        edit_sca.Init();
        edit_rot.Init();
        edit_clr.Init();

        content_markers_window.Init();
    }

    private void ListWindowsInit()
    {
        string getParam0(IData d) { return d.GetParameter(0); }
        string getParam1(IData d) { return d.GetParameter(1); }
        string getParam2(IData d) { return d.GetParameter(2); }
        string getParamTimer2(IData d) { return Utils.Sec2Text(d.GetParameter(2)); }

        GetParameter[] getParamsObject = new GetParameter[] { getParam0, getParam1, getParam2 };
        GetParameter[] getParamsMarker = new GetParameter[] { getParam0, getParamTimer2 };
        GetParameter[] getParamsCheckpoint = new GetParameter[] { getParam1, getParamTimer2 };
        GetParameter[] getParamsPrefab = new GetParameter[] { getParam0, getParam1 };

        void actionEditorPrefabAll(List<Prefab> prefabs)
        {
            int length = LevelManager.level.Prefabs.Count;
            CreateObject.CreateEditorPrefab(prefabs);
            ObjectEditorWindow.PrefabSelect(length);
            LeftEditorOpen("object_editor");
        }

        void actionEditorPrefabStandard(int index) { actionEditorPrefabAll(PrefabStandard.GetPrefabStandard(index).Prefabs); }
        void actionEditorPrefabLevel(int index) { actionEditorPrefabAll(LevelManager.level.EditorPrefabs[index].Prefabs); }
        void actionEditorPrefabMemory(int index) { actionEditorPrefabAll(PrefabMemory.GetPrefabMemory(index).Prefabs); }

        void actionMarker(int index) { marker_editor.MarkerSelect(index); LeftEditorOpen("marker_editor"); }
        void actionCheckpoint(int index) { marker_editor.MarkerSelect(index); LeftEditorOpen("checkpoint_editor"); }
        void actionPrefab(int index) { ObjectEditorWindow.PrefabSelect(index); LeftEditorOpen("object_editor"); }

        create_prefab_standard.Init(LevelManager.EditorPrefabStandardList(), getParam0, getParamsObject, actionEditorPrefabStandard);
        create_prefab_level.Init(LevelManager.EditorPrefabLevelList(), getParam0, getParamsObject, actionEditorPrefabLevel);
        create_prefab_memory.Init(LevelManager.EditorPrefabMemoryList(), getParam0, getParamsObject, actionEditorPrefabMemory);
        marker_list.Init(LevelManager.MarkerList(), getParam0, getParamsMarker, actionMarker);
        checkpoint_list.Init(LevelManager.CheckpointList(), getParam1, getParamsCheckpoint, actionCheckpoint);
        prefab_list.Init(LevelManager.PrefabList(), getParam0, getParamsPrefab, actionPrefab);
    }

    public void GameOpen(string key)//Не работает, проверить
    {
        game_window.SetActive(true);
        if (game_active_window != null)
            game_active_window.Close();
        if (game_windows.TryGetValue(key, out IWindow value))
        {
            if (game_active_window != null)
            {
                GameClose();
                return;
            }
            game_active_window = value;
            value.Open();
        }
    }
    public void LeftEditorOpen(string key)
    {
        if (left_active_window != null)
            left_active_window.Close();
        if (left_windows.TryGetValue(key, out IWindow value))
        {
            left_active_window = value;
            editor_left_scroll_rect.content = value.Open();
            editor_left_scroll_rect.verticalScrollbar.value = 1f;
        }
    }
    public void RightEditorOpen(string key)
    {
        if (right_active_window != null)
            right_active_window.Close();
        if (left_windows.TryGetValue(key, out IWindow value))
        {
            right_active_window = value;
            editor_right_scroll_rect.content = value.Open();
            editor_right_scroll_rect.verticalScrollbar.value = 1f;
        }
    }
    public void GameClose()
    {
        if (game_active_window != null)
        {
            game_active_window.Close();
            game_active_window = null;
        }
    }
    public void LeftEditorClose()
    {
        if (left_active_window != null)
        {
            left_active_window.Close();
            left_active_window = null;
            editor_left_scroll_rect.content = editor_left_rect_null;
        }
    }
    public void RightEditorClose()
    {
        if (right_active_window != null)
        {
            right_active_window.Close();
            right_active_window = null;
            editor_right_scroll_rect.content = editor_right_rect_null;
        }
    }
}