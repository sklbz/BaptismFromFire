using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState {
 
    virtual public StateName getState() {
        return StateName.DEFAULT;
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
        return StateName.GROUNDED;
    }

    public override CharacterState handleInput(CharacterController controller) {

        if (Input.GetButtonDown("Jump"))
            return new JumpingState();

        return this;
    }
}

public class IdleState : GroundState {
    public override CharacterState handleInput(CharacterController controller) {

        if (base.handleInput().getState() == StateName.GROUNDED)
            return this;

        if (Input.GetButton("Move"))
            return new MovingState();

        return this;
    }
}

public class MovingState : GroundState {
    public override CharacterState handleInput(CharacterController controller) {


        if (base.handleInput().getState() == StateName.GROUNDED)
            return this;

        if (!Input.GetButton("Move"))
            return new MovingState();

        return this;
    }
}

public class JumpingState : CharacterState {

    public override StateName getState() {
        return StateName.JUMPING;
    }

    public override CharacterState handleInput(CharacterController controller) {
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
