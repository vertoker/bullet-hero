using System.Collections;
using UnityEngine;

public class Management : GlobalBehaviour
{
    public ParticleSystem effectDamage;
    public GameObject effectCollectCoin;
    public void Awake()
    {
        if (!PlayerPrefs.HasKey("version"))
        {
            PlayerPrefs.SetInt("version", 1);
            PlayerPrefs.SetInt("money", 0);
            PlayerPrefs.SetInt("level", 1);

            /// b - blocked, l - locked, u - unlocked, s - select
            PlayerPrefs.SetString("skins", "slllllllbb");

            PlayerPrefs.SetFloat("music", 1f);
            PlayerPrefs.SetFloat("sound", 1f);
            PlayerPrefs.SetString("control-type", "finger");/// finger, joystick, mouse
            PlayerPrefs.SetString("hand", "right");/// left, right
            PlayerPrefs.SetString("graphics", "low");/// low, medium, high
            PlayerPrefs.SetString("notifications", "on");/// on, off
            PlayerPrefs.SetString("orientation", "horizontal");/// horizontal, vertical
            PlayerPrefs.SetString("language", "english");/// on, off
        }
    }

    public void AddMoney(Vector2 pos)
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 1);
        GameObject obj = Instantiate(effectCollectCoin, pos, Quaternion.identity);
        obj.GetComponent<ParticleSystem>().Play();
        Destroy(obj, 1f);
    }

    public void DamagePlayer(Vector2 pos)
    {
        effectDamage.transform.localPosition = pos;
        effectDamage.Play();
    }
}