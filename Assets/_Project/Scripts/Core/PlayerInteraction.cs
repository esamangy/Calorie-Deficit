using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour {
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private float maxReach = 2f;
    private Selectable lastSelected;

    private void Update() {
        HandleHover();
    }

    private void HandleHover(){
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
    public void OnInteract(InputValue value) {
        if(lastSelected != null){
            lastSelected.Interact();
        }
    }
}
