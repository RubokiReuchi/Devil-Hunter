using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public GameObject fadeCanvas;
    GraphicRaycaster raycaster;
    Image fadePanel;
    public bool on;
    [HideInInspector] public bool black;

    // Start is called before the first frame update
    void Start()
    {
        raycaster = fadeCanvas.GetComponent<GraphicRaycaster>();
        fadePanel = GetComponent<Image>();
        if (on) fadePanel.color = new Color(0, 0, 0, 1);
        else fadePanel.color = new Color(0, 0, 0, 0);
        on = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadePanel.color.a < 0.1f) black = false;
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

        if (fadePanel.color.a < 0.1f) raycaster.enabled = false;
        else raycaster.enabled = true;
    }

    IEnumerator Co_Fade()
    {
        yield return new WaitForSeconds(0.2f);
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
