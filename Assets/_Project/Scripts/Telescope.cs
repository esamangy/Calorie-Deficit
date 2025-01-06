using UnityEngine;

public class Telescope : Selectable {
    [SerializeField] private Collider thisCollider;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject telescopeCamera;
    private Outline outline;
    private bool isHeld = false;
    private bool isUsed = false;
    private Camera mainCamera;
    private LayerMask defaultCameraMask;
    private void Awake() {
        mainCamera = Camera.main;
        defaultCameraMask = mainCamera.cullingMask;
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public override void Interact(PlayerInteraction.Handedness handedness) {
        if(isHeld){
            if(isUsed){
                telescopeCamera.SetActive(false);
                mainCamera.cullingMask = defaultCameraMask;
                isUsed = false;
                PlayerHUD.Instance.ToggleVignette(false);
            } else {
                telescopeCamera.SetActive(true);
                mainCamera.cullingMask = ~LayerMask.GetMask("Tool");
                isUsed = true;
                PlayerHUD.Instance.ToggleVignette(true);
            }
        } else {
            PlayerInventory.Instance.SetSelecting(this, handedness);
            thisCollider.enabled = false;
            rb.isKinematic = true;
            outline.enabled = false;
            isHeld = true;
        }
    }

    public override void OnHoverEnter() {
        outline.enabled = true;
    }

    public override void OnHoverExit() {
        outline.enabled = false;
    }

    public override void Drop() {
        thisCollider.enabled = true;
        rb.isKinematic = false;
        isHeld = false;
        isUsed = false;
        PlayerHUD.Instance.ToggleVignette(false);
    }
}
