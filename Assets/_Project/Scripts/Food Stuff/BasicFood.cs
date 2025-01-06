using UnityEngine;

public class BasicFood : Selectable {
    [SerializeField] private FoodItemSO foodItemSO;
    [SerializeField] private Collider thisCollider;
    [SerializeField] private Rigidbody rb;
    private Outline outline;
    private bool isHeld = true;
    private void Awake() {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        rb.isKinematic = true;
        thisCollider.enabled = false;
    }
    public override void Interact(PlayerInteraction.Handedness handedness) {
        if(isHeld) {
            GameManager.Instance.AddCalories(foodItemSO.calorieCount);
            Destroy(gameObject);
        } else {
            thisCollider.enabled = false;
            rb.isKinematic = true;
            outline.enabled = false;
            isHeld = true;
            PlayerInventory.Instance.SetSelecting(this, handedness);
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
