using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Menu : GlobalBehaviour
{
    public DataGame data;
    public Player player;
    public Generate generate;
    InputController controller;
    GameObject game, pause, lose;
    GameObject menu, stats, skins;
    GameObject settings, authors, reset;
    GameObject endStats;

    SwipeReset swipe;
    Skins skinsScript;
    GameObject pauseBut, input;
    Joystick joyStick;
    Image[] healthPoints;
    Image progressBar;
    Image blackScreen;
    Text title, musician;
    AudioSource audioSource;
    RectTransform titleTR, musicianTR;

    const float speedBlackScreen = 0.5f;
    float lengthAudioClip = 0;
    Coroutine progressBarUpdate, mainEnd, deathFadeOut;

    void Awake()
    {
        Transform tr = GetComponent<Transform>();
        game = tr.GetChild(0).gameObject;
        pause = tr.GetChild(1).gameObject;
        lose = tr.GetChild(2).gameObject;
        menu = tr.GetChild(3).gameObject;
        stats = tr.GetChild(4).gameObject;
        skins = tr.GetChild(5).gameObject;
        settings = tr.GetChild(6).gameObject;
        authors = tr.GetChild(7).gameObject;
        reset = tr.GetChild(8).gameObject;
        endStats = tr.GetChild(9).gameObject;

        skinsScript = GetComponent<Skins>();
        audioSource = data.GetComponent<AudioSource>();
        input = tr.GetChild(0).GetChild(2).gameObject;
        controller = input.GetComponent<InputController>();
        joyStick = tr.GetChild(0).GetChild(3).GetComponent<Joystick>();
        pauseBut = tr.GetChild(0).GetChild(4).gameObject;
        title = tr.GetChild(0).GetChild(5).GetComponent<Text>();
        musician = tr.GetChild(0).GetChild(6).GetComponent<Text>();
        titleTR = tr.GetChild(0).GetChild(5).GetComponent<RectTransform>();
        musicianTR = tr.GetChild(0).GetChild(6).GetComponent<RectTransform>();
        swipe = tr.GetChild(8).GetChild(0).GetComponent<SwipeReset>();
        healthPoints = game.transform.GetChild(0).GetComponentsInChildren<Image>();
        progressBar = game.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        blackScreen = tr.GetChild(10).GetComponent<Image>();
        data.UpdateMoney();

        Vector2 aspect = new Vector2(1080f * generate.cam.cam.aspect, 1080f);
        game.GetComponent<RectTransform>().sizeDelta = aspect;
        pause.GetComponent<RectTransform>().sizeDelta = aspect;
        lose.GetComponent<RectTransform>().sizeDelta = aspect;
        menu.GetComponent<RectTransform>().sizeDelta = aspect;
        stats.GetComponent<RectTransform>().sizeDelta = aspect;
        skins.GetComponent<RectTransform>().sizeDelta = aspect;
        settings.GetComponent<RectTransform>().sizeDelta = aspect;
        authors.GetComponent<RectTransform>().sizeDelta = aspect;
        reset.GetComponent<RectTransform>().sizeDelta = aspect;
        blackScreen.GetComponent<RectTransform>().sizeDelta = aspect;

        game.SetActive(false);
        pause.SetActive(false);
        lose.SetActive(false);
        menu.SetActive(true);
        stats.SetActive(false);
        skins.SetActive(false);
        settings.SetActive(false);
        authors.SetActive(false);
        reset.SetActive(false);
        endStats.SetActive(false);

        pauseBut.SetActive(true);
        Color c = blackScreen.color;
        blackScreen.color = new Color(c.r, c.g, c.b, 1f);
    }

    void Start()
    {
        StartCoroutine(AnimOpen());
    }

    public void Buttons(int id)
    {
        switch (id)
        {
            case 1://Open stats
                menu.SetActive(false);
                stats.SetActive(true);
                break;
            case 2://Close stats
                stats.SetActive(false);
                menu.SetActive(true);
                break;
            case 3://Open skins
                menu.SetActive(false);
                skinsScript.Open();
                skins.SetActive(true);
                break;
            case 4://Close skins
                skins.SetActive(false);
                skinsScript.Close();
                menu.SetActive(true);
                break;
            case 5://Open settings
                menu.SetActive(false);
                settings.SetActive(true);
                break;
            case 6://Close settings
                settings.SetActive(false);
                menu.SetActive(true);
                break;
            case 7://Open authors
                settings.SetActive(false);
                authors.SetActive(true);
                break;
            case 8://Close authors
                authors.SetActive(false);
                settings.SetActive(true);
                break;
            case 9://Start reset
                settings.SetActive(false);
                reset.SetActive(true);
                swipe.StartReset();
                break;
            case 10://End reset
                swipe.ExitReset();
                reset.SetActive(false);
                settings.SetActive(true);
                break;
        }
    }

    public void GameButtons(int id)
    {
        switch (id)
        {
            case 1://Start game
                player.Play();
                UpdateHealthPoints();
                generate.StartGame();
                menu.SetActive(false);
                pauseBut.SetActive(true);
                game.SetActive(true);
                input.GetComponent<Image>().enabled = !controller.isJoystick;
                joyStick.SetActivity(controller.isJoystick);
                StartCoroutine(TitleOpen());
                StartCoroutine(WaitTitleAnim());
                break;
            case 2://Pause
                Time.timeScale = 0f;
                joyStick.SetActivity(false);
                pauseBut.SetActive(false);
                pause.SetActive(true);
                generate.PauseGame(false);
                audioSource.Pause();
                break;
            case 3://Exit (three of them windows)
                StopCoroutine(progressBarUpdate);
                StopCoroutine(mainEnd);
                player.End();
                data.UpdateMoney();
                generate.ClearData(); 
                pause.SetActive(false);
                lose.SetActive(false);
                game.SetActive(false);
                endStats.SetActive(false);
                menu.SetActive(true);
                pauseBut.SetActive(true);
                Time.timeScale = 1f;
                break;
            case 4://Continie
                Time.timeScale = 1f;
                joyStick.SetActivity(controller.isJoystick);
                pauseBut.SetActive(true);
                pause.SetActive(false);
                generate.PauseGame(true);
                audioSource.Play();
                break;
            case 5://Restart game
                StartCoroutine(AnimClose());
                StartCoroutine(Restart());
                break;
            case 6://Add for Restart game

                break;
        }
    }
    // Black screen anim
    public IEnumerator AnimOpen()
    {
        Color c = blackScreen.color;
        for (float i = 1; i > 0; i -= Time.deltaTime / speedBlackScreen)
        {
            blackScreen.color = new Color(c.r, c.g, c.b, i);
            yield return null;
        }

    }
    public IEnumerator AnimClose()
    {
        Color c = blackScreen.color;
        for (float i = 0; i < 1; i += Time.deltaTime / speedBlackScreen)
        {
            blackScreen.color = new Color(c.r, c.g, c.b, i);
            yield return null;
        }
    }

    IEnumerator WaitTitleAnim()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(TitleClose());
    }
    //Title anim
    IEnumerator TitleOpen()
    {
        Color ct = title.color;
        Vector2 posct = titleTR.localPosition;
        Color cm = musician.color;
        Vector2 poscm = musicianTR.localPosition;
        for (float i = 0; i <= 1; i += Time.deltaTime / 0.4f)
        {
            title.color = new Color(ct.r, ct.g, ct.b, i);
            titleTR.localPosition = posct - new Vector2(i * 230f, 0);
            musician.color = new Color(cm.r, cm.g, cm.b, i);
            musicianTR.localPosition = poscm - new Vector2(i * 230f, 0);
            yield return null;
        }
    }
    IEnumerator TitleClose()
    {
        Color ct = title.color;
        Vector2 posct = titleTR.localPosition;
        Color cm = musician.color;
        Vector2 poscm = musicianTR.localPosition;
        for (float i = 1; i >= 0; i -= Time.deltaTime / 0.7f)
        {
            title.color = new Color(ct.r, ct.g, ct.b, i);
            titleTR.localPosition = posct + new Vector2((1 - i) * 230f, 0);
            musician.color = new Color(cm.r, cm.g, cm.b, i);
            musicianTR.localPosition = poscm + new Vector2((1 - i) * 230f, 0);
            yield return null;
        }
        title.color = new Color(ct.r, ct.g, ct.b, 0f);
        musician.color = new Color(cm.r, cm.g, cm.b, 0f);
    }
    public void SetTrack(Track track)
    {
        audioSource.clip = null;
        audioSource.volume = 1f;
        audioSource.clip = track.clip;
        title.text = track.title;
        musician.text = track.musician;
        if ((track.endFadeOut - track.startFadeOut) > 0) 
        { lengthAudioClip = track.endFadeOut; mainEnd = StartCoroutine(TrackFadeOut(track)); }
        else { lengthAudioClip = track.clip.length; }
        audioSource.Play();
        progressBarUpdate = StartCoroutine(UpdateProgressBar());
        return;
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(0.55f);
        player.Play();
        UpdateHealthPoints();
        input.GetComponent<Image>().enabled = !controller.isJoystick;
        joyStick.SetActivity(controller.isJoystick);
        pauseBut.SetActive(true);
        pause.SetActive(false);
        lose.SetActive(false);
        menu.SetActive(false);
        game.SetActive(true);
        pauseBut.SetActive(true);
        generate.ClearData();
        progressBar.fillAmount = 0f;
        StartCoroutine(SpawnAnimator());
        StartCoroutine(AnimOpen());
    }

    IEnumerator SpawnAnimator()
    {
        yield return new WaitForSeconds(0.35f);
        generate.StartGame();
    }

    public void ResetProgress()
    {
        StartCoroutine(AnimClose());
        StartCoroutine(Reset());

        IEnumerator Reset()
        {
            yield return new WaitForSeconds(0.6f);
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(0);
        }
    }

    public void Lose()
    {
        StartCoroutine(FadeOut());
        StopCoroutine(progressBarUpdate);

        generate.EndGame();
        data.UpdateMoney();
        joyStick.SetActivity(false);
        pauseBut.SetActive(false);
        lose.SetActive(true);
    }

    public void UpdateHealthPoints()
    {
        for (int i = 0; i < healthPoints.Length; i++)
        {
            if (i < player.health)
            { healthPoints[i].color = new Color(1, 0, 0, 1); }
            else
            { healthPoints[i].color = new Color(1, 0, 0, 0.4f); }
        }
        return;
    }

    IEnumerator TrackFadeOut(Track track)
    {
        yield return new WaitForSeconds(track.startFadeOut);
        StartCoroutine(FadeOut());

        IEnumerator FadeOut()
        {
            for (float i = 0; i < track.endFadeOut - track.startFadeOut; i += Time.deltaTime * Time.timeScale)
            {
                audioSource.volume = 1f - i / (track.endFadeOut - track.startFadeOut);
                yield return null;
            }
            EndTrack();
        }
    }
    public IEnumerator FadeOut()
    {
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            audioSource.volume = 1f - i;
            yield return null;
        }
        audioSource.Stop();
    }

    void EndTrack()
    {
        endStats.SetActive(true);
        StopCoroutine(progressBarUpdate);
        player.End();
        data.UpdateMoney();
        generate.EndGame();
        pauseBut.SetActive(false);
        game.SetActive(false);
        Time.timeScale = 1f;
    }

    IEnumerator UpdateProgressBar()
    {
        for (float i = 0; i < lengthAudioClip; i += Time.deltaTime * Time.timeScale)
        {
            progressBar.fillAmount = i / lengthAudioClip;
            yield return null;
        }
    }
}