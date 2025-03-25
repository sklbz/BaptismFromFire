using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    CharacterState characterState;
    Rigidbody2D rigidbody;
    [SerializeField]
    LayerMask groundMask;
    float sqrJumpHeight, jumpForce;
    Transform[] groundChecks;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        jumpForce = Mathf.Sqrt(-2f * Physics2D.gravity.y * rigidbody.gravityScale);
    }

    void Update()
    {
        
    }

    void VerticalJump() {
        float velocity = sqrJumpHeight * jumpForce;

        rigidbody.velocity = new Vector2(velocity, 0f);
    }
}
