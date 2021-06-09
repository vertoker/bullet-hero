using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void RenderFrameUpdate(float sec);
public delegate void RenderStop();
public static class EditorTimer
{
    private static bool play = false;
    [SerializeField] private static float secCurrent = 0;
    private static float secLength = 0;
    private static float timeScale = 1;
    private static int secLengthFrame;
    private static Coroutine timer;

    public static float TimeScale { get { return timeScale; } set { timeScale = (float)Math.Round(value, 1); } }
    public static float SecCurrent { get { return secCurrent; } set { secCurrent = value; Render(); } }
    public static bool IsPlay { get { return play; } }

    private static List<RenderFrameUpdate> updates = new List<RenderFrameUpdate>();
    private static List<RenderStop> stops = new List<RenderStop>();
    private static int lengthUpdates = 0, lengthStops = 0;

    public static void Init(float secLength)
    {
        EditorTimer.secLength = secLength;
        secLengthFrame = Mathf.FloorToInt(secLength * Utils.FRAMES_PER_SECOND);
    }
    public static void PlayPause()
    {
        if (play)
            Pause();
        else
            Play();
    }
    public static void Play()
    {
        if (!play)
        {
            play = true;
            if (secCurrent == secLength && timeScale > 0)
                secCurrent = 0;
            timer = CoroutineManager.Start(Timer());
            SpriteChanger.SpriteMod(0, 1);
        }
    }
    public static void Pause()
    {
        if (play)
        {
            play = false;
            CoroutineManager.Stop(timer);
            SpriteChanger.SpriteMod(0, 0);
        }
    }

    public static void Add(RenderFrameUpdate update)
    {
        updates.Add(update);
        lengthUpdates++;
    }
    public static void Add(RenderStop stop)
    {
        stops.Add(stop);
        lengthStops++;
    }
    public static bool Remove(RenderFrameUpdate update)
    {
        if (updates.Remove(update))
        {
            lengthUpdates--;
            return true;
        }
        return false;
    }
    public static bool Remove(RenderStop stop)
    {
        if (stops.Remove(stop))
        {
            lengthStops--;
            return true;
        }
        return false;
    }

    public static void MinusScale()
    {
        if (timeScale > -3f)
            TimeScale -= 0.1f;
    }
    public static void PlusScale()
    {
        if (timeScale < 3f)
            TimeScale += 0.1f;
    }
    public static void StandardScale()
    {
        TimeScale = 1f;
    }

    public static IEnumerator Timer()
    {
        for (float i = secCurrent; true; i += Time.deltaTime * timeScale)
        {
            secCurrent = Mathf.Clamp(i, 0, secLength);
            Render();
            if (i > secLength || i < 0)
                break;
            yield return null;
        }

        Stop();
        Pause();

        /*editorBlock.RenderLocalSecMarker();
        timeline.RenderSecLine();
        play.EndGame();*/
    }

    private static void Render()
    {
        for (int i = 0; i < lengthUpdates; i++)
            updates[i].Invoke(secCurrent);
    }
    private static void Stop()
    {
        for (int i = 0; i < lengthStops; i++)
            stops[i].Invoke();
    }
}
