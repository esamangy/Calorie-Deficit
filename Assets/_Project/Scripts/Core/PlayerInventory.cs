using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    public static PlayerInventory Instance { get; private set;}
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
            leftHandSelecting.transform.position = leftHandPosition.position;
        }
        if(rightHandSelecting != null){
            rightHandSelecting.transform.position = rightHandPosition.position;
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
}
