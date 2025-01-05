using UnityEngine;

public class Telescope : Selectable {
    private Outline outline;
    private void Awake() {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public override void Interact(PlayerInteraction.Handedness handedness) {
        PlayerInventory.Instance.SetSelecting(this, handedness);
    }

    public override void OnHoverEnter() {
        outline.enabled = true;
    }

    public override void OnHoverExit() {
        outline.enabled = false;
    }
}
