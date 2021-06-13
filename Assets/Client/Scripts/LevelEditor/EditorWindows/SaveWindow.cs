using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SaveWindow : MonoBehaviour, IWindow
{
    [SerializeField] private RectTransform parent;
    public static SaveWindow Instance;

    private void Awake()
    { Instance = this; }

    public RectTransform Open()
    {
        parent.gameObject.SetActive(true);
        LevelManager.Save();
        //Localize.Instance.TranslateRandomText(0);
        return parent;
    }
    public void Close()
    {
        parent.gameObject.SetActive(false);
    }
}
