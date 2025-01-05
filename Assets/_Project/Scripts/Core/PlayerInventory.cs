using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    public static PlayerInventory Instance { get; private set;}
    [SerializeField] private float smoothingMultiplier = 3f;
    [SerializeField] private Transform leftHandPosition;
    [SerializeField] private Transform rightHandPosition;
    private Selectable leftHandSelecting;
    private Selectable rightHandSelecting;

    private void Awake() {
        if(Instance != null) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Update() {
        if(leftHandSelecting != null){
            leftHandSelecting.transform.position = Vector3.Lerp(leftHandSelecting.transform.position, leftHandPosition.position, Time.deltaTime * smoothingMultiplier);
        }
        if(rightHandSelecting != null){
            rightHandSelecting.transform.position = Vector3.Lerp(rightHandSelecting.transform.position, rightHandPosition.position, Time.deltaTime * smoothingMultiplier);
        }
    }

    public Selectable GetLeftHandSelecting() {
        return leftHandSelecting;
    }

    public Selectable GetRightHandSelecting() {
        return rightHandSelecting;
    }

    public void SetSelecting(Selectable item, PlayerInteraction.Handedness handedness) {
        if(handedness == PlayerInteraction.Handedness.Right) {
            SetRightHandSelecting(item);
        } else {
            SetLeftHandSelecting(item);
        }
    }
    public void SetLeftHandSelecting(Selectable item) {
        leftHandSelecting = item;
    }

    public void SetRightHandSelecting(Selectable item) {
        rightHandSelecting = item;
    }

    public bool HasLeftSelecting() {
        return leftHandSelecting != null;
    }

    public bool HasRightSelecting() {
        return rightHandSelecting != null;
    }

    public Vector3 GetHandLocation(PlayerInteraction.Handedness handedness){
        return handedness == PlayerInteraction.Handedness.Left ? leftHandPosition.position : rightHandPosition.position;
    }
}
