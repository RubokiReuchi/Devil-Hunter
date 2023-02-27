using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    PlayerInputActions inputActions;
    public PlayerInputActions.NormalActions input;

    private void Awake()
    {
        instance = this;

        inputActions = new PlayerInputActions();
        input = inputActions.Normal;
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
}
