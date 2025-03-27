using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState {
 
    virtual public StateName getState() {
        return StateName.DEFAULT;
    }

    virtual public CharacterState handleInput(CharacterController controller) {

        handleHorizontal(controller);

        controller.Move(5f);

        return this;
    }

    void handleHorizontal(CharacterController controller) {
        float motion = Input.GetAxisRaw("Horizontal");

        controller.Move(motion);
        Debug.Log("hi");
    }
}

public class GroundState : CharacterState {

    public override StateName getState() {
        return StateName.GROUNDED;
    }

    public override CharacterState handleInput(CharacterController controller) {
        base.handleInput(controller);

        if (Input.GetButtonDown("Jump"))
        {
            controller.VerticalJump();
            return new JumpingState();
        }

        return this;
    }
}

public class IdleState : GroundState {
    public override CharacterState handleInput(CharacterController controller) {

        Debug.Log("hey");
        if (base.handleInput(controller).getState() == StateName.GROUNDED)
            return this;

        if (Input.GetAxisRaw("Horizontal") != 0)
            return new MovingState();

        return this;
    }
}

public class MovingState : GroundState {
    public override CharacterState handleInput(CharacterController controller) {


        if (base.handleInput(controller).getState() == StateName.GROUNDED)
            return this;

        if (Input.GetAxisRaw("Horizontal") == 0)
            return new IdleState();

        return this;
    }
}

public class JumpingState : CharacterState {

    public override StateName getState() {
        return StateName.JUMPING;
    }

    public override CharacterState handleInput(CharacterController controller) {
        if (controller.IsGrounded())
            return new IdleState();

        return this;
    }
}

public class WallSlidingState : CharacterState {

    public override StateName getState() {
        return StateName.WALL_SLIDING;
    }

    public override CharacterState handleInput(CharacterController controller) {
        return this;
    }
}

public class DashingState : CharacterState {

    public override StateName getState() {
        return StateName.DASHING;
    }

    public override CharacterState handleInput(CharacterController controller) {
        return this;
    }
}
