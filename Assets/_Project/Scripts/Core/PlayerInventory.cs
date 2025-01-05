using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    public static PlayerInventory Instance { get; private set;}
    [SerializeField] private float smoothingMultiplier = 3f;
    [SerializeField] private Transform leftHandPosition;
    [SerializeField] private Transform rightHandPosition;
    [SerializeField] private Transform cameraRoot;
    private Selectable leftHandSelecting;
    private Selectable rightHandSelecting;
    private Vector3 leftSmoothedPosiiton;
    private Vector3 rightSmoothedPosiiton;
    private void Awake() {
        if(Instance != null) {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start() {
        leftSmoothedPosiiton = leftHandPosition.position;
        rightSmoothedPosiiton = rightHandPosition.position;
    }

    private void Update() {
        leftSmoothedPosiiton = Vector3.Lerp(leftSmoothedPosiiton, leftHandPosition.position, Time.deltaTime * smoothingMultiplier);
        rightSmoothedPosiiton = Vector3.Lerp(rightSmoothedPosiiton, rightHandPosition.position, Time.deltaTime * smoothingMultiplier);
        if(leftHandSelecting != null){
            leftHandSelecting.transform.position = leftSmoothedPosiiton;
            leftHandSelecting.transform.rotation = cameraRoot.transform.rotation;
        }
        if(rightHandSelecting != null){
            rightHandSelecting.transform.position = rightSmoothedPosiiton;
            rightHandSelecting.transform.rotation = cameraRoot.transform.rotation;
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
            rightSmoothedPosiiton = item.transform.position;
            SetRightHandSelecting(item);
        } else {
            leftSmoothedPosiiton = item.transform.position;
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
        return handedness == PlayerInteraction.Handedness.Left ? leftSmoothedPosiiton : rightSmoothedPosiiton;
    }
}
