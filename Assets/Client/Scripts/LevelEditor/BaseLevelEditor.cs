using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseLevelEditor : MonoBehaviour
{
    public Camera cam;
    public GraphicRaycaster m_Raycaster;
    public EventSystem m_EventSystem;
    RectTransform tr;
    [HideInInspector] public PlayWindow play;
    [HideInInspector] public TimelineWindow timeline;
    [HideInInspector] public EditorBlockWindow editorBlock;
    public AudioClip clip;
    //Level params
    [HideInInspector] public LevelData data;
    [HideInInspector] public List<Marker> markers;
    [HideInInspector] public List<Checkpoint> checkpoints;
    [HideInInspector] public List<Prefab> prefabs;
    [HideInInspector] public List<EditorPrefab> editorPrefabs;
    [HideInInspector] public List<GameEvent> gameEvents;
    [HideInInspector] public Effects effects;

    public static Color butEnableBG() { return new Color(1f, 1f, 1f); }
    public static Color butNotSelectBG() { return new Color(0.85f, 0.85f, 0.85f); }
    public static Color butDisableBG() { return new Color(0.7f, 0.7f, 0.7f); }
    public static Color butEnable() { return new Color(0.1f, 0.1f, 0.1f); }
    public static Color butDisable() { return new Color(0f, 0f, 0f); }
    public enum ActiveWindow 
    {
        None = 0, EditorLeft = 1, EditorRight = 2, EditorDown = 3,
        Timeline = 4, Toolbar = 5, GameStream = 6
    }
    public enum EditorRenderType
    {
        None = 0, EditorLR = 1, EditorLRD = 2
    }

    public ActiveWindow activeWindow = ActiveWindow.None;
    [HideInInspector] public EditorRenderType downRender;
    [HideInInspector] public bool isTouchSecLine = false;
    [HideInInspector] public bool active = false;
    [HideInInspector] public bool isPlay = false;
    [HideInInspector] public float secCurrent = 0;
    private Vector2 halfScreen;
    private float camAspect;

    [HideInInspector] public float timeScale = 1f;

    public void Awake()
    {
        halfScreen = new Vector2(Screen.width, Screen.height) / 2f;
        tr = GetComponent<RectTransform>();
        play = tr.GetChild(0).GetComponent<PlayWindow>();
        timeline = tr.GetChild(1).GetComponent<TimelineWindow>();
        editorBlock = tr.GetChild(2).GetComponent<EditorBlockWindow>();
        camAspect = GetAspect();
        LoadLevel();
    }

    public void Update()
    {
        activeWindow = FingerFocusScreen();
        if (Input.GetKey(KeyCode.Mouse0))
        {
            PointerEventData m_PointerEventData = new PointerEventData(m_EventSystem)
            { position = Input.mousePosition };
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            if (results.Count > 0)
            {
                if (results[0].gameObject.tag == "MarkerUI")
                {
                    string name = results[0].gameObject.name;
                    string type = name.Substring(0, name.Length - (name.Length - 3));
                    int id = int.Parse(name.Substring(3, name.Length - 3));
                    editorBlock.OpenMarkerEditor(type, id);
                }
                else if (results[0].gameObject.tag == "ObjectUI")
                {

                }
            }
        }
    }

    public ActiveWindow FingerFocusScreen()
    {
        Vector2 pos = ((Vector2)Input.mousePosition - halfScreen) / halfScreen;
        if (pos.y < -0.2f) { return ActiveWindow.Timeline; } // -100
        else
        {
            if (pos.x < 0f)
            {
                if (pos.y <= 0) { return ActiveWindow.Toolbar; }
                else { return ActiveWindow.GameStream; }
            }
            else
            {
                switch (downRender)
                {
                    case EditorRenderType.None:
                        return ActiveWindow.None;

                    case EditorRenderType.EditorLR:
                        if (pos.x < 0.5f) // 480
                        { return ActiveWindow.EditorLeft; }
                        return ActiveWindow.EditorRight;

                    case EditorRenderType.EditorLRD:
                        if (pos.y < 0.3125f) // 150 
                        { return ActiveWindow.EditorDown; }
                        if (pos.x < 0.5f) // 480
                        { return ActiveWindow.EditorLeft; }
                        return ActiveWindow.EditorRight;
                }
            }
        }
        return ActiveWindow.None;
    }

    public void LoadLevel()
    {
        Level level = FileManager.Load("0 demo level");
        data = level.data;
        markers = level.markers;
        checkpoints = level.checkpoints;
        editorPrefabs = level.editorPrefabs;
        gameEvents = level.gameEvents;
        prefabs = level.prefabs;
        effects = level.effects;

        if (data.startFadeOut > data.endFadeOut) 
        { data.endFadeOut = data.startFadeOut; }

        LoadEditor();
    }

    private void LoadEditor()
    {
        timeline.Load();
    }

    public float GetAspect() 
    { return cam.aspect; }

    [HideInInspector] public Coroutine timer;
    public IEnumerator Timer()
    {
        for (float i = secCurrent; i <= timeline.secLength; i += Time.deltaTime * Time.timeScale * timeScale)
        {
            secCurrent = i;
            editorBlock.RenderLocalSecMarker();
            timeline.RenderSecLine();
            play.Timer(i);

            if (secCurrent < 0f) 
            { break; }
            yield return null;
        }

        isPlay = false;
        secCurrent = 0;
        editorBlock.RenderLocalSecMarker();
        timeline.RenderSecLine();
        play.EndGame();
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(BaseLevelEditor))]
public class CustomButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        BaseLevelEditor editor = (BaseLevelEditor)target;

        if (GUILayout.Button("Save Test Level")) 
        {
            FileManager.CreateTestLevel();
        }
    }
}
#endif