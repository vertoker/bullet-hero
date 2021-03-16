using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Base")]
    public int health = 3;
    public bool isImmortality = false;
    private bool isDamaged = false;
    public Transform player;
    public Transform target;
    public InputController input;
    public static Player Instance;
    private GameManager gameManager;
    [Header("Other")]
    public GameObject trailEffect;
    public SpriteRenderer playerSR;
    private GameObject objPreset;

    public void Awake()
    {
        Instance = this;
    }

    public void SetPlayerPreset(GameObject preset, GameManager gameManager)
    {
        this.gameManager = gameManager;
        if (objPreset != null) { Destroy(objPreset); }
        objPreset = Instantiate(preset, player.position, Quaternion.identity, player);
        playerSR = objPreset.GetComponent<SpriteRenderer>();
        End();
        return;
    }

    public void Play()
    {
        health = 3;
        player.localEulerAngles = Vector3.zero;
        trailEffect.SetActive(true);
        playerSR.enabled = true;
        target.localPosition = Vector3.zero;
        player.localPosition = Vector3.zero;
        return;
    }

    public void End()
    {
        trailEffect.SetActive(false);
        target.gameObject.SetActive(false);
        playerSR.enabled = false;
        target.localPosition = Vector3.zero;
        player.localPosition = Vector3.zero;
        return;
    }

    public void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, target.position, 0.15f);
        if (isDamaged)
        {
            isDamaged = false;
            StartCoroutine(EndImmortality());
            if (health == 0)
                End();
        }
    }

    public void Damage()
    {
        if (!isImmortality && !isDamaged)
        {
            isImmortality = true;
            isDamaged = true;
            health -= 1;
        }
    }

    public IEnumerator EndImmortality()
    {
        yield return new WaitForSeconds(1f); 
        isImmortality = false;
    }
}