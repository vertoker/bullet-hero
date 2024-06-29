using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Skins : GlobalBehaviour
{
    public DataGame data;
    public Color select;
    public Color active;
    public Color standard;
    GameObject buy; Text price;
    public Management management;
    public Transform seter;
    ButtonSkin[] buttons;
    int activeID;
    char[] save;
    int length;

    public void Start()
    {
        buy = transform.GetChild(5).GetChild(1).gameObject;
        price = buy.transform.GetChild(2).GetComponent<Text>();
        price.text = string.Empty;
        buy.SetActive(false); 

        length = seter.childCount;
        buttons = new ButtonSkin[length];
        save = PlayerPrefs.GetString("skins").ToCharArray();

        for (int i = 0; i < length; i++)
        {
            ButtonSkin bs = new ButtonSkin(); 
            Transform tr = seter.GetChild(i);
            bs.background = tr.GetComponent<Outline>();
            bs.skinBase = tr.GetChild(0).GetComponent<Image>();
            bs.lockIm = tr.GetChild(1).gameObject;
            bs.blockIm = tr.GetChild(2).gameObject;

            if (save[i] == 's') 
            { 
                bs.background.effectColor = active;
                data.SetSelectID(i);
                activeID = i;
            }
            else { bs.background.effectColor = standard; }
            bs.skinBase.sprite = data.GetSpriteSkin(i);
            bs.lockIm.SetActive(save[i] == 'b' || save[i] == 'l');
            bs.blockIm.SetActive(save[i] == 'b');
            buttons[i] = bs;
        }
    }

    public void Open()
    {
        buy.SetActive(false);
    }

    public void Close()
    {
        buttons[activeID].background.effectColor = standard;
        activeID = data.IdSelect;
        buttons[activeID].background.effectColor = select;
    }

    public void Buttons(int id)
    {
        if (save[id] != 'b')
        {
            int lastID = activeID;
            activeID = id;
            buy.SetActive(save[activeID] == 'l');
            price.text = data.GetPriceSkin(activeID).ToString();

            if (save[lastID] == 's') 
            {
                if (save[activeID] == 'u')
                {
                    save[lastID] = 'u';
                    save[activeID] = 's';

                    buttons[lastID].background.effectColor = standard;
                    buttons[activeID].background.effectColor = active;
                    PlayerPrefs.SetString("skins", new string(save));
                    data.SetSelectID(activeID);
                }
                else
                {
                    buttons[activeID].background.effectColor = select;
                }
            }
            else
            {
                if (save[activeID] == 's')
                {
                    buttons[data.IdSelect].background.effectColor = standard;
                    buttons[lastID].background.effectColor = standard;
                    buttons[activeID].background.effectColor = active;
                    data.SetSelectID(activeID);
                }
                else if (save[activeID] == 'u')
                {
                    save[data.IdSelect] = 'u';
                    save[activeID] = 's';

                    buttons[data.IdSelect].background.effectColor = standard;
                    buttons[lastID].background.effectColor = standard;
                    buttons[activeID].background.effectColor = active;
                    PlayerPrefs.SetString("skins", new string(save));
                    data.SetSelectID(activeID);
                }
                else if (save[activeID] == 'l')
                {
                    buttons[lastID].background.effectColor = standard;
                    buttons[activeID].background.effectColor = select;
                }
            }
        }
        return;
    }

    public void Buy()
    {
        int money = PlayerPrefs.GetInt("money");
        int price = data.GetPriceSkin(activeID);
        if (money >= price)
        {
            PlayerPrefs.SetInt("money", money - price);
            save[data.IdSelect] = 'u';
            save[activeID] = 's';
            PlayerPrefs.SetString("skins", new string(save));
            buttons[data.IdSelect].background.effectColor = standard;
            buttons[activeID].background.effectColor = active;
            buttons[activeID].lockIm.SetActive(false);
            data.SetSelectID(activeID);
            buy.SetActive(false);
            data.UpdateMoney();
        }
    }

    public class ButtonSkin
    {
        public Outline background;
        public Image skinBase;
        public GameObject lockIm;
        public GameObject blockIm;
    }
}