using UnityEngine;

public class Coin : Selectable {
    [SerializeField] private int score;
    private Outline outline;
    private void Awake() {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public override void Interact() {
        GameManager.Instance.AddScore(score);
        Destroy(gameObject);
    }

    public override void OnHoverEnter() {
        outline.enabled = true;
    }

    public override void OnHoverExit() {
        outline.enabled = false;
    }
}
