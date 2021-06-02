using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SaveWindow : MonoBehaviour, IWindow
{
    public static SaveWindow Instance;

    private void Awake()
    { Instance = this; }

    public RectTransform Open()
    {
        LevelManager.Save();
        Localize.Instance.TranslateRandomText(0);
        return GetComponent<RectTransform>();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
