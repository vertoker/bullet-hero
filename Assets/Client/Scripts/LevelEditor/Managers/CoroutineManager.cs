using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;
    private readonly List<Coroutine> coroutines = new List<Coroutine>();

    private void Awake()
    {
        Instance = this;
    }

    public Coroutine StartArray(int length, UnityAction<int> action)
    {
        Coroutine coroutine = StartCoroutine(TaskSolve(length, action));
        coroutines.Add(coroutine);
        return coroutine;
    }
    public Coroutine StartArray<T1>(T1 param1, int length, UnityAction<T1, int> action)
    {
        Coroutine coroutine = StartCoroutine(TaskSolve(param1, length, action));
        coroutines.Add(coroutine);
        return coroutine;
    }

    public void Stop(Coroutine coroutine)
    {
        if (coroutines.Remove(coroutine))
            StopCoroutine(coroutine);
    }

    private IEnumerator TaskSolve(int length, UnityAction<int> action)
    {
        for (int i = 0; i < length; i++)
        {
            action.Invoke(i);
            yield return null;
        }
    }
    private IEnumerator TaskSolve<T1>(T1 parameter, int length, UnityAction<T1, int> action)
    {
        for (int i = 0; i < length; i++)
        {
            action.Invoke(parameter, i);
            yield return null;
        }
    }
}
