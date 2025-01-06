using UnityEngine;

public class Telescope : Selectable {
    [SerializeField] private Collider thisCollider;
    [SerializeField] private Rigidbody rb;
    private Outline outline;
    private bool isHeld = false;
    private void Awake() {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public override void Interact(PlayerInteraction.Handedness handedness) {
        if(isHeld){

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
    }
}
