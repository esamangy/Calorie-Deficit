using Unity.VisualScripting;

public interface IState {
    public void OnEnter();
    public void Update();
    public void FixedUpdate();
    public void OnExit();
}
