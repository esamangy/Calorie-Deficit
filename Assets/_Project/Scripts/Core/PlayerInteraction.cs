using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour {
    public enum Handedness {
        Right,
        Left
    }
    [SerializeField] private InputReader input;
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private Animator leftHandAnimator;
    [SerializeField] private Animator rightHandAnimator;
    [SerializeField] private float maxReach = 2f;
    private Selectable lastSelected;
    private Selectable usingSelectable;
    private bool isUsing => usingSelectable != null;
    private void OnEnable() {
        input.Interact += OnInteract;
        input.InteractAlt += OnInteractAlt;
    }

    private void OnDisable() {
        input.Interact -= OnInteract;
        input.InteractAlt -= OnInteractAlt;
    }
    private void Update() {
        HandleHover();
    }

    private void HandleHover(){
        if(isUsing){
            lastSelected = null;
            return;
        }
        Ray ray = new Ray(playerCameraRoot.position, playerCameraRoot.forward);

        RaycastHit hit;
        int layermask = LayerMask.GetMask("Ignore Raycast");

        if(Physics.Raycast(ray, out hit, maxReach, ~layermask, QueryTriggerInteraction.Ignore)) {
            if(hit.transform.TryGetComponent(out Selectable selectable)) {
                if(selectable != lastSelected){
                    if(lastSelected != null){
                        lastSelected.OnHoverExit();
                    }
                    selectable.OnHoverEnter();
                }
                lastSelected = selectable;
            } else if(hit.transform.TryGetComponent(out RootReference rootReference)){
                if(rootReference.GetReference().TryGetComponent(out selectable)){
                    if(selectable != lastSelected){
                        if(lastSelected != null){
                            lastSelected.OnHoverExit();
                        }
                        selectable.OnHoverEnter();
                    }
                    lastSelected = selectable;
                }
            } else {
                if(lastSelected != null){
                    lastSelected.OnHoverExit();
                    lastSelected = null;
                }
            }
            
        } else {
            if(lastSelected != null){
                lastSelected.OnHoverExit();
                lastSelected = null;
            }
        }
    }
    public void OnInteract() {
        if(PlayerInventory.Instance.HasLeftSelecting()) {
            Selectable selectable = PlayerInventory.Instance.GetLeftHandSelecting();
            if(selectable is BasicFood){
                leftHandAnimator.SetTrigger("Eat");
            } else {
                if(isUsing){
                    leftHandAnimator.SetTrigger("Grab");
                    usingSelectable = null;
                } else {
                    leftHandAnimator.SetTrigger("Use");
                    usingSelectable = selectable;
                }
            }
            selectable.Interact(Handedness.Left);
        } else {
            if(lastSelected != null){
                lastSelected.Interact(Handedness.Left);
                leftHandAnimator.SetTrigger("Grab");
            }
        }
    }

    public void OnInteractAlt() {
        if(PlayerInventory.Instance.HasRightSelecting()) {
            Selectable selectable = PlayerInventory.Instance.GetRightHandSelecting();
            if(selectable is BasicFood){
                rightHandAnimator.SetTrigger("Eat");
            } else {
                if(isUsing){
                    rightHandAnimator.SetTrigger("Grab");
                    usingSelectable = null;
                } else {
                    rightHandAnimator.SetTrigger("Use");
                    usingSelectable = selectable;
                }
            }
            selectable.Interact(Handedness.Left);
        } else {
            if(lastSelected != null){
                lastSelected.Interact(Handedness.Right);
                rightHandAnimator.SetTrigger("Grab");
            }
        }
    }
}
