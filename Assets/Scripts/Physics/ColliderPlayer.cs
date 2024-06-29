using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlayer : MonoBehaviour
{
    public float radius = 0.5f;
    private Transform tr;
    private int counter = 0;
    private int counter2 = 0;

    private static ColliderPlayer instance;
    public static ColliderPlayer Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<ColliderPlayer>();
        return instance;
    }

    public float GetRadius()
    {
        return radius * Mathf.Max(tr.localScale.x, tr.localScale.y); 
    }
    public Vector2 GetPos()
    { return tr.localPosition; }

    public void Awake()
    {
        tr = transform;
    }
    public void Trigger()
    {
        counter2 = counter;
        counter++;
    }
    public void FixedUpdate()
    {
        if (counter != counter2) 
        { tr.GetComponent<SpriteRenderer>().color = Color.red; }
        else { tr.GetComponent<SpriteRenderer>().color = Color.white; }
        counter2 = counter;
    }
}
