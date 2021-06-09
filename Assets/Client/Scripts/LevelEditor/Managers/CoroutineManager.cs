using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public static class CoroutineManager
{
    private static readonly List<Coroutine> coroutines = new List<Coroutine>();
    private static MonoBehaviour monoBehaviour;

    public static void Init(MonoBehaviour monoBehaviour)
    {
        CoroutineManager.monoBehaviour = monoBehaviour;
    }

    public static Coroutine Start(IEnumerator enumerator)
    {
        Coroutine coroutine = monoBehaviour.StartCoroutine(enumerator);
        coroutines.Add(coroutine);
        return coroutine;
    }
    public static Coroutine StartArray(int length, UnityAction<int> action)
    {
        Coroutine coroutine = monoBehaviour.StartCoroutine(TaskSolve(length, action));
        coroutines.Add(coroutine);
        return coroutine;
    }
    public static Coroutine StartArray<T1>(T1 param1, int length, UnityAction<T1, int> action)
    {
        Coroutine coroutine = monoBehaviour.StartCoroutine(TaskSolve(param1, length, action));
        coroutines.Add(coroutine);
        return coroutine;
    }

    public static void Stop(Coroutine coroutine)
    {
        if (coroutines.Remove(coroutine))
            monoBehaviour.StopCoroutine(coroutine);
    }

    private static IEnumerator TaskSolve(int length, UnityAction<int> action)
    {
        for (int i = 0; i < length; i++)
        {
            action.Invoke(i);
            yield return null;
        }
    }
    private static IEnumerator TaskSolve<T1>(T1 parameter, int length, UnityAction<T1, int> action)
    {
        for (int i = 0; i < length; i++)
        {
            action.Invoke(parameter, i);
            yield return null;
        }
    }
}
