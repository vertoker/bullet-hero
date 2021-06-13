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
    private RectTransform contentTimelineRect;

    [SerializeField] private Scrollbar scrollbarHorizontal;
    [SerializeField] private RectTransform timelineRect;
    [SerializeField] private RectTransform scrollView;
    [SerializeField] private RectTransform secLine;
    [SerializeField] private RectTransform secMarker;
    [SerializeField] private RectTransform secLineMarker;
    [SerializeField] private RectTransform secMarkerScrollbar;
    [SerializeField] private RectTransform content;
    [Header("Pools")]
    [SerializeField] private RectTransform secondInstance;
    [SerializeField] private RectTransform prefabInstance;
    private SpawnPool secondsPool;
    private SpawnPool prefabsPool;

    private bool isPlay = false;
    private float timelineLength, camAspect;
    private float secWidth, width, secLength;
    private float secCurrent = 0;

    private Vector2 viewportSizeDelta;
    private Vector2 contentSizeDelta;

    #region Pool Delegates
    private GetListRender secondsList = (int startFrame, int endFrame, int startHeigth, int endHeigth, out int[] indexes) =>
    {
        int startSec = Mathf.FloorToInt(startFrame / Utils.FRAMES_PER_SECOND);
        int endSec = Mathf.FloorToInt(endFrame / Utils.FRAMES_PER_SECOND);
        int length = endSec - startSec + 1;

        IData[] data = new IData[length];
        indexes = new int[length];
        for (int i = 0; i < length; i++)
        {
            data[i] = new Second(startSec + i);
            indexes[i] = i;
        }
        return data;
    };
    private void CustomizeSeconds(RectTransform tr, IData data, int index)
    {
        float seconds = float.Parse(data.GetParameter(0));
        tr.localPosition = new Vector2(seconds * Utils.SecondLength - timelineLength / 2f, 0);
    }
    private void CustomizePrefab(RectTransform tr, IData data, int index)
    {
        int startFrame = int.Parse(data.GetParameter(9));
        int offsetFrame = int.Parse(data.GetParameter(1));
        int heigth = int.Parse(data.GetParameter(14));
        tr.gameObject.name = index.ToString();
        tr.localPosition = new Vector2(startFrame / Utils.FRAMES_PER_SECOND * Utils.SecondLength, heigth * -Utils.LayerLength);
        tr.sizeDelta = new Vector2(Utils.Frame2Sec(offsetFrame) * Utils.SecondLength, Utils.LayerLength);
        tr.GetChild(0).GetComponent<TMP_Text>().text = data.GetParameter(0);
    }
    #endregion

    private void Awake()
    {
        camAspect = cam.aspect;
        width = 1080f * camAspect;
        secWidth = 10.8f * camAspect;

        //Aspect Update
        //scrollView.sizeDelta = new Vector2(width, 440f);

        contentTimelineRect = contentTimeline.GetComponent<RectTransform>();
        secondsPool = new SpawnPool(0, 30, secondInstance, contentSeconds, secondsList, CustomizeSeconds);
        prefabsPool = new SpawnPool(100, 50, prefabInstance, contentTimeline, StaticSearchPrefabsEditor.searchPrefabsEditor, CustomizePrefab);

        viewportSizeDelta = timelineRect.sizeDelta + scrollView.sizeDelta;
        viewportSizeDelta = new Vector2(viewportSizeDelta.x, -viewportSizeDelta.y);
    }

    public void Init(float length)
    {
        secLength = length;
        timelineLength = 100f * secLength;
        secLine.sizeDelta = new Vector2(timelineLength, Utils.LayerLength);
        content.sizeDelta = new Vector2(timelineLength, 1000f);
        contentSizeDelta = new Vector2(timelineLength, -1000f);

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
        secCurrent = sec;
        secMarker.localPosition = new Vector2(sec * 100f - timelineLength * scrollbarHorizontal.value, -25);
        secMarkerScrollbar.localPosition = new Vector2(sec / secLength * width - 540f * camAspect, -390f);
        secLineMarker.position = new Vector2(secMarker.position.x, 0f);
    }
    public void TimelineScrollRectUpdate()
    {
        secLine.pivot = new Vector2(scrollbarHorizontal.value, 1f);
        secLine.localPosition = new Vector2(width * scrollbarHorizontal.value - width / 2f, 0f);
        secLineMarker.position = new Vector2(secMarker.position.x, 0f);
        UpdateSecLineMarker(secCurrent);

        RenderTimeline();
    }
    public void RenderTimeline()
    {
        Vector2 contentStart = Vector2.zero;
        Vector2 contentEnd = contentSizeDelta;
        Vector2 viewportStart = -contentTimelineRect.localPosition;
        Vector2 viewportEnd = viewportStart + viewportSizeDelta;
        //Debug.Log(string.Join(" - ", contentStart, contentEnd, viewportStart, viewportEnd));

        Utils.RenderTimelineBorders(out int startFrame, out int endFrame, out int startHeigth, out int endHeigth,
            contentStart, contentEnd, viewportStart, viewportEnd);

        //Debug.Log(string.Join(" - ", startFrame, endFrame, startHeigth, endHeigth));
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
    public void WatchSecondMarker()
    {
        //float timeTarget = secCurrent - secWidth / 2 + secWidth * secCurrent;
        scrollbarHorizontal.value = Mathf.Clamp01(secCurrent / secLength);
    }
}
public delegate IData[] GetListRender(int startFrame, int endFrame, int startHeigth, int endHeigth, out int[] indexes);
public delegate void CustomizeObjectTimeline(RectTransform tr, IData data, int index);
public class SpawnPool
{
    private readonly Transform parent;
    private readonly RectTransform objectInstance;
    private readonly List<RectTransform> objectsEnable;
    private readonly Queue<RectTransform> objectsDisable;

    private GetListRender getList;
    private CustomizeObjectTimeline customization;

    public SpawnPool(int initDisable, int initEnable, RectTransform objectInstance, Transform parent, GetListRender getList, CustomizeObjectTimeline customization)
    {
        this.parent = parent;
        this.getList = getList;
        this.customization = customization;
        this.objectInstance = objectInstance;
        objectsEnable = new List<RectTransform>();
        objectsDisable = new Queue<RectTransform>();
        FillPool(initDisable + initEnable);
        EnableObjects(initEnable);
    }

    public void Render(int startFrame, int endFrame, int startHeigth, int endHeigth)
    {
        IData[] objects = getList.Invoke(startFrame, endFrame, startHeigth, endHeigth, out int[] indexes);
        int length = objects.Length;

        if (length < objectsEnable.Count)
        {
            DisableObjects(objectsEnable.Count - length);
        }
        else if (length > objectsEnable.Count)
        {
            int offset = length - objectsEnable.Count;
            if (offset > objectsDisable.Count)
                FillPool(offset - objectsDisable.Count);
            EnableObjects(offset);
        }

        for (int i = 0; i < length; i++)
            customization.Invoke(objectsEnable[i], objects[i], indexes[i]);
    }

    private void FillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            RectTransform tr = Object.Instantiate(objectInstance, parent);
            objectsDisable.Enqueue(tr);
        }
    }

    private void DisableObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            objectsEnable[i].gameObject.SetActive(false);
            objectsDisable.Enqueue(objectsEnable[i]);
        }
        objectsEnable.RemoveRange(0, count);
    }
    private void EnableObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            RectTransform tr = objectsDisable.Dequeue();
            tr.gameObject.SetActive(true);
            objectsEnable.Add(tr);
        }
    }
}