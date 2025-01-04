using UnityEngine;

public class Tomato : Selectable {
    [SerializeField] private FoodItemSO foodItemSO;
    private Outline outline;
    private void Awake() {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public override void Interact() {
        GameManager.Instance.AddCalories(foodItemSO.calorieCount);
        Destroy(gameObject);
    }

    public override void OnHoverEnter() {
        outline.enabled = true;
    }

    public override void OnHoverExit() {
        outline.enabled = false;
    }
}
