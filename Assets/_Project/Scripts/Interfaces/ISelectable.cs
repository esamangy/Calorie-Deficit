using UnityEngine;

public abstract class Selectable : MonoBehaviour {
    public abstract void OnHoverEnter();
    public abstract void OnHoverExit();
    public abstract void Interact();
}
