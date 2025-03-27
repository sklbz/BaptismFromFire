using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    CharacterState characterState;
    new Rigidbody2D rigidbody;
    [SerializeField]
    LayerMask groundMask;
    float sqrJumpHeight, jumpForce;
    List<Transform> groundChecks;

    void Start()
    {
        characterState = GetComponent<CharacterState>();
        rigidbody = GetComponent<Rigidbody2D>();
        jumpForce = Mathf.Sqrt(-2f * Physics2D.gravity.y * rigidbody.gravityScale);
    }

    void Update()
    {

        characterState = characterState.handleInput();
    }

    public void VerticalJump() {
        float velocity = sqrJumpHeight * jumpForce;

        rigidbody.velocity = new Vector2(0f, velocity);
    }

    public bool IsGrounded() {

        bool groundCheckDown = false;
        foreach (Transform groundCheck in groundChecks)
        {
            if (Physics2D.OverlapCircle(groundCheck.position, .1f, groundMask))
            {
                groundCheckDown = true;
                break;
            }
        };
        return groundCheckDown;
    }

    public StateName getState() {
        return characterState.getState();
    }
}
