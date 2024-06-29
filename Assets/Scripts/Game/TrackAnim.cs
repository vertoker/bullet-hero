using System.Collections.Generic;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TrackAnim : MonoBehaviour// Unity Animator Analog
{
    public PrefabItem[] items;
    private Generate generate;

    public void Start()
    {
        generate = GetComponent<Generate>();
    }

    public void StartTrack()
    {

    }

    public IEnumerator MainProcess()
    {
        for (float i = 0; i < 1; i += Time.deltaTime * Time.timeScale)
        {

            yield return null;
        }
    }

    public void Call(int idItem, DataItem itemType)
    {
        PrefabItem item = items[idItem];
        switch (itemType)
        {
            case DataItem.Bullet:
                generate.Bullet((Bullet)item);
                break;
            case DataItem.BulletRow:
                generate.BulletRow((BulletRow)item);
                break;
            case DataItem.BulletExplosion:
                generate.BulletExplosion((BulletExplosion)item);
                break;
            case DataItem.BulletDivision:
                generate.BulletDivision((BulletDivision)item);
                break;
            case DataItem.LineSimple:
                generate.LineSimple((LineSimple)item);
                break;
            case DataItem.CircleSimple:
                generate.CircleSimple((CircleSimple)item);
                break;
            case DataItem.StandardGun:
                generate.StandardGun((StandardGun)item);
                break;
            case DataItem.SquareSnake:
                generate.SquareSnake((SquareSnake)item);
                break;
            case DataItem.Pillar:
                generate.Pillar((Pillar)item);
                break;
            case DataItem.AnimSprite:
                generate.AnimSprite((AnimSprite)item);
                break;
            case DataItem.AnimText:
                generate.AnimText((AnimText)item);
                break;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TrackAnim))]
public class AddData : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TrackAnim track = (TrackAnim)target;
    }
}
#endif