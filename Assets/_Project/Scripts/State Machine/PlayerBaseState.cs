public abstract class PlayerBaseState : IState {
    protected PlayerController player;

    protected PlayerBaseState(PlayerController player) {
        this.player = player;
    }
    public virtual void FixedUpdate() {
        // noop
    }

    public virtual void OnEnter() {
        // noop
    }

    public virtual void OnExit() {
        // noop
    }

    public virtual void Update() {
        // noop
    }
}