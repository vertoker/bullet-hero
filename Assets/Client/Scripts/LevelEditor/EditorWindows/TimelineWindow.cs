using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TimelineWindow : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private EditorTimer timer;
    [SerializeField] private Transform parent;

    [SerializeField] private Scrollbar scrollbarHorizontal;
    [SerializeField] private RectTransform scrollView;
    [SerializeField] private RectTransform secLine;
    [SerializeField] private RectTransform secLineScrollbar;
    [SerializeField] private RectTransform secsTransform;
    [SerializeField] private RectTransform secLineMarker;
    [SerializeField] private RectTransform content;
    [Header("Pools")]
    [SerializeField] private GameObject secondInstance;
    [SerializeField] private GameObject prefabInstance;
    private SpawnPool secondsPool;
    private SpawnPool prefabsPool;

    private bool isTouchSecLine = false, active = false;
    private float timelineLength, camAspect;
    private float secWidth, width, secLength;
    private float secCurrent;

    private void Awake()
    {
        camAspect = cam.aspect;
        width = 1080f * camAspect;
        secWidth = 10.8f * camAspect;

        //Aspect Update
        //scrollView.sizeDelta = new Vector2(width, 440f);

        secondsPool = new SpawnPool(30, secondInstance, parent);
        prefabsPool = new SpawnPool(150, prefabInstance, parent);
    }

    public void Init()
    {
        secLength = LevelManager.level.LevelData.EndFadeOut;
        timelineLength = 100f * secLength;
        secLine.sizeDelta = new Vector2(timelineLength, 50f);
        content.sizeDelta = new Vector2(timelineLength, 1000f);
    }

    public void TimelineScrollRectUpdate()
    {

    }

    public void Update()
    {
        if (active)
        {
            secLine.pivot = new Vector2(scrollbarHorizontal.value, 1f);
            secLine.localPosition = new Vector2(width * scrollbarHorizontal.value, 0f);
        }
    }

    public void SecLineDown()
    {
        isTouchSecLine = true;
        timer.Pause();//Спрайт не обрабатывает
        float locWidth = timelineLength - width;
        float start = locWidth * scrollbarHorizontal.value;
        float posX_01 = Input.mousePosition.x / Screen.width;
        float posTimeline = start + posX_01 * width;
        secCurrent = posTimeline / timelineLength * secLength;
        if (secCurrent < 0) { secCurrent = 0; }
        else if (secCurrent > secLength)
        { secCurrent = secLength; }
        secLineMarker.localPosition = new Vector2(secCurrent * 100 - timelineLength * scrollbarHorizontal.value, -25);
        secLineScrollbar.localPosition = new Vector2(secCurrent / secLength * width - 540f * camAspect, -440f);
        editorBlock.RenderLocalSecMarker();
        play.Timer(secCurrent);
        if (isPlay) { StopCoroutine(timer); }
    }
    public void SecLineDrag()
    {
        float locWidth = timelineLength - width;
        float start = locWidth * scrollbarHorizontal.value;
        float posX_01 = Input.mousePosition.x / Screen.width;
        float posTimeline = start + posX_01 * width;
        editor.secCurrent = posTimeline / timelineLength * secLength;
        if (editor.secCurrent < 0) { editor.secCurrent = 0; }
        else if (editor.secCurrent > secLength)
        { editor.secCurrent = secLength; }
        secLineMarker.localPosition = new Vector2(editor.secCurrent * 100 - timelineLength * scrollbarHorizontal.value, -25);
        secLineScrollbar.localPosition = new Vector2(editor.secCurrent / secLength * width - 540f * camAspect, -440f);
        editor.editorBlock.RenderLocalSecMarker();
        editor.play.Timer(editor.secCurrent);
    }
    public void SecLineUp()
    {
        editor.isTouchSecLine = false;
        if (editor.isPlay) { editor.timer = StartCoroutine(editor.Timer()); }
    }
    public void RenderSecLine()
    {
        secLineMarker.localPosition = new Vector2(editor.secCurrent * 100 - timelineLength * scrollbarHorizontal.value, -25);
        secLineScrollbar.localPosition = new Vector2(editor.secCurrent / secLength * width - 540f * camAspect, -440f);
    }

    public void UpdateSecLineMarker(Vector2 input)
    {
        if (editor.isTouchSecLine)
        {
            float maxDist = timelineLength - width;
            float start = maxDist * scrollbarHorizontal.value;
            float posX_01 = Input.mousePosition.x / Screen.width;
            float posTimeline = start + posX_01 * width;
            editor.secCurrent = posTimeline / timelineLength * secLength;
            secLineMarker.localPosition = new Vector2(editor.secCurrent * 100 - timelineLength * scrollbarHorizontal.value, -25);
            editor.play.Timer(editor.secCurrent);
        }
    }

    public bool WatchedSecMarker()
    {
        float secStart = (secLength - secWidth) * scrollbarHorizontal.value;
        if (secStart < 0f) { secStart = 0f; }
        float secEnd = secStart + secWidth;
        if (secEnd > secLength) { secEnd = secLength; }
        return editor.secCurrent < secStart || editor.secCurrent > secEnd;
    }
    public void WatchSecMarker()
    {
        float time = editor.secCurrent / secLength;
        scrollbarHorizontal.value = time + time / timelineLength * width;
        if (scrollbarHorizontal.value > 1f) { scrollbarHorizontal.value = 1f; }
    }
}
public class SpawnPool
{
    private readonly int startCapacity;
    private readonly GameObject objectInstance;
    private readonly Queue<GameObject> objectsEnable;
    private readonly Queue<GameObject> objectsDisable;

    public SpawnPool(int startCapacity, GameObject objectInstance, Transform parent)
    {
        this.startCapacity = startCapacity;
        this.objectInstance = objectInstance;
        objectsEnable = new Queue<GameObject>();
        objectsDisable = new Queue<GameObject>();
        for (int i = 0; i < startCapacity; i++)
        {
            GameObject obj = Object.Instantiate(objectInstance, parent);
            objectsDisable.Enqueue(obj);
        }
    }
}