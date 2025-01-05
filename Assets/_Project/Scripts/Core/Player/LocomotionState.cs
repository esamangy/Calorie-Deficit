using UnityEngine;

public class LocomotionState : PlayerBaseState {
    public LocomotionState(PlayerController player) : base(player) { }

    public override void FixedUpdate() {
        // call Players move logic
        player.HandleMovement();
    }
}
