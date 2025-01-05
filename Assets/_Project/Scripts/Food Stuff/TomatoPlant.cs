using UnityEngine;

public class TomatoPlant : Selectable {
    [SerializeField] private PlantSO plantSO;
    private Outline outline;
    private void Awake() {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public override void Interact(PlayerInteraction.Handedness handedness) {
        Selectable tomato = Instantiate(plantSO.foodPrefab, PlayerInventory.Instance.GetHandLocation(handedness), Quaternion.identity).GetComponent<Selectable>();
        PlayerInventory.Instance.SetSelecting(tomato, handedness);
        Destroy(gameObject);
    }

    public override void OnHoverEnter() {
        outline.enabled = true;
    }

    public override void OnHoverExit() {
        outline.enabled = false;
    }
}
