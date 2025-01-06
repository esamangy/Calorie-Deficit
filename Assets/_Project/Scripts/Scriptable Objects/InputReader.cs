using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using static PlayerInput;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions {
    public event UnityAction<Vector2> Move = delegate { };
    public event UnityAction<Vector2, bool> Look = delegate { };
    public event UnityAction<bool> Jump = delegate { };
    public event UnityAction<bool> Sprint = delegate { };
    public event UnityAction<bool, bool> Interact = delegate { };
    public event UnityAction<bool, bool> InteractAlt = delegate { };
    PlayerInput inputActions;
    public Vector3 Direction => inputActions.Player.Move.ReadValue<Vector2>();
    [HideInInspector]
    public bool IsSprinting;

    private void OnEnable() {
        if (inputActions == null) {
            inputActions = new PlayerInput();
            inputActions.Player.SetCallbacks(this);
        }
        inputActions.Enable();
    }

    private void OnDisable() {
        inputActions.Disable();
    }
    public void OnInteract(InputAction.CallbackContext context) {
        Interact.Invoke(context.started, context.canceled);
    }

    public void OnLook(InputAction.CallbackContext context) {
        Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
    }

    public bool IsDeviceMouse(InputAction.CallbackContext context) {
        return context.control.device.name == "Mouse";
    }

    public void OnMove(InputAction.CallbackContext context) {
        Move.Invoke(context.ReadValue<Vector2>());
    }

    public PlayerInput GetPlayerInput() {
        return inputActions;
    }

    public void OnJump(InputAction.CallbackContext context) {
        switch (context.phase){
            case InputActionPhase.Started:
                Jump.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                Jump.Invoke(false);
                break;
        }
    }

    public void OnInteractAlt(InputAction.CallbackContext context) {
        InteractAlt.Invoke(context.started, context.canceled);
    }

    public void OnSprint(InputAction.CallbackContext context) {
        switch (context.phase){
            case InputActionPhase.Started:
                Sprint.Invoke(true);
                IsSprinting = true;
                break;
            case InputActionPhase.Canceled:
                Sprint.Invoke(false);
                IsSprinting = false;
                break;
        }
    }
}
