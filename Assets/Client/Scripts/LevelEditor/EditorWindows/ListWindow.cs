using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public delegate List<IData> GetList();
public delegate string GetParameter(IData item);

public class ListWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TMP_InputField search;
    [SerializeField] private RectTransform objUI;
    [SerializeField] private Transform parent;

    private Queue<RectTransform> poolEnable;
    private Queue<RectTransform> poolDisable;
    private readonly Coroutine process;

    private GetList getList;
    private GetParameter getName;
    private GetParameter[] parameters;
    private int lengthParameters;
    private int lengthList;

    public void Init(GetList getList, GetParameter getName, GetParameter[] parameters)
    {
        poolEnable = new Queue<RectTransform>();
        poolDisable = new Queue<RectTransform>();
        search.onValueChanged.AddListener(new UnityAction<string>(StartSearch));
        this.getList = getList;
        this.getName = getName;
        this.parameters = parameters;
        lengthParameters = parameters.Length;
        lengthList = getList.Invoke().Count;
    }

    public void StartSearch(string search_source)
    {
        StopSearch();
        CoroutineManager.Instance.StartArray(search_source, lengthList, new UnityAction<string, int>(SearchProcess));
    }
    public void StopSearch()
    {
        if (process != null)
        {
            CoroutineManager.Instance.Stop(process);
            ClearUI();
        }
    }

    private void SearchProcess(string search_source, int index)
    {
        IData element = getList.Invoke()[index];
        if (getName.Invoke(element).Contains(search_source))
            AddItemUI(element);
    }

    private void AddItemUI(IData item)
    {
        if (poolDisable.Count == 0)
            poolDisable.Enqueue(Instantiate(objUI, parent));
        RectTransform obj = poolDisable.Dequeue();
        poolEnable.Enqueue(obj);
        Vector3 pos = new Vector3(poolEnable.Count * -200, 0, 0);
        obj.position = pos;
        for (int i = 0; i < lengthParameters; i++)
            obj.GetChild(i).GetComponent<TMP_Text>().text = parameters[i].Invoke(item);
        obj.gameObject.SetActive(true);
    }
    private void ClearUI()
    {
        while (poolEnable.Count > 0)
        {
            RectTransform obj = poolEnable.Dequeue();
            obj.gameObject.SetActive(false);
            poolDisable.Enqueue(obj);
        }
    }

    public void Init()
    {

    }
    public RectTransform Open()
    {
        return GetComponent<RectTransform>();
    }
    public void Close()
    {

    }
}