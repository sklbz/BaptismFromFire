using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class hover : MonoBehaviour

{
    public float hoverForce = 10.0f;     // The force applied when hovering
    public float hoverDuration = 1.0f;   // How long the hover force should be applied
    float HoverTime;
    float hoverTimer {
        get {
            return HoverTime;
        }
        set {
            HoverTime = Mathf.Clamp(value, 0, float.MaxValue);
        }
    }
    Rigidbody2D rb2D;
    public bool canHover;
    CharacterJump characterJump;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        hoverTimer = 1f;
        characterJump = GetComponent<CharacterJump>();
    }

    private void Update()
    {
        if (IsGrounded() || !canHover)
        {
            rb2D.gravityScale = characterJump.gravityModifier;
            return;
        }


        if (!Input.GetButton("Hover"))
        {
            hoverTimer -= Time.deltaTime;
            rb2D.gravityScale = characterJump.gravityModifier;
            return;
        }

        if (hoverTimer >= hoverDuration * Time.timeScale)
        {
            rb2D.gravityScale = characterJump.gravityModifier;
            return;

        }
        
        Debug.Log("Hover");

        // Apply an upward force to simulate hovering
        //rb2D.AddForce(Vector2.up * hoverForce, ForceMode2D.Impulse);
        rb2D.gravityScale = 0;
        hoverTimer += Time.deltaTime;

    }

    private bool IsGrounded()
    {
        return characterJump._state == CharacterJumpingState.STATE_GROUNDED;
    }
}