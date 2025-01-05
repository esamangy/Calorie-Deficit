using UnityEngine;

public class HandTracker : MonoBehaviour {
    [SerializeField] private Transform handParent;
    [SerializeField] private Transform handGrabSpot;
    [SerializeField] private PlayerInteraction.Handedness handedness;

    private void Update() {
        Vector3 targetPos = PlayerInventory.Instance.GetHandLocation(handedness);
        Vector3 delta = targetPos - handGrabSpot.position;
        handParent.position += delta;
    }
}