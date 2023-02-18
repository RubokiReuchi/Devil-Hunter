using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    Image fadePanel;
    public bool on;
    [HideInInspector] public bool black;

    // Start is called before the first frame update
    void Start()
    {
        fadePanel = GetComponent<Image>();
        if (on) fadePanel.color = new Color(0, 0, 0, 255);
        else fadePanel.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        black = false;
        float alpha;
        if (on)
        {
            if (fadePanel.color.a < 1) alpha = fadePanel.color.a + Time.deltaTime;
            else
            {
                alpha = 1;
                StartCoroutine("Co_Fade");
            }
        }
        else
        {
            if (fadePanel.color.a > 0) alpha = fadePanel.color.a - Time.deltaTime;
            else alpha = 0;
        }
        fadePanel.color = new Color(0, 0, 0, alpha);
    }

    IEnumerator Co_Fade()
    {
        yield return new WaitForSeconds(0.2f);
        on = false;
        black = true;
    }

    public void FadeOn()
    {
        on = true;
    }

    public void FadeOff()
    {
        on = false;
    }
}
