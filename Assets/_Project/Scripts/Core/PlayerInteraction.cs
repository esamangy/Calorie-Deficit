using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {
    public enum Handedness {
        Right,
        Left,
        None,
    }
    public static PlayerInteraction Instance;
    [SerializeField] private InputReader input;
    [SerializeField] private Transform playerCameraRoot;
    [SerializeField] private Animator leftHandAnimator;
    [SerializeField] private Animator rightHandAnimator;
    [SerializeField] private float maxReach = 2f;
    private Selectable lastSelected;
    private Selectable usingSelectable;
    private Handedness usingHand = Handedness.None;
    public bool isUsing => usingSelectable != null;
    private float interactStartTime;
    private float interactAltStartTime;
    private const float HOLD_TIME = .4f;
    private void Awake() {
        if(Instance != null) {
            Destroy(this);
            return;
        }
        Instance = this;
    }
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
    public void OnInteract(bool started, bool cancelled) {
        float time;
        if(started){
            interactStartTime = Time.time;
            return;
        } else if(cancelled){
            time = Time.time - interactStartTime;
        } else {return;}

        if(time > HOLD_TIME){
            //Held
            if(isUsing && usingHand != Handedness.Left) return;
            if(PlayerInventory.Instance.HasLeftSelecting()) {
                Selectable selectable = PlayerInventory.Instance.GetLeftHandSelecting();
                if(selectable is BasicFood){
                    leftHandAnimator.SetTrigger("Eat");
                    usingSelectable = selectable;
                    StartCoroutine(DelayInteract(Handedness.Left, selectable, .9f));
                    PlayerInventory.Instance.MoveLeftToAndBack();
                } else {
                    if(isUsing){
                        leftHandAnimator.SetTrigger("Grab");
                        usingSelectable = null;
                        usingHand = Handedness.None;
                        PlayerInventory.Instance.MoveLeftBack();
                        selectable.Interact(Handedness.Left);
                    } else {
                        leftHandAnimator.SetTrigger("Use");
                        usingSelectable = selectable;
                        usingHand = Handedness.Left;
                        PlayerInventory.Instance.MoveLeftTo();
                        StartCoroutine(DelayInteract(Handedness.Left, selectable, .5f));
                    }
                }
            }
        } else {
            if(PlayerInventory.Instance.HasLeftSelecting()) {
                Drop(Handedness.Left);
            } else {
                if(lastSelected != null){
                    lastSelected.Interact(Handedness.Left);
                    leftHandAnimator.SetTrigger("Grab");
                }
            }
        }
    }

    public void OnInteractAlt(bool started, bool cancelled) {
        float time;
        if(started){
            interactAltStartTime = Time.time;
            return;
        } else if(cancelled){
            time = Time.time - interactAltStartTime;
        } else {return;}

        if(time > HOLD_TIME){
            //held
            if(isUsing && usingHand != Handedness.Right) return;
            if(PlayerInventory.Instance.HasRightSelecting()) {
                Selectable selectable = PlayerInventory.Instance.GetRightHandSelecting();
                if(selectable is BasicFood){
                    rightHandAnimator.SetTrigger("Eat");
                    usingSelectable = selectable;
                    StartCoroutine(DelayInteract(Handedness.Left, selectable, .9f));
                    PlayerInventory.Instance.MoveRightToAndBack();
                } else {
                    if(isUsing){
                        rightHandAnimator.SetTrigger("Grab");
                        usingSelectable = null;
                        usingHand = Handedness.None;
                        PlayerInventory.Instance.MoveRightBack();
                        selectable.Interact(Handedness.Right);
                    } else {
                        rightHandAnimator.SetTrigger("Use");
                        usingSelectable = selectable;
                        usingHand = Handedness.Right;
                        PlayerInventory.Instance.MoveRightTo();
                        StartCoroutine(DelayInteract(Handedness.Right, selectable, .5f));
                    }
                }
            }
        } else {
            if(PlayerInventory.Instance.HasRightSelecting()) {
                Drop(Handedness.Right);
            } else {
                if(lastSelected != null){
                    lastSelected.Interact(Handedness.Right);
                    rightHandAnimator.SetTrigger("Grab");
                }
            }
        }
        
    }

    private void Drop(Handedness handedness){
        PlayerInventory inventory = PlayerInventory.Instance;
        Selectable item = inventory.GetSelecteable(handedness);
        inventory.SetSelecting(null, handedness);

        Vector3 moveToPos;
        if(Physics.Raycast(playerCameraRoot.position, playerCameraRoot.forward, out RaycastHit hit, 1.3f)){
            moveToPos = hit.point;
        } else {
            moveToPos = playerCameraRoot.position + (playerCameraRoot.forward * 1.3f);
        }

        item.transform.position = moveToPos;
        item.Drop();
    }

    private IEnumerator DelayInteract(Handedness handedness, Selectable selectable, float delay) {
        yield return new WaitForSeconds(delay);
        selectable.Interact(handedness);
    }
}
