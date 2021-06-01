using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void RenderFrameUpdate();
public class EditorTimer : MonoBehaviour
{
    private bool isPlay = false;
    private static float secCurrent = 0;
    private float frameCurrent = 0;
    private float secLength = 0;
    private float timeScale = 1;
    private Coroutine timer;

    public float TimeScale { get { return timeScale; } set { timeScale = value; } }
    public static float SecCurrent { get { return secCurrent; } }

    private List<RenderFrameUpdate> updates = new List<RenderFrameUpdate>();
    private int length = 0;

    public static EditorTimer Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Init(float secLength)
    {
        this.secLength = secLength;
        timer = StartCoroutine(Timer());
    }
    public void PlayPause()
    {
        isPlay = !isPlay;
        if (isPlay)
            timer = StartCoroutine(Timer());
        else
            StopCoroutine(timer);
    }

    public int Add(RenderFrameUpdate update)
    {
        updates.Add(update);
        length++;
        return length;
    }
    public void RemoveAt(int id)
    {
        updates.RemoveAt(id);
        length--;
    }

    public IEnumerator Timer()
    {
        for (float i = secCurrent; i <= secLength; i += Time.deltaTime * timeScale)
        {
            secCurrent = Mathf.Clamp(i, 0, secLength);
            frameCurrent = Sec2Frame(secCurrent);
            Render();
            yield return null;
        }
        
        isPlay = false;
        secCurrent = 0;
        frameCurrent = 0;

        /*editorBlock.RenderLocalSecMarker();
        timeline.RenderSecLine();
        play.EndGame();*/
    }

    private void Render()
    {
        for (int i = 0; i < length; i++)
        {
            updates[i].Invoke();
        }
    }

    public const float FRAMES_PER_SECOND = 60;
    public static float Frame2Sec(int frame)
    {
        return frame / FRAMES_PER_SECOND;
    }
    public static int Sec2Frame(float sec)
    {
        return Mathf.FloorToInt(sec * FRAMES_PER_SECOND);
    }
    public static string Sec2Text(float sec)
    {
        int min = 0; string timerText = string.Empty;
        int frames = (int)((sec - (int)sec) * 60f);
        while (sec >= 60f) { min++; sec -= 60; }

        sec = (int)sec; if (frames < 0)
        { frames = 60 + frames; sec -= 1; }

        if (min < 10) { timerText += "0" + min + ":"; }
        else { timerText += min + ":"; }

        if (sec < 10f) { timerText += "0" + sec.ToString("0"); }
        else { timerText += sec.ToString("0"); }

        if (frames < 10) { timerText += ".0" + frames; }
        else { timerText += "." + frames; }

        return timerText;
        /*timerTimeLine.text = timerText;
        timerMarkers.text = timerText;
        timerSecMarkers.text = sec + "." + frames;*/
    }
}
