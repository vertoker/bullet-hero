using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public enum WindowFocus
{
    None = 0, EditorLeft = 1, EditorRight = 2, EditorDown = 3,
    Timeline = 4, Toolbar = 5, GameStream = 6
}
public interface IWindow
{
    public void Init();
    public RectTransform Open();
    public void Close();
}

public class WindowManager : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] private WindowFocus selectWindow = WindowFocus.None;
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;
    [Header("Camera")]
    [SerializeField] private Camera cam;
    private Vector2 halfScreen;
    private float camAspect;

    public static Color butEnableBG() { return new Color(1f, 1f, 1f); }
    public static Color butNotSelectBG() { return new Color(0.85f, 0.85f, 0.85f); }
    public static Color butDisableBG() { return new Color(0.7f, 0.7f, 0.7f); }
    public static Color butEnable() { return new Color(0.1f, 0.1f, 0.1f); }
    public static Color butDisable() { return new Color(0f, 0f, 0f); }

    private Dictionary<string, IWindow> left_windows, right_windows;
    private IWindow left_active_Window, right_active_Window;

    [Header("EditorLeft")]
    [SerializeField] private ScrollRect editor_left__scroll_rect;
    [SerializeField] private ObjectEditorWindow object_editor;
    [SerializeField] private CreateMarkerWindow create_marker;
    [SerializeField] private CreateCheckpointWindow create_checkpoint;
    [SerializeField] private CreatePrefabWindow create_prefab;
    [SerializeField] private ListWindow create_prefab_standard;
    [SerializeField] private ListWindow create_prefab_level;
    [SerializeField] private ListWindow create_prefab_memory;
    [Header("EditorRight")]
    [SerializeField] private ScrollRect editor_right_scroll_rect;
    [SerializeField] private MarkerObjectWindow marker_object_window;
    [SerializeField] private ListWindow marker_list;
    [SerializeField] private ListWindow checkpoint_list;
    [SerializeField] private EditPosWindow edit_pos;
    [SerializeField] private EditScaWindow edit_sca;
    [SerializeField] private EditRotWindow edit_rot;
    [SerializeField] private EditClrWindow edit_clr;
    [SerializeField] private ListWindow object_list;

    private void Start()
    {
        halfScreen = new Vector2(Screen.width, Screen.height) / 2f;
        camAspect = cam.aspect;
        LevelManager.Load("0 demo level");

        left_windows = new Dictionary<string, IWindow>()
        {
            { "object_editor", object_editor },
            { "create_marker", create_marker },
            { "create_checkpoint", create_checkpoint },
            { "create_prefab", create_prefab },
            { "create_prefab_standard", create_prefab_standard },
            { "create_prefab_level", create_prefab_level },
            { "create_prefab_memory", create_prefab_memory }
        };
        right_windows = new Dictionary<string, IWindow>()
        {
            { "marker_object_window", marker_object_window },
            { "marker_list", marker_list },
            { "checkpoint_list", checkpoint_list },
            { "edit_pos", edit_pos },
            { "edit_sca", edit_sca },
            { "edit_rot", edit_rot },
            { "edit_clr", edit_clr },
            { "object_list", object_list }
        };
    }

    public void LeftEditorOpen(string key)
    {
        if (left_active_Window != null)
            left_active_Window.Close();
        left_active_Window = left_windows[key];
        left_active_Window.Open();
    }
    public void RightEditorOpen(string key)
    {
        if (right_active_Window != null)
            right_active_Window.Close();
        right_active_Window = right_windows[key];
        right_active_Window.Open();
    }
    public void LeftEditorClose(string key)
    {
        left_windows[key].Close();
    }
    public void RightEditorClose(string key)
    {
        right_windows[key].Close();
    }

    private void Update()
    {
        selectWindow = FingerFocusScreen();
        if (selectWindow == WindowFocus.Timeline && Input.GetKey(KeyCode.Mouse0))
            Raycaster();
    }

    private WindowFocus FingerFocusScreen()
    {
        Vector2 pos = ((Vector2)Input.mousePosition - halfScreen) / halfScreen;
        if (pos.y < -0.2f) { return WindowFocus.Timeline; } // -100
        else
        {
            if (pos.x < 0f)
            {
                if (pos.y <= 0) { return WindowFocus.Toolbar; }
                else { return WindowFocus.GameStream; }
            }
            else
            {
                return WindowFocus.None;
                /*switch (downRender)
                {
                    case EditorRenderType.None:
                        return WindowFocus.None;

                    case EditorRenderType.EditorLR:
                        if (pos.x < 0.5f) // 480
                        { return WindowFocus.EditorLeft; }
                        return WindowFocus.EditorRight;

                    case EditorRenderType.EditorLRD:
                        if (pos.y < 0.3125f) // 150 
                        { return WindowFocus.EditorDown; }
                        if (pos.x < 0.5f) // 480
                        { return WindowFocus.EditorLeft; }
                        return WindowFocus.EditorRight;
                }*/
            }
        }
        return WindowFocus.None;
    }

    private void Raycaster()
    {
        PointerEventData m_PointerEventData = new PointerEventData(eventSystem)
        { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(m_PointerEventData, results);

        if (results.Count > 0)
        {
            switch (results[0].gameObject.tag)
            {
                case "MarkerUI":
                    string name = results[0].gameObject.name;
                    string type = name.Substring(0, name.Length - (name.Length - 3));
                    int id = int.Parse(name.Substring(3, name.Length - 3));
                    break;
                case "ObjectUI":
                    break;
            }
        }
    }
}