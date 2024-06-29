using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayWindow : MonoBehaviour
{
    public BaseLevelEditor editor;
    public RectTransform playWindow;
    public RectTransform levelWindow;
    private bool isActivePlayWindow = true;

    public RectTransform scrollView;
    public ScrollRect scrollRect;
    public RectTransform contentTimeline;
    public RectTransform contentMarkers;

    public Sprite play, pause;
    private bool isPlay = false;
    public Sprite select, handOff, handOn, cut, copy;
    private int countTools = 1;
    private float camAspect;
    public Sprite folderOpen, folderNew;

    private Text timerTimeLine, timeScaleTimeline, lvlManagerTimeline;
    private But[] butsIconTimeline;
    private bool[] butsActiveTimeline;
    private const int countButsTimeline = 16;

    private Text timerMarkers, timerSecMarkers, timeScaleMarkers, lvlManagerMarkers;
    private But[] butsIconMarkers;
    private bool[] butsActiveMarkers;
    private const int countButsMarkers = 16;

    public RectTransform buts;
    private LVLManagerWindow[] windows;
    private int activeWindows = 0;
    private const int countWindows = 5;

    public void Awake()
    {
        camAspect = editor.GetAspect();
        timerTimeLine = contentTimeline.GetChild(0).GetChild(0).GetComponent<Text>();
        timerMarkers = contentMarkers.GetChild(0).GetChild(0).GetComponent<Text>();
        timerSecMarkers = contentMarkers.GetChild(1).GetChild(0).GetComponent<Text>();
        timeScaleTimeline = contentTimeline.GetChild(13).GetChild(0).GetComponent<Text>();
        timeScaleMarkers = contentMarkers.GetChild(13).GetChild(0).GetComponent<Text>();
        lvlManagerTimeline = contentTimeline.GetChild(15).GetChild(0).GetComponent<Text>();
        lvlManagerMarkers = contentMarkers.GetChild(15).GetChild(0).GetComponent<Text>();
        levelWindow.gameObject.SetActive(false);
        playWindow.gameObject.SetActive(true);

        butsIconTimeline = new But[countButsTimeline]
        {
            new But(contentTimeline.GetChild(0).GetComponent<Image>(), true),
            new But(contentTimeline.GetChild(1).GetComponent<Image>(), true, contentTimeline.GetChild(1).GetChild(0).GetComponent<Image>()),
            new But(contentTimeline.GetChild(2).GetComponent<Image>(), true, contentTimeline.GetChild(2).GetChild(0).GetComponent<Image>(), contentTimeline.GetChild(2).GetChild(1).GetComponent<Image>()),
            new But(contentTimeline.GetChild(3).GetComponent<Image>(), true, contentTimeline.GetChild(3).GetChild(0).GetComponent<Image>(), contentTimeline.GetChild(3).GetChild(1).GetComponent<Image>()),
            new But(contentTimeline.GetChild(4).GetComponent<Image>(), true, contentTimeline.GetChild(4).GetChild(0).GetComponent<Image>(), contentTimeline.GetChild(4).GetChild(1).GetComponent<Image>()),
            new But(contentTimeline.GetChild(5).GetComponent<Image>(), true, contentTimeline.GetChild(5).GetChild(0).GetComponent<Image>()),
            new But(contentTimeline.GetChild(6).GetComponent<Image>(), true, contentTimeline.GetChild(6).GetChild(0).GetComponent<Image>()),
            new But(contentTimeline.GetChild(7).GetComponent<Image>(), true, contentTimeline.GetChild(7).GetChild(0).GetComponent<Image>()),
            new But(contentTimeline.GetChild(8).GetComponent<Image>(), true, contentTimeline.GetChild(8).GetChild(0).GetComponent<Image>()),
            new But(contentTimeline.GetChild(9).GetComponent<Image>(), true, contentTimeline.GetChild(9).GetChild(0).GetComponent<Image>()),
            new But(contentTimeline.GetChild(10).GetComponent<Image>(), true, contentTimeline.GetChild(10).GetChild(0).GetComponent<Image>()),
            new But(contentTimeline.GetChild(11).GetComponent<Image>(), true, contentTimeline.GetChild(11).GetChild(0).GetComponent<Image>()),
            new But(contentTimeline.GetChild(12).GetComponent<Image>(), true, contentTimeline.GetChild(12).GetChild(0).GetComponent<Image>()),
            new But(contentTimeline.GetChild(13).GetComponent<Image>(), false, contentTimeline.GetChild(13).GetChild(1).GetComponent<Image>(), contentTimeline.GetChild(13).GetChild(2).GetComponent<Image>()),
            new But(contentTimeline.GetChild(14).GetComponent<Image>(), true, contentTimeline.GetChild(14).GetChild(0).GetComponent<Image>()),
            new But(contentTimeline.GetChild(15).GetComponent<Image>(), true, contentTimeline.GetChild(15).GetChild(1).GetComponent<Image>(), contentTimeline.GetChild(15).GetChild(2).GetComponent<Image>())
        };
        butsActiveTimeline = new bool[countButsTimeline]
        {
            true, // Timer
            true, // Play
            true, // Plus Object
            true, // Plus Marker
            true, // Plus Checkpoint
            false, // Delete
            true, // Tools Timeline (Select/Hand/Cut/Copy)
            false, // Undo
            false, // Redo
            true, // Find
            false, // Replace
            false, // Paste
            true, // Threads
            true, // Time scale made
            true, // Save
            true // LVL Manager
        };

        butsIconMarkers = new But[countButsMarkers]
        {
            new But(contentMarkers.GetChild(0).GetComponent<Image>(), true),
            new But(contentMarkers.GetChild(1).GetComponent<Image>(), true),
            new But(contentTimeline.GetChild(2).GetComponent<Image>(), true, contentTimeline.GetChild(2).GetChild(0).GetComponent<Image>()),
            new But(contentMarkers.GetChild(3).GetComponent<Image>(), true, contentMarkers.GetChild(3).GetChild(0).GetComponent<Image>(), contentMarkers.GetChild(3).GetChild(1).GetComponent<Image>()),
            new But(contentMarkers.GetChild(4).GetComponent<Image>(), true, contentMarkers.GetChild(4).GetChild(0).GetComponent<Image>(), contentMarkers.GetChild(4).GetChild(1).GetComponent<Image>()),
            new But(contentMarkers.GetChild(5).GetComponent<Image>(), true, contentMarkers.GetChild(5).GetChild(0).GetComponent<Image>(), contentMarkers.GetChild(5).GetChild(1).GetComponent<Image>()),
            new But(contentMarkers.GetChild(6).GetComponent<Image>(), true, contentMarkers.GetChild(6).GetChild(0).GetComponent<Image>(), contentMarkers.GetChild(6).GetChild(1).GetComponent<Image>()),
            new But(contentMarkers.GetChild(7).GetComponent<Image>(), true, contentMarkers.GetChild(7).GetChild(0).GetComponent<Image>()),
            new But(contentMarkers.GetChild(8).GetComponent<Image>(), true, contentMarkers.GetChild(8).GetChild(0).GetComponent<Image>()),
            new But(contentMarkers.GetChild(9).GetComponent<Image>(), true, contentMarkers.GetChild(9).GetChild(0).GetComponent<Image>()),
            new But(contentMarkers.GetChild(10).GetComponent<Image>(), true, contentMarkers.GetChild(10).GetChild(0).GetComponent<Image>()),
            new But(contentMarkers.GetChild(11).GetComponent<Image>(), true, contentMarkers.GetChild(11).GetChild(0).GetComponent<Image>()),
            new But(contentMarkers.GetChild(12).GetComponent<Image>(), true, contentMarkers.GetChild(12).GetChild(0).GetComponent<Image>()),
            new But(contentMarkers.GetChild(13).GetComponent<Image>(), false, contentMarkers.GetChild(13).GetChild(1).GetComponent<Image>(), contentMarkers.GetChild(13).GetChild(2).GetComponent<Image>()),
            new But(contentMarkers.GetChild(14).GetComponent<Image>(), true, contentMarkers.GetChild(14).GetChild(0).GetComponent<Image>()),
            new But(contentMarkers.GetChild(15).GetComponent<Image>(), true, contentMarkers.GetChild(15).GetChild(1).GetComponent<Image>(), contentMarkers.GetChild(15).GetChild(2).GetComponent<Image>())
        };
        butsActiveMarkers = new bool[countButsMarkers]
        {
            true, // Timer
            true, // Timer Sec Local
            true, // Play
            true, // Plus Pos Marker
            true, // Plus Rot Marker
            true, // Plus Scale Marker
            false, // Plus Color Marker
            true, // Delete
            false, // Cut
            false, // Copy
            true, // Paste
            false, // Undo
            false, // Redo
            true, // Time scale made
            true, // Save
            true // LVL Manager
        };

        windows = new LVLManagerWindow[countWindows]
        {
            new LVLManagerWindow(buts.GetChild(0).GetComponent<Image>(), buts.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(), buts.GetChild(0).GetChild(1).GetComponent<Image>(), levelWindow.GetChild(1).gameObject),
            new LVLManagerWindow(buts.GetChild(1).GetComponent<Image>(), buts.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>(), buts.GetChild(1).GetChild(1).GetComponent<Image>(), levelWindow.GetChild(2).gameObject),
            new LVLManagerWindow(buts.GetChild(2).GetComponent<Image>(), buts.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>(), buts.GetChild(2).GetChild(1).GetComponent<Image>(), levelWindow.GetChild(3).gameObject),
            new LVLManagerWindow(buts.GetChild(3).GetComponent<Image>(), buts.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>(), buts.GetChild(3).GetChild(1).GetComponent<Image>(), levelWindow.GetChild(4).gameObject),
            new LVLManagerWindow(buts.GetChild(4).GetComponent<Image>(), buts.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>(), buts.GetChild(4).GetChild(1).GetComponent<Image>(), levelWindow.GetChild(4).gameObject)
        };

        //Graphics Update
        levelWindow.sizeDelta = new Vector2(540f * camAspect, 540f);
        playWindow.sizeDelta = new Vector2(540f * camAspect, 540f);
        scrollView.sizeDelta = new Vector2(540f * camAspect, 100f);
        UpdateButs(countButsTimeline, countButsMarkers);
        StartButsLVLManager();
    }

    public void UpdateButs(int idTimeline, int idMarkers)
    {
        if (idTimeline == countButsTimeline)
        {
            for (int i = 0; i < countButsTimeline; i++)
            {
                if (butsActiveTimeline[i])
                { butsIconTimeline[i].UpdateColor(BaseLevelEditor.butEnableBG(), BaseLevelEditor.butEnable(), butsActiveTimeline[i]); }
                else { butsIconTimeline[i].UpdateColor(BaseLevelEditor.butDisableBG(), BaseLevelEditor.butDisable(), butsActiveTimeline[i]); }
            }
        }
        else
        {
            if (butsActiveTimeline[idTimeline])
            { butsIconTimeline[idTimeline].UpdateColor(BaseLevelEditor.butEnableBG(), BaseLevelEditor.butEnable(), butsActiveTimeline[idTimeline]); }
            else { butsIconTimeline[idTimeline].UpdateColor(BaseLevelEditor.butDisableBG(), BaseLevelEditor.butDisable(), butsActiveTimeline[idTimeline]); }
        }

        if (idMarkers == countButsMarkers)
        {
            for (int i = 0; i < countButsMarkers; i++)
            {
                if (butsActiveMarkers[i])
                { butsIconMarkers[i].UpdateColor(BaseLevelEditor.butEnableBG(), BaseLevelEditor.butEnable(), butsActiveMarkers[i]); }
                else { butsIconMarkers[i].UpdateColor(BaseLevelEditor.butDisableBG(), BaseLevelEditor.butDisable(), butsActiveMarkers[i]); }
            }
        }
        else
        {
            if (butsActiveMarkers[idMarkers])
            { butsIconMarkers[idMarkers].UpdateColor(BaseLevelEditor.butEnableBG(), BaseLevelEditor.butEnable(), butsActiveMarkers[idMarkers]); }
            else { butsIconMarkers[idMarkers].UpdateColor(BaseLevelEditor.butDisableBG(), BaseLevelEditor.butDisable(), butsActiveMarkers[idMarkers]); }
        }
        return;
    }
    public void StartButsLVLManager()
    {
        for (int i = 0; i < countWindows; i++)
        {
            windows[i].UpdateColor(BaseLevelEditor.butNotSelectBG(), BaseLevelEditor.butDisable(), false);
        }
        windows[activeWindows].UpdateColor(BaseLevelEditor.butEnableBG(), BaseLevelEditor.butEnable(), true);
    }
    public void UpdateButsLVLManager(int idNext)
    {
        windows[activeWindows].UpdateColor(BaseLevelEditor.butNotSelectBG(), BaseLevelEditor.butDisable(), false);
        windows[idNext].UpdateColor(BaseLevelEditor.butEnableBG(), BaseLevelEditor.butEnable(), true);
        activeWindows = idNext;
    }

    public void Timer(float sec)
    {
        int min = 0; string timerText = string.Empty;
        int frames = (int)((sec - (int)sec) * 60f);
        while (sec >= 60f) { min++; sec -= 60; }

        sec = (int)sec; if (frames < 0) 
        { frames = 60 + frames; sec -= 1; }

        if (min < 10) { timerText += "0" + min + ":"; }
        else { timerText += min + ":"; }

        if (sec < 10f) { timerText += "0" + sec.ToString("0"); }
        else { timerText += sec.ToString("0"); }

        if (frames < 10) { timerText += ".0" + frames; }
        else { timerText += "." + frames; }

        timerTimeLine.text = timerText;
        timerMarkers.text = timerText;
        timerSecMarkers.text = sec + "." + frames;
    }

    public void EndGame()
    {
        butsIconTimeline[1].butIcons[0].sprite = play;
        butsIconMarkers[2].butIcons[0].sprite = play;
        editor.timeline.Pause();
        isPlay = false;
        Timer(0);
    }

    public void ButtonsTimeline(int id)
    {
        switch (id)
        {
            case 0:// Timer
                editor.timeline.WatchSecMarker();
                break;
            case 1:// Play
                if (isPlay)
                {
                    butsIconTimeline[1].butIcons[0].sprite = play;
                    editor.timeline.Pause();
                    isPlay = false;
                }
                else
                {
                    butsIconTimeline[1].butIcons[0].sprite = pause;
                    editor.timeline.Play();
                    isPlay = true;
                }
                break;
            case 2:// Plus Object
                break;
            case 3:// Plus Marker
                break;
            case 4:// Plus Checkpoint
                break;
            case 5:// Delete
                break;
            case 6:// Tools Timeline (Select/Hand/Cut/Copy)
                switch (countTools)
                {
                    case 1:
                        countTools = 2;
                        butsIconTimeline[6].butIcons[0].sprite = handOff;
                        break;
                    case 2:
                        countTools = 3;
                        butsIconTimeline[6].butIcons[0].sprite = cut;
                        break;
                    case 3:
                        countTools = 4;
                        butsIconTimeline[6].butIcons[0].sprite = copy;
                        break;
                    case 4:
                        countTools = 1;
                        butsIconTimeline[6].butIcons[0].sprite = select;
                        break;
                }
                break;
            case 7:// Undo
                break;
            case 8:// Redo
                break;
            case 9:// Find
                break;
            case 10:// Replace
                break;
            case 11:// Paste
                break;
            case 12:// Threads
                break;
            case 15:// Timescale left 12
                if (editor.timeScale > -1f)
                {
                    editor.timeScale -= 0.1f;
                    timeScaleTimeline.text = editor.timeScale.ToString("0.0");
                    timeScaleMarkers.text = editor.timeScale.ToString("0.0");
                }
                break;
            case 17:// Timescale text 12
                editor.timeScale = 1f;
                timeScaleTimeline.text = editor.timeScale.ToString("0.0");
                timeScaleMarkers.text = editor.timeScale.ToString("0.0");
                break;
            case 16:// Timescale right 12
                if (editor.timeScale < 3f)
                {
                    editor.timeScale += 0.1f;
                    timeScaleTimeline.text = editor.timeScale.ToString("0.0");
                    timeScaleMarkers.text = editor.timeScale.ToString("0.0");
                }
                break;
            case 13:// Save 13
                break;
            case 14:// LVL Manager 14
                if (isActivePlayWindow)
                {
                    lvlManagerMarkers.text = "GM";
                    lvlManagerTimeline.text = "GM";
                    butsIconMarkers[15].butIcons[0].sprite = pause;
                    butsIconMarkers[15].butIcons[1].sprite = play;
                    butsIconTimeline[15].butIcons[0].sprite = pause;
                    butsIconTimeline[15].butIcons[1].sprite = play;
                    levelWindow.gameObject.SetActive(true);
                    playWindow.gameObject.SetActive(false);
                    isActivePlayWindow = false;
                }
                else
                {
                    lvlManagerMarkers.text = "LVL";
                    lvlManagerTimeline.text = "LVL";
                    butsIconMarkers[15].butIcons[0].sprite = folderNew;
                    butsIconMarkers[15].butIcons[1].sprite = folderOpen;
                    butsIconTimeline[15].butIcons[0].sprite = folderNew;
                    butsIconTimeline[15].butIcons[1].sprite = folderOpen;
                    levelWindow.gameObject.SetActive(false);
                    playWindow.gameObject.SetActive(true);
                    isActivePlayWindow = true;
                }
                break;
        }
    }
    public void ButtonsMarkers(int id)
    {
        switch (id)
        {
            case 0:// Timer
            case 1:// Timer Sec Local
                editor.timeline.WatchSecMarker();
                break;
            case 2:// Play
                if (isPlay)
                {
                    butsIconMarkers[2].butIcons[0].sprite = play;
                    editor.timeline.Pause();
                    isPlay = false;
                }
                else
                {
                    butsIconMarkers[2].butIcons[0].sprite = pause;
                    editor.timeline.Play();
                    isPlay = true;
                }
                break;
            case 3:// Plus Pos Marker
                break;
            case 4:// Plus Rot Marker
                break;
            case 5:// Plus Scale Marker
                break;
            case 6:// Plus Color Marker
                break;
            case 7:// Delete
                break;
            case 8:// Cut
                break;
            case 9:// Copy
                break;
            case 10:// Paste
                break;
            case 11:// Undo
                break;
            case 12:// Redo
                break;
            case 15:// Timescale left 15
                if (editor.timeScale > -1f)
                {
                    editor.timeScale -= 0.1f;
                    timeScaleTimeline.text = editor.timeScale.ToString("0.0");
                    timeScaleMarkers.text = editor.timeScale.ToString("0.0");
                }
                break;
            case 17:// Timescale text 17
                editor.timeScale = 1f;
                timeScaleTimeline.text = editor.timeScale.ToString("0.0");
                timeScaleMarkers.text = editor.timeScale.ToString("0.0");
                break;
            case 16:// Timescale right 16
                if (editor.timeScale < 3f)
                {
                    editor.timeScale += 0.1f;
                    timeScaleTimeline.text = editor.timeScale.ToString("0.0");
                    timeScaleMarkers.text = editor.timeScale.ToString("0.0");
                }
                break;
            case 13:// Save 13
                break;
            case 14:// LVL Manager 14
                if (isActivePlayWindow)
                {
                    lvlManagerMarkers.text = "GM";
                    lvlManagerTimeline.text = "GM";
                    butsIconMarkers[15].butIcons[0].sprite = pause;
                    butsIconMarkers[15].butIcons[1].sprite = play;
                    butsIconTimeline[15].butIcons[0].sprite = pause;
                    butsIconTimeline[15].butIcons[1].sprite = play;
                    levelWindow.gameObject.SetActive(true);
                    playWindow.gameObject.SetActive(false);
                    isActivePlayWindow = false;
                }
                else
                {
                    lvlManagerMarkers.text = "LVL";
                    lvlManagerTimeline.text = "LVL";
                    butsIconMarkers[15].butIcons[0].sprite = folderNew;
                    butsIconMarkers[15].butIcons[1].sprite = folderOpen;
                    butsIconTimeline[15].butIcons[0].sprite = folderNew;
                    butsIconTimeline[15].butIcons[1].sprite = folderOpen;
                    levelWindow.gameObject.SetActive(false);
                    playWindow.gameObject.SetActive(true);
                    isActivePlayWindow = true;
                }
                break;
        }
    }

    public void LevelManagerButtons(int id)
    {
        UpdateButsLVLManager(id);
        switch (id)
        {
            case 0:// Level Data
                break;
            case 1:// New Level
                break;
            case 2:// Open level
                break;
            case 3:// Save
                break;
            case 4:// Quit
                break;
        }
    }
}

public class But
{
    public Image butBG;
    public Image[] butIcons;
    private int length = 0;
    private bool isButsInBG = true;

    public void UpdateColor(Color BG, Color icon, bool isAct)
    {
        butBG.color = BG;
        if (isButsInBG) { butBG.GetComponent<Button>().enabled = isAct; }
        for (int i = 0; i < length; i++)
        {
            butIcons[i].color = icon;
            if (!isButsInBG) { butIcons[i].GetComponent<Button>().enabled = isAct; }
        }
    }

    public But(Image BG, bool isButsInBG, params Image[] list)
    {
        butBG = BG;
        butIcons = list;
        length = list.Length;
        this.isButsInBG = isButsInBG;
    }

    public But(Image BG)
    {
        butBG = BG;
    }
}

public class LVLManagerWindow
{
    public Image butBG;
    public TextMeshProUGUI butText;
    public Image butLogo;
    public GameObject window;

    public LVLManagerWindow(Image butBG, TextMeshProUGUI butText, Image butLogo, GameObject window)
    {
        this.butBG = butBG;
        this.butText = butText;
        this.butLogo = butLogo;
        this.window = window;
    }

    public void UpdateColor(Color BG, Color icon, bool isOn)
    {
        butBG.color = BG;
        butText.color = icon;
        butLogo.color = icon;
        window.SetActive(isOn);
    }
}