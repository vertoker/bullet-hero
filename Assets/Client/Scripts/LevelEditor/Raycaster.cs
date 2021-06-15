using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public enum WindowFocus
{
    None = 0, EditorLeft = 1, EditorRight = 2, EditorDown = 3,
    Timeline = 4, Toolbar = 5, GameStream = 6
}
public class Raycaster : MonoBehaviour
{
    //[SerializeField] private WindowFocus selectWindow = WindowFocus.None;
    //[Space()]
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;
    private WindowManager window;
    private Vector2 halfScreen;
    private bool click = true;

    public void Init(WindowManager window)
    {
        this.window = window;
        halfScreen = new Vector2(Screen.width, Screen.height) / 2f;
    }

    private void Update()
    {
        //selectWindow = FingerFocusScreen();
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
                switch (WindowManager.RenderType)
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
                }
            }
        }
        return WindowFocus.None;
    }

    public void BeginDrag() { click = false; }
    public void PointerUp() 
    {
        if (click)
            Raycasting();
        click = true;
    }

    private void Raycasting()
    {
        PointerEventData m_PointerEventData = new PointerEventData(eventSystem)
        { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(m_PointerEventData, results);

        if (results.Count > 0)
        {
            int id = Utils.String2Int(results[0].gameObject.name);
            string tag = results[0].gameObject.tag;
            switch (tag)
            {
                case "PrefabTimeline":
                    ObjectEditorWindow.PrefabSelect(id);
                    window.LeftEditorOpen("object_editor");
                    break;
                case "CheckpointTimeline":
                    CheckpointEditorWindow.CheckpointSelect(id);
                    window.LeftEditorOpen("checkpoint_editor");
                    break;
                case "MarkerTimeline":

                    break;
                case "Untagged":
                    window.LeftEditorClose();
                    window.RightEditorClose();
                    break;
            }
        }
    }
}
