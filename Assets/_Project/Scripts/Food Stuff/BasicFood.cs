using UnityEngine;

public class BasicFood : Selectable {
    [SerializeField] private FoodItemSO foodItemSO;
    public override void Interact(PlayerInteraction.Handedness handedness) {
        GameManager.Instance.AddCalories(foodItemSO.calorieCount);
        Destroy(gameObject);
    }

    public override void OnHoverEnter() {
        // noop
    }

    public override void OnHoverExit() {
        // noop
    }
}
