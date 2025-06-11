using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine; 

using static StateName;
using static Direction;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    CharacterState characterState;
    new Rigidbody2D rigidbody;
    [SerializeField]
    LayerMask groundMask, wallMask;
    [SerializeField]
    List<Transform> groundChecks;
    [SerializeField]
    Transform leftCheck, rightCheck;
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

        // DEBUG INFO
        switch (characterState.getState())
        {
            case DEFAULT:
                Debug.Log("default");

                break;
            case GROUNDED:
                Debug.Log("grounded");

                break;
            case JUMPING:
                Debug.Log("jumping");

                break;
            case DOUBLE_JUMPING:
                Debug.Log("double jumping");

                break;
            case WALL_SLIDING:
                Debug.Log("wall sliding");

                break;
            case DASHING:
                Debug.Log("dashing");

                break;
            default:
                Debug.Log("Unhandled case");

                break;

        }
    }

    public void Move(float motion) {

        motion *= speed * speedFactor * Time.unscaledDeltaTime * rigidbody.gravityScale;

        rigidbody.velocity = new Vector2(motion, rigidbody.velocity.y);
    }

    public void VerticalJump() {
        float velocity = sqrJumpHeight * jumpForce;

        rigidbody.velocity = new Vector2(0f, velocity);
    }

    public void WallJump(Direction direction) {
        if (direction != LEFT && direction != RIGHT)
            return;

        float velocity = sqrJumpHeight * jumpForce;

        float horizontal_motion = direction == LEFT ? -velocity : velocity;

        Vector2 motion = new Vector2(horizontal_motion * 100f, velocity);
        rigidbody.velocity = motion;
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

    public Direction IsWalled() {
        if (Physics2D.OverlapCircle(leftCheck.position, .1f, wallMask))
            return LEFT;

        if (Physics2D.OverlapCircle(rightCheck.position, .1f, wallMask))
            return RIGHT;

        return NONE;
    }

    public StateName getState() {
        return characterState.getState();
    }

    public void Spawn() {
        characterState = new SpawnState();
    }

    public void ReturnToIdle() {
        if (getState() != DEFAULT)
            return;

        characterState = new IdleState();
    }
}
