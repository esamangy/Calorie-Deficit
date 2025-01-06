using UnityEngine;

public class TomatoPlant : Selectable {
    [SerializeField] private PlantSO plantSO;
    [SerializeField] private Transform spawnPoint;
    private Outline outline;
    private void Awake() {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public override void Interact(PlayerInteraction.Handedness handedness) {
        Selectable tomato = Instantiate(plantSO.foodPrefab, spawnPoint.position, Quaternion.identity).GetComponent<Selectable>();
        tomato.Interact(handedness);
        PlayerInventory.Instance.SetSelecting(tomato, handedness);
        Destroy(gameObject);
    }

    public override void OnHoverEnter() {
        outline.enabled = true;
    }

    public override void OnHoverExit() {
        outline.enabled = false;
    }

    public override void Drop() {
        Debug.LogError("Dropped tomato plant. This should not be possible");
    }
}
