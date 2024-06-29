using UnityEngine.Experimental.Rendering.Universal;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public DataGame data;
    public Generate generate;
    public InputController input;
    public Sprite musicOn;
    public Sprite musicOff;
    public Sprite soundOn;
    public Sprite soundOff;
    public Material standard;
    public Material standardLit;
    Slider sliderMusic;
    float lastMusic = 0;
    Slider sliderSound;
    float lastSound = 0;
    Image imMusic;
    Image imSound;

    Text textControlType;
    Image handImage;
    Image handLeft;
    Image handRight;
    Image notificationsOn;
    Image notificationsOff;
    Text textOrientation;

    public void Awake()
    {
        Transform tr = transform.GetChild(6);
        sliderMusic = tr.GetChild(0).GetComponent<Slider>();
        imMusic = tr.GetChild(0).GetChild(0).GetComponent<Image>();
        sliderSound = tr.GetChild(1).GetComponent<Slider>();
        imSound = tr.GetChild(1).GetChild(0).GetComponent<Image>();
        lastMusic = sliderMusic.value;
        lastSound = sliderSound.value;

        textControlType = tr.GetChild(2).GetChild(0).GetComponent<Text>();
        string control_type = PlayerPrefs.GetString("control-type");
        input.isFinger = control_type == "finger";
        input.isJoystick = control_type == "joystick";
        if (control_type == "finger")
        {
            PlayerPrefs.SetString("control-type", "finger");
            textControlType.text = "FINGER";
        }
        else if (control_type == "joystick")
        {
            PlayerPrefs.SetString("control-type", "joystick");
            textControlType.text = "JOYSTICK";
        }
        else
        {
            PlayerPrefs.SetString("control-type", "mouse");
            textControlType.text = "MOUSE";
        }

        handImage = tr.GetChild(3).GetChild(0).GetComponent<Image>();
        handLeft = tr.GetChild(3).GetChild(4).GetComponent<Image>();
        handRight = tr.GetChild(3).GetChild(5).GetComponent<Image>();
        bool isRight = PlayerPrefs.GetString("hand") == "right";
        if (isRight)
        {
            PlayerPrefs.SetString("hand", "right");
            handImage.rectTransform.localScale = new Vector3(1, 1, 1);
            handLeft.enabled = false;
            handRight.enabled = true;
        }
        else
        {
            PlayerPrefs.SetString("hand", "left");
            handImage.rectTransform.localScale = new Vector3(-1, 1, 1);
            handRight.enabled = false;
            handLeft.enabled = true;
        }

        notificationsOn = tr.GetChild(4).GetChild(4).GetComponent<Image>();
        notificationsOff = tr.GetChild(4).GetChild(5).GetComponent<Image>();
        bool isOn = PlayerPrefs.GetString("notifications") == "on";
        if (isOn)
        {
            PlayerPrefs.SetString("notifications", "on");
            notificationsOff.enabled = false;
            notificationsOn.enabled = true;
        }
        else
        {
            PlayerPrefs.SetString("notifications", "off");
            notificationsOn.enabled = false;
            notificationsOff.enabled = true;
        }

        textOrientation = tr.GetChild(5).GetChild(0).GetComponent<Text>();
        GraphicsSet();
    }

    #region Music&Sound
    public void ActMusic()
    {
        PlayerPrefs.SetFloat("music", sliderMusic.value);
        print("m");
    }
    public void ActSound()
    {
        PlayerPrefs.SetFloat("sound", sliderSound.value);
        print("s");
    }

    public void ButMusic()
    {
        if (sliderMusic.value == 0f)
        {
            if (lastMusic < 0.05f) { lastMusic = 1f; }
            sliderMusic.value = lastMusic;
            PlayerPrefs.SetFloat("music", lastMusic);
        }
        else
        {
            lastMusic = sliderMusic.value;
            sliderMusic.value = 0f;
            PlayerPrefs.SetFloat("music", 0f);
        }
    }
    public void ButSound()
    {
        if (sliderSound.value == 0f)
        {
            if (lastSound < 0.05f) { lastSound = 1f; }
            sliderSound.value = lastSound;
            PlayerPrefs.SetFloat("sound", lastSound);
        }
        else
        {
            lastSound = sliderSound.value;
            sliderSound.value = 0f;
            PlayerPrefs.SetFloat("sound", 0f);
        }
    }
    #endregion

    public void ButControlType()
    {
        string control_type = PlayerPrefs.GetString("control-type");
        if (control_type == "finger")
        {
            PlayerPrefs.SetString("control-type", "joystick");
            textControlType.text = "JOYSTICK";
        }
        else if (control_type == "joystick")
        {
            PlayerPrefs.SetString("control-type", "mouse");
            textControlType.text = "MOUSE";
        }
        else
        {
            PlayerPrefs.SetString("control-type", "finger");
            textControlType.text = "FINGER";
        }
        control_type = PlayerPrefs.GetString("control-type");
        input.isJoystick = control_type == "joystick";
        input.isFinger = control_type == "finger";
        return;
    }

    public void ButHand()
    {
        bool isRight = PlayerPrefs.GetString("hand") == "right";
        if (isRight)
        {
            PlayerPrefs.SetString("hand", "left");
            handImage.rectTransform.localScale = new Vector3(-1, 1, 1);
            handRight.enabled = false;
            handLeft.enabled = true;
        }
        else
        {
            PlayerPrefs.SetString("hand", "right");
            handImage.rectTransform.localScale = new Vector3(1, 1, 1);
            handLeft.enabled = false;
            handRight.enabled = true;
        }
        return;
    }

    public void ButNotifications()
    {
        bool isOn = PlayerPrefs.GetString("notifications") == "on";
        if (isOn)
        {
            PlayerPrefs.SetString("notifications", "off");
            notificationsOn.enabled = false;
            notificationsOff.enabled = true;
        }
        else
        {
            PlayerPrefs.SetString("notifications", "on");
            notificationsOff.enabled = false;
            notificationsOn.enabled = true;
        }
        return;
    }

    public void ButGraphics()
    {
        switch (PlayerPrefs.GetString("graphics"))
        {
            case "low":
                PlayerPrefs.SetString("graphics", "medium");
                break;
            case "medium":
                PlayerPrefs.SetString("graphics", "high");
                break;
            case "high":
                PlayerPrefs.SetString("graphics", "extreme");
                break;
            case "extreme":
                PlayerPrefs.SetString("graphics", "low");
                break;
        }
        GraphicsSet();
        return;
    }

    public void GraphicsSet()
    {
        switch (PlayerPrefs.GetString("graphics"))
        {
            case "low":
                QualitySettings.SetQualityLevel(0);
                break;
            case "medium":
                QualitySettings.SetQualityLevel(1);
                break;
            case "high":
                QualitySettings.SetQualityLevel(2);
                break;
            case "extreme":
                QualitySettings.SetQualityLevel(3);
                break;
        }
        textOrientation.text = PlayerPrefs.GetString("graphics");
        bool isLight = PlayerPrefs.GetString("graphics") != "low";
        bool isShadow = PlayerPrefs.GetString("graphics") != "medium" && isLight;
        Material material = standard; if (isLight) { material = standardLit; }
        for (int i = 0; i < data.assets.Length; i++)
        {
            if (data.assets[i].obj.GetComponent<SpriteRenderer>() && data.assets[i].isSpriteRenderer)
            {
                data.assets[i].obj.GetComponent<SpriteRenderer>().material = material;
            }
            if (data.assets[i].obj.GetComponent<ShadowCaster2D>() && data.assets[i].isShadowCaster2D)
            {
                data.assets[i].obj.GetComponent<ShadowCaster2D>().enabled = isShadow;
            }
            if (data.assets[i].obj.GetComponent<Light2D>() && data.assets[i].isLight2D)
            {
                data.assets[i].obj.GetComponent<Light2D>().enabled = isLight;
            }
        }
        generate.GraphicsSet(isShadow);
    }
}
[System.Serializable]
public class GraphicAsset
{
    public GameObject obj;
    public bool isSpriteRenderer = true;
    public bool isShadowCaster2D = true;
    public bool isLight2D = true;
}