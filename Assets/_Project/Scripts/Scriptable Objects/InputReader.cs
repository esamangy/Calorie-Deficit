using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInput;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions {
    public event UnityAction<Vector2> Move = delegate { };
    public event UnityAction<Vector2, bool> Look = delegate { };

    PlayerInput inputActions;
    public Vector3 Direction => inputActions.Player.Move.ReadValue<Vector2>();

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
        // noop
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
}
