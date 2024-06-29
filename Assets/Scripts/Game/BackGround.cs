using UnityEngine;

public class BackGround : MonoBehaviour
{
    public bool isMovement = false;
    public Vector2 speed;
    public Transform[] trs;
    int activeImage = 4;
    const float offset = 426.407979f;
    const float pixelSize = 1.11045f;
    TransformBorders[] tb;
    Vector3 x, y;
    int l;

    private void Awake()
    {
        l = trs.Length;
        tb = new TransformBorders[l];
        tb[0] = new TransformBorders()
        {
            u1 = 8, u2 = 6, u3 = 7,
            d1 = 5, d2 = 3, d3 = 4,
            l1 = 8, l2 = 2, l3 = 5,
            r1 = 7, r2 = 1, r3 = 4
        };
        tb[1] = new TransformBorders()
        {
            u1 = 6, u2 = 7, u3 = 8,
            d1 = 3, d2 = 4, d3 = 5,
            l1 = 6, l2 = 0, l3 = 3,
            r1 = 8, r2 = 2, r3 = 5
        };
        tb[2] = new TransformBorders()
        {
            u1 = 7, u2 = 8, u3 = 6,
            d1 = 4, d2 = 5, d3 = 3,
            l1 = 7, l2 = 1, l3 = 4,
            r1 = 6, r2 = 0, r3 = 3
        };
        tb[3] = new TransformBorders()
        {
            u1 = 2, u2 = 0, u3 = 1,
            d1 = 8, d2 = 6, d3 = 7,
            l1 = 2, l2 = 5, l3 = 8,
            r1 = 1, r2 = 4, r3 = 7
        };
        tb[4] = new TransformBorders()
        {
            u1 = 0, u2 = 1, u3 = 2,
            d1 = 6, d2 = 7, d3 = 8,
            l1 = 0, l2 = 3, l3 = 6,
            r1 = 2, r2 = 5, r3 = 8
        };
        tb[5] = new TransformBorders()
        {
            u1 = 1, u2 = 2, u3 = 0,
            d1 = 7, d2 = 8, d3 = 6,
            l1 = 1, l2 = 4, l3 = 7,
            r1 = 0, r2 = 3, r3 = 6
        };
        tb[6] = new TransformBorders()
        {
            u1 = 5, u2 = 3, u3 = 4,
            d1 = 2, d2 = 0, d3 = 1,
            l1 = 5, l2 = 8, l3 = 2,
            r1 = 4, r2 = 7, r3 = 1
        };
        tb[7] = new TransformBorders()
        {
            u1 = 3, u2 = 4, u3 = 5,
            d1 = 0, d2 = 1, d3 = 2,
            l1 = 3, l2 = 6, l3 = 0,
            r1 = 5, r2 = 8, r3 = 2
        };
        tb[8] = new TransformBorders()
        {
            u1 = 4, u2 = 5, u3 = 3,
            d1 = 1, d2 = 2, d3 = 0,
            l1 = 4, l2 = 7, l3 = 1,
            r1 = 3, r2 = 6, r3 = 0
        };
        x = new Vector3(offset, 0, 0);
        y = new Vector3(0, offset, 0);
        Vector3 rand = new Vector3(Random.Range(-45, 45), Random.Range(-45, 45), 0) * pixelSize;
        for (int i = 0; i < l; i++)
        {
            trs[i].localPosition += rand;
        }
    }

    public void FixedUpdate()
    {
        if (!isMovement) { return; }
        for (int i = 0; i < l; i++)
        {
            trs[i].localPosition += (Vector3)speed;
        }
        Vector2 pos = trs[activeImage].localPosition;
        if (pos.x > 90f)
        {
            trs[tb[activeImage].r1].localPosition -= x;
            trs[tb[activeImage].r2].localPosition -= x;
            trs[tb[activeImage].r3].localPosition -= x;
            activeImage = tb[activeImage].l2;
        }
        else if (pos.x < -90f)
        {
            trs[tb[activeImage].l1].localPosition += x;
            trs[tb[activeImage].l2].localPosition += x;
            trs[tb[activeImage].l3].localPosition += x;
            activeImage = tb[activeImage].r2;
        }
        if (pos.y > 90f)
        {
            trs[tb[activeImage].u1].localPosition -= y;
            trs[tb[activeImage].u2].localPosition -= y;
            trs[tb[activeImage].u3].localPosition -= y;
            activeImage = tb[activeImage].d2;
        }
        else if (pos.y < -90f)
        {
            trs[tb[activeImage].d1].localPosition += y;
            trs[tb[activeImage].d1].localPosition += y;
            trs[tb[activeImage].d1].localPosition += y;
            activeImage = tb[activeImage].u2;
        }
    }

    public class TransformBorders
    {
        public int u1, u2, u3;
        public int d1, d2, d3;
        public int l1, l2, l3;
        public int r1, r2, r3;
    }
}