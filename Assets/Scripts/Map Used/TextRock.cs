using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum KEYS_TEXT
{
    NONE,
    MOVE,
    JUMP,
    JUMP2,
    DASH,
    ATTACK1,
    AIR_ATTACK1,
    LOOK_UP,
    LOOK_DOWN,
    AIM
}

public class TextRock : MonoBehaviour
{
    public KEYS_TEXT text;
    public TextMesh textMesh;
    public SpriteRenderer sprite;
    bool onTrigger;
    Dante_Movement dm;

    // Start is called before the first frame update
    void Start()
    {
        dm = GameObject.FindGameObjectWithTag("Dante").GetComponent<Dante_Movement>();

        string button;
        switch (text)
        {
            case KEYS_TEXT.NONE:
                break;
            case KEYS_TEXT.MOVE:
                string positive = dm.input.Move.bindings[dm.input.Move.bindings.IndexOf(b => b.name == "positive")].ToString().ToUpper();
                positive = positive.Substring(16, positive.Length - 16);
                string negative = dm.input.Move.bindings[dm.input.Move.bindings.IndexOf(b => b.name == "negative")].ToString().ToUpper();
                negative = negative.Substring(16, negative.Length - 16);
                textMesh.text = "Use '" + negative + "' & '" + positive + "' to\n move your self.";
                break;
            case KEYS_TEXT.JUMP:
                button = dm.input.Jump.bindings[0].ToDisplayString().ToUpper();
                textMesh.text = "Press '" + button + "' to\n jump, if you don't\nrealese '" + button + "'\nyou will jump higher.";
                break;
            case KEYS_TEXT.JUMP2:
                textMesh.text = "Use the jump\nto evade enemy\nattacks.";
                break;
            case KEYS_TEXT.DASH:
                button = dm.input.Dash.bindings[0].ToDisplayString().ToUpper();
                textMesh.text = "Press '" + button + "' to\nroll, you can dodge\nenemies hits rolling into\nits attacks.";
                break;
            case KEYS_TEXT.ATTACK1:
                button = dm.input.Attack1.bindings[0].ToDisplayString().ToUpper();
                textMesh.text = "Press '" + button + "' to attack.";
                break;
            case KEYS_TEXT.AIR_ATTACK1:
                button = dm.input.Attack1.bindings[0].ToDisplayString().ToUpper();
                textMesh.text = "Press '" + button + "' on\nthe air to attack and\nnullifies gravity for a\nshort time.";
                break;
            case KEYS_TEXT.LOOK_UP:
                button = dm.input.LookUp.bindings[0].ToDisplayString().ToUpper();
                textMesh.text = "Mantein '" + button + "'\npressed to\nlook up.";
                break;
            case KEYS_TEXT.LOOK_DOWN:
                button = dm.input.LookDown.bindings[0].ToDisplayString().ToUpper();
                textMesh.text = "Mantein '" + button + "'\npressed to\nlook down.";
                break;
            case KEYS_TEXT.AIM:
                button = dm.input.Aim.bindings[0].ToDisplayString().ToUpper();
                textMesh.text = "Mantein '" + button + "'\npressed to\nfocus your atention\nin a near enemy.";
                break;
            default:
                break;
        }
    }

    void Update()
    {
        float alpha;
        if (onTrigger)
        {
            if (textMesh.color.a < 1) alpha = textMesh.color.a + Time.deltaTime;
            else alpha = 1;
        }
        else
        {
            if (textMesh.color.a > 0) alpha = textMesh.color.a - Time.deltaTime;
            else alpha = 0;
        }
        textMesh.color = new Color(1, 1, 1, alpha);
        sprite.color = new Color(1, 1, 1, alpha);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dante"))
        {
            onTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Dante"))
        {
            onTrigger = false;
        }
    }
}
