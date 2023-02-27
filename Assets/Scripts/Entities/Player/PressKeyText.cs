using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum INPUTS
{
    LIMIT
}

public class PressKeyText : MonoBehaviour
{
    TextMesh text;
    [SerializeField] INPUTS input;
    public SpriteRenderer squareSprite;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMesh>();

        switch (input)
        {
            case INPUTS.LIMIT:
                text.text = InputManager.instance.input.Limit.bindings[0].ToDisplayString().ToUpper();
                break;
            default:
                break;
        }
    }

    void Update()
    {
        text.color = new Color(0, 0, 0, squareSprite.color.a);
    }
}
