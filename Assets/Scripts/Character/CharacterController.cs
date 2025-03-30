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
    [SerializeField]
    List<Transform> groundChecks;
    float sqrJumpHeight = 2f, jumpForce, jumpFactor = .8f;
    float speed = 4f, speedFactor = 130f;

    void Start()
    {
        characterState = new IdleState();
        rigidbody = GetComponent<Rigidbody2D>();
        jumpForce = Mathf.Sqrt(-2f * Physics2D.gravity.y * rigidbody.gravityScale * jumpFactor);
    }

    void FixedUpdate()
    {
        characterState = characterState.handleInput(this);
    }

    public void Move(float motion) {

        motion *= speed * speedFactor * Time.unscaledDeltaTime * rigidbody.gravityScale;

        rigidbody.velocity = new Vector2(motion, rigidbody.velocity.y);
    }

    public void VerticalJump() {
        float velocity = sqrJumpHeight * jumpForce;

        rigidbody.velocity = new Vector2(0f, velocity);
    }

    public void WallJump(float direction) {
        if (Mathf.Pow(direction, 2) != 1)
            direction /= Mathf.Abs(direction);
        float velocity = sqrJumpHeight;

        Vector2 motion = new Vector2(direction, 1);
        rigidbody.velocity = new Vector2(1, 1);
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
