using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void RenderFrameUpdate(float sec, int frame);
public delegate void RenderStop();
public class EditorTimer : MonoBehaviour
{
    private bool isPlay = false;
    [SerializeField] private float secCurrent = 0;
    [SerializeField] private int frameCurrent = 1;
    private float secLength = 0;
    private float timeScale = 1;
    private int secLengthFrame;
    private Coroutine timer;

    public static float TimeScale { get { return Instance.timeScale; } set { Instance.timeScale = (float)Math.Round(value, 1); } }
    public static float SecCurrent { get { return Instance.secCurrent; } }
    public static int FrameCurrent { get { return Instance.frameCurrent; } }

    private List<RenderFrameUpdate> updates = new List<RenderFrameUpdate>();
    private List<RenderStop> stops = new List<RenderStop>();
    private int lengthUpdates = 0, lengthStops = 0;

    private static EditorTimer Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Init(float secLength)
    {
        Instance.secLength = secLength;
        secLengthFrame = Mathf.FloorToInt(secLength * Utils.FRAMES_PER_SECOND);
    }
    public void PlayPause()
    {
        isPlay = !isPlay;
        if (isPlay)
            timer = StartCoroutine(Timer());
        else
            StopCoroutine(timer);
    }
    public void Play()
    {
        isPlay = true;
        timer = StartCoroutine(Timer());
    }
    public void Pause()
    {
        isPlay = false;
        StopCoroutine(timer);
    }

    public static void Add(RenderFrameUpdate update)
    {
        Instance.updates.Add(update);
        Instance.lengthUpdates++;
    }
    public static void Add(RenderStop stop)
    {
        Instance.stops.Add(stop);
        Instance.lengthStops++;
    }
    public static bool Remove(RenderFrameUpdate update)
    {
        if (Instance.updates.Remove(update))
        {
            Instance.lengthUpdates--;
            return true;
        }
        return false;
    }
    public static bool Remove(RenderStop stop)
    {
        if (Instance.stops.Remove(stop))
        {
            Instance.lengthStops--;
            return true;
        }
        return false;
    }

    public void MinusScale()
    {
        if (timeScale > -3f)
            TimeScale -= 0.1f;
    }
    public void PlusScale()
    {
        if (timeScale < 3f)
            TimeScale += 0.1f;
    }
    public void StandardScale()
    {
        TimeScale = 1f;
    }

    public IEnumerator Timer()
    {
        for (float i = secCurrent; true; i += Time.deltaTime * timeScale)
        {
            secCurrent = Mathf.Clamp(i, 0, secLength);
            frameCurrent = Mathf.Clamp(Utils.Sec2Frame(i), 1, secLengthFrame);
            Render();
            if (i > secLength || i < 0)
                break;
            yield return null;
        }

        isPlay = false;
        Stop();

        /*editorBlock.RenderLocalSecMarker();
        timeline.RenderSecLine();
        play.EndGame();*/
    }

    private void Render()
    {
        for (int i = 0; i < lengthUpdates; i++)
            updates[i].Invoke(secCurrent, frameCurrent);
    }
    private void Stop()
    {
        for (int i = 0; i < lengthStops; i++)
            stops[i].Invoke();
    }
}
