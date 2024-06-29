using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCircle : MonoBehaviour
{
    public float diameter = 0.5f;
    public ColliderPlayer player;
    private float radius;
    private Transform tr;
    public float GetRadius()
    {
        if (tr.localScale.x > tr.localScale.y)
        { return radius * tr.localScale.x; }
        return radius * tr.localScale.y;
    }
    public Vector2 GetPos()
    { return tr.localPosition; }

    public void Awake()
    {
        tr = transform;
        radius = diameter / 2f;
    }

    public void FixedUpdate()
    {
        if (Vector2.Distance(player.GetPos(), GetPos()) <= player.GetRadius() + GetRadius()) 
        { player.Trigger(); }
    }
}