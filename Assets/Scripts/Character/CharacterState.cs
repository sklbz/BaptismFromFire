using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateName;

public class CharacterState {
 
    virtual public StateName getState() {
        return DEFAULT;
    }

    virtual public CharacterState handleInput(CharacterController controller) {

        handleHorizontal(controller);

        return this;
    }

    void handleHorizontal(CharacterController controller) {
        float motion = Input.GetAxisRaw("Horizontal");

        controller.Move(motion);
    }
}

public class GroundState : CharacterState {

    public override StateName getState() {
        return GROUNDED;
    }

    public override CharacterState handleInput(CharacterController controller) {
        base.handleInput(controller);

        if (Input.GetButtonDown("Jump"))
        {
            controller.VerticalJump();
            return new JumpingState();
        }

        if (!controller.IsGrounded())
            return new JumpingState();

        return this;
    }
}

public class IdleState : GroundState {
    public override CharacterState handleInput(CharacterController controller) {

        CharacterState baseState = base.handleInput(controller);
        if (baseState.getState() != GROUNDED)
            return baseState;

        if (Input.GetAxisRaw("Horizontal") != 0)
            return new MovingState();

        return this;
    }
}

public class MovingState : GroundState {
    public override CharacterState handleInput(CharacterController controller) {

        CharacterState baseState = base.handleInput(controller);
        if (baseState.getState() != GROUNDED)
            return baseState;

        if (Input.GetAxisRaw("Horizontal") == 0)
            return new IdleState();

        return this;
    }
}

public class JumpingState : CharacterState {

    public override StateName getState() {
        return JUMPING;
    }

    public override CharacterState handleInput(CharacterController controller) {
        base.handleInput(controller);

        if (controller.IsGrounded())
            return new IdleState();

        return this;
    }
}

public class WallSlidingState : CharacterState {

    public override StateName getState() {
        return WALL_SLIDING;
    }

    public override CharacterState handleInput(CharacterController controller) {
        if (controller.IsGrounded())
            return new IdleState();

        if (Input.GetButtonDown("Jump"))
            return new JumpingState();

        return this;
    }
}

public class WallLeftState : WallSlidingState {
    public override CharacterState handleInput(CharacterController controller) {
        CharacterState baseState = base.handleInput(controller);
        StateName baseName = baseState.getState();

        if (baseName == GROUNDED)
            return baseState;
        if (baseName == JUMPING)
        {
            float direction = 1f;
            controller.WallJump(direction);
            return baseState;
        }

        return this;
    }
}

public class DashingState : CharacterState {

    public override StateName getState() {
        return DASHING;
    }

    public override CharacterState handleInput(CharacterController controller) {
        return this;
    }
}

public class SpawnState : CharacterState {
    public override StateName getState() {
        return DEFAULT;
    }

    public override CharacterState handleInput(CharacterController controller) {
        return this;
    }

}
