using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleCounter : MonoBehaviour
{
    public Dante_Stats stats;
    public Dante_Skills skills;
    Animator anim;
    bool childrenEnabled = true;
    int actualLevel = -1;

    [Header("Letters")]
    public Sprite s;
    public Color sColor;
    Color sBorderColor;
    public Sprite a;
    public Color aColor;
    Color aBorderColor;
    public Sprite b;
    public Color bColor;
    Color bBorderColor;
    public Sprite c;
    public Color cColor;
    Color cBorderColor;
    public Sprite d;
    public Color dColor;
    Color dBorderColor;
    public Sprite e;
    public Color eColor;
    Color eBorderColor;

    [Header("UI")]
    public GameObject letterBorder;
    public GameObject letter;
    public Image barBackground;
    public Image barCharge;
    public Slider styleBar;

    void Start()
    {
        anim = GetComponent<Animator>();

        sBorderColor = new Color(sColor.r - 0.3f, sColor.g - 0.3f, sColor.b - 0.3f);
        aBorderColor = new Color(aColor.r - 0.3f, aColor.g - 0.3f, aColor.b - 0.3f);
        bBorderColor = new Color(bColor.r - 0.3f, bColor.g - 0.3f, bColor.b - 0.3f);
        cBorderColor = new Color(cColor.r - 0.3f, cColor.g - 0.3f, cColor.b - 0.3f);
        dBorderColor = new Color(dColor.r - 0.3f, dColor.g - 0.3f, dColor.b - 0.3f);
        eBorderColor = new Color(eColor.r - 0.3f, eColor.g - 0.3f, eColor.b - 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        // enable and disable
        if (childrenEnabled && stats.styleLevel == 0 && stats.styleCount == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            childrenEnabled = false;
            actualLevel = -1;
        }
        else if (!childrenEnabled && (stats.styleLevel > 0 || stats.styleCount > 0))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            childrenEnabled = true;
        }

        if (!childrenEnabled) return;
        
        // letters
        if (actualLevel != stats.styleLevel)
        {
            if (actualLevel == -1) SwapLetter();
            else if (actualLevel < stats.styleLevel) anim.SetTrigger("LevelUp");
            else if (actualLevel > stats.styleLevel) anim.SetTrigger("LevelDown");
        }

        // slider
        styleBar.value = stats.styleCount;
    }

    public void SwapLetter()
    {
        actualLevel = stats.styleLevel;
        Image aux;
        switch (stats.styleLevel)
        {
            case 0: // E
                aux = letter.GetComponent<Image>();
                aux.sprite = e;
                aux.SetNativeSize();
                aux.color = eColor;

                aux = letterBorder.GetComponent<Image>();
                aux.sprite = e;
                letterBorder.GetComponent<RectTransform>().sizeDelta = letter.GetComponent<RectTransform>().sizeDelta * 1.05f;
                aux.color = eBorderColor;
                break;
            case 1: // D
                aux = letter.GetComponent<Image>();
                aux.sprite = d;
                aux.SetNativeSize();
                aux.color = dColor;

                aux = letterBorder.GetComponent<Image>();
                aux.sprite = d;
                letterBorder.GetComponent<RectTransform>().sizeDelta = letter.GetComponent<RectTransform>().sizeDelta * 1.05f;
                aux.color = dBorderColor;
                break;
            case 2: // C
                aux = letter.GetComponent<Image>();
                aux.sprite = c;
                aux.SetNativeSize();
                aux.color = cColor;

                aux = letterBorder.GetComponent<Image>();
                aux.sprite = c;
                letterBorder.GetComponent<RectTransform>().sizeDelta = letter.GetComponent<RectTransform>().sizeDelta * 1.05f;
                aux.color = cBorderColor;
                break;
            case 3: // B
                aux = letter.GetComponent<Image>();
                aux.sprite = b;
                aux.SetNativeSize();
                aux.color = bColor;

                aux = letterBorder.GetComponent<Image>();
                aux.sprite = b;
                letterBorder.GetComponent<RectTransform>().sizeDelta = letter.GetComponent<RectTransform>().sizeDelta * 1.05f;
                aux.color = bBorderColor;
                break;
            case 4: // A
                aux = letter.GetComponent<Image>();
                aux.sprite = a;
                aux.SetNativeSize();
                aux.color = aColor;

                aux = letterBorder.GetComponent<Image>();
                aux.sprite = a;
                letterBorder.GetComponent<RectTransform>().sizeDelta = letter.GetComponent<RectTransform>().sizeDelta * 1.05f;
                aux.color = aBorderColor;
                break;
            case 5: // S
                aux = letter.GetComponent<Image>();
                aux.sprite = s;
                aux.SetNativeSize();
                aux.color = sColor;

                aux = letterBorder.GetComponent<Image>();
                aux.sprite = s;
                letterBorder.GetComponent<RectTransform>().sizeDelta = letter.GetComponent<RectTransform>().sizeDelta * 1.05f;
                aux.color = sBorderColor;
                break;
        }

        // slider colors
        barCharge.color = letter.GetComponent<Image>().color;
        barBackground.color = letterBorder.GetComponent<Image>().color;
    }
}
