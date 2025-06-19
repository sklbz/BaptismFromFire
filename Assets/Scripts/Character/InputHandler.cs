using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static InputSource;
using static Direction;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    InputSource source = KEYBOARD;

    InputWrapper joystick;
    Button jumpButton;
    bool isJumpButtonPressed = false;

    Direction desiredDirection = NONE;
    bool wantToJump = false;

    void Start()
    {
        joystick = FindObjectOfType<InputWrapper>();
        jumpButton = FindObjectOfType<Button>();
        jumpButton.onClick.AddListener(JumpListener);

        if (source != JOYSTICK)
        {
            joystick.gameObject.SetActive(false);
            joystick.transform.parent.gameObject.SetActive(false);
            jumpButton.gameObject.SetActive(false);
        }
    }

    public float GetHorizontal() {
        switch (source)
        {
            case AI:
                switch (desiredDirection)
                {
                    case LEFT:
                        return -1;
                    case RIGHT:
                        return 1;
                    default:
                        return 0;
                }
            case KEYBOARD:
                return Input.GetAxisRaw("Horizontal");
            case JOYSTICK:
                return joystick.Horizontal();
            default:
                return 0;
        }
    }

    public bool GetJump() {
        switch (source)
        {
            case AI:
                return wantToJump;
            case KEYBOARD:
                return Input.GetButtonDown("Jump");
            case JOYSTICK:
                return isJumpButtonPressed;
            default:
                return false;
        }
    }

    void OnTriggerStay2D(Collider2D collision) {
        switch (collision.tag)
        {
            case "AI_Left":
                desiredDirection = LEFT;

                break;
            case "AI_Right":
                desiredDirection = RIGHT;

                break;
            case "AI_Jump":
                StartCoroutine(JumpAI());

                break;
        }
    }

    private void OnTriggerExit2D() {
        desiredDirection = NONE;
    }

    void JumpListener() {
        isJumpButtonPressed = true;
        StartCoroutine(ResetJumpButton());
    }

    IEnumerator ResetJumpButton() {
        yield return new WaitForEndOfFrame();

        isJumpButtonPressed = false;
    }

    IEnumerator JumpAI() {
        wantToJump = true;
        
        yield return new WaitForEndOfFrame();

        wantToJump = false;
    }
}
