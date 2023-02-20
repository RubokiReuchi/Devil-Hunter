using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
