using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : Menu
{
    // variables
    int screenSizeIndex;
    List<string> screenSizeOptions = new();
    bool fullscreen = true;
    int graphicsIndex = 5;
    string[] graphicsOptions = { "Vey Low", "Low", "Medium", "High", "Very High", "Ultra" };
    int framerateIndex = 1;
    int[] framerateOptions = { 30, 60, 90, 120, 144, -1 };
    bool vsync = true;

    // ui elements
    public GameObject buttonScreenSizeDown;
    public GameObject buttonScreenSizeUp;
    public TextMeshProUGUI textScreenSize;
    public Image imageFullscreen;
    public GameObject buttonGraphicsDown;
    public GameObject buttonGraphicsUp;
    public TextMeshProUGUI textGraphics;
    public GameObject buttonFramerateDown;
    public GameObject buttonFramerateUp;
    public TextMeshProUGUI textFramerate;
    public TextMeshProUGUI wordFramerate;
    public Image imageVsync;

    // audio
    public AudioMixer audioMixer;

    void Start()
    {
        GetResolutions();
        screenSizeIndex = screenSizeOptions.Count - 1;

        CalculateScreenSize();
        CalculateFullscreen();
        CalculateGraphics();
        CalculateFramerate();
        CalculateVsync();
    }

    void GetResolutions()
    {
        Resolution[] resolutions = Screen.resolutions;

        foreach (Resolution resolution in resolutions)
        {
            screenSizeOptions.Add(resolution.width + " x " + resolution.height + " @"+ resolution.refreshRate + "hz");
        }
    }

    public void DecreaseScreenSize()
    {
        screenSizeIndex--;
        CalculateScreenSize();
    }

    public void IncreaseScreenSize()
    {
        screenSizeIndex++;
        CalculateScreenSize();
    }

    void CalculateScreenSize()
    {
        Screen.SetResolution(Screen.resolutions[screenSizeIndex].width, Screen.resolutions[screenSizeIndex].height, fullscreen, Screen.resolutions[screenSizeIndex].refreshRate);
        
        textScreenSize.text = screenSizeOptions[screenSizeIndex];
        if (screenSizeIndex == 0)
        {
            buttonScreenSizeDown.SetActive(false);
            buttonScreenSizeUp.SetActive(true);
        }
        else if (screenSizeIndex == screenSizeOptions.Count - 1)
        {
            buttonScreenSizeDown.SetActive(true);
            buttonScreenSizeUp.SetActive(false);
        }
        else if (screenSizeOptions.Count == 1)
        {
            buttonScreenSizeDown.SetActive(false);
            buttonScreenSizeUp.SetActive(false);
        }
        else
        {
            buttonScreenSizeDown.SetActive(true);
            buttonScreenSizeUp.SetActive(true);
        }
    }

    public void ToggleFullscreen()
    {
        fullscreen = !fullscreen;
        CalculateFullscreen();
    }

    void CalculateFullscreen()
    {
        Screen.fullScreen = fullscreen;

        if (fullscreen) imageFullscreen.enabled = true;
        else imageFullscreen.enabled = false;
    }

    public void DecreaseGraphics()
    {
        graphicsIndex--;
        CalculateGraphics();
    }

    public void IncreaseGraphics()
    {
        graphicsIndex++;
        CalculateGraphics();
    }

    void CalculateGraphics()
    {
        QualitySettings.SetQualityLevel(graphicsIndex);

        textGraphics.text = graphicsOptions[graphicsIndex];
        if (graphicsIndex == 0)
        {
            buttonGraphicsDown.SetActive(false);
            buttonGraphicsUp.SetActive(true);
        }
        else if (graphicsIndex == graphicsOptions.Length - 1)
        {
            buttonGraphicsDown.SetActive(true);
            buttonGraphicsUp.SetActive(false);
        }
        else if (graphicsOptions.Length == 1)
        {
            buttonGraphicsDown.SetActive(false);
            buttonGraphicsUp.SetActive(false);
        }
        else
        {
            buttonGraphicsDown.SetActive(true);
            buttonGraphicsUp.SetActive(true);
        }
    }

    public void DecreaseFramerate()
    {
        framerateIndex--;
        CalculateFramerate();
    }

    public void IncreaseFramerate()
    {
        framerateIndex++;
        CalculateFramerate();
    }

    void CalculateFramerate()
    {
        Application.targetFrameRate = framerateOptions[framerateIndex];

        if (framerateIndex == framerateOptions.Length - 1) textFramerate.text = "Unlimited";
        else textFramerate.text = framerateOptions[framerateIndex].ToString();
        if (framerateIndex == 0)
        {
            buttonFramerateDown.SetActive(false);
            buttonFramerateUp.SetActive(true);
        }
        else if (framerateIndex == framerateOptions.Length - 1)
        {
            buttonFramerateDown.SetActive(true);
            buttonFramerateUp.SetActive(false);
        }
        else if (framerateOptions.Length == 1)
        {
            buttonFramerateDown.SetActive(false);
            buttonFramerateUp.SetActive(false);
        }
        else
        {
            buttonFramerateDown.SetActive(true);
            buttonFramerateUp.SetActive(true);
        }
    }

    public void ToggleVsync()
    {
        vsync = !vsync;
        CalculateVsync();
    }

    void CalculateVsync()
    {
        if (vsync) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;

        if (vsync)
        {
            imageVsync.enabled = true;
            wordFramerate.color = Color.grey;
        }
        else
        {
            imageVsync.enabled = false;
            wordFramerate.color = Color.white;
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetFxVolume(float volume)
    {
        audioMixer.SetFloat("FxVolume", volume);
    }
}
