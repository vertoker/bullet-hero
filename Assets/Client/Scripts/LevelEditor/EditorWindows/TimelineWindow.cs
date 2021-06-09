using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TimelineWindow : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform contentSeconds;
    [SerializeField] private Transform contentTimeline;

    [SerializeField] private Scrollbar scrollbarHorizontal;
    [SerializeField] private RectTransform scrollView;
    [SerializeField] private RectTransform secLine;
    [SerializeField] private RectTransform secMarker;
    [SerializeField] private RectTransform secLineMarker;
    [SerializeField] private RectTransform secMarkerScrollbar;
    [SerializeField] private RectTransform content;
    [Header("Pools")]
    [SerializeField] private GameObject secondInstance;
    [SerializeField] private GameObject prefabInstance;
    private SpawnPool secondsPool;
    private SpawnPool prefabsPool;

    private bool isPlay = false;
    private float timelineLength, camAspect;
    private float secWidth, width, secLength;
    private float secCurrent = 0;

    private void Awake()
    {
        camAspect = cam.aspect;
        width = 1080f * camAspect;
        secWidth = 10.8f * camAspect;

        //Aspect Update
        //scrollView.sizeDelta = new Vector2(width, 440f);

        secondsPool = new SpawnPool(30, secondInstance, contentSeconds);
        prefabsPool = new SpawnPool(150, prefabInstance, contentTimeline);
    }

    public void Init(float length)
    {
        secLength = length;
        timelineLength = 100f * secLength;
        secLine.sizeDelta = new Vector2(timelineLength, 50f);
        content.sizeDelta = new Vector2(timelineLength, 1000f);

        EditorTimer.Add(UpdateSecLineMarker);
        UpdateSecLineMarker(secCurrent);
        TimelineScrollRectUpdate();
    }

    public void UpdateSecLineMarker(float sec)
    {
        /*float maxDist = timelineLength - width;
        float start = maxDist * scrollbarHorizontal.value;
        float posX_01 = Input.mousePosition.x / Screen.width;
        float posTimeline = start + posX_01 * width;
        secCurrent = posTimeline / timelineLength * secLength;
        Debug.Log(secCurrent * 100 - timelineLength * scrollbarHorizontal.value);*/
        secMarker.localPosition = new Vector2(sec * 100f - timelineLength * scrollbarHorizontal.value, -25);
        secMarkerScrollbar.localPosition = new Vector2(sec / secLength * width - 540f * camAspect, -390f);
        secLineMarker.position = new Vector2(secMarker.position.x, 0f);
    }
    public void TimelineScrollRectUpdate()
    {
        secLine.pivot = new Vector2(scrollbarHorizontal.value, 1f);
        secLine.localPosition = new Vector2(width * scrollbarHorizontal.value - width / 2f, 0f);
        secLineMarker.position = new Vector2(secMarker.position.x, 0f);
        //RenderTimeline();
    }
    public void RenderTimeline(int startFrame, int endFrame, int startHeigth, int endHeigth)
    {
        secondsPool.Render(startFrame, endFrame, startHeigth, endHeigth);
        prefabsPool.Render(startFrame, endFrame, startHeigth, endHeigth);
    }

    public void SecLineDown()
    {
        isPlay = EditorTimer.IsPlay;
        EditorTimer.Pause();
        float locWidth = timelineLength - width;
        float start = locWidth * scrollbarHorizontal.value;
        float posX_01 = Input.mousePosition.x / Screen.width;
        float posTimeline = start + posX_01 * width;

        secCurrent = posTimeline / timelineLength * secLength;
        if (secCurrent < 0) { secCurrent = 0; }
        else if (secCurrent > secLength)
        { secCurrent = secLength; }

        EditorTimer.SecCurrent = secCurrent;
    }
    public void SecLineDrag()
    {
        float locWidth = timelineLength - width;
        float start = locWidth * scrollbarHorizontal.value;
        float posX_01 = Input.mousePosition.x / Screen.width;
        float posTimeline = start + posX_01 * width;

        secCurrent = posTimeline / timelineLength * secLength;
        if (secCurrent < 0) { secCurrent = 0; }
        else if (secCurrent > secLength)
        { secCurrent = secLength; }

        EditorTimer.SecCurrent = secCurrent;
    }
    public void SecLineUp()
    {
        if (isPlay)
            EditorTimer.Play();
    }
    public bool WatchedSecMarker()
    {
        float secStart = (secLength - secWidth) * scrollbarHorizontal.value;
        if (secStart < 0f) { secStart = 0f; }
        float secEnd = secStart + secWidth;
        if (secEnd > secLength) { secEnd = secLength; }
        return secCurrent < secStart || secCurrent > secEnd;
    }
    public void WatchSecMarker()
    {
        float time = secCurrent / secLength;
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

    public void Render(int startFrame, int endFrame, int startHeigth, int endHeigth)
    {

    }
}