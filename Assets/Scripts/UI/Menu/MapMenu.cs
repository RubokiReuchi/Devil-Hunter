using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenu : MonoBehaviour
{
    public GameObject map;
    MapInterface mapInterface;

    Vector3 touchOffset;
    bool touching;

    private void OnEnable()
    {
        touching = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        mapInterface = map.GetComponent<MapInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        // move
        if (!touching && InputManager.instance.input.Attack2.WasPressedThisFrame())
        {
            touching = true;
            touchOffset = map.transform.position - Input.mousePosition;
        }

        if (touching)
        {
            map.transform.position = Input.mousePosition + touchOffset;
            mapInterface.RefreshPlayerMark();
        }

        if (touching && InputManager.instance.input.Attack2.WasReleasedThisFrame())
        {
            touching = false;
        }

        // zoom
        if (InputManager.instance.input.Zoom.ReadValue<float>() > 0)
        {
            map.transform.localScale += Vector3.one * 0.1f;
            mapInterface.RefreshPlayerMark();
        }
        else if (InputManager.instance.input.Zoom.ReadValue<float>() < 0)
        {
            map.transform.localScale -= Vector3.one * 0.1f;
            mapInterface.RefreshPlayerMark();
        }
    }

    public void CenterOnPlayer()
    {
        map.transform.localPosition += transform.position - mapInterface.playerMark.transform.position;
        mapInterface.RefreshPlayerMark();
    }
}
