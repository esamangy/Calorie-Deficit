using UnityEngine;

public class JumpState : PlayerBaseState {
    public JumpState(PlayerController player) : base(player) { }

    public override void FixedUpdate() {
        // call Players jump and move logic
        player.HandleJump();
        player.HandleMovement();
    }
}
