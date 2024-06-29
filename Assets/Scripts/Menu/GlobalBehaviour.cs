using System.Collections;
using UnityEngine;

public class GlobalBehaviour : MonoBehaviour
{
    public enum GunType { Standard = 0, }
    public static readonly Vector2[][] posBullets = new Vector2[][] 
    {
        new Vector2[] { new Vector2(-0.3f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0.3f, 0.5f) }
    };
    public static Vector2[] PositionsBulletsGun(GunType gunType)
    {
        return posBullets[(int)gunType];
    }
    private Color mainColor = new Color(1f, 0.2f, 0.3f);
    public Color MainColor(float a = 1)
    {
        return new Color(mainColor.r, mainColor.g, mainColor.b, a);
    }
    public void DestroyObj(Transform tr)
    {
        Destroy(tr.gameObject);
    }
}