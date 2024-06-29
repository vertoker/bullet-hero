using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBox : MonoBehaviour
{
    private ColliderPlayer player;
    private Transform tr;
    public Vector2 GetPos()
    { return tr.localPosition; }
    static bool CollisionDetectionCircleRectangle(Vector2 player, float radius, Vector2 pos, float widthX, float heightY, float rotZ)//No angle
    {
        player = RotateVector(player - pos, -rotZ);

        float distanceX = Mathf.Abs(player.x);
        float distanceY = Mathf.Abs(player.y);

        if (distanceX > (widthX / 2 + radius)) { return false; }
        if (distanceY > (heightY / 2 + radius)) { return false; }
        if (distanceX <= (widthX / 2)) { return true; }
        if (distanceY <= (heightY / 2)) { return true; }

        float cDist_sqX = distanceX - widthX / 2;
        float cDist_sqY = distanceY - heightY / 2;
        float cDist_sq = cDist_sqX * cDist_sqX + cDist_sqY * cDist_sqY;

        return cDist_sq <= radius * radius;

        Vector2 RotateVector(Vector2 a, float offsetAngle)//метод вращения объекта
        {
            float power = Mathf.Sqrt(a.x * a.x + a.y * a.y);//коэффициент силы
            float angle = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - 90f + offsetAngle;//угол из координат с offset'ом
            return Quaternion.Euler(0, 0, angle) * Vector2.up * power;
            //построение вектора из изменённого угла с коэффициентом силы
        }
    }

    public void Awake() 
    {
        tr = transform;
        player = ColliderPlayer.Instance();
    }

    public void FixedUpdate()
    {
        if (CollisionDetectionCircleRectangle(player.GetPos(), player.GetRadius(), GetPos(), tr.localScale.x, tr.localScale.y, tr.localEulerAngles.z))
        { player.Trigger(); }
    }
}

/*
    public float GetMaxPoint()
    {
        float realWidthX = tr.localScale.x / 2f;
        float realHeigthY = tr.localScale.y / 2f;

        Vector2 a = player.GetPos() - GetPos();
        Vector2 aVector = new Vector2(realWidthX, realHeigthY);
        float maxDist = Mathf.Sqrt(realWidthX * realWidthX + realHeigthY * realHeigthY);

        float angle = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - tr.localEulerAngles.z;
        float aVertex = Mathf.Atan2(aVector.y, aVector.x) * Mathf.Rad2Deg;

        float minDist = 0;
        if (angle < 0) { angle += 360; }
        if (angle > 360 - aVertex) { angle -= 360; }

        if (angle > -aVertex && angle <= aVertex) 
        { minDist = realWidthX;  }
        else if (angle > aVertex && angle <= 180 - aVertex) 
        { minDist = realHeigthY; angle -= 90; aVertex = 90 - aVertex; }
        else if (angle > 180 - aVertex && angle <= 180 + aVertex) 
        { minDist = realWidthX; angle -= 180; }
        else if (angle > 180 + aVertex && angle <= 360 - aVertex) 
        { minDist = realHeigthY; angle -= 270; aVertex = 90 - aVertex; }

        float end = Mathf.Abs(angle) / aVertex;
        print(angle + " " + aVertex + " " + minDist + " " + maxDist + " " + (minDist + (maxDist - minDist) * end));
        return minDist + (maxDist - minDist) * end;
    }
    float GetMaxPointRectangle()
    {
        Vector2 pos = GetPos();
        float offset = tr.localScale.y - tr.localScale.x;
        realParameter = Mathf.Min(tr.localScale.x, tr.localScale.y) / 2f;
        Vector2 aLoc = RotateVector(player.GetPos() - pos, -transform.eulerAngles.z);

        float power = Mathf.Abs(offset) / 2f;
        float lAngle = transform.eulerAngles.z;
        if (offset > 0f)//y
        {
            if (Mathf.Abs(aLoc.y) > power)
            { return tr.localScale.x / 2f; }
            lAngle += 90f;
        }
        else if (offset < 0f)//x
        {
            if (aLoc.x > power) 
            { power = +power; }
            else if (aLoc.x < -power)
            { power = -power; }
            else { return tr.localScale.y / 2f; }
        }
        Vector2 center = Quaternion.Euler(0, 0, lAngle) * Vector2.right * power;
        pos += center;

        //Square collider
        float minDist = realParameter;
        Vector2 a = player.GetPos() - pos;
        float maxDist = Mathf.Sqrt(realParameter * realParameter + realParameter * realParameter);
        float angle = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - transform.eulerAngles.z + 360f;

        while (angle > 45f) { angle -= 90f; }
        return minDist + (maxDist - minDist) * (Mathf.Abs(angle) / 45f);
    }*/