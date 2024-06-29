using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TimelineWindow : MonoBehaviour
{
    public BaseLevelEditor editor;
    public Scrollbar scrollbarHorizontal;
    public RectTransform scrollView;
    public RectTransform secLine;
    public RectTransform secLineScrollbar;
    public RectTransform secsTransform;
    public RectTransform secLineMarker;
    public RectTransform content;
    public GameObject prefabObj;
    public GameObject secObj;

    [HideInInspector] public float secLength;
    [HideInInspector] public float timelineLength;
    [HideInInspector] public float width;
    private float camAspect;
    private float secWidth;

    public void Awake()
    {
        camAspect = editor.GetAspect();
        width = 1080f * camAspect;
        secWidth = 10.8f * camAspect;

        //Aspect Update
        scrollView.sizeDelta = new Vector2(width, 440f);
    }

    public void Update()
    {
        if (editor.active) 
        {
            secLine.pivot = new Vector2(scrollbarHorizontal.value, 1f);
            secLine.localPosition = new Vector2(width * scrollbarHorizontal.value, 0f);
        }
    }

    public void Load()
    {
        editor.secCurrent = 0;
        if (editor.data.endFadeOut > 0) 
        { secLength = editor.data.endFadeOut; }
        else { secLength = editor.clip.length; }
        timelineLength = 100f * secLength;
        editor.play.Timer(editor.secCurrent);

        secLine.sizeDelta = new Vector2(timelineLength, 50f);
        content.sizeDelta = new Vector2(timelineLength, 1000f);
        for (int i = 1; i <= (int)secLength; i++)
        {
            RectTransform tr = Instantiate(secObj, secsTransform).GetComponent<RectTransform>();
            tr.localPosition = new Vector2(i * 100, 0);
            tr.name = "Sec (" + i + ")";
        }
        for (int i = 0; i < editor.prefabs.Count; i++)
        {
            RectTransform tr = Instantiate(prefabObj, content).GetComponent<RectTransform>();
            tr.localPosition = new Vector2(editor.prefabs[i].GetMin() * 100, editor.prefabs[i].height * -50 - 50);
            tr.sizeDelta = new Vector2(100 * editor.prefabs[i].tExist, 50);
            TextMeshProUGUI txt = tr.GetChild(0).GetComponent<TextMeshProUGUI>();
            txt.text = editor.prefabs[i].name;
        }

        editor.active = true;
    }

    public void SecLineDown()
    {
        editor.isTouchSecLine = true;
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
        if (editor.isPlay) { StopCoroutine(editor.timer); }
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

    public void Play()
    {
        editor.timer = StartCoroutine(editor.Timer());
        editor.isPlay = true;
    }

    public void Pause()
    {
        StopCoroutine(editor.timer);
        editor.isPlay = false;
    }
}