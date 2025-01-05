public abstract class PlayerBaseState : IState {
    protected PlayerController player;

    protected PlayerBaseState(PlayerController player) {
        this.player = player;
    }
    public void FixedUpdate() {
        // noop
    }

    public void OnEnter() {
        // noop
    }

    public void OnExit() {
        // noop
    }

    public void Update() {
        // noop
    }
}