using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraActions : MonoBehaviour
{
    CinemachineVirtualCamera cameraV;
    // Shake
    CinemachineBasicMultiChannelPerlin chanel;

    // Look Up and Down
    CinemachineFramingTransposer transposer;
    [NonEditable] public bool camCentered;
    public float lookUpY;
    public float lookDownY;

    // Time Stop
    float restoreSpeed;
    bool restoreTime;

    // Start is called before the first frame update
    void Start()
    {
        cameraV = GetComponent<CinemachineVirtualCamera>();
        chanel = cameraV.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        transposer = cameraV.GetCinemachineComponent<CinemachineFramingTransposer>();
        camCentered = true;
    }

    void Update()
    {
        if (camCentered)
        {
            if (transposer.m_ScreenY > 0.55f)
            {
                transposer.m_ScreenY -= Time.deltaTime;
                if (transposer.m_ScreenY < 0.55f) transposer.m_ScreenY = 0.55f;
            }
            if (transposer.m_ScreenY < 0.55f)
            {
                transposer.m_ScreenY += Time.deltaTime;
                if (transposer.m_ScreenY > 0.55f) transposer.m_ScreenY = 0.55f;
            }
        }

        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * restoreSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }

    public void ShakeCamera(float duration, float force)
    {
        chanel.m_AmplitudeGain = force;
        if (force >= 2.0f) StopTime(0.1f, 5);
        StartCoroutine("Co_ShakeCamera", duration);
    }

    IEnumerator Co_ShakeCamera(float duration)
    {
        yield return new WaitForSeconds(duration);
        chanel.m_AmplitudeGain = 0;
    }

    public void LookUp()
    {
        camCentered = false;
        if (transposer.m_ScreenY < lookUpY)
        {
            transposer.m_ScreenY += Time.deltaTime;
        }
        else if (transposer.m_ScreenY > lookUpY)
        {
            transposer.m_ScreenY = lookUpY;
        }
    }

    public void LookDown()
    {
        camCentered = false;
        if (transposer.m_ScreenY > lookDownY)
        {
            transposer.m_ScreenY -= Time.deltaTime;
        }
        else if (transposer.m_ScreenY < lookDownY)
        {
            transposer.m_ScreenY = lookDownY;
        }
    }

    void StopTime(float changeTime, int restoreSpeed)
    {
        this.restoreSpeed = restoreSpeed;

        restoreTime = true;
        Time.timeScale = changeTime;
    }
}
