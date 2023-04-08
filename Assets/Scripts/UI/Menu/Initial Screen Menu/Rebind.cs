using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Rebind : MonoBehaviour
{
    public InputActionAsset actions;

    public void ResetBindings()
    {
        actions.RemoveAllBindingOverrides();
        EventSystem.current.SetSelectedGameObject(null);
    }
}
